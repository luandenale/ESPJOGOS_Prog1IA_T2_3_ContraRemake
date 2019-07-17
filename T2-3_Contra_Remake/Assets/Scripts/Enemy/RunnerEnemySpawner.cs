using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerEnemySpawner : MonoBehaviour
{
    [SerializeField] float playerXPosStart;
    [SerializeField] float playerXPosEnd;
    [SerializeField] GameObject runnerEnemyPrefab;

    private bool _startedSpawning;

    private void Start()
    {
        _startedSpawning = false;
        gameObject.transform.position = new Vector3(playerXPosEnd + 7.5f, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    private void Update()
    {
        if (PlayerManager.instance.transform.position.x > playerXPosStart && !_startedSpawning)
        {
            _startedSpawning = true;
            StartCoroutine(SpawnRoutine());
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while(PlayerManager.instance.transform.position.x < playerXPosEnd)
        {
            yield return new WaitForSeconds(1.5f);
            GameObject __runnerSpawned = Instantiate(runnerEnemyPrefab, transform.position, Quaternion.identity);
            __runnerSpawned.GetComponent<RunnerEnemyController>().playerXPosTrigger = Camera.main.transform.position.x;
            __runnerSpawned.GetComponent<RunnerEnemyController>().triggeredBySpawner = true;
        }
        yield return null;
    }
}
