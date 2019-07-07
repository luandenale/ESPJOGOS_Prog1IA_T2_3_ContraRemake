using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] float playerXPosTrigger;
    [SerializeField] Animator explosionsAnimator;

    private Animator _bridgeAnimator;

    private void Awake()
    {
        _bridgeAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(PlayerManager.instance.transform.position.x > playerXPosTrigger)
        {
            _bridgeAnimator.SetTrigger("Destroy");
            explosionsAnimator.SetTrigger("Explode");
        }
    }

}
