using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] _UILives;

    private bool _triggeredRestart;
    private int _extraLives;

    private void Start()
    {
        _triggeredRestart = false;
        _extraLives = 2;
    }

    private void Update()
    {
        if (PlayerManager.instance.PlayerDied && !_triggeredRestart )
        {
            if(_extraLives > 0)
            {
                _triggeredRestart = true;

                _UILives[_extraLives].active = false;

                _extraLives--;

                StartCoroutine(ResetPlayer());
            }
            else
            {
                _UILives[_extraLives].active = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.instance.PlayPause();
            if (Time.timeScale > 0f)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }
    }

    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(1.75f);
        PlayerManager.instance.ResetPlayer();
        _triggeredRestart = false;
    }
}
