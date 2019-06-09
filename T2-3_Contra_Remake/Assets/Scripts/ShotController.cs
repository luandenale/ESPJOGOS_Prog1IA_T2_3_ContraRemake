using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public Vector2 shotDirection = new Vector2(0f, 0f);
    public float shotSpeed = 10f;
    public float shotDamage = 10f;
    public string shotType;

    private SpriteRenderer[] _shotsSprites;

    private void Awake()
    {
        _shotsSprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < _shotsSprites.Length; i++)
        {
            if (_shotsSprites[i].name == shotType)
                _shotsSprites[i].enabled = true;
        }
        transform.Translate(shotDirection * shotSpeed * Time.deltaTime);
    }
}
