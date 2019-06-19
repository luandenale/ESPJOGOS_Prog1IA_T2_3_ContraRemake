using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShot : MonoBehaviour
{
    public bool activate;

    private bool _triggered;
    private float _xDir;
    private BoxCollider2D _boxCollider;

    [SerializeField] Transform rotationCenter;

    private void Awake()
    {
        activate = false;
        _triggered = false;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (activate)
        {
            _boxCollider.enabled = true;

            if (!_triggered)
            {
                _triggered = true;
                _xDir = PlayerManager.instance.PlayerDirection.x;
            }

            GetComponent<SpriteRenderer>().enabled = true;

            rotationCenter.Rotate(0f, 0f, -_xDir * 45f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PowerUp")
            collision.GetComponent<PowerUpController>().DropPowerUp(4f);
        else if (collision.tag == "StaticPowerUp")
            collision.GetComponent<StaticPowerUp>().DropPowerUp();
    }
}
