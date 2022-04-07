using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleControl : MonoBehaviour
{

    public enum Type { Normal, Move }
    public Type type;

    public float speed;
    Vector3 pos;
    public float delta;

    void Awake()
    {
        pos = transform.position;
    }

    void Update()
    {
        if (type == Type.Move)
        {
            Vector3 v = pos;

            v.x += delta * Mathf.Sin(Time.time * speed);

            transform.position = v;
        }
    }
    
}
