using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShot : MonoBehaviour
{
    public float xForce = 0f;

    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody;
    private Animator _animator;
    private bool _hit;
    private bool _shot;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _shot = false;
    }

    private void Update()
    {
        if(xForce > 0f && !_shot)
        {
            _shot = true;
            _rigidbody.AddForce(new Vector2(-xForce, 0.5f), ForceMode2D.Impulse);
        }
        if (_hit && _collider.enabled)
        {
            _collider.enabled = false;
            _rigidbody.velocity = new Vector2(0f, 0f);
            _rigidbody.bodyType = RigidbodyType2D.Static;
            _animator.SetTrigger("Explode");

            Destroy(gameObject,0.5f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer != LayerMask.NameToLayer("Shot"))
        {
            _hit = true;
            if(collider.tag == "Player")
                PlayerManager.instance.PlayerDied = true;
        }
    }
}
