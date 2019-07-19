using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHiddenEnemy : MonoBehaviour
{
    [SerializeField] GameObject shot;
    [SerializeField] Transform spawnPoint;

    private Animator _animator;
    private BoxCollider2D _collider;
    private Rigidbody2D _rigidbody2D;

    private bool _startedShooting;
    private bool _destroyed;
    private float _zAngle;

    public bool hit;
    public bool active;

    private void Start()
    {
        hit = false;
        active = false;
        _destroyed = false;
        _startedShooting = false;
        _zAngle = 100f;
        _animator = GetComponent<Animator>();
        _collider = GetComponent<BoxCollider2D>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        shot.GetComponentsInChildren<SpriteRenderer>()[2].enabled = false;
    }


    private void Update()
    {
        if(PlayerManager.instance != null)
        {
            if (!PlayerManager.instance.PlayerDied && active)
            {
                _animator.SetTrigger("Start");
                if (!hit)
                {
                    Vector3 __playerPosition = new Vector3(PlayerManager.instance.transform.position.x, PlayerManager.instance.transform.position.y + 0.75f);
                    Vector2 __difference = __playerPosition - transform.position;
                    float __angle = Mathf.Atan2(__difference.y, __difference.x);
                    float __angleInDegrees = __angle * Mathf.Rad2Deg;

                    if (__angleInDegrees <= -170f && __angleInDegrees >= -180f)
                    {
                         _zAngle = 100f;
                    }
                    else if (__angleInDegrees <= -160f && __angleInDegrees >= -170f)
                    {
                        _zAngle = 115f;
                    }
                    else if (__angleInDegrees <= -140f && __angleInDegrees >= -160f)
                    {
                        _zAngle = 130f;
                    }
                }
                else if (!_destroyed)
                {
                    _destroyed = true;
                    StartCoroutine(DeathAnimation());
                }
            }

        }
    }

    private IEnumerator DeathAnimation()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        _rigidbody2D.AddForce(new Vector2(0f, 5.5f), ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        _animator.SetTrigger("Explode");
        _collider.enabled = false;
        Destroy(gameObject, 0.6f);
    }

    public void Shoot()
    {
        StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        if (!PlayerManager.instance.PlayerDied)
        {
            if(!_destroyed)
                Instantiate(shot, spawnPoint.position, Quaternion.Euler(0, 0, _zAngle));
            yield return new WaitForSeconds(0.15f);
            if (!_destroyed)
                Instantiate(shot, spawnPoint.position, Quaternion.Euler(0, 0, _zAngle));
            yield return new WaitForSeconds(0.15f);
            if (!_destroyed)
                Instantiate(shot, spawnPoint.position, Quaternion.Euler(0, 0, _zAngle));
        }
    }
}
