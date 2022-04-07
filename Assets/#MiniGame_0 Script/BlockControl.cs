using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour
{
    public enum Type { A, B, C };
    public Type type;

    public float speed;
    Vector3 pos;
    public float delta;
    public Vector3 spawnPos;

    void Awake()
    {
        pos = transform.position;
    }
    void Update()
    {
        Vector3 vec = pos;

        if (type == Type.A)
        {
            vec.x += delta * Mathf.Sin(Time.time * speed);

            transform.position = vec;
        }
        if (type == Type.B)
        {
            vec.z += delta * Mathf.Sin(Time.time * speed);

            transform.position = vec;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(transform);
        }

        
    }
    
    

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.transform.SetParent(null);
        }
    }
}
