using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject mesh;
    public GameObject explosion;
    public Rigidbody rb;
    void Start()
    {
        StartCoroutine("Explosion");
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(2.5f);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        mesh.SetActive(false);
        explosion.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 12, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        foreach (RaycastHit hit in rayHits)
        {
            hit.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5f);
    }
}
