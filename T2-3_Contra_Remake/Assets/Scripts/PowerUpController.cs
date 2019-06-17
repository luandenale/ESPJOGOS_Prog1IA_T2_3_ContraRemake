using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    [SerializeField] float shotSpeed = 4f;
    [SerializeField] Vector2 shotDirection = new Vector2(1f, 1f);

    public Weapon PowerUpType;

    private float _yInitialPos;
    private bool _capsuleDestroyed;
    private bool _autoDestroy;
    private Animator _powerUpAnimator;
    private Rigidbody2D _powerUpRigidbody;

    private void Awake()
    {
        _yInitialPos = transform.position.y;
        _capsuleDestroyed = false;
        _autoDestroy = false;
        _powerUpAnimator = GetComponent<Animator>();
        _powerUpRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(UpDownRoutine());
    }

    private void Update()
    {
        if (_autoDestroy)
            DestroyImmediate(gameObject);
        if (!_capsuleDestroyed)
        {
            transform.Translate(new Vector2(shotDirection.x,0f) * shotSpeed * Time.deltaTime);
            transform.Translate(new Vector2(0f, shotDirection.y) * (shotSpeed/3) * Time.deltaTime);
        }
    }

    private IEnumerator UpDownRoutine()
    {
        while (!_capsuleDestroyed)
        {
            if (transform.localPosition.y > _yInitialPos + 0.3f)
            {
                shotDirection = new Vector2(shotDirection.x, 0f);
                yield return new WaitForSeconds(0.1f);
                shotDirection = new Vector2(shotDirection.x, -1f);
            }
            else if (transform.localPosition.y < _yInitialPos - 0.3f)
            {
                shotDirection = new Vector2(shotDirection.x, 0f);
                yield return new WaitForSeconds(0.1f);
                shotDirection = new Vector2(shotDirection.x, 1f);
            }
            yield return null;
        }
        yield return null;
    }

    public void DropPowerUp(float p_upForce)
    {
        if (!_capsuleDestroyed)
        {
            _capsuleDestroyed = true;

            if (p_upForce == 4f)
                _powerUpAnimator.SetTrigger("Hit");
            else
                _powerUpAnimator.SetTrigger("SkipExplosionAnim");

            _powerUpAnimator.SetTrigger(PowerUpType.ToString());

            _powerUpRigidbody.bodyType = RigidbodyType2D.Dynamic;

            _powerUpRigidbody.velocity = new Vector2(0f, 0f);

            _powerUpRigidbody.AddForce(new Vector2(1f, 0f), ForceMode2D.Impulse);
            _powerUpRigidbody.AddForce(new Vector2(0f, p_upForce), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PowerUpType != Weapon.RAPID)
                collision.gameObject.GetComponent<PlayerManager>().CurrentWeapon = PowerUpType;
            else
                collision.gameObject.GetComponent<PlayerManager>().ShotSpeedModificator = 1.5f;
            _autoDestroy = true;
        }
    }
}
