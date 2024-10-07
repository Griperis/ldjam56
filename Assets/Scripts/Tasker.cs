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
public class Task
{
    public string Name {
        get {
            return $"{targetName} {progress}/{total}";
        }
    }

    public int Score
    {
        get
        {
            return Mathf.RoundToInt(scoreModifier * gatheredScore);
        }
    }

    // Color the task is identified by - the outline and should be displayed in the UI
    public Color color;
    // List of ACTIVE targets, hit targets get remvoed
    public List<ShittableObject> targets;

    // This assumess the targets are from a same batch
    // e. g. Ambulance(s), Yellow Car(s), ...
    private string targetName;
    private int progress = 0;
    private int total = 0;
    private float scoreModifier = 2.0f;
    private int gatheredScore = 0;


    public Task(List<ShittableObject> targets, Color color)
    {
        this.targets = targets;
        this.color = color;

        Assert.IsTrue(targets.Count > 0);
        targetName = Tasker.GetObjectSoleName(targets[0]);
        total = targets.Count;
        
        foreach (var target in targets) {
            target.ToggleOutline(true);
            target.SetOutlineColor(color);
        }
    }

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

    public bool IsCompleted()
    {
        return !targets.Any();
    }

    // Completes target 'target' from this Task
    private void CompleteTarget(ShittableObject target)
    {
        targets.Remove(target);
        progress++;
        gatheredScore += target.score;
        target.ToggleOutline(false);
    }
}


public class Tasker : MonoBehaviour
{
    public int maxConcurrentTasks = 3;
    public int maxObjectsInTask = 3;
    public int tasksCompleted = 0;

    

    public List<Task> tasks = new List<Task>();

    [Header("Audio")]
    public AudioClip taskCompletedClip;
    
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
        for (int i = 0; i < maxConcurrentTasks; i++)
        {
            GenerateNewTask();
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

        var task = new Task(GetNewTaskTargets(), GetTaskColor());
        tasks.Add(task);
        TaskAdded(task);
        tasksCreated++;
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
        scoreManager.AddScore(task.Score);
        FloatingTextManager.CreateFloatingText(lastShittableObject.transform, $"+{task.Score} task", outlineColor: task.color);
        GenerateNewTask();
        AudioManager.PlayAudioClip(taskCompletedClip, lastShittableObject.transform, 0.6f);
    }

    private void TaskAdded(Task task)
    {
        Debug.Log($"Added task {task.Name}");
        inGameUi.UpdateTasks(tasks);
    }

    private List<ShittableObject> GetNewTaskTargets()
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
        int count = Random.Range(1, Mathf.Min(maxObjectsInTask + 1, allObjs.Count));

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
