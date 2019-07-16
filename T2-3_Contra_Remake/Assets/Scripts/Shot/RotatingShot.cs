using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShot : MonoBehaviour
{
    public bool activate;

    private bool _triggered;
    private float _xDir;
    private BoxCollider2D _boxCollider;

    [SerializeField] Transform rotationCenter;

    private void Awake()
    {
        activate = false;
        _triggered = false;
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (activate)
        {
            _boxCollider.enabled = true;

            if (!_triggered)
            {
                _triggered = true;
                _xDir = PlayerManager.instance.PlayerDirection.x;
            }

            GetComponent<SpriteRenderer>().enabled = true;

            rotationCenter.Rotate(0f, 0f, -_xDir * 45f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PowerUp")
        {
            AudioManager.instance.PlayPowerUpExplode();
            collision.GetComponent<PowerUpController>().DropPowerUp(6f);
        }
        else if (collision.tag == "StaticPowerUp")
        {
            if (collision.GetComponent<StaticPowerUp>().canExplode)
            {
                AudioManager.instance.PlayPowerUpExplode();
                collision.GetComponent<StaticPowerUp>().DropPowerUp();
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (collision.GetComponent<RunnerEnemyController>() != null)
            {
                AudioManager.instance.PlayEnemyExplode();
                collision.GetComponent<RunnerEnemyController>().hit = true;
            }
            else if (collision.GetComponent<ShooterEnemyController>() != null)
            {
                AudioManager.instance.PlayEnemyExplode();
                collision.GetComponent<ShooterEnemyController>().hit = true;
            }
            else if (collision.GetComponent<NormalCannonEnemyController>() != null)
            {
                AudioManager.instance.PlayHitCannon();
                collision.GetComponent<NormalCannonEnemyController>().life -= 12.5f;
                if (collision.GetComponent<NormalCannonEnemyController>().life <= 0)
                    AudioManager.instance.PlayEnemyExplode();
            }
            else if (collision.GetComponent<BigCannonEnemyController>() != null)
            {
                AudioManager.instance.PlayHitCannon();
                collision.GetComponent<BigCannonEnemyController>().life -= 12.5f;
                if (collision.GetComponent<BigCannonEnemyController>().life <= 0)
                    AudioManager.instance.PlayEnemyExplode();
            }
            else if (collision.GetComponent<BossDoor>() != null)
            {
                AudioManager.instance.PlayHitCannon();
                collision.GetComponent<BossDoor>().life -= 12.5f;
                if (collision.GetComponent<BossDoor>().life <= 0)
                    AudioManager.instance.PlayEnemyExplode();
            }
            else if (collision.GetComponent<BossCannons>() != null)
            {
                AudioManager.instance.PlayHitCannon();
                collision.GetComponent<BossCannons>().life -= 12.5f;
            }
            else if (collision.GetComponent<BossHiddenEnemy>() != null)
            {
                AudioManager.instance.PlayEnemyExplode();
                collision.GetComponent<BossHiddenEnemy>().hit = true;
            }
        }
    }
}

