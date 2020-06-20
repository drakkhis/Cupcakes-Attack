using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class spawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _PowerUpPrefab;
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
                    Vector3 spawnpoint = new Vector3(UnityEngine.Random.Range(-8.0f, 8.0f), 7.5f, 0);
                    GameObject newEnemy = Instantiate(_enemyPrefab, spawnpoint, Quaternion.identity);
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
                    Vector3 spawnpoint = new Vector3(UnityEngine.Random.Range(-8.0f, 8.0f), 7.5f, 0);
                    GameObject newEnemy = Instantiate(_enemyPrefab, spawnpoint, Quaternion.identity);
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
                    Vector3 spawnpoint = new Vector3(UnityEngine.Random.Range(-8.0f, 8.0f), 7.5f, 0);
                    GameObject newEnemy = Instantiate(_enemyPrefab, spawnpoint, Quaternion.identity);
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

    IEnumerator spawnPowerUpControl()
    {
        yield return new WaitForSeconds(3.0f);
        while (active == true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(5.0f, 10.0f));
            Vector3 spawnpoint = new Vector3(UnityEngine.Random.Range(-18.0f, 18.0f), 9.7f, 0);
            int randomPowerup = 0;
            float randPercent = UnityEngine.Random.Range(0f, 100f);
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
