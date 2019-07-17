using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemyController : MonoBehaviour
{
    [SerializeField] RunnerEnemyFeet feet;
    public float playerXPosTrigger;
    public bool triggeredBySpawner;

    public bool hit;

    private bool _destroyed;
    private bool _active;
    private Rigidbody2D _rigidbody2D;
    private BoxCollider2D _collider2D;
    private Animator _animator;

    private void Awake()
    {
        triggeredBySpawner = false;
        hit = false;
        _destroyed = false;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gameObject.transform.position = new Vector3(playerXPosTrigger + 7.5f, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if ((PlayerManager.instance.transform.position.x > playerXPosTrigger && !_destroyed) || triggeredBySpawner)
            _active = true;

        if (!hit)
        {
            if(_active)
                transform.Translate(new Vector2(-2.5f, 0f) * Time.deltaTime);
        }
        else
        {
            if (!_destroyed)
            {
                _destroyed = true;
                _active = false;
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && !feet.IsTouchingGround && !_destroyed)
        {
            _rigidbody2D.AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
            _animator.SetBool("Jump", true);
        }else
            _animator.SetBool("Jump", false);
    }
}
