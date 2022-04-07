using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public enum Type { Main, MiniGame }
    public Type type;
    public Transform target;
    public Vector3 offset;

    float zDistance;

    void Awake()
    {
        transform.position = target.position + offset;
        zDistance = target.position.z - transform.position.z;
    }
    void Update()
    {
        if (type == Type.Main)
        {
            transform.position = target.position + offset;
        }
        else if (type == Type.MiniGame)
        {
            Vector3 pos = transform.position;
            pos.z = target.position.z - zDistance;
            transform.position = pos;
        }
    }
}
