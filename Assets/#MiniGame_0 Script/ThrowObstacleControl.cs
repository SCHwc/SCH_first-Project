using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObstacleControl : MonoBehaviour
{
    void Awake()
    {
        Destroy(gameObject, 2f);
    }

}
