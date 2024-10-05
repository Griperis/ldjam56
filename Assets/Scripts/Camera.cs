using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 20, 0);
    public PidgeonController pidgeon;

    void Update()
    {
        transform.position = pidgeon.transform.position + offset;
    }
}
