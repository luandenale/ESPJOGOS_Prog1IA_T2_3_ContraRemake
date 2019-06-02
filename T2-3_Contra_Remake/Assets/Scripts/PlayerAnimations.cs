using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AnimTriggers
{
    public const string Jump = "Jump";

    public const string OnGround = "OnGround";
    public const string IsShooting = "IsShooting";
    public const string IsWalking = "IsWalking";
    public const string IsAimingUp = "IsAimingUp";
    public const string IsAimingDown = "IsAimingDown";

}

public class PlayerAnimations : MonoBehaviour
{
    private Animator _playerAnim;
    

    private void Awake()
    {
        _playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        // Player is touching the ground
        if (PlayerManager.instance.IsPlayerTouchingGround)
        {
            OnTheGroundAnimations();
        }
        // Player on the air
        else
        {
            OnTheAirAnimation();
        }

        // Shooting Animation
        if (PlayerManager.instance.IsPlayerShooting)
        {
            _playerAnim.SetBool(AnimTriggers.IsShooting, true);
            PlayerManager.instance.IsPlayerShooting = false;
        }
        else
            _playerAnim.SetBool(AnimTriggers.IsShooting, false);
    }

    private void OnTheGroundAnimations()
    {
        _playerAnim.SetBool(AnimTriggers.OnGround, true);

        // Set aim up and down triggers
        if (PlayerManager.instance.PlayerDirection.y > 0 && !PlayerManager.instance.IsPlayerWalking)
            _playerAnim.SetBool(AnimTriggers.IsAimingUp, true);
        else if (PlayerManager.instance.PlayerDirection.y < 0 && !PlayerManager.instance.IsPlayerWalking)
            _playerAnim.SetBool(AnimTriggers.IsAimingDown, true);
        else
        {
            _playerAnim.SetBool(AnimTriggers.IsAimingUp, false);
            _playerAnim.SetBool(AnimTriggers.IsAimingDown, false);
        }

        // Set walking anim
        if (PlayerManager.instance.IsPlayerWalking)
            _playerAnim.SetBool(AnimTriggers.IsWalking, true);
        else
            _playerAnim.SetBool(AnimTriggers.IsWalking, false);
    }

    private void OnTheAirAnimation()
    {
        _playerAnim.SetBool(AnimTriggers.OnGround, false);
        if (PlayerManager.instance.PlayerJumped)
        {
            PlayerManager.instance.PlayerJumped = false;
            _playerAnim.SetTrigger(AnimTriggers.Jump);
        }
    }
}
