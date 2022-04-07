using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    bool pDown;

    bool onPortal;

    public enum Type
    {
        Main, Mini_0, Mini_1, Mini_2, Exit
    }

    public Type pType;
    void Awake()
    {
        onPortal = false;
    }
    void Update()
    {
        GetInput();
        InPortal();
    }

    void GetInput()
    {
        pDown = Input.GetButtonDown("InPortal");
    }

    void InPortal()
    {
        if (onPortal)
        {
            if (pDown)
            {
                switch (pType)
                {
                    case Type.Main:
                        SceneManager.LoadScene("MainRoom");
                        break;

                    case Type.Mini_0:
                        SceneManager.LoadScene("MiniRoom_0");
                        break;

                    case Type.Mini_1:
                        SceneManager.LoadScene("MiniRoom_1");
                        break;

                    case Type.Mini_2:
                        break;

                    case Type.Exit:
                        SceneManager.LoadScene("WaitingRoom");
                        break;
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            onPortal = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") { onPortal = false; }
    }

}
