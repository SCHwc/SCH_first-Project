using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum Type { Player, EnemyMelee, EnemyMissile }
    public Type type;
    public int damage;
    public bool isMelee;
    public bool isRock;

    void Awake()
    {
        if (type == Type.Player) { Destroy(gameObject, 3); }
        if (type == Type.EnemyMissile && !isRock) { Destroy(gameObject, 4f); }
    }
    void OnCollisionEnter(Collision other)
    {
        if (!isRock && other.gameObject.tag == "Ground")
        {
            Destroy(gameObject, 3);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
