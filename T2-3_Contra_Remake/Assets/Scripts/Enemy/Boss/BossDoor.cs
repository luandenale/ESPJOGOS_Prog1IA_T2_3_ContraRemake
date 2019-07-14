using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private Animator _animator;
    private BoxCollider2D _collider;
    public float life = 320f;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (life <= 0)
        {
            _collider.enabled = false;
            _animator.SetTrigger("Explode");
        }
    }
}
