using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCannonEnemyController : MonoBehaviour
{
    [SerializeField] GameObject shot;
    [SerializeField] Transform spawnPoint;

    private Animator _normalCannonAnimator;
    private BoxCollider2D _normalCannonCollider;
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
        _normalCannonAnimator = GetComponent<Animator>();
        _normalCannonCollider = GetComponent<BoxCollider2D>();
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

    private void Update()
    {

        if (Vector2.Distance(transform.position, PlayerManager.instance.transform.position) < 10f)
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

                for (int i=0; i< _normalCannonAnimator.parameterCount; i++)
                {
                    _normalCannonAnimator.ResetTrigger(i);
                }

                if (__angleInDegrees >= 75f && __angleInDegrees < 105f)
                {
                    _normalCannonAnimator.SetTrigger("Up");
                    _zAngle = 0f;
                }
                else if (__angleInDegrees >= 105f && __angleInDegrees < 135f)
                {
                    _normalCannonAnimator.SetTrigger("UpLeft");
                    _zAngle = 30f;
                }
                else if (__angleInDegrees >= 135f && __angleInDegrees < 165f)
                {
                    _normalCannonAnimator.SetTrigger("LeftUp");
                    _zAngle = 60f;
                }
                else if ((__angleInDegrees >= 165f && __angleInDegrees <= 180f) || (__angleInDegrees >= -180f && __angleInDegrees < -165f))
                {
                    _normalCannonAnimator.SetTrigger("Left");
                    _zAngle = 90f;
                }
                else if (__angleInDegrees >= -165f && __angleInDegrees < -135f)
                {
                    _normalCannonAnimator.SetTrigger("LeftDown");
                    _zAngle = 120f;
                }
                else if (__angleInDegrees >= -135f && __angleInDegrees < -105f)
                {
                    _normalCannonAnimator.SetTrigger("DownLeft");
                    _zAngle = 150f;
                }
                else if (__angleInDegrees >= -105f && __angleInDegrees < -75f)
                {
                    _normalCannonAnimator.SetTrigger("Down");
                    _zAngle = 180f;
                }
                else if (__angleInDegrees >= -75f && __angleInDegrees < -45f)
                {
                    _normalCannonAnimator.SetTrigger("DownRight");
                    _zAngle = -150f;
                }
                else if (__angleInDegrees >= -45f && __angleInDegrees < -15f)
                {
                    _normalCannonAnimator.SetTrigger("RightDown");
                    _zAngle = -120f;
                }
                else if (__angleInDegrees >= -15f && __angleInDegrees < 15f)
                {
                    _normalCannonAnimator.SetTrigger("Right");
                    _zAngle = -90f;
                }
                else if (__angleInDegrees >= 15f && __angleInDegrees < 45f)
                {
                    _normalCannonAnimator.SetTrigger("RightUp");
                    _zAngle = -60f;
                }
                else if (__angleInDegrees >= 45f && __angleInDegrees < 75f)
                {
                    _normalCannonAnimator.SetTrigger("UpRight");
                    _zAngle = -30f;
                }
                if (!_startedShooting)
                {
                    _startedShooting = true;
                    _normalCannonAnimator.SetTrigger("Unlock");
                    StartCoroutine(StartShooting());
                }
            }
            else
            {
                _active = false;
                _normalCannonAnimator.SetTrigger("Explode");
                _normalCannonCollider.enabled = false;
                Destroy(gameObject, 1.1f);
            }
        }
        
    }

    private IEnumerator StartShooting()
    {
        while (_active && !PlayerManager.instance.PlayerDied)
        {
            yield return new WaitForSeconds(1f);

            if (!PlayerManager.instance.PlayerDied && life > 0f)
            {

                Instantiate(shot, spawnPoint.position, Quaternion.Euler(0, 0, _zAngle));
            }
        }
    }
}
