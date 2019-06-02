using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] Sprites;

    public static PlayerManager instance = null;

    public bool IsPlayerTouchingGround;
    public bool PlayerJumped = false;
    public Vector2 PlayerDirection { get; private set; }
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
        SetPlayerDirection();

        if (_playerRigidBody.velocity.x != 0)
            IsPlayerWalking = true;
        else
            IsPlayerWalking = false;

        // Set Sprites Dir
        if(PlayerDirection.x > 0)
            for (int i = 0; i < Sprites.Length; i++)
                 Sprites[i].flipX = false;
        else if (PlayerDirection.x < 0)
            for (int i = 0; i < Sprites.Length; i++)
                Sprites[i].flipX = true;
    }

    private void SetPlayerDirection()
    {
        if (_playerRigidBody.velocity.x > 0)
            PlayerDirection = new Vector2(1f, PlayerDirection.y);
        else if(_playerRigidBody.velocity.x < 0)
            PlayerDirection = new Vector2(-1f, PlayerDirection.y);

        if (_playerRigidBody.velocity.y > 0)
            PlayerDirection = new Vector2(PlayerDirection.x, 1f);
        else if (_playerRigidBody.velocity.y < 0)
        {
            PlayerDirection = new Vector2(PlayerDirection.x, -1f);
            IsPlayerTouchingGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            IsPlayerTouchingGround = true;
    }
}
