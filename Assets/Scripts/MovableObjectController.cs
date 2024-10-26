using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObjectController : MonoBehaviour
{

    public float MovementSpeed = 6.9f;
    public float TurnSpeed = 5.0f;


    Waypoint TargetWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        TargetWaypoint = FindNearestWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (TargetWaypoint == null) 
        {
            return;
        }

        Vector3 translation = (TargetWaypoint.transform.position - transform.position).normalized * MovementSpeed * Time.deltaTime;
        transform.Translate(translation, Space.World);

        Vector3 target_vector = (TargetWaypoint.transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(target_vector, transform.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, TurnSpeed * Time.deltaTime);
        
        float distance = GetDistanceToWaypoint(TargetWaypoint);

        if (distance < 1)
        {
            HandleWaypointArrival(TargetWaypoint);
        }
    }

    private float GetDistanceToWaypoint(Waypoint inWaypoint) 
    {
        Vector3 vector = inWaypoint.transform.position - transform.position;
        return vector.magnitude;
    }

    private Waypoint FindNearestWaypoint()
    {
        Waypoint[] waypoints = Object.FindObjectsOfType<Waypoint>();
        Waypoint nearest = null;
        float min_distance = float.MaxValue;

        foreach (Waypoint waypoint in waypoints)
        {
            float distance = GetDistanceToWaypoint(waypoint);
            Debug.Log("Distance to: " + waypoint.ToString() + " = " + distance.ToString());

            if (distance <= min_distance)
            {
                min_distance = distance;
                nearest = waypoint;
            }
        }

        if (TargetWaypoint != null)
        {
            Debug.Log(TargetWaypoint.ToString() + " selected!");
        }
        return nearest;
    }

    private void HandleWaypointArrival(Waypoint inWaypoint) 
    {
        if (inWaypoint.Type == EWaypointType.Basic) 
        {
            if (inWaypoint.Neighbours.Length > 0)
            {
                TargetWaypoint = inWaypoint.Neighbours[Random.Range(0, inWaypoint.Neighbours.Length - 1)];
            }
            else 
            {
                TargetWaypoint = null;
            }
        }
    }
}
