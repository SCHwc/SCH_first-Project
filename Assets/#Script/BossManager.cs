using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    Vector3 lookVec;
    Vector3 tauntVec;

    public bool isLook;
    bool isDead;
    public float checkRadius;
    public int curHp;
    public int maxHp;
    public int exp;

    public GameObject missile;
    public GameObject Rock;
    public Transform missilePortA;
    public Transform missilePortB;
    Transform target;
    Rigidbody rb;
    BoxCollider bc;
    MeshRenderer[] meshs;
    NavMeshAgent nav;
    Animator anim;
    public BoxCollider meleeArea;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;
        StartCoroutine(GetPattern());
    }

    // 아래 기능은 범위 체크할 때 사용
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, checkRadius);
    //}

    void Update()
    {
        target = GameObject.Find("Player").transform;

        if (isDead)
        {
            StopAllCoroutines();
            return;
        }

        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
        {
            nav.SetDestination(tauntVec);
        }

    }
    
    IEnumerator GetPattern()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 5);

        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(GetMissileShot());
                break;

            case 2:
            case 3:
                StartCoroutine(GetRockShot());
                break;

            case 4:
                StartCoroutine(GetTaunt());
                break;
        }
    }

    IEnumerator GetMissileShot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileB.target = target;

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(GetPattern());
    }

    IEnumerator GetRockShot()
    {
        isLook = false;
        anim.SetTrigger("doRockShot");
        Instantiate(Rock, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);

        isLook = true;
        StartCoroutine(GetPattern());
    }

    IEnumerator GetTaunt()
    {
        tauntVec = target.position + lookVec;
        isLook = false;
        nav.isStopped = false;
        bc.enabled = false;
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1.5f);
        isLook = true;
        nav.isStopped = true;
        bc.enabled = true;

        StartCoroutine(GetPattern());

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHp -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
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

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        foreach (MeshRenderer mesh in meshs) { mesh.material.color = Color.red; }
        yield return new WaitForSeconds(0.1f);

        if (curHp > 0) { foreach (MeshRenderer mesh in meshs) { mesh.material.color = Color.white; } }
        else
        {
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
    }
}
