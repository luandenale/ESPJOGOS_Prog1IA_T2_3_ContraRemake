using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private AudioSource _audioSource;
    private Animator _animator;

    private bool _selected;
    private bool _canStart;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _selected = false;
        _canStart = false;
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            _animator.SetTrigger("Select");
            _selected = true;
        }

        if(_selected && _canStart)
            SceneManager.LoadScene("Level1");
    }

    public void PlayIntroMusic()
    {
        _audioSource.Play();
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        while (_audioSource.isPlaying)
            yield return null;

        _canStart = true;
    }
}
