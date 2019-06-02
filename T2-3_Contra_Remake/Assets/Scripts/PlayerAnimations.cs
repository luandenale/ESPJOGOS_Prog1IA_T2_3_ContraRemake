using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AnimTriggers
{
    public const string OnGround = "OnGround";
    public const string IsWalking = "IsWalking";

    public const string Jump = "Jump";
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
            _playerAnim.SetBool(AnimTriggers.OnGround, true);
            if (PlayerManager.instance.IsPlayerWalking)
                _playerAnim.SetBool(AnimTriggers.IsWalking, true);
            else
                _playerAnim.SetBool(AnimTriggers.IsWalking, false);

        }
        else
        {
            _playerAnim.SetBool(AnimTriggers.OnGround, false);
            if(PlayerManager.instance.PlayerJumped)
            {
                PlayerManager.instance.PlayerJumped = false;
                _playerAnim.SetTrigger(AnimTriggers.Jump);
            }
        }
    }
}
