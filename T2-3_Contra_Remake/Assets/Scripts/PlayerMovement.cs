using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(2f,6f)]
    [SerializeField] float _walkSpeed;

    private Rigidbody2D _playerRigidBody;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Horizontal movement
        if(Input.GetAxis("Horizontal") != 0)
        {
            float __walkingDelta = Input.GetAxisRaw("Horizontal") * _walkSpeed;

            _playerRigidBody.velocity = new Vector2(__walkingDelta, _playerRigidBody.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.X) && PlayerManager.instance.IsPlayerTouchingGround)
        {
            PlayerManager.instance.PlayerJumped = true;
            _playerRigidBody.AddForce(new Vector3(0f, 6f, 0f), ForceMode2D.Impulse);
            PlayerManager.instance.IsPlayerTouchingGround = false;
        }

    }

}
