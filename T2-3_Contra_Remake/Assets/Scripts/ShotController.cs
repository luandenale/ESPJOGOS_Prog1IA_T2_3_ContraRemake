using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public Vector2 shotDirection = new Vector2(0f, 0f);
    public float _shotSpeed = 10f;

    private void Awake()
    {

    }

    private void Start()
    {
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(shotDirection * _shotSpeed * Time.deltaTime);
    }
}
