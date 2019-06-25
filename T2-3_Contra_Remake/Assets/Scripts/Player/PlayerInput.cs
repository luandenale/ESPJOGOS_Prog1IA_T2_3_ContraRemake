using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointPositions
{
    public Vector3 RegularPosition = new Vector3(0.7f, 0.975f, 0f);
    public Vector3 CrouchPosition = new Vector3(0.7f, 0.4f, 0f);
    public Vector3 StraightUpPosition = new Vector3(0.245f, 1.8f, 0f);
    public Vector3 JumpingPosition = new Vector3(0f, 0.9f, 0f);
    public Vector3 DiagonalUpPosition = new Vector3(0.4f, 1.45f, 0f);
    public Vector3 DiagonalDownPosition = new Vector3(0.5f, 0.7f, 0f);

    public Vector3 WaterRegularPosition = new Vector3(0.7f, 0.4f, 0f);
    public Vector3 WaterDiagonalPosition = new Vector3(0.45f, 0.92f, 0f);
    public Vector3 WaterUpPosition = new Vector3(0.235f, 1.31f, 0f);
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
    private bool _triggeredDeath = false;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _playerCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!PlayerManager.instance.PlayerDied)
        {
            // Horizontal movement
            if(Input.GetAxis("Horizontal") != 0 && !PlayerManager.instance.IsPlayerGettingOutOfWater)
            {
                float __walkingDelta = Input.GetAxisRaw("Horizontal") * _walkSpeed;
                _playerRigidBody.velocity = new Vector2(__walkingDelta, _playerRigidBody.velocity.y);

                if(__walkingDelta != 0f)
                    PlayerManager.instance.PlayerDirection = new Vector2(__walkingDelta/_walkSpeed, PlayerManager.instance.PlayerDirection.y);
            }

            // Vertical Direction
            if (Input.GetAxis("Vertical") != 0 && !PlayerManager.instance.IsPlayerGettingOutOfWater)
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
                    _playerRigidBody.AddForce(new Vector3(0f, 7f, 0f), ForceMode2D.Impulse);
                    PlayerManager.instance.IsPlayerTouchingGround = false;
                }
            }

            // Shooting Action
            if(Input.GetKey(KeyCode.Z) && PlayerManager.instance.CurrentWeapon == Weapon.MACHINEGUN && _gunCooledDown && 
                !(PlayerManager.instance.IsPlayerTouchingWater && PlayerManager.instance.PlayerDirection.y == -1f))
            {
                StartCoroutine(GunCoolingDownRoutine());

                ShotSpawnPoint.localPosition = SetSpawnPoint();

                InstantiateShot();

                PlayerManager.instance.IsPlayerShooting = true;
            }
            else if (Input.GetKeyDown(KeyCode.Z) && _gunCooledDown &&
                !(PlayerManager.instance.IsPlayerTouchingWater && PlayerManager.instance.PlayerDirection.y == -1f))
            {
                StartCoroutine(GunCoolingDownRoutine());

                ShotSpawnPoint.localPosition = SetSpawnPoint();

                if (PlayerManager.instance.CurrentWeapon == Weapon.LASER)
                    StartCoroutine(IntantiateLaserShot());
                else
                    InstantiateShot();

                PlayerManager.instance.IsPlayerShooting = true;
            }
        }
        else
        {
            if(!_triggeredDeath)
            {
                _triggeredDeath = true;
                _playerRigidBody.velocity = new Vector2(0f, 0f);
                _playerRigidBody.AddForce(new Vector2(-PlayerManager.instance.PlayerDirection.x * 2f, 4.5f), ForceMode2D.Impulse);
            }
        }

    }

    private IEnumerator IntantiateLaserShot()
    {
        AudioManager.instance.PlayLaserShot();
        Vector3 __originalPosition = ShotSpawnPoint.position;
        Quaternion __originalRotation;
        Vector2 __originalDirection;

        // Aiming up
        if (PlayerManager.instance.PlayerDirection.y == 1f)
        {
            // Straight Up
            if (!PlayerManager.instance.IsPlayerWalking)
            {
                __originalRotation = Quaternion.Euler(0, 0, -90);
                __originalDirection = new Vector2(-1f, 0f);
            }
            // Diagonal
            else
            {
                // Left
                if (PlayerManager.instance.PlayerDirection.x == 1f)
                {
                    __originalRotation = Quaternion.Euler(0, 0, 45);
                    __originalDirection = new Vector2(1f, 0f);
                }
                // Right
                else
                {
                    __originalRotation = Quaternion.Euler(0, 0, -45);
                    __originalDirection = new Vector2(-1f, 0f);
                }
            }
        }
        // Aiming Down
        else if (PlayerManager.instance.PlayerDirection.y == -1f)
        {
            // Straight Down
            if (!PlayerManager.instance.IsPlayerTouchingGround)
            {
                __originalRotation = Quaternion.Euler(0, 0, -90);
                __originalDirection = new Vector2(1f, 0f);
            }
            else if (!PlayerManager.instance.IsPlayerWalking)
            {
                __originalRotation = Quaternion.identity;
                __originalDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, 0f);
            }
            // Diagonal
            else
            {
                // Left
                if (PlayerManager.instance.PlayerDirection.x == 1f)
                {
                    __originalRotation = Quaternion.Euler(0, 0, -45);
                    __originalDirection = new Vector2(1f, 0f);
                }
                // Right
                else
                {
                    __originalRotation = Quaternion.Euler(0, 0, 45);
                    __originalDirection = new Vector2(-1f, 0f);
                }
            }
        }
        else
        {
            __originalRotation = Quaternion.identity;
            __originalDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, 0f);
        }

        for (int i=0; i<5; i++)
        {
            GameObject __firedShot;

            __firedShot = Instantiate(_shot, __originalPosition, __originalRotation);
            __firedShot.GetComponent<ShotController>().shotDirection = __originalDirection;

            __firedShot.GetComponent<ShotController>().shotSpeed = 15f * PlayerManager.instance.ShotSpeedModificator;
            __firedShot.GetComponent<ShotController>().shotType = "Laser";
            __firedShot.GetComponent<ShotController>().shotDamage = 7.5f;

            yield return new WaitForSeconds(0.005f);
        }
    }

    private IEnumerator GunCoolingDownRoutine()
    {
        _gunCooledDown = false;

        switch (PlayerManager.instance.CurrentWeapon)
        {
            case Weapon.MACHINEGUN:
                yield return new WaitForSeconds(0.15f);
                break;
            case Weapon.SPREAD:
                yield return new WaitForSeconds(0.3f);
                break;
            case Weapon.FIRE:
                yield return new WaitForSeconds(0.4f);
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
            AudioManager.instance.PlaySpreadShot();
            float __dir = -0.2f;
            for (int i = 0; i < 5; i++)
            {
                float __xShotOffset = 0;
                float __yShotOffset = 0;
                Vector3 __shotPosition;
                float __xFactor = PlayerManager.instance.PlayerDirection.x;
                float __yFactor = PlayerManager.instance.PlayerDirection.y;

                if (i == 0 || i == 4)
                    __xShotOffset = __yShotOffset = 0;
                else if (i == 1 || i == 3)
                    __xShotOffset = __yShotOffset = 0.2f;
                else
                    __xShotOffset = __yShotOffset = 0.4f;

                if (PlayerManager.instance.IsPlayerWalking)
                    __shotPosition = new Vector3(ShotSpawnPoint.position.x + (__xShotOffset * __xFactor), ShotSpawnPoint.position.y + (__yShotOffset * __yFactor), ShotSpawnPoint.position.z);
                else if(PlayerManager.instance.PlayerDirection.y == 1f || !PlayerManager.instance.IsPlayerTouchingGround)
                    __shotPosition = new Vector3(ShotSpawnPoint.position.x, ShotSpawnPoint.position.y + (__yShotOffset * __yFactor), ShotSpawnPoint.position.z);
                else
                    __shotPosition = new Vector3(ShotSpawnPoint.position.x + (__xShotOffset * __xFactor), ShotSpawnPoint.position.y, ShotSpawnPoint.position.z);

                GameObject __spreadShot = Instantiate(_shot, __shotPosition, Quaternion.identity);

                __spreadShot.GetComponent<ShotController>().shotSpeed = 12.5f * PlayerManager.instance.ShotSpeedModificator;
                __spreadShot.GetComponent<ShotController>().shotType = "Spread";
                __spreadShot.GetComponent<ShotController>().shotDamage = 10f;

                if (PlayerManager.instance.IsPlayerWalking)
                {
                    if (PlayerManager.instance.PlayerDirection.y == 0)
                        __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, PlayerManager.instance.PlayerDirection.y + __dir);
                    else
                    {

                        __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(PlayerManager.instance.PlayerDirection.x + (__xFactor * __dir), PlayerManager.instance.PlayerDirection.y - (__yFactor * __dir));
                    }
                }
                else if (PlayerManager.instance.PlayerDirection.y == 1f)
                    __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(__dir, 1f);
                else if (PlayerManager.instance.PlayerDirection.y == -1f)
                    if (PlayerManager.instance.IsPlayerTouchingGround)
                        __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, __dir);
                    else
                        __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(__dir, -1f);
                else
                    __spreadShot.GetComponent<ShotController>().shotDirection = new Vector2(PlayerManager.instance.PlayerDirection.x, PlayerManager.instance.PlayerDirection.y + __dir);

                __dir += 0.1f;
            }
        }
        else
        {
            GameObject __firedShot = Instantiate(_shot, ShotSpawnPoint.position, Quaternion.identity);

            __firedShot.GetComponent<ShotController>().shotSpeed = 12.5f * PlayerManager.instance.ShotSpeedModificator;

            if (PlayerManager.instance.CurrentWeapon == Weapon.REGULAR)
            {
                AudioManager.instance.PlayRegularShot();
                __firedShot.GetComponent<ShotController>().shotType = "Regular";
                __firedShot.GetComponent<ShotController>().shotDamage = 10f;
            }
            else if (PlayerManager.instance.CurrentWeapon == Weapon.MACHINEGUN)
            {
                AudioManager.instance.PlayMachineGunShot();
                __firedShot.GetComponent<ShotController>().shotType = "MachineGun";
                __firedShot.GetComponent<ShotController>().shotDamage = 10f;
            }
            else if (PlayerManager.instance.CurrentWeapon == Weapon.FIRE)
            {
                AudioManager.instance.PlayFireShot();
                __firedShot.GetComponent<ShotController>().shotSpeed = 7.5f * PlayerManager.instance.ShotSpeedModificator;
                __firedShot.GetComponent<ShotController>().shotType = "Fire";
                __firedShot.GetComponent<ShotController>().shotDamage = 12.5f;
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
        {
            if(!PlayerManager.instance.IsPlayerTouchingWater)
                __spawnPoint = _spawnPositions.RegularPosition;
            else
                __spawnPoint = _spawnPositions.WaterRegularPosition;
        }
        // Shooting crouched
        if (PlayerManager.instance.PlayerDirection.y == -1f && !PlayerManager.instance.IsPlayerWalking && !PlayerManager.instance.IsPlayerTouchingWater)
        {
            __spawnPoint = _spawnPositions.CrouchPosition;
        }
        // Shooting Up
        if (PlayerManager.instance.PlayerDirection.y == 1f && !PlayerManager.instance.IsPlayerWalking)
        {
            if (!PlayerManager.instance.IsPlayerTouchingWater)
                __spawnPoint = _spawnPositions.StraightUpPosition;
            else
                __spawnPoint = _spawnPositions.WaterUpPosition;
        }
        // Shooting MidAir
        if (!PlayerManager.instance.IsPlayerTouchingGround && !PlayerManager.instance.IsPlayerTouchingWater)
            __spawnPoint = _spawnPositions.JumpingPosition;
        if (PlayerManager.instance.IsPlayerWalking)
        {
            if (PlayerManager.instance.PlayerDirection.y == 1f)
            {
                if (!PlayerManager.instance.IsPlayerTouchingWater)
                    __spawnPoint = _spawnPositions.DiagonalUpPosition;
                else
                    __spawnPoint = _spawnPositions.WaterDiagonalPosition;
            }
            else if (PlayerManager.instance.PlayerDirection.y == -1f)
            {
                if (!PlayerManager.instance.IsPlayerTouchingWater)
                    __spawnPoint = _spawnPositions.DiagonalDownPosition;
            }
        }

        if (PlayerManager.instance.PlayerDirection.x < 0)
            __spawnPoint.x = -__spawnPoint.x;

        return __spawnPoint;
    }

}
