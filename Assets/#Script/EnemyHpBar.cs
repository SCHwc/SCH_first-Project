using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Vector3 distance = Vector3.up * 20f;
    Transform targetPos;
    RectTransform rect;

    public void Setup(Transform target)
    {
        // UI가 쫓아다닐 타겟
        targetPos = target;
        rect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        // 오브젝트의 위치가 갱신된 이후 체력바의 위치를 설정하기 위해 LateUpdate로 호출
        if (targetPos == null)
        {
            Destroy(gameObject);
            return;
        }
        // 오브젝트의 월드 좌표를 기준으로 화면에서의 좌표 값을 구함
        Vector3 pos = Camera.main.WorldToScreenPoint(targetPos.position);
        // 화면내에서 좌표+distance만큼 떨어진 위치를 체력바의 위치로 설정
        rect.position = pos + distance;
    }
}
