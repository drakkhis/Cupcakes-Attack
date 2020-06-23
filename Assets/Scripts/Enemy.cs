using System.Collections;
using UnityEngine;

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
    private spawnManager _spawnManager;
    [SerializeField]
    private GameObject _EnemylaserPrefab;
    private bool active = true;
    private float _centerLine;
    private bool bounceLeft;
    private int _curWave;
    [SerializeField]
    private int enemyID;
    [SerializeField]
    private int _shields;
    private GameObject _shieldObj;
    [SerializeField]
    private float _shieldChancePercent;
    private GameObject _other;
    [SerializeField]
    private bool _smartEnemy;
    [SerializeField]
    private int _smartEnemyPercent;
    GameObject _trackedPowerUp;
    [SerializeField]
    private bool _avoidShot = false;
    void SetInActive()
    {
        _collider2D.enabled = false;
    }

    public void SetEnemyID(int ID)
    {
        enemyID = ID;
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
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<spawnManager>();
        _centerLine = transform.position.x;
        _shieldObj = transform.Find("Shields").gameObject;
        _shieldObj.SetActive(false);
        float _shieldRandom = UnityEngine.Random.Range(0f, 100.0f);
        if (_shieldChancePercent >= _shieldRandom)
        {
            _shieldObj.SetActive(true);
            _shields = 1;
        }
        float _smartRandom = UnityEngine.Random.Range(0f, 100.0f);
        if (_smartEnemyPercent >= _smartRandom)
        {
            _smartEnemy = true;
        }
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        if (_Animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn_Manager is NULL");
        }
        if (_collider2D == null)
        {
            Debug.LogError("Collider2D is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL");
        }
        _curWave = _spawnManager.CurWave();



        StartCoroutine(ShootEnemyLaser());
    }

    // Update is called once per frame
    void Update()
    {

        EnemyMovement();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);

        // If it hits something...
        if (hit.collider != null && hit.transform.gameObject != _trackedPowerUp)
        {
            if (hit.transform.gameObject.CompareTag("powerUp"))
            {
                _trackedPowerUp = hit.transform.gameObject;
                Vector3 offset = new Vector3(0, -1.05f, 0);
                Instantiate(_EnemylaserPrefab, transform.position + offset, Quaternion.identity);
            }
        }


    }

    private void EnemyMovement()
    {
        float wave_speed = _enemySpeed + (_curWave * 1.5f);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 5);

        if (hitColliders.Length > 0 && hitColliders[0].transform.gameObject.CompareTag("Player"))
        {
            Vector3 target = new Vector3(hitColliders[0].transform.position.x, hitColliders[0].transform.position.y, 0);
            Vector3 myPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            transform.position = Vector3.MoveTowards(myPosition, target, wave_speed * Time.deltaTime);
        }
        else if ((hitColliders.Length > 0) && (hitColliders[0].transform.gameObject.CompareTag("Laser")) && (_avoidShot == true))
        {
            Vector3 target = new Vector3(hitColliders[0].transform.position.x, hitColliders[0].transform.position.y, 0);
            Vector3 myPosition = new Vector3(this.transform.position.x, this.transform.position.y, 0);
            transform.position = Vector3.MoveTowards(myPosition, target, -(wave_speed*2) * Time.deltaTime);
        }
        else
        {
            switch (enemyID)
            {
                case 0:
                    transform.Translate(Vector3.down * wave_speed * Time.deltaTime);
                    if (transform.position.x > (_centerLine - 5.0f) && bounceLeft == false)
                    {
                        transform.Translate(Vector3.left * wave_speed * Time.deltaTime);
                    }
                    else
                    {
                        bounceLeft = true;
                    }

                    if (transform.position.x < (_centerLine + 5.0f) && bounceLeft == true)
                    {
                        transform.Translate(Vector3.right * wave_speed * Time.deltaTime);
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
                    break;
                case 1:
                    transform.Translate(Vector3.down * wave_speed * Time.deltaTime);
                    if (transform.position.x > (_centerLine - 5.0f) && bounceLeft == false)
                    {
                        transform.Translate(Vector3.left * wave_speed * Time.deltaTime);
                    }
                    else
                    {
                        bounceLeft = true;
                    }

                    if (transform.position.x < (_centerLine + 5.0f) && bounceLeft == true)
                    {
                        transform.Translate(Vector3.right * wave_speed * Time.deltaTime);
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
                    break;
                case 2:
                    transform.Translate(Vector3.right * wave_speed * Time.deltaTime);

                    if (transform.position.x > 20.0f)
                    {
                        _centerLine = UnityEngine.Random.Range(-10.0f, 10.0f);
                        transform.position = new Vector3(-20f, _centerLine, 0);
                    }
                    break;
                case 3:
                    transform.Translate(Vector3.down * wave_speed * Time.deltaTime);

                    if (transform.position.y < -22.0f)
                    {
                        _centerLine = UnityEngine.Random.Range(-18.0f, 18.0f);
                        transform.position = new Vector3(_centerLine, 10f, 0);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_other == other.gameObject) return;
        _other = other.gameObject;
        if (_shields != 0)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _shields--;
                _shieldObj.SetActive(false);
                Player player = other.transform.GetComponent<Player>();
                if (player != null)
                {
                    other.GetComponent<Player>().damage();
                }
            }
            if (other.gameObject.CompareTag("Laser"))
            {
                _shields--;
                _shieldObj.SetActive(false);
                Destroy(other.gameObject);
            }
        }
        else
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
    }

    IEnumerator ShootEnemyLaser()
    {
        while (active == true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(2.0f, 5.0f));
            if (active == true)
            {
                if (GameObject.Find("Player") != null)
                {
                    Vector2 playerPos = GameObject.Find("Player").transform.position;

                    if (_smartEnemy && playerPos.y > this.transform.position.y)
                    {
                        Vector3 offset = new Vector3(0, 1.05f, 0);
                        GameObject _shot = Instantiate(_EnemylaserPrefab, transform.position + offset, Quaternion.Inverse(this.transform.rotation));
                        _shot.GetComponent<EnemyLaser>().FireUp();
                    }
                    else
                    {

                        Vector3 offset = new Vector3(0, -1.05f, 0);
                        Instantiate(_EnemylaserPrefab, transform.position + offset, Quaternion.identity);
                    }
                }
            }

        }
    }

    private void OnEnemyDeath()
    {
        _spawnManager.EnemyDestroyed();
        _enemySpeed = 0;
        active = false;
        _audioSource.Play();
        SetInActive();
        _Animator.SetTrigger("onEnemyDeath");
        Destroy(gameObject, AnimationLength("Enemy Destroyed"));
    }
}
