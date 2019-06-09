using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPositions
{
    public Vector3 RegularPosition = new Vector3(0.55f, 0.75f, 0f);
    public Vector3 CrouchPosition = new Vector3(0.55f, 0.32f, 0f);
    public Vector3 StraightUpPosition = new Vector3(0.175f, 1.4f, 0f);
    public Vector3 JumpingPosition = new Vector3(0.05f, 0.75f, 0f);
    public Vector3 DiagonalUpPosition = new Vector3(0.35f, 1.13f, 0f);
    public Vector3 DiagonalDownPosition = new Vector3(0.4f, 0.5f, 0f);
}

public class PlayerInput: MonoBehaviour
{
    [Range(2f,6f)]
    [SerializeField] float _walkSpeed;
    [SerializeField] GameObject _shot;
    [SerializeField] Transform ShotSpawnPoint;

    private Rigidbody2D _playerRigidBody;
    private BoxCollider2D _playerCollider;
    private SpawnPointPositions _spawnPositions = new SpawnPointPositions();
    private bool _gunCooledDown = true;

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

            if(__walkingDelta != 0f)
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
        if(Input.GetKey(KeyCode.Z) && PlayerManager.instance.CurrentWeapon == Weapon.MACHINEGUN && _gunCooledDown)
        {
            StartCoroutine(GunCoolingDownRoutine());

            ShotSpawnPoint.localPosition = SetSpawnPoint();

            InstantiateShot();

            PlayerManager.instance.IsPlayerShooting = true;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && _gunCooledDown)
        {
            StartCoroutine(GunCoolingDownRoutine());

            ShotSpawnPoint.localPosition = SetSpawnPoint();

            InstantiateShot();

            PlayerManager.instance.IsPlayerShooting = true;
        }

    }

    private IEnumerator GunCoolingDownRoutine()
    {
        _gunCooledDown = false;

        switch (PlayerManager.instance.CurrentWeapon)
        {
            case Weapon.MACHINEGUN:
                yield return new WaitForSeconds(0.1f);
                break;
            case Weapon.SPREAD:
                yield return new WaitForSeconds(0.3f);
                break;
            default:
                yield return new WaitForSeconds(0.2f);
                break;
        }

        _gunCooledDown = true;
    }

    private void InstantiateShot()
    {
        // Spread Shot is treated separately as it spwans 6 shots
        if (PlayerManager.instance.CurrentWeapon == Weapon.SPREAD)
        {
            float __yDir = -0.2f;
            for (int i = 0; i < 5; i++)
            {
                GameObject __spreadShot = Instantiate(_shot, ShotSpawnPoint.position, Quaternion.identity);

                __spreadShot.GetComponent<ShotController>().shotSpeed = 10f;
                __spreadShot.GetComponent<ShotController>().shotType = "Spread";
                __spreadShot.GetComponent<ShotController>().shotDamage = 10f;

                if (PlayerManager.instance.IsPlayerWalking)
                    __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, PlayerManager.instance.PlayerDirection.y + __yDir);
                else if (PlayerManager.instance.PlayerDirection.y == 1f)
                    __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(0f, 1f + __yDir);
                else if (PlayerManager.instance.PlayerDirection.y == -1f)
                    if (PlayerManager.instance.IsPlayerTouchingGround)
                        __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, __yDir);
                    else
                        __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(0f, -1f + __yDir);
                else
                    __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, PlayerManager.instance.PlayerDirection.y + __yDir);

                __yDir += 0.1f;
            }
        }
        else
        {
            GameObject __firedShot = Instantiate(_shot, ShotSpawnPoint.position, Quaternion.identity);

            __firedShot.GetComponent<ShotController>().shotSpeed = 10f;

            if (PlayerManager.instance.CurrentWeapon == Weapon.REGULAR)
            {
                __firedShot.GetComponent<ShotController>().shotType = "Regular";
                __firedShot.GetComponent<ShotController>().shotDamage = 10f;
            }
            else if (PlayerManager.instance.CurrentWeapon == Weapon.MACHINEGUN)
            {
                __firedShot.GetComponent<ShotController>().shotType = "MachineGun";
                __firedShot.GetComponent<ShotController>().shotDamage = 10f;
            }

            if (PlayerManager.instance.IsPlayerWalking)
                __firedShot.GetComponent<ShotController>().shotDirection = PlayerManager.instance.PlayerDirection;
            else if (PlayerManager.instance.PlayerDirection.y == 1f)
                __firedShot.GetComponent<ShotController>().shotDirection = new Vector2(0f, 1f);
            else if (PlayerManager.instance.PlayerDirection.y == -1f)
                if (PlayerManager.instance.IsPlayerTouchingGround)
                    __firedShot.GetComponent<ShotController>().shotDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, 0f);
                else
                    __firedShot.GetComponent<ShotController>().shotDirection = new Vector2(0f, -1f);
            else
                __firedShot.GetComponent<ShotController>().shotDirection = PlayerManager.instance.PlayerDirection;
        }

    }

    private Vector3 SetSpawnPoint()
    {
        Vector3 __spawnPoint = new Vector3(0, 0, 0);

        // Shooting regular
        if (!PlayerManager.instance.IsAimingDown && !PlayerManager.instance.IsAimingUp)
            __spawnPoint = _spawnPositions.RegularPosition;
        // Shooting crouched
        if (PlayerManager.instance.PlayerDirection.y == -1f && !PlayerManager.instance.IsPlayerWalking)
        {
            __spawnPoint = _spawnPositions.CrouchPosition;
        }
        // Shooting Up
        if (PlayerManager.instance.PlayerDirection.y == 1f && !PlayerManager.instance.IsPlayerWalking)
            __spawnPoint = _spawnPositions.StraightUpPosition;
        // Shooting MidAir
        if (!PlayerManager.instance.IsPlayerTouchingGround)
            __spawnPoint = _spawnPositions.JumpingPosition;
        if (PlayerManager.instance.IsPlayerWalking)
        {
            if (PlayerManager.instance.PlayerDirection.y == 1f)
                __spawnPoint = _spawnPositions.DiagonalUpPosition;
            else if (PlayerManager.instance.PlayerDirection.y == -1f)
                __spawnPoint = _spawnPositions.DiagonalDownPosition;
        }

        if (PlayerManager.instance.PlayerDirection.x < 0)
            __spawnPoint.x = -__spawnPoint.x;

        return __spawnPoint;
    }

}
