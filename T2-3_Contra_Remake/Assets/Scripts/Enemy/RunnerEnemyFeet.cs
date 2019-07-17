using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemyFeet : MonoBehaviour
{
    private BoxCollider2D _collider;

    public bool IsTouchingGround;

    private void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        IsTouchingGround = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
            IsTouchingGround = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
            IsTouchingGround = false;
    }
}
