using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemyController : MonoBehaviour
{
    public bool hit;
    private bool _destroyed;
    private bool _visible;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _collider2D;
    private Animator _animator;


    private void Start()
    {
        hit = false;
        _destroyed = false;
        _visible = false;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void OnBecameInvisible()
    {
        _visible = false;
    }

    private void OnBecameVisible()
    {
        _visible = true;
    }

    private void Update()
    {
        if (!hit)
        {
            if(_visible)
                transform.Translate(new Vector2(-3.5f, 0f) * Time.deltaTime);
        }
        else
        {
            if (!_destroyed)
            {
                _destroyed = true;
                _rigidbody2D.AddForce(new Vector2(0f, 5.5f), ForceMode2D.Impulse);
                _collider2D.enabled = false;

                _animator.SetBool("Jump", true);
                _animator.SetBool("Die", true);

                Destroy(gameObject, 1.5f);
            }
            else
            {
                if (_rigidbody2D.velocity.y <= 0)
                    _rigidbody2D.bodyType = RigidbodyType2D.Static;
            }
        }
    }
}
