using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private BossBackground _bossBackground;
    private BossCannons _bossCannons;
    private BossDoor _bossDoor;
    private BossHiddenEnemy _bossHiddenEnemy;
    private bool _levelEnded;
    private bool _startBoss;

    private void Start()
    {
        _bossBackground = GetComponentInChildren<BossBackground>();
        _bossCannons = GetComponentInChildren<BossCannons>();
        _bossDoor = GetComponentInChildren<BossDoor>();
        _bossHiddenEnemy = GetComponentInChildren<BossHiddenEnemy>();

        _startBoss = false;
    }

    private void Update()
    {
        if(!_levelEnded && _bossDoor.life <= 0)
        {
            _levelEnded = true;
            _bossCannons.life = 0;
            _bossBackground.selfDestruct = true;
            _bossHiddenEnemy.hit = true;
            StartCoroutine(FinishedLevel());
            AudioManager.instance.StopMainMusic();
            AudioManager.instance.PlayBossExplode();
        }
        if (PlayerManager.instance != null)
        {
            if (!_startBoss)
            {
                if (PlayerManager.instance.transform.position.x > 62f)
                {
                    _startBoss = true;
                    StartCoroutine(InitiateBoss());
                    AudioManager.instance.PlayBossInitialSound();
                }
            }

            if (PlayerManager.instance.SwitchBossLayers)
            {
                _bossBackground.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Shot";
                _bossCannons.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Shot";
                _bossDoor.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Shot";
            }
        }
    }

    private IEnumerator InitiateBoss()
    {
        while(Camera.main.transform.position.x < 67.31f)
        {
            Camera.main.transform.Translate(new Vector3(2,0,0) * Time.deltaTime);
            yield return null;
        }
        _bossCannons.startedShooting = true;
        _bossHiddenEnemy.active = true;
    }

    private IEnumerator FinishedLevel()
    {
        while (AudioManager.instance.IsSFXPlaying()) yield return null;
        PlayerManager.instance.FinishedLevel = true;
        AudioManager.instance.StageClear();
    }
}
