using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    [SerializeField]
    private int points;
    private Player _player;
    private Animator _Animator;
    private Collider2D _collider2D;
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _EnemylaserPrefab;
    private bool active = true;
    private float _centerLine;
    private bool bounceLeft;
    void SetInActive()
    {
        _collider2D.enabled = false;
    }



    float AnimationLength(string name)
    {
        float time = 0;
        RuntimeAnimatorController ac = _Animator.runtimeAnimatorController;

        for (int i = 0; i < ac.animationClips.Length; i++)
            if (ac.animationClips[i].name == name)
                time = ac.animationClips[i].length;

        return time;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _Animator = gameObject.GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
        _centerLine = transform.position.x;
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if (_Animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
        StartCoroutine(shootEnemyLaser());
    }

    // Update is called once per frame
    void Update()
    {

        enemyMovement();

    }

    private void enemyMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if (transform.position.x > (_centerLine - 5.0f) && bounceLeft == false)
        {
            transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
        }
        else
        {
            bounceLeft = true;
        }

        if (transform.position.x < (_centerLine + 5.0f) && bounceLeft == true)
        {
            transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        }
        else
        {
            bounceLeft = false;
        }

        if (transform.position.y < -22.0f)
        {
            _centerLine = UnityEngine.Random.Range(-18.0f, 18.0f);
            transform.position = new Vector3(_centerLine, 10f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                other.GetComponent<Player>().damage();
            }
            OnEnemyDeath();
        }
        if (other.gameObject.CompareTag("Laser"))
        {
            if (_player != null)
            {
                _player.AddScore(points);
            }
            Destroy(other.gameObject);
            OnEnemyDeath();
        }
    }

    IEnumerator shootEnemyLaser()
    {
        while (active == true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.0f, 5.0f));
            if (active == true)
            {
                Vector3 offset = new Vector3(0, -1.05f, 0);
                Instantiate(_EnemylaserPrefab, transform.position + offset, Quaternion.identity);
            }

        }
    }
    private void OnEnemyDeath()
    {
        _enemySpeed = 0;
        active = false;
        _audioSource.Play();
        SetInActive();
        _Animator.SetTrigger("onEnemyDeath");
        Destroy(gameObject, AnimationLength("Enemy Destroyed"));
    }
}
