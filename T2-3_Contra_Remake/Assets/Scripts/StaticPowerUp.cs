using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPowerUp : MonoBehaviour
{
    [SerializeField] GameObject powerUp;
    [SerializeField] float upForce;
    private Animator _staticPowerUpAnimator;

    private void Awake()
    {
        _staticPowerUpAnimator = GetComponent<Animator>();
    }

    public void DropPowerUp()
    {
        powerUp.gameObject.SetActive(true);

        _staticPowerUpAnimator.SetTrigger("Explode");

        powerUp.GetComponent<PowerUpController>().DropPowerUp(upForce);
    }
}
