using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    [SerializeField] AudioClip _bossExplode;
    [SerializeField] AudioClip _bossInitialSound;
    [SerializeField] AudioClip _bridgeExplode;
    [SerializeField] AudioClip _die;
    [SerializeField] AudioClip _enemyExplode;
    [SerializeField] AudioClip _explosionSelect;
    [SerializeField] AudioClip _fireShot;
    [SerializeField] AudioClip _hitCannon;
    [SerializeField] AudioClip _hitFloor;
    [SerializeField] AudioClip _laserShot;
    [SerializeField] AudioClip _machineGunShot;
    [SerializeField] AudioClip _pause;
    [SerializeField] AudioClip _powerUpExplode;
    [SerializeField] AudioClip _powerUpPick;
    [SerializeField] AudioClip _regularShot;
    [SerializeField] AudioClip _spreadShot;

    [SerializeField] AudioClip _levelClear;

    [SerializeField] AudioSource _sfxAudioSource;
    [SerializeField] AudioSource _musicAudioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void PlayBossExplode()
    {
        _sfxAudioSource.PlayOneShot(_bossExplode);
    }

    public void PlayBossInitialSound()
    {
        _sfxAudioSource.PlayOneShot(_bossInitialSound);
    }

    public void PlayBridgeExplode()
    {
        _sfxAudioSource.PlayOneShot(_bridgeExplode);
    }

    public void PlayDie()
    {
        _sfxAudioSource.PlayOneShot(_die);
    }

    public void PlayEnemyExplode()
    {
        _sfxAudioSource.PlayOneShot(_enemyExplode);
    }

    public void PlayExplosionSelect()
    {
        _sfxAudioSource.PlayOneShot(_explosionSelect);
    }

    public void PlayFireShot()
    {
        _sfxAudioSource.PlayOneShot(_fireShot);
    }

    public void PlayHitCannon()
    {
        _sfxAudioSource.PlayOneShot(_hitCannon);
    }

    public void PlayHitFloor()
    {
        _sfxAudioSource.PlayOneShot(_hitFloor);
    }

    public void PlayLaserShot()
    {
        _sfxAudioSource.PlayOneShot(_laserShot);
    }

    public void PlayMachineGunShot()
    {
        _sfxAudioSource.PlayOneShot(_machineGunShot);
    }

    public void PlayPause()
    {
        _sfxAudioSource.PlayOneShot(_pause);
    }

    public void PlayPowerUpExplode()
    {
        _sfxAudioSource.PlayOneShot(_powerUpExplode);
    }

    public void PlayPowerUpPick()
    {
        _sfxAudioSource.PlayOneShot(_powerUpPick);
    }

    public void PlayRegularShot()
    {
        _sfxAudioSource.PlayOneShot(_regularShot);
    }

    public void PlaySpreadShot()
    {
        _sfxAudioSource.PlayOneShot(_spreadShot);
    }

    public void StopMainMusic()
    {
        _musicAudioSource.Stop();
    }

    public void StageClear()
    {
        _musicAudioSource.PlayOneShot(_levelClear);
    }

    public bool IsSFXPlaying()
    {
        return _sfxAudioSource.isPlaying;
    }
}
