using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KittyHead : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private spawnManager _spawnManager;
    [SerializeField]
    private int enemyID;
    private int _curRound;
    public Transform pointToGo;
    public float speed;
    private bool _active = false;
    private GameObject _healthObject;
    private float _curHealth;
    [SerializeField]
    private float _maxHealth;
    [SerializeField]
    private GameObject _bossAttackPrefab;

    // Start is called before the first frame update
    void Start()
    {
        pointToGo = GameObject.Find("BossSpawnPoint").transform;
        _healthObject = GameObject.Find("Canvas").transform.Find("BossHealthBar").gameObject;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<spawnManager>();
        _curRound = _spawnManager.CurRound();
        _maxHealth *= _curRound;
        _curHealth = _maxHealth;
        _healthObject.GetComponent<Image>().fillAmount = _curHealth / _maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

        if (enemyID == 666)
        {
            float step = speed * Time.deltaTime;
            this.transform.position = Vector3.MoveTowards(this.transform.position, pointToGo.position, step);
            if (transform.position.y == pointToGo.position.y && _active == false)
            {
                _active = true;
                _healthObject.SetActive(true);
                StartCoroutine(BossAttack());

            }
            
        }
        else
        {
            this.transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
        }

    }

    IEnumerator BossAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.0f, 4.0f));
            Vector3 offset = new Vector3(0, 2f, 0);
            Instantiate(_bossAttackPrefab, transform.position + offset, Quaternion.identity);
        }

    }

    private void OnEnemyDeath()
    {
        _healthObject.SetActive(false);
        _spawnManager.EnemyDestroyed();
        _spawnManager.Explode(transform.position);
        Destroy(gameObject, 0.25f);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyID == 666)
        {
            if (_active == true)
            {
                if (other.gameObject.CompareTag("Laser"))
                {
                    Destroy(other.gameObject);
                    _curHealth--;
                    float _barAmount = _curHealth / _maxHealth;
                    _healthObject.GetComponent<Image>().fillAmount = _barAmount;
                    if (_curHealth < 1)
                    {
                        OnEnemyDeath();
                    }
                }
            }

        }
        else
        {
            if (other.gameObject.CompareTag("Laser"))
            {
                Destroy(other.gameObject);
                _spawnManager.Explode(transform.position);
                _spawnManager.StartSpawning();
                Destroy(gameObject, 0.25f);
            }
        }

    }
    public void SetEnemyID(int ID)
    {
        enemyID = ID;
    }
}
