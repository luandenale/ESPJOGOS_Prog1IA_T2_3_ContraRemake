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
    [SerializeField] SpriteRenderer[] Sprites;

    private Animator _playerAnim;

    private bool _coolingDown = false;
    private Coroutine _coolingDownStraightRoutineReference = null;

    private void Awake()
    {
        _playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        FlipSprites();

        // Player is touching the ground
        if (PlayerManager.instance.IsPlayerTouchingGround)
        {
            OnTheGroundAnimations();
        }
        // Player on the air
        else
        {
            ResetBackSprites();
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

    private void FlipSprites()
    {
        // Set Sprites Dir
        if (PlayerManager.instance.PlayerDirection.x > 0)
            for (int i = 0; i < Sprites.Length; i++)
                Sprites[i].flipX = false;
        else if (PlayerManager.instance.PlayerDirection.x < 0)
            for (int i = 0; i < Sprites.Length; i++)
                Sprites[i].flipX = true;
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
        {
            SwitchRunningSprites();
            _playerAnim.SetBool(AnimTriggers.IsWalking, true);
        }
        else
        {
            ResetBackSprites();
            _playerAnim.SetBool(AnimTriggers.IsWalking, false);
        }
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

    private void SwitchRunningSprites()
    {
        
        // Player is aiming up
        if(PlayerManager.instance.PlayerDirection.y > 0)
        {
            
            if (PlayerManager.instance.IsPlayerShooting)
            {

            }
            else
            {
                Sprites[3].enabled = true;
                // Disable all other sprites
                for (int i = 0; i < Sprites.Length; i++)
                    if (i != 3)
                        Sprites[i].enabled = false;
            }
        }
        // Player is aiming down
        else if(PlayerManager.instance.PlayerDirection.y < 0)
        {
            if (PlayerManager.instance.IsPlayerShooting)
            {

            }
            else
            {
                Sprites[5].enabled = true;
                // Disable all other sprites
                for (int i = 0; i < Sprites.Length; i++)
                    if (i != 5)
                        Sprites[i].enabled = false;
            }
        }
        // Running straight ahead
        else
        {
            if (PlayerManager.instance.IsPlayerShooting)
            {
                _coolingDown = true;

                if (_coolingDownStraightRoutineReference!=null)
                    StopCoroutine(_coolingDownStraightRoutineReference);
                _coolingDownStraightRoutineReference = StartCoroutine(CoolingDownStraightRoutine());

                StartCoroutine(ShootStraight());
            }
            else if(!_coolingDown)
            {
                Sprites[0].enabled = true;
                // Disable all other sprites
                for (int i = 1; i < Sprites.Length; i++)
                    Sprites[i].enabled = false;

            }
        }
    }

    private void ResetBackSprites()
    {
        Sprites[0].enabled = true;
        // Disable all other sprites
        for (int i = 1; i < Sprites.Length; i++)
            Sprites[i].enabled = false;
    }

    private IEnumerator CoolingDownStraightRoutine()
    {

        Sprites[1].enabled = true;
        // Disable all other sprites
        for (int i = 0; i < Sprites.Length; i++)
            if (i != 1)
                Sprites[i].enabled = false;

        yield return new WaitForSeconds(1.5f);
         _coolingDown = false;
    }

    private IEnumerator ShootStraight()
    {
        Sprites[2].enabled = true;
        // Disable all other sprites
        for (int i = 0; i < Sprites.Length; i++)
            if (i != 2)
                Sprites[i].enabled = false;

        yield return new WaitForSeconds(0.6f);

        Sprites[1].enabled = true;
        // Disable all other sprites
        for (int i = 0; i < Sprites.Length; i++)
            if (i != 1)
                Sprites[i].enabled = false;
    }
}
