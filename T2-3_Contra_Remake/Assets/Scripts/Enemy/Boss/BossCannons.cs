using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCannons : MonoBehaviour
{
    [SerializeField] GameObject _bossShot;

    private Animator _animator;
    private bool _firstDestroyed = false;
    private bool _bothDestroyed = false;

    public bool startedShooting = false;
    public float life = 320;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (startedShooting)
        {
            _animator.SetTrigger("BothShooting");
        }

        if (life <= 160f)
        {
            if(!_firstDestroyed)
                AudioManager.instance.PlayEnemyExplode();
            _firstDestroyed = true;
            _animator.SetTrigger("DestroyFirst");
        }
        if (life <= 0f && !_bothDestroyed)
        {
            _bothDestroyed = true;
            if (_firstDestroyed)
                _animator.SetTrigger("DestroySecond");
            else
                _animator.SetTrigger("DestroyBoth");
            AudioManager.instance.PlayEnemyExplode();
        }
    }

    public void FirstShoot()
    {
        GameObject __shot = Instantiate(_bossShot, new Vector3(transform.position.x - 0.1f, transform.position.y, 0f), Quaternion.identity);
        __shot.GetComponent<BossShot>().xForce = Random.Range(1f, 4f);
    }

    public void SecondShoot()
    {
        GameObject __shot = Instantiate(_bossShot, new Vector3(transform.position.x - 1.1f, transform.position.y, 0f), Quaternion.identity);
        __shot.GetComponent<BossShot>().xForce = Random.Range(1f, 4f);
    }
}
