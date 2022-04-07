using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MentUI : MonoBehaviour
{
    public GameObject ment;

    void Awake()
    {
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            ment.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ment.SetActive(false);
        }
    }
}
