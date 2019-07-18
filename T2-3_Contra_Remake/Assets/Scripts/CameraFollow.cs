using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float _initialPos = -67.31f;
    private float _finalPos = 67.31f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerManager.instance != null)
        {
            if (PlayerManager.instance.PlayerDirection.x > 0f && PlayerManager.instance.transform.position.x > _initialPos && PlayerManager.instance.transform.position.x < _finalPos)
            {
                if (PlayerManager.instance.transform.position.x > transform.position.x)
                    transform.position = new Vector3(PlayerManager.instance.transform.position.x, transform.position.y, transform.position.z);
            }
        }
    }
}
