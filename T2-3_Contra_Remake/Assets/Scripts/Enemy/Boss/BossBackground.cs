using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBackground : MonoBehaviour
{
    private Animator _animator;
    public bool selfDestruct;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (selfDestruct)
            _animator.SetTrigger("Explode");
    }
}
