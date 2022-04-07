using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Coin, Weapon, Ammo, Grenade, Potion, Heart }
    public Type type;

    public int value;

    Rigidbody rb;
    SphereCollider sc;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            rb.isKinematic = true;
            sc.enabled = false;
        }
    }
}
