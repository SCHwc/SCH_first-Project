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
        // UI�� �Ѿƴٴ� Ÿ��
        targetPos = target;
        rect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        // ������Ʈ�� ��ġ�� ���ŵ� ���� ü�¹��� ��ġ�� �����ϱ� ���� LateUpdate�� ȣ��
        if (targetPos == null)
        {
            Destroy(gameObject);
            return;
        }
        // ������Ʈ�� ���� ��ǥ�� �������� ȭ�鿡���� ��ǥ ���� ����
        Vector3 pos = Camera.main.WorldToScreenPoint(targetPos.position);
        // ȭ�鳻���� ��ǥ+distance��ŭ ������ ��ġ�� ü�¹��� ��ġ�� ����
        rect.position = pos + distance;
    }
}
