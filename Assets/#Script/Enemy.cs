using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public static Enemy enemy;
    public enum Type { A, B, C}
    public Type type;
    public Vector3 spawnPos;

    bool isAtk;
    bool isChase;
    public bool isDead;
    bool isDamage;
    public int curHp;
    public int maxHp;
    public int exp;
    public float checkRadius;
    Transform target = null;
    public GameObject bullet;
    Rigidbody rb;
    BoxCollider bc;
    MeshRenderer[] meshs;
    NavMeshAgent nav;
    Animator anim;
    public BoxCollider meleeArea;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spawnPos = transform.position+ new Vector3((Random.Range(-3,3)),0, (Random.Range(-3, 3)));
        isChase = true;

    }


    void Update()
    {
        if (!isDamage) { InvokeRepeating("CheckingTarget", 0f, 0.5f); }

        if (isDamage) { target = GameObject.Find("Player").transform; }

        if (target != null && nav.enabled)
        {
            anim.SetBool("isWalk", true);
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }

        if (target == null && nav.enabled && transform.position != spawnPos)
        {
            nav.SetDestination(spawnPos);
            nav.isStopped = !isChase;
        }
        if (transform.position == spawnPos)
        {
            anim.SetBool("isWalk", false);
        }
    }
    void CheckingTarget()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, checkRadius, 1 << 8);

        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].tag == "Player")
                {
                    target = cols[i].gameObject.transform;
                }
            }
        }
        else { target = null; }
    }

    void FixedUpdate()
    {
        FreezeVelocity();
        GetTargetAtk();
    }
    void FreezeVelocity()
    {
        if (isChase)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }

    void GetTargetAtk()
    {
        if (!isDead)
        {
            float radius = 0;
            float range = 0;

            switch (type)
            {
                case Type.A:
                    radius = 1.5f;
                    range = 2.5f;
                    break;

                case Type.B:
                    radius = 1f;
                    range = 8f;
                    break;

                case Type.C:
                    radius = 0.5f;
                    range = 30f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, radius
                , transform.forward, range, LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAtk)
            {
                StartCoroutine(GetAttack());
            }
        }
    }
    IEnumerator GetAttack()
    {
        isChase = false;
        isAtk = true;
        anim.SetBool("isAttack", true);
        switch (type)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(0.7f);
                break;

            case Type.B:
                yield return new WaitForSeconds(0.2f);
                rb.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rb.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;

            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instBullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody bullet_rb = instBullet.GetComponent<Rigidbody>();
                bullet_rb.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;
        }

        isChase = true;
        isAtk = false;
        anim.SetBool("isAttack", false);

    }
    public void HitByGrenade(Vector3 explosionPos)
    {
        curHp -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHp -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            rb.AddForce(transform.forward * -10, ForceMode.Impulse); // ³Ë¹é
            Invoke("GetKnockBackOff", 1f);
            StartCoroutine(OnDamage(reactVec, false));
        }
        else if (other.tag == "HandGunBullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHp -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec, false));
        }
        else if (other.tag == "SubGunBullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHp -= bullet.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactVec, false));
        }
    }
    void GetKnockBackOff()
    {
        rb.velocity = Vector3.zero;
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in meshs) { mesh.material.color = Color.red; }
        isDamage = true;
        yield return new WaitForSeconds(0.1f);

        if (curHp > 0)
        {
            foreach (MeshRenderer mesh in meshs) { mesh.material.color = Color.white; }
        }
        else
        {
            isChase = false;
            foreach (MeshRenderer mesh in meshs) { mesh.material.color = Color.gray; }
            anim.SetTrigger("doDie");
            nav.enabled = false;
            gameObject.layer = 12;
            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;
                rb.freezeRotation = false;
                rb.AddForce(reactVec * 5, ForceMode.Impulse);
                rb.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rb.AddForce(reactVec * 7, ForceMode.Impulse);
            }
            Information.stats.curExp += exp;
            Destroy(gameObject, 3f);
        }

        yield return new WaitForSeconds(4f);
        isDamage = false;
    }
}
