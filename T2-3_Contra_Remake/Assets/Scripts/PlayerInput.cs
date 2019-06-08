using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput: MonoBehaviour
{
    [Range(2f,6f)]
    [SerializeField] float _walkSpeed;
    [SerializeField] GameObject _shot;

    private Rigidbody2D _playerRigidBody;
    private BoxCollider2D _playerCollider;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Horizontal movement
        if(Input.GetAxis("Horizontal") != 0)
        {
            float __walkingDelta = Input.GetAxisRaw("Horizontal") * _walkSpeed;
            _playerRigidBody.velocity = new Vector2(__walkingDelta, _playerRigidBody.velocity.y);

            PlayerManager.instance.PlayerDirection = new Vector2(__walkingDelta/_walkSpeed, PlayerManager.instance.PlayerDirection.y);
        }

        // Vertical Direction
        if (Input.GetAxis("Vertical") != 0)
            PlayerManager.instance.PlayerDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, Input.GetAxisRaw("Vertical"));
        else
            PlayerManager.instance.PlayerDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, 0f);

        // Jump Action
        if (Input.GetKeyDown(KeyCode.X) && PlayerManager.instance.IsPlayerTouchingGround)
        {
            // Check if its trying to jump down through floor
            if(PlayerManager.instance.PlayerDirection.y < 0)
            {
                PlayerManager.instance.PlayerJumpingDown = true;
            }
            // Regular jump
            else
            {
                PlayerManager.instance.PlayerJumped = true;
                _playerRigidBody.AddForce(new Vector3(0f, 6f, 0f), ForceMode2D.Impulse);
                PlayerManager.instance.IsPlayerTouchingGround = false;
            }
        }

        // Shooting Action
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameObject __firedShot = Instantiate(_shot, gameObject.transform.position, Quaternion.identity);
            __firedShot.GetComponent<ShotController>()._shotSpeed = 10f;
            __firedShot.GetComponent<ShotController>().shotDirection = PlayerManager.instance.PlayerDirection;
            PlayerManager.instance.IsPlayerShooting = true;
        }

    }

}
