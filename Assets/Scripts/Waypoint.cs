using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWaypointType 
{
    Basic
}

[ExecuteInEditMode]

public class Waypoint : MonoBehaviour
{
    public EWaypointType Type = EWaypointType.Basic;
    public Waypoint[] Neighbours;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrawGizmos()
    {
        foreach (Waypoint neighbour in Neighbours)
        {
            Gizmos.DrawLine(transform.position + new Vector3(0.0f, 0.5f, 0.0f), neighbour.transform.position + new Vector3(0.0f, 0.5f, 0.0f));
        }
    }
}
