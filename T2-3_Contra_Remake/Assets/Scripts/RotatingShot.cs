using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShot : MonoBehaviour
{
    public bool activate;

    private bool _triggered;
    private float _xDir;

    [SerializeField] Transform rotationCenter;

    private void Awake()
    {
        activate = false;
        _triggered = false;
    }

    private void Update()
    {
        if (activate)
        {
            if (!_triggered)
            {
                _triggered = true;
                _xDir = PlayerManager.instance.PlayerDirection.x;
            }

            GetComponent<SpriteRenderer>().enabled = true;

            rotationCenter.Rotate(0f, 0f, -_xDir * 45f);
        }
    }
}
