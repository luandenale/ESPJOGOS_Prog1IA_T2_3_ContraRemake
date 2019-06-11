using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShot : MonoBehaviour
{
    public bool activate;

    [SerializeField] Transform rotationCenter;

    private void Awake()
    {
        activate = false;
    }

    private void Update()
    {
        if (activate)
        {
            GetComponent<SpriteRenderer>().enabled = true;

            rotationCenter.Rotate(0f, 0f, -45f);
        }
    }
}
