using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    REGULAR,
    MACHINEGUN,
    SPREAD
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance = null;

    public Weapon CurrentWeapon;
    public Vector2 PlayerDirection;
    public bool PlayerJumped = false;
    public bool PlayerJumpingDown = false;
    public bool IsPlayerTouchingGround;
    public bool IsPlayerShooting;
    public bool IsPlayerWalking { get; private set; }
    public bool IsAimingUp { get; private set; }
    public bool IsAimingDown { get; private set; }

    private Rigidbody2D _playerRigidBody;
    private SpriteRenderer _playerSprite;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);

        _playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_playerRigidBody.velocity.y != 0)
        {
            IsPlayerTouchingGround = false;
        }

        if (_playerRigidBody.velocity.x != 0)
            IsPlayerWalking = true;
        else
            IsPlayerWalking = false;
    }

    private void OnCollisionStay2D(Collision2D p_collision)
    {
        if (p_collision.gameObject.tag == "Ground")
        {
            // Check if its firmly on the floor
            if(_playerRigidBody.velocity.y == 0)
                IsPlayerTouchingGround = true;

            // Check to jump down through
            if (PlayerJumpingDown)
            {
                BoxCollider2D __platformCollider = p_collision.gameObject.GetComponent<BoxCollider2D>();

                PlayerJumpingDown = false;
                __platformCollider.isTrigger = true;
                _playerRigidBody.AddForce(new Vector3(0f, -1f, 0f), ForceMode2D.Impulse);
                StartCoroutine(DisableGroundPeriodically(__platformCollider));
            }
        }
    }

    private IEnumerator DisableGroundPeriodically(BoxCollider2D p_collider)
    {
        yield return new WaitForSeconds(0.5f);
        p_collider.isTrigger = false;
    }
}
