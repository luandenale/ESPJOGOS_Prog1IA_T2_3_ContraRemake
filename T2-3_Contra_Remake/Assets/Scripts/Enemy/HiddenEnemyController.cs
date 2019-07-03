using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenEnemyController : MonoBehaviour
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
    public bool shoot;

    private void Start()
    {
        hit = false;
        _destroyed = false;
        _visible = false;
        _active = false;
        _startedShooting = false;
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
            _zAngle = 90f;
            _sprite.flipX = false;
            _collider.offset = new Vector2(-0.23f, -0.15f);
        }
        else
        {
            _zAngle = -90f;
            _sprite.flipX = true;
            _collider.offset = new Vector2(0.23f, -0.15f);
        }
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, PlayerManager.instance.transform.position) < 10f)
        {
            _active = true;
        }else
            _active = false;
        if (_active && !PlayerManager.instance.PlayerDied)
        {
            if (!hit)
            {
                Vector3 __playerPosition = new Vector3(PlayerManager.instance.transform.position.x, PlayerManager.instance.transform.position.y + 0.75f);
                Vector2 __difference = __playerPosition - transform.position;

                Flip();

                if (PlayerManager.instance.transform.position.x > transform.position.x)
                    _direction = Vector2.right;
                else
                    _direction = Vector2.left;

                if (!_startedShooting)
                {
                    _startedShooting = true;
                    StartCoroutine(StartShooting());
                }
            }
            else if (!_destroyed)
            {
                _destroyed = true;
                _active = false;
                StartCoroutine(DeathAnimation());
            }
        }

    }

    private IEnumerator DeathAnimation()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody2D.AddForce(new Vector2(0f, 5.5f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        _animator.SetTrigger("Death");
        _collider.enabled = false;
        Destroy(gameObject, 0.6f);
    }

    private IEnumerator StartShooting()
    {
        _animator.SetTrigger("Shooting");
        while (_active && !PlayerManager.instance.PlayerDied)
        {
            if (shoot)
            {
                shoot = false;
                Vector3 __shotPosition = spawnPoint.position;
                if (_direction == Vector2.right)
                    __shotPosition = new Vector3(spawnPoint.position.x + 1.15f, spawnPoint.position.y, spawnPoint.position.z);

                Instantiate(shot, __shotPosition, Quaternion.Euler(0, 0, _zAngle));
            }
            yield return null;
        }
    }
}
