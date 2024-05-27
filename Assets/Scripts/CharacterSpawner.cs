using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private float minTime = 1;
    [SerializeField]
    private float maxTime = 5;


    void Start()
    {
        
        StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy()
    {

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            print("Spawn");

        StartCoroutine(SpawnEnemy());
    }

}
