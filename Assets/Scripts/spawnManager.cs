using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class spawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;
    private bool active = true;
    [SerializeField]
    private GameObject _explosionPrefab;
    private int _enemies_Spwaned = 0;
    private int _enemies_Destroyed = 0;
    [SerializeField]
    private int[] _waves;
    private int _curWave = 0;
    [SerializeField]
    private Text _waveText;
    [SerializeField]
    private GameObject _waveTextObj;
    [SerializeField]
    private Enemys[] _enemys;
    private List<GameObject> _enemyPrefab;
    private List<int> _EnemyWeight;
    [SerializeField]
    private Enemys[] _powerups;

    [Serializable]
    private class Enemys
    {
        [SerializeField]
        public GameObject _EnemyPrefabObject;
        [SerializeField]
        public int _EnemySpawnWeight;
    }

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
    public int CurWave()
    {
        return _curWave;
    }

    IEnumerator spawnEnemyControl()
    {
        yield return new WaitForSeconds(3.0f);
        while (active == true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(5.0f);
        }
    }

    private void SpawnEnemy()
    {
        switch (_curWave)
        {
            case 0:
                if (_enemies_Spwaned < _waves[_curWave])
                {
                    if (_enemies_Spwaned == 0)
                    {
                        StartCoroutine(WaveText(_curWave + 1));
                    }
                    Vector3 spawnpoint;
                    GameObject randomEnemy = RandomItem(_enemys);
                    int index = 0;
                    for (int i = 0; i < _enemys.Length; i++)
                    {
                        if (_enemys[i]._EnemyPrefabObject == randomEnemy)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == 2)
                    {
                        spawnpoint = new Vector3(-20f, UnityEngine.Random.Range(-10f, 10f), 0);
                    }
                    else
                    {
                        spawnpoint = new Vector3(UnityEngine.Random.Range(-18.0f, 18.0f), 7.5f, 0);
                    }

                    GameObject newEnemy = Instantiate(randomEnemy, spawnpoint, Quaternion.identity);
                    newEnemy.GetComponent<Enemy>().SetEnemyID(index);
                    _enemies_Spwaned++;
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                if (_enemies_Destroyed == _waves[_curWave])
                {
                    _curWave++;
                    _enemies_Spwaned = 0;
                    _enemies_Destroyed = 0;
                }
                break;
            case 1:
                if (_enemies_Spwaned < _waves[_curWave])
                {
                    if (_enemies_Spwaned == 0)
                    {
                        StartCoroutine(WaveText(_curWave + 1));
                    }
                    Vector3 spawnpoint;
                    GameObject randomEnemy = RandomItem(_enemys);
                    int index = 0;
                    for (int i = 0; i < _enemys.Length; i++)
                    {
                        if (_enemys[i]._EnemyPrefabObject == randomEnemy)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == 2)
                    {
                        spawnpoint = new Vector3(-20f, UnityEngine.Random.Range(-10f, 10f), 0);
                    }
                    else
                    {
                        spawnpoint = new Vector3(UnityEngine.Random.Range(-18.0f, 18.0f), 7.5f, 0);
                    }

                    GameObject newEnemy = Instantiate(randomEnemy, spawnpoint, Quaternion.identity);
                    newEnemy.GetComponent<Enemy>().SetEnemyID(index);
                    _enemies_Spwaned++;
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                if (_enemies_Destroyed == _waves[_curWave])
                {
                    _curWave++;
                    _enemies_Spwaned = 0;
                    _enemies_Destroyed = 0;
                }
                break;
            case 2:
                if (_enemies_Spwaned < _waves[_curWave])
                {
                    if (_enemies_Spwaned == 0)
                    {
                        StartCoroutine(WaveText(_curWave + 1));
                    }
                    Vector3 spawnpoint;
                    GameObject randomEnemy = RandomItem(_enemys);
                    int index = 0;
                    for (int i = 0; i < _enemys.Length; i++)
                    {
                        if (_enemys[i]._EnemyPrefabObject == randomEnemy)
                        {
                            index = i;
                            break;
                        }
                    }
                    if (index == 2)
                    {
                        spawnpoint = new Vector3(-20f, UnityEngine.Random.Range(-10f, 10f), 0);
                    }
                    else
                    {
                        spawnpoint = new Vector3(UnityEngine.Random.Range(-18.0f, 18.0f), 7.5f, 0);
                    }

                    GameObject newEnemy = Instantiate(randomEnemy, spawnpoint, Quaternion.identity);
                    newEnemy.GetComponent<Enemy>().SetEnemyID(index);
                    _enemies_Spwaned++;
                    newEnemy.transform.parent = _enemyContainer.transform;
                }
                if (_enemies_Destroyed == _waves[_curWave])
                {
                    _curWave = 0;
                    _enemies_Spwaned = 0;
                    _enemies_Destroyed = 0;
                }
                break;
            default:
                break;
        }

    }

    IEnumerator WaveText(int v)
    {
        _waveText.text = "Wave " + v;
        _waveTextObj.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        _waveTextObj.SetActive(false);
    }


    private GameObject RandomItem(Enemys[] prefabs)
    {
        var results = prefabs.OrderByDescending(x => x._EnemySpawnWeight).ToList();
        GameObject tempObject;
        int tempInt;
        _enemyPrefab = new List<GameObject>();
        _EnemyWeight = new List<int>();
        for (var i = 0; i < results.Count; i++)
        {
            tempObject = results[i]._EnemyPrefabObject;
            _enemyPrefab.Add(tempObject);
            tempInt = results[i]._EnemySpawnWeight;
            _EnemyWeight.Add(tempInt);
        }
        var total = 0f;
        var roll = Random.Range(0, _EnemyWeight.Sum());
        for (var i = 0; i < _EnemyWeight.Count; i++)
        {
            total += _EnemyWeight[i];
            if (roll < total)
                return _enemyPrefab[i];
        }
        return _enemyPrefab[Random.Range(0, _enemyPrefab.Count)];
    }

    IEnumerator spawnPowerUpControl()
    {
        yield return new WaitForSeconds(3.0f);
        while (active == true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
            Vector3 spawnpoint = new Vector3(UnityEngine.Random.Range(-18.0f, 18.0f), 9.7f, 0);
            GameObject randomPowerup = RandomItem(_powerups);
            GameObject InstPowerUp = Instantiate(randomPowerup, spawnpoint, Quaternion.identity);
            int index = 0;
            for (int i = 0; i < _powerups.Length; i++)
            {
                if (_powerups[i]._EnemyPrefabObject == randomPowerup)
                {
                    index = i;
                    break;
                }
            }
            InstPowerUp.GetComponent<powerUp>().SetPowerupID(index);
        }
    }

    public void EnemyDestroyed()
    {
        _enemies_Destroyed++;
    }
    public void Explode(Vector3 pos)
    {
        GameObject _explosion = Instantiate(_explosionPrefab, pos, Quaternion.identity);
        Destroy(_explosion, 3.0f);
    }
    public void onPlayerDeath()
    {
        active = false;
    }
}
