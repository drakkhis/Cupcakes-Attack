using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwanManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _PowerUpPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool active = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(spawnEnemyControl());
        StartCoroutine(spawnPowerUpControl());
    }

    IEnumerator spawnEnemyControl()
    {
        yield return new WaitForSeconds(3.0f);
        while (active == true)
        {
            Vector3 spawnpoint = new Vector3(Random.Range(-8.0f, 8.0f), 7.5f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnpoint, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }

    IEnumerator spawnPowerUpControl()
    {
        yield return new WaitForSeconds(3.0f);
        while (active == true)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f));
            Vector3 spawnpoint = new Vector3(Random.Range(-8.0f, 8.0f), 7.5f, 0);
            int randomPowerup = Random.Range(0, 4);
            Instantiate(_PowerUpPrefab[randomPowerup], spawnpoint, Quaternion.identity);
        }
    }

    public void onPlayerDeath()
    {
        active = false;
    }
}
