using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // GetUse()메인루틴 > GetSwing()서브루틴 > GetUse()메인루틴
    // GetUse()메인루틴 + GetSwing() 코루틴(Co-Op)

    public enum Type { Melee, Range };
    public Type type;
    public enum GunType { Hand, Sub,None };
    public GunType gunType;
    public int damage;
    public float rate;
    public int maxAmmo;

    public BoxCollider bc;
    public TrailRenderer effect;
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    public void GetUse()
    {
        if (type == Type.Melee)
        {
            StopCoroutine("GetSwing");
            StartCoroutine("GetSwing");
        }
        else if (type == Type.Range)
        {
            if (gunType == GunType.Hand && Information.stats.HandGun_curAmmo > 0)
            {
                Information.stats.HandGun_curAmmo--;
                StartCoroutine("GetShot");
            }
            else if (gunType == GunType.Sub && Information.stats.SubGun_curAmmo > 0)
            {
                Information.stats.SubGun_curAmmo--;
                StartCoroutine("GetShot");
            }
        }
    }

    IEnumerator GetSwing()
    {
        // yield return null;  1프레임 대기
        bc.enabled = true;
        effect.enabled = true;

        yield return new WaitForSeconds(0.5f);
        bc.enabled = false;
        yield return new WaitForSeconds(0.3f);
        effect.enabled = false;
    }

    IEnumerator GetShot()
    {
        GameObject instBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bullet_rb = instBullet.GetComponent<Rigidbody>();
        bullet_rb.velocity = bulletPos.forward * 50;
        yield return null;
        GameObject instBulletCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody bulletCase_rb = instBulletCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up;
        bulletCase_rb.AddForce(caseVec, ForceMode.Impulse);
        bulletCase_rb.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
