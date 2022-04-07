using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mini1Control : MonoBehaviour
{
    public GameObject Finish_UI;

    public float zSpeed;
    public float xSpeed;

    public float limitX = 5.2f;

    float gravity = -37f;
    float posY = 0.05f;
    float limitY = -1f;
    float jTime = 0.8f;

    bool lDown;
    bool rDown;
    bool jDown;

    bool isMove;
    bool isJump;

    public TrailRenderer lEffect;
    public TrailRenderer rEffect;
    MeshRenderer[] meshs;
    Vector3 moveVector;
    Rigidbody rb;
    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        GetInput();
        GetJump();
        GetMove();

        transform.position += Vector3.forward * zSpeed * Time.deltaTime;

        if (transform.position.y < limitY)
        {
            Destroy(gameObject);
            Time.timeScale = 0;
            Finish_UI.SetActive(true);
        }
    }

    void GetInput()
    {
        lDown = Input.GetButtonDown("Left");
        rDown = Input.GetButtonDown("Right");
        jDown = Input.GetButtonDown("Jump");
    }

    void GetMove()
    {
        StopCoroutine("xMove");
        StartCoroutine("xMove");
    }

    IEnumerator xMove()
    {
        if (lDown)
        {
            StopCoroutine("effect");
            StartCoroutine("effect");
            moveVector = new Vector3(-limitX, 0, 0);
            rb.velocity = moveVector;
            //transform.position -= moveVector * xSpeed * Time.deltaTime;
            //rb.AddForce(Vector3.left*limitX);
        }
        else if (rDown)
        {
            StopCoroutine("effect");
            StartCoroutine("effect");
            moveVector = new Vector3(limitX, 0, 0);
            rb.velocity = moveVector;
            //transform.position = moveVector * xSpeed * Time.deltaTime;
        }

        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector3.zero;

    }

    IEnumerator effect()
    {
        lEffect.enabled = true;
        rEffect.enabled = true;
        yield return new WaitForSeconds(0.7f);

        lEffect.enabled = false;
        rEffect.enabled = false;
    }

    void GetJump()
    {
        if (jDown && !isJump)
        {
            StopCoroutine("Jump");
            StartCoroutine("Jump");
        }
    }
    IEnumerator Jump()
    {
        float cur = 0;
        float per = 0;
        float v0 = -gravity;

        rb.useGravity = false;
        isJump = true;

        anim.SetBool("isJump", true);
        anim.SetTrigger("doJump");

        while (per < 1)
        {
            cur += Time.deltaTime;
            per = cur / jTime;

            float y = posY + (v0 * per) + (gravity * per * per);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            yield return null;
        }

        rb.useGravity = true;
        isJump = false;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("isJump", false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            Time.timeScale = 0;
            Finish_UI.SetActive(true);
            foreach (MeshRenderer mesh in meshs)
            {
                mesh.material.color = Color.red;
            }
        }
    }

}
