using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public Vector2 shotDirection = new Vector2(0f, 0f);
    public float shotSpeed = 10f;
    public float shotDamage = 10f;
    public string shotType;

    [SerializeField] RotatingShot _rotatingShot;
    private SpriteRenderer[] _shotsSprites;
    private BoxCollider2D _boxCollider;
    private bool _hit;

    private void Awake()
    {
        _hit = false;
        _shotsSprites = GetComponentsInChildren<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(shotType == "Fire")
            _rotatingShot.activate = true;

        for(int i = 0; i < _shotsSprites.Length; i++)
        {
            if (_shotsSprites[i].name == shotType)
                _shotsSprites[i].enabled = true;
        }

        switch (shotType)
        {
            case "Regular":
                _boxCollider.size = new Vector2(0.1f, 0.1f);
                break;
            case "MachineGun":
                _boxCollider.size = new Vector2(0.15f, 0.15f);
                break;
            case "Spread":
                _boxCollider.size = new Vector2(0.25f, 0.25f);
                break;
            case "Fire":
                _boxCollider.enabled = false;
                break;
            case "Laser":
                _boxCollider.size = new Vector2(0.5f, 0.15f);
                break;
        }
        if (_hit)
        {
            GetComponent<Animator>().SetTrigger("Hit");
            _boxCollider.enabled = false;
            Destroy(gameObject, 0.25f);
        }else
            transform.Translate(shotDirection * shotSpeed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(shotType != "Fire")
        {
            if(collision.tag == "PowerUp")
            {
                _hit = true;
                AudioManager.instance.PlayPowerUpExplode();
                collision.GetComponent<PowerUpController>().DropPowerUp(4f);
            }
            else if(collision.tag == "StaticPowerUp")
            {
                _hit = true;
                if (collision.GetComponent<StaticPowerUp>().canExplode)
                {
                    AudioManager.instance.PlayPowerUpExplode();
                    collision.GetComponent<StaticPowerUp>().DropPowerUp();
                }
            }
            else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(collision.GetComponent<BossShot>() == null)
                    _hit = true;
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
                else if (collision.GetComponent<HiddenEnemyController>() != null)
                {
                    AudioManager.instance.PlayEnemyExplode();
                    collision.GetComponent<HiddenEnemyController>().hit = true;
                }
                else if (collision.GetComponent<NormalCannonEnemyController>() != null)
                {
                    AudioManager.instance.PlayHitCannon();
                    collision.GetComponent<NormalCannonEnemyController>().life -= shotDamage;
                    if (collision.GetComponent<NormalCannonEnemyController>().life <= 0)
                        AudioManager.instance.PlayEnemyExplode();
                }
                else if (collision.GetComponent<BigCannonEnemyController>() != null)
                {
                    AudioManager.instance.PlayHitCannon();
                    collision.GetComponent<BigCannonEnemyController>().life -= shotDamage;
                    if(collision.GetComponent<BigCannonEnemyController>().life <= 0)
                        AudioManager.instance.PlayEnemyExplode();
                }
                else if (collision.GetComponent<BossDoor>() != null)
                {
                    AudioManager.instance.PlayHitCannon();
                    collision.GetComponent<BossDoor>().life -= shotDamage;
                    if (collision.GetComponent<BossDoor>().life <= 0)
                        AudioManager.instance.PlayEnemyExplode();
                }
                else if (collision.GetComponent<BossCannons>() != null)
                {
                    AudioManager.instance.PlayHitCannon();
                    collision.GetComponent<BossCannons>().life -= shotDamage;
                }
                else if (collision.GetComponent<BossHiddenEnemy>() != null)
                {
                    AudioManager.instance.PlayEnemyExplode();
                    collision.GetComponent<BossHiddenEnemy>().hit = true;
                }
            }
        }
    }
}
