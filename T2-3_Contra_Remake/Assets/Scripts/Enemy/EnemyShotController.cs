using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotController : MonoBehaviour
{
    public float shotSpeed = 5f;
    public string shotType;

    private SpriteRenderer[] _shotsSprites;
    private BoxCollider2D _boxCollider;
    private bool _hit;

    private void Start()
    {
        _hit = false;
        _shotsSprites = GetComponentsInChildren<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        for (int i = 0; i < _shotsSprites.Length; i++)
        {
            if (_shotsSprites[i].name == shotType)
                _shotsSprites[i].enabled = true;
        }

        if (_hit)
        {
            _boxCollider.enabled = false;
            Destroy(gameObject);
        }
        else
            transform.Translate(new Vector2(0f,1f) * shotSpeed * Time.deltaTime);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            _hit = true;
            PlayerManager.instance.PlayerDied = true;
        }
    }
}
