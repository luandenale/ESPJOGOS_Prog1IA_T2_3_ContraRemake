using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public Vector2 shotDirection = new Vector2(0f, 0f);
    public float _shotSpeed = 10f;

    private Rigidbody2D _shotRigidBody;
    private BoxCollider2D _shotCollider;

    private void Awake()
    {
        _shotRigidBody = GetComponent<Rigidbody2D>();
        _shotCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        Destroy(this, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        _shotRigidBody.velocity = new Vector2(shotDirection.x * _shotSpeed, shotDirection.y * _shotSpeed);
    }
}
