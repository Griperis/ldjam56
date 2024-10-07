using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SocialPlatforms.Impl;

/* 
 * Possible task types:
 * - Shit on X/N AssetName (e. g. Shit on 0/3 ambulances) - DONE
 * - Shit X times on AssetName - TODO
 * - Shit X times on Any Asset
*/

public abstract class Task
{
    virtual public string Name
    {
        get
        {
            return $"{targetName} {progress}/{total} (+{awardScore})";
        }
    }

    // Color the task is identified by - the outline and should be displayed in the UI
    public Color color;
    // List of ACTIVE targets, hit targets get remvoed
    public List<ShittableObject> targets;

    // Precomputed value at the start of the task how much score will completing the task add
    public int awardScore = 0;
    // This assumess the targets are from a same batch
    // e. g. Ambulance(s), Yellow Car(s), ...
    protected string targetName;
    protected int progress = 0;
    protected int total = 0;
    protected float scoreModifier;

    public Task(List<ShittableObject> targets, Color color, float scoreModifier)
    {
        this.targets = targets;
        this.color = color;
        this.scoreModifier = scoreModifier;
        awardScore = CalculateAwardScore();

        Assert.IsTrue(targets.Count > 0);
        targetName = Tasker.GetObjectSoleName(targets[0]);
        total = targets.Count;

        foreach (var target in targets)
        {
            target.ToggleOutline(true);
            target.SetOutlineColor(color);
        }
    }

    abstract public bool IsCompleted();

    abstract public int CalculateAwardScore();

    public bool IsObjectRelevant(ShittableObject target)
    {
        return targets.Contains(target);
    }

    public bool ObjectHit(ShittableObject target)
    {
        var idx = targets.IndexOf(target);
        if (idx == -1)
        {
            return false;
        }
        else
        {
            CompleteTarget(target);
            return true;
        }
    }

    // Closes the task, without adding any score
    public void Close()
    {
        foreach (var target in targets)
        {
            target.ToggleOutline(false);
        }
        targets.Clear();
    }

    // Completes target 'target' from this Task
    private void CompleteTarget(ShittableObject target)
    {
        targets.Remove(target);
        progress++;
        target.ToggleOutline(false);
    }
}

public class HitSpecificTask : Task
{
    public HitSpecificTask(List<ShittableObject> targets, Color color) : base(targets, color, GetRarityScoreModifier(targets.Count)) { }

    override public bool IsCompleted()
    {
        return !targets.Any();
    }

    public override int CalculateAwardScore()
    {
        return Mathf.RoundToInt(scoreModifier * targets.Count * targets[0].score);
    }

    public static float GetRarityScoreModifier(int count)
    {
        return 5.0f * Mathf.Max(1 / (0.1f * count * count), 1.0f); 
    }
}

public class HitAnyTask : Task
{
    override public string Name
    {
        get
        {
            return $"Any {targetName} {progress}/{total} (+{awardScore})";
        }
    }

    public HitAnyTask(List<ShittableObject> targets, int total, Color color) : base(targets, color, 2.0f)
    {
        this.total = total;
        awardScore = CalculateAwardScore();
    }

    override public bool IsCompleted()
    {
        return progress >= total;
    }

    public override int CalculateAwardScore()
    {
        return Mathf.RoundToInt(scoreModifier * total * targets[0].score);
    }
}


public class Tasker : MonoBehaviour
{
    [HideInInspector]
    public int tasksCompleted = 0;

    [Header("Tasker Configuration")]
    public int maxConcurrentTasks = 3;

    [Header("Hit Selected Task")]
    public int hitSelectedMaxObjects = 3;

    [Header("Hit Any Tasks")]
    public int hitAnyMaxObjects = 7;
    public int hitAnyMinObjectsIfApplicable = 3;

    [Header("Audio")]
    public AudioClip taskCompletedClip;
    
    public List<Task> tasks = new List<Task>();
    
    private Dictionary<string, List<ShittableObject>> shittableObjsByName = new Dictionary<string, List<ShittableObject>>();
    
    private ScoreManager scoreManager;
    private SimpleRuntimeUI inGameUi;

    private int tasksCreated = 0;

    private void Awake()
    {
       scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            throw new MissingComponentException("ScoreManager object not found in scene!");
        }

