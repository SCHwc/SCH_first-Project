using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini0Control : MonoBehaviour
{
    float hAxis;
    float vAxis;
    float atkDelay;
    float limitY = -1f;
    public float speed;
    public float jPower;

    bool wDown;
    bool jDown;
    bool fDown;
    public bool iDown;
    public bool tDown;

    bool isJump;
    bool isBorder;
    public Vector3 spawnPos;
    Animator anim;
    public Rigidbody rb;
    public Camera followCam;
    Vector3 moveVector;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        transform.position = spawnPos;
    }

    void Update()
    {
        GetInput();
        GetMove();
        GetLook();
        GetJump();
        if (transform.position.y < limitY)
        {
            transform.position = spawnPos;
        }
    }

    //void GetWeaponFind()
    //{

    //   for (int i = 0; i < Information.stats.weapons.Length; i++)
    //    {
    //        Information.stats.weapons[i] = 
    //    }

    //}
    public void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");     // �¿��̵�Ű
        vAxis = Input.GetAxisRaw("Vertical");       // �յ��̵�Ű
        wDown = Input.GetButton("Walk");        // �ȱ��̵�Ű
        jDown = Input.GetButtonDown("Jump");        // ����Ű
        iDown = Input.GetButtonDown("Interation");      // ��ȣ�ۿ�Ű
        fDown = Input.GetButton("Fire1");       // ��Ŭ��(����Ű)
    }


    void GetMove()
    {
        moveVector = new Vector3(hAxis, 0, vAxis).normalized;

        if (!isBorder)
        {
            transform.position += moveVector * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        }

        anim.SetBool("isRun", moveVector != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void GetLook()
    {
        transform.LookAt(transform.position + moveVector); // Ű���� ȸ��

        if (fDown) // ���콺 ȸ��
        {
            Ray ray = followCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nVec = rayHit.point - transform.position;
                nVec.y = 0;
                transform.LookAt(transform.position + nVec);
            }
        }
    }

    void GetJump()
    {
        if (jDown && !isJump)
        {
            rb.AddForce(Vector3.up * jPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

  
  
    void FreezeRotation()
    {
        rb.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Wall"));
    }
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }

        if (other.gameObject.tag == "Obstacle")
        {
            // Vector3 reactVec = other.transform.position - transform.position;
            StartCoroutine(GetCollision());
            Destroy(other.gameObject);
        }
    }
    IEnumerator GetCollision()
    {
        //reactVec = reactVec.normalized;
        //reactVec += Vector3.up * 3;
        // PlayerControl.pControl.rb.AddForce(reactVec * 20, ForceMode.Impulse);
        rb.AddForce(transform.forward * -30, ForceMode.Impulse);
        yield return new WaitForSeconds(1.5f);
        rb.velocity = Vector3.zero;
    }
}
