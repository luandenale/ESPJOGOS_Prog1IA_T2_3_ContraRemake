using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] _UILives;
    [SerializeField] RectTransform _slidingImage;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _godModeText;

    private bool _playerInstantiated;
    private bool _triggeredRestart;
    private int _extraLives;

    private AsyncOperation _asyncScene;

    private void Start()
    {
        _playerInstantiated = false;
        _triggeredRestart = false;
        _extraLives = 2;

        StartCoroutine(SlideIntro());

        _asyncScene = SceneManager.LoadSceneAsync("Level1");
        _asyncScene.allowSceneActivation = false;
    }

    private void Update()
    {
        if (_playerInstantiated)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                if (!PlayerManager.instance.GODMODE)
                {
                    PlayerManager.instance.GODMODE = true;
                    _godModeText.SetActive(true);
                }
                else
                {
                    PlayerManager.instance.GODMODE = false;
                    _godModeText.SetActive(false);
                }
            }else if (Input.GetKeyDown(KeyCode.R))
            {
                Physics2D.IgnoreLayerCollision(0, 4, false);
                _asyncScene.allowSceneActivation = true;
                Destroy(PlayerManager.instance.gameObject);
            }

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

    private IEnumerator SlideIntro()
    {
        while(_slidingImage.localScale.x > 0)
        {
            _slidingImage.localScale = new Vector3(_slidingImage.localScale.x - 0.025f, 1f, 1f);
            yield return new WaitForSeconds(0.01f);
        }

        _player.SetActive(true);
        _playerInstantiated = true;

        yield return null;
    }

    private IEnumerator ResetPlayer()
    {
        yield return new WaitForSeconds(1.75f);
        PlayerManager.instance.ResetPlayer();
        _triggeredRestart = false;
    }
}
