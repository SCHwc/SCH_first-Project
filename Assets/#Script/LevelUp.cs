using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    Text text;
    public Vector3 pos;
    Color color;
    void Awake()
    {
        text = GetComponent<Text>();
        pos = transform.position;
        StartCoroutine(GetLevelUp());
    }

    void Update()
    {
        transform.position += Vector3.up * 10f * Time.deltaTime;
        color = text.color;

        if (color.a > 0) { color.a -= Time.deltaTime / 2.5f; }
        text.color = color;

        //if (gameObject.activeSelf == false)
        //{
        //    color.a = 1;
        //    transform.position = pos;
        //}
        //    text.color = color;
    }

   IEnumerator GetLevelUp()
    {
        //transform.position += Vector3.up * 10f * Time.deltaTime;
        //color = text.color;

        //if (color.a > 0) { color.a -= Time.deltaTime / 2.5f; }
        //text.color = color;

        yield return new WaitForSeconds(3f);

        color.a = 1;
        text.color = color;
        transform.position = pos;
        gameObject.SetActive(false);

    }
}
