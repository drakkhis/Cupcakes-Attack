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
            Vector3 spawnpoint = new Vector3(Random.Range(-18.0f, 18.0f), 9.7f, 0);
            int randomPowerup = 0;
            float randPercent = Random.Range(0f, 100f);
            if (randPercent > 95f) // 5%
            {
                randomPowerup = 5; //Lightning Shot
                
            }
            else if (randPercent > 80f) // 15%
            {
                randomPowerup = 4; //Health
            }
            else if (randPercent > 50f) // 30%
            {
                randomPowerup = 3; //Ammo
            }
            else if (randPercent > 30f) // 20%
            {
                randomPowerup = 2; //Shield
            }
            else if (randPercent > 10f) // 20%
            {
                randomPowerup = 1; //Speed
            }
            else if (randPercent > 0f) // 10%
            {
                randomPowerup = 0; //Tripple Shot
            }
            else
            {
                randomPowerup = 4;
            }
            Instantiate(_PowerUpPrefab[randomPowerup], spawnpoint, Quaternion.identity);
        }
    }

    public void onPlayerDeath()
    {
        active = false;
    }
}
