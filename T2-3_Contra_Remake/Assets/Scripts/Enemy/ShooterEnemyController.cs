using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemyController : MonoBehaviour
{
    [SerializeField] GameObject shot;
    [SerializeField] Transform spawnPoint;

    private Animator _animator;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _sprite;
    private Vector2 _direction;
    private bool _visible;
    private bool _active;
    private bool _startedShooting;
    private bool _destroyed;
    private float _zAngle;

    public bool hit;

    private void Start()
    {
        hit = false;
        _destroyed = false;
        _visible = false;
        _active = false;
        _startedShooting = false;
        _zAngle = 0f;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _direction = Vector2.left;
        shot.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;
    }

    private void OnBecameInvisible()
    {
        _visible = false;
    }

    private void OnBecameVisible()
    {
        _visible = true;
    }

    private void Flip()
    {
        if (_direction == Vector2.left)
        {
            _sprite.flipX = false;
            _collider.offset = new Vector2(0.15f, -0.15f);
        }
        else
        {
            _sprite.flipX = true;
            _collider.offset = new Vector2(-0.15f, -0.15f);
        }
    }

    private void Update()
    {

        if (Vector2.Distance(transform.position, PlayerManager.instance.transform.position) < 10f)
        {
            _active = true;
        }
        if (_active)
        {
            if (!hit)
            {
                Vector3 __playerPosition = new Vector3(PlayerManager.instance.transform.position.x, PlayerManager.instance.transform.position.y + 0.75f);
                Vector2 __difference = __playerPosition - transform.position;
                float __angle = Mathf.Atan2(__difference.y, __difference.x);
                float __angleInDegrees = __angle * Mathf.Rad2Deg;

                Flip();

                for (int i = 0; i < _animator.parameterCount; i++)
                {
                    _animator.ResetTrigger(i);
                }

                if (__angleInDegrees >= 75f && __angleInDegrees <= 90f)
                {
                    _direction = Vector2.left;
                    _animator.SetBool("AimingUp", true);
                    _zAngle = 0f;
                }
                else if (__angleInDegrees >= 105f && __angleInDegrees <= 135f)
                {
                    _direction = Vector2.left;
                    _animator.SetBool("AimingUp", true);
                    _zAngle = 30f;
                }
                else if (__angleInDegrees >= 135f && __angleInDegrees <= 165f)
                {
                    _direction = Vector2.left;
                    _animator.SetBool("AimingUp", false);
                    _zAngle = 60f;
                }
                else if ((__angleInDegrees >= 165f && __angleInDegrees <= 180f) || (__angleInDegrees >= -180f && __angleInDegrees <= -165f))
                {
                    _direction = Vector2.left;
                    _animator.SetBool("AimingUp", false);
                    _zAngle = 90f;
                }
                else if (__angleInDegrees >= -15f && __angleInDegrees <= 15f)
                {
                    _direction = Vector2.right;
                    _animator.SetBool("AimingUp", false);
                    _zAngle = -90f;
                }
                else if (__angleInDegrees >= 15f && __angleInDegrees <= 45f)
                {
                    _direction = Vector2.right;
                    _animator.SetBool("AimingUp", false);
                    _zAngle = -60f;
                }
                else if (__angleInDegrees >= 45f && __angleInDegrees <= 75f)
                {
                    _direction = Vector2.right;
                    _animator.SetBool("AimingUp", true);
                    _zAngle = -30f;
                }
                else if (__angleInDegrees >= 90f && __angleInDegrees <= 105f)
                {
                    _direction = Vector2.right;
                    _animator.SetBool("AimingUp", true);
                    _zAngle = 0f;
                }
                if (!_startedShooting)
                {
                    _startedShooting = true;
                    StartCoroutine(StartShooting());
                }
            }
            else if(!_destroyed)
            {
                _destroyed = true;
                _active = false;
                StartCoroutine(DeathAnimation());
            }
        }

    }

    private IEnumerator DeathAnimation()
    {
        _rigidbody2D.AddForce(new Vector2(0f, 5.5f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        _animator.SetTrigger("Explode");
        _collider.enabled = false;
        Destroy(gameObject, 0.4f);
    }

    private IEnumerator StartShooting()
    {
        while (_active)
        {
            yield return new WaitForSeconds(1f);

            Vector3 __shotPosition = spawnPoint.position;
            if(_direction == Vector2.right)
                __shotPosition = new Vector3(spawnPoint.position.x + (2* Mathf.Abs(spawnPoint.localPosition.x)), spawnPoint.position.y, spawnPoint.position.z);


            Instantiate(shot, __shotPosition, Quaternion.Euler(0, 0, _zAngle));
        }
    }
}
