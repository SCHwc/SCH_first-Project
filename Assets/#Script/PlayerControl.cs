using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl pControl;
    float hAxis;
    float vAxis;
    float atkDelay;
    float limitY = -1f;
    public float speed;
    public float jPower;

    bool wDown;
    bool jDown;
    bool dDown;
    bool fDown;
    bool rDown;
    bool gDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool sDown4;
    bool offDown;
    public bool iDown;
    public bool tDown;

    bool isJump;
    bool isDodge;
    bool isBorder;
    bool isDamage;
    bool isSwap;
    bool isReload;
    bool isDie;
    bool isAtk = true;
    public Vector3 spawnPos;
    public GameObject gnObj;
    MeshRenderer[] meshs;
    Animator anim;
    public Rigidbody rb;
    public GameObject[] weapons;
    public Camera followCam;
    Vector3 moveVector;
    Vector3 dodgeVector;
    public GameManager manager;
    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        meshs = GetComponentsInChildren<MeshRenderer>();

        transform.position = spawnPos;
    }

    void Update()
    {
        GetInput();
        GetMove();
        GetLook();
        GetJump();
        GetDodgeOn();
        GetSwap();
        GetDrink();
        GetThrow();
        GetAttack();
        GetReload();

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
        hAxis = Input.GetAxisRaw("Horizontal");     // 좌우이동키
        vAxis = Input.GetAxisRaw("Vertical");       // 앞뒤이동키
        wDown = Input.GetButton("Walk");        // 걷기이동키
        jDown = Input.GetButtonDown("Jump");        // 점프키
        dDown = Input.GetButtonDown("Dodge");       // 회피이동키
        iDown = Input.GetButtonDown("Interation");      // 상호작용키
        tDown = Input.GetButtonDown("Inventory");       // 인벤토리키
        fDown = Input.GetButton("Fire1");       // 좌클릭(공격키)
        gDown = Input.GetButtonDown("Fire2");       // 우클릭(수류탄투척)
        rDown = Input.GetButtonDown("Reload");      // 재장전키
        sDown1 = Input.GetButtonDown("Swap1");      // 스왑1
        sDown2 = Input.GetButtonDown("Swap2");      // 스왑2
        sDown3 = Input.GetButtonDown("Swap3");      // 스왑3
        sDown4 = Input.GetButtonDown("Potion");      // 스왑3
        offDown = Input.GetButtonDown("WeaponOff");      // 무기내리기

    }


    void GetMove()
    {
        moveVector = new Vector3(hAxis, 0, vAxis).normalized;

        if (isDodge) { moveVector = dodgeVector; }
        if (isSwap || !isAtk || isReload || isDie) { moveVector = Vector3.zero; }

        if (!isBorder)
        {
            transform.position += moveVector * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        }

        anim.SetBool("isRun", moveVector != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    IEnumerator GetDie()
    {
        gameObject.layer = 14;
        anim.SetTrigger("doDie");
        isDie = true;

        yield return new WaitForSeconds(2f);
        gameObject.layer = 8;
        manager.GameOver();
    }
    void GetLook()
    {
        transform.LookAt(transform.position + moveVector); // 키보드 회전

        if (fDown && !isDie) // 마우스 회전
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
        if (jDown && !isJump && !isDodge && !isReload && isAtk && !isSwap && !isDie)
        {
            rb.AddForce(Vector3.up * jPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void GetDodgeOn()
    {
        if (dDown && !isJump && !isDodge && !isSwap && !isReload && isAtk && !isDie)
        {
            dodgeVector = moveVector;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("GetDodgeOff", 0.5f);
        }
    }

    void GetDodgeOff()
    {
        isDodge = false;
        speed /= 2;
    }

    void GetAttack()
    {
        if (Information.stats.equipWeapon == null) { return; }

        atkDelay += Time.deltaTime;
        isAtk = Information.stats.equipWeapon.rate < atkDelay;

        if (fDown && isAtk && !isDodge && !isSwap && !isJump && !isReload && !isDie)
        {
            Information.stats.equipWeapon.GetUse();
            anim.SetTrigger(Information.stats.equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            atkDelay = 0;
        }
    }

    void GetSwap()
    {
        int index = -1;
        if (sDown1 && (!Information.stats.hasWeapons[0] || Information.stats.equipWeaponIndex == 0)) { return; }
        if (sDown2 && (!Information.stats.hasWeapons[1] || Information.stats.equipWeaponIndex == 1)) { return; }
        if (sDown3 && (!Information.stats.hasWeapons[2] || Information.stats.equipWeaponIndex == 2)) { return; }

        if (offDown) { index = -1; }
        if (sDown1) { index = 0; }
        if (sDown2) { index = 1; }
        if (sDown3) { index = 2; }

        if ((sDown1 || sDown2 || sDown3) && !isJump && !isDodge && isAtk && !isReload && !isDie)
        {
            if (Information.stats.equipWeapon != null)
            {
                Information.stats.equipWeapon.gameObject.SetActive(false);
            }
            Information.stats.equipWeaponIndex = index;
            Information.stats.equipWeapon = weapons[index].GetComponent<Weapon>();
            Information.stats.equipWeapon.gameObject.SetActive(true);
            anim.SetTrigger("doSwap");
            isSwap = true;
            Invoke("GetSwapOff", 0.5f);
        }

        if (offDown && !isJump && !isDodge && isAtk && !isReload && !isDie)
        {
            if (Information.stats.equipWeapon != null)
            {
                Information.stats.equipWeapon.gameObject.SetActive(false);
            }
            Information.stats.equipWeaponIndex = index;
            anim.SetTrigger("doSwap");
            isSwap = true;
            Invoke("GetSwapOff", 0.5f);
        }
    }

    void GetSwapOff()
    {
        isSwap = false;
    }

    void GetDrink()
    {
        if (Information.stats.potionCnt <= 0 || Information.stats.curHp == Information.stats.maxHp)
        {
            return;
        }

        if (sDown4)
        {
            Information.stats.curHp += 20;
            Information.stats.potionCnt--;
        }

    }

    void GetReload()
    {
        if (Information.stats.equipWeapon == null) { return; }
        if (Information.stats.equipWeapon.type == Weapon.Type.Melee) { return; }
        if (Information.stats.ammo == 0) { return; }

        if (rDown && !isDodge && !isJump && !isSwap && isAtk && !isDie)
        {
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("GetReloadOff", 0.5f);
        }
    }
    void GetReloadOff()
    {
        Weapon weapon = GetComponent<Weapon>();

        int reAmmo = Information.stats.ammo < Information.stats.equipWeapon.maxAmmo ? Information.stats.ammo : Information.stats.equipWeapon.maxAmmo;
        if (Information.stats.equipWeapon.gunType == Weapon.GunType.Hand) { Information.stats.HandGun_curAmmo = reAmmo; }
        else if (Information.stats.equipWeapon.gunType == Weapon.GunType.Sub) { Information.stats.SubGun_curAmmo = reAmmo; }

        Information.stats.ammo -= reAmmo;
        isReload = false;
    }
    void GetThrow()
    {
        if (Information.stats.hasGrenades == 0) return;

        if (gDown && !isDodge)
        {
            Ray ray = followCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nVec = rayHit.point - transform.position;
                nVec.y = 10f;

                GameObject instGrenade = Instantiate(gnObj, transform.position, transform.rotation);
                Rigidbody rbGrenade = instGrenade.GetComponent<Rigidbody>();
                rbGrenade.AddForce(nVec, ForceMode.Impulse);
                rbGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);

                Information.stats.hasGrenades--;
            }
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                Information.stats.curHp -= enemyBullet.damage;

                bool isBossAtk = other.name == "Boss Melee Area";
                StartCoroutine(OnDamage(isBossAtk));
            }

            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }
    }
    IEnumerator OnDamage(bool isBossAtk)
    {
        isDamage = true;
        if (Information.stats.curHp <= 0)
        {
            StartCoroutine(GetDie());
        }
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.red;
        }

        if (isBossAtk)
        {
            rb.AddForce(transform.forward * -20, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(1.5f);

        if (isBossAtk)
        {
            rb.velocity = Vector3.zero;
        }

        isDamage = false;
        foreach (MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }

    }
}