        inGameUi = FindObjectOfType<SimpleRuntimeUI>();
        if (scoreManager == null)
        {
            throw new MissingComponentException("SimpleRuntimeUI object not found in scene!");
        }
    }

    private void Start()
    {
        var allShittableObjects = FindObjectsOfType<ShittableObject>();
        foreach (var obj in allShittableObjects)
        {
            string soleName = GetObjectSoleName(obj);
            if (shittableObjsByName.ContainsKey(soleName))
            {
                shittableObjsByName[soleName].Add(obj);
            }
            else
            {
                shittableObjsByName.Add(soleName, new List<ShittableObject>());
            }
        }
        PopulateWithTasks();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RefreshTasks();
            }
        }
    }

    // Generates new task out of the all shittable objects in the scene
    public void GenerateNewTask()
    {
        if (tasks.Count > maxConcurrentTasks)
        {
            Debug.Log("Adding more tasks would exceed the maxConcurrentTasks limit!");
            return;
        }
        if (shittableObjsByName.Values.Count == 0)
        {
            Debug.Log("No shittable objects found in current scene!");
            return;
        }
        // Shouldn't this be deterministic?
        var randInt = Random.Range(0, 2);
        Task task = null;
        if (randInt == 0)
        {
            task = new HitSpecificTask(GetNewTaskTargets(), GetTaskColor());
        }
        else if (randInt == 1)
        {
            var taskTargets = GetNewTaskTargets(true);
            // Degrade the task to "HitSpecificTask" if there are only less than X targets to
            // give the player larger score
            if (taskTargets.Count <= 2)
            {
                // Rare objects get more score
                task = new HitSpecificTask(taskTargets, GetTaskColor());
            }
            else
            {
                task = new HitAnyTask(
                    taskTargets,
                    Mathf.Clamp(
                        taskTargets.Count,
                        Mathf.Min(taskTargets.Count, hitAnyMinObjectsIfApplicable),
                        hitAnyMaxObjects),
                    GetTaskColor()
                );
            }
        }
        tasks.Add(task);
        TaskAdded(task);
        tasksCreated++;
    }

    public void PopulateWithTasks()
    {
        for (int i = 0; i < maxConcurrentTasks; i++)
        {
            GenerateNewTask();
        }
    }

    public void RefreshTasks()
    {
        foreach (var task in tasks)
        {
            task.Close();
        }
        tasks.Clear();
        PopulateWithTasks();
    }

    // Called from the outside, from the shittable object
    public void ShittableObjectHit(ShittableObject shittableObject)
    {
        var tasksToRemove = new List<Task>();
        foreach (var task in tasks)
        {
            if (task.ObjectHit(shittableObject))
            {
                if (task.IsCompleted())
                {
                    tasksToRemove.Add(task);
                }
            }
        }

        foreach (var task in tasksToRemove)
        {
            tasks.Remove(task);
            TaskCompleted(task, shittableObject);
        }

        inGameUi.UpdateTasks(tasks);

    }
    public static string GetObjectSoleName(ShittableObject obj)
    {
        string name = obj.name;
        var bracketIndex = name.IndexOf('(');
        if (bracketIndex < 0)
        {
            return name;
        }
        else
        {
            // Name (DDD) -> Name; -1 from the bracket index to also remove the space
            return name.Remove(bracketIndex - 1, name.Length - bracketIndex + 1);
        }
    }

    private void TaskCompleted(Task task, ShittableObject lastShittableObject)
    {
        Debug.Log($"Completed task {task.Name}");
        tasksCompleted++;
        scoreManager.AddScore(task.awardScore);
        FloatingTextManager.CreateFloatingText(lastShittableObject.transform, $"+{task.awardScore}", outlineColor: task.color);
        GenerateNewTask();
        AudioManager.PlayAudioClip(taskCompletedClip, lastShittableObject.transform, 0.6f);
    }

    private void TaskAdded(Task task)
    {
        Debug.Log($"Added task {task.Name}");
        inGameUi.UpdateTasks(tasks);
    }

    private List<ShittableObject> GetNewTaskTargets(bool allObjectsFromBatch = false)
    {
        var targetsInTasks = GetTargetsInTasks();
        
        // Adjust the all shittable objects map and remove objects that are already
        // assigned as targets in some other tasks.
        var adjustedMap = new Dictionary<string, List<ShittableObject>>();
        foreach (var pair in shittableObjsByName)
        {
            var adjustedObjects = new List<ShittableObject>(pair.Value);
            adjustedObjects.RemoveAll(o => targetsInTasks.Contains(o));
            if (adjustedObjects.Count == 0)
            {
                continue;
            }

            if (!adjustedMap.ContainsKey(pair.Key))
            {
                adjustedMap.Add(pair.Key, adjustedObjects);
            }

        }
        // Get a list of objects from random key (random type of asset)
        var allObjs = new List<ShittableObject>(adjustedMap.ElementAt(Random.Range(0, adjustedMap.Count)).Value);
        if (allObjectsFromBatch)
        {
            return allObjs;
        }
        
        int count = Random.Range(1, Mathf.Min(hitSelectedMaxObjects + 1, allObjs.Count));
        var pickedObjs = new List<ShittableObject>(count);
        while (count > 0)
        {
            var randomIndex = Random.Range(0, allObjs.Count);
            var randomObject = allObjs[randomIndex];
            if (!pickedObjs.Contains(randomObject))
            {
                count--;
                pickedObjs.Add(randomObject);
            }
        }

        return pickedObjs;
    }

    private List<ShittableObject> GetTargetsInTasks()
    {
        List<ShittableObject> targets = new List<ShittableObject>();
        foreach (var task in tasks)
        {
            targets.AddRange(task.targets);
        }
        return targets;
    }

    private Color GetTaskColor()
    {
        Color[] possibleColors = { Color.red, Color.blue, Color.magenta, Color.yellow, Color.green };
        return possibleColors[tasksCreated % possibleColors.Length];
    }
}
