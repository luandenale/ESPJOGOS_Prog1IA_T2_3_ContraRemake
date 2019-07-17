using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCannonEnemyController : MonoBehaviour
{
    [SerializeField] GameObject shot;
    [SerializeField] Transform spawnPoint;

    private Animator _cannonAnimator;
    private BoxCollider2D _cannonCollider;
    private bool _visible;
    private bool _active;
    private bool _startedShooting;
    private float _zAngle;

    public float life;

    private void Start()
    {
        life = 100f;
        _visible = false;
        _active = false;
        _startedShooting = false;
        _zAngle = 0f;
        _cannonAnimator = GetComponent<Animator>();
        _cannonCollider = GetComponent<BoxCollider2D>();
        shot.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
    }

    private void OnBecameInvisible()
    {
        _visible = false;
        _active = false;
    }

    private void OnBecameVisible()
    {
        _visible = true;
    }

    private void Update()
    {

        if (_visible)
        {
            _active = true;
        }
        if (_active && !PlayerManager.instance.PlayerDied)
        {
            if (life > 0)
            {
                Vector3 __playerPosition = new Vector3(PlayerManager.instance.transform.position.x, PlayerManager.instance.transform.position.y + 0.75f);
                Vector2 __difference = __playerPosition - transform.position;
                float __angle = Mathf.Atan2(__difference.y, __difference.x);
                float __angleInDegrees = __angle * Mathf.Rad2Deg;

                for (int i = 0; i < _cannonAnimator.parameterCount; i++)
                {
                    _cannonAnimator.ResetTrigger(i);
                }

                if (__angleInDegrees >= 105f && __angleInDegrees <= 135f)
                {
                    if(_zAngle != 30f)
                    {
                        _cannonAnimator.SetTrigger("DiagUp");
                        _zAngle = 30f;
                    }
                }
                else if (__angleInDegrees >= 135f && __angleInDegrees <= 165f)
                {
                    if (_zAngle != 60f)
                    {
                        _cannonAnimator.SetTrigger("DiagStraight");
                        _zAngle = 60f;
                    }
                }
                else if ((__angleInDegrees >= 165f && __angleInDegrees <= 180f) || (__angleInDegrees >= -180f && __angleInDegrees <= -165f))
                {
                    if (_zAngle != 90f)
                    {
                        _cannonAnimator.SetTrigger("Straight");
                        _zAngle = 90f;
                    }
                }
                if (!_startedShooting)
                {
                    _startedShooting = true;
                    _cannonAnimator.SetTrigger("Rise");
                    StartCoroutine(StartShooting());
                }
            }
            else
            {
                _active = false;
                _cannonAnimator.SetBool("Unlocked", false);
                _cannonAnimator.SetTrigger("Explode");
                _cannonCollider.enabled = false;
                Destroy(gameObject, 1.1f);
            }
        }

    }

    private IEnumerator StartShooting()
    {
        while (_active)
        {
            yield return new WaitForSeconds(1.5f);

            if (!PlayerManager.instance.PlayerDied && life > 0f)
            {
                Instantiate(shot, spawnPoint.position, Quaternion.Euler(0, 0, _zAngle));
                yield return new WaitForSeconds(0.2f);
                Instantiate(shot, spawnPoint.position, Quaternion.Euler(0, 0, _zAngle));
                yield return new WaitForSeconds(0.2f);
                Instantiate(shot, spawnPoint.position, Quaternion.Euler(0, 0, _zAngle));
            }
        }
    }

    public void SetUnlocked()
    {
        _cannonAnimator.SetBool("Unlocked", true);
    }
}
