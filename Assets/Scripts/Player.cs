using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private InputAction moveAction;
    [SerializeField]
    private InputAction fireAction;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _boostSpeed = 8.5f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _coolDown = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int health = 3;
    [SerializeField]
    private int shields = 0;
    private spwanManager _spawnManager;
    [SerializeField]
    private bool _trippleShot = false;
    [SerializeField]
    private bool _speedBoost = false;
    [SerializeField]
    private GameObject _trippleShotPrefab;
    private GameObject shieldObj;
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightWing;
    [SerializeField]
    private GameObject _leftWing;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _LaserFireClip;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        shieldObj = transform.Find("Shields").gameObject;
        moveAction.Enable();
        fireAction.Enable();
        shieldObj.SetActive(false);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<spwanManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is null");
        }
        else
        {
            _audioSource.clip = _LaserFireClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        calculateMovement();
        if (Time.timeScale != 0)
        {
            if (fireAction.triggered && Time.time > _canFire)
            {
                shootLaser();
            }
        }



    }

    public void powerUp_TripleShot()
    {
        _trippleShot = true;
        StartCoroutine(TrippleShotTimer());
    }

    public void powerUp_Speed()
    {
        _speedBoost = true;
        StartCoroutine(SpeedTimer());
    }

    public void powerUp_Shield()
    {
        shields = 2;
        shieldObj.SetActive(true);
    }

    private IEnumerator TrippleShotTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _trippleShot = false;
    }

    private IEnumerator SpeedTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoost = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyLaser"))
        {
            Destroy(other.gameObject);
            damage();
        }
    }

    public void damage()
    {
        if (shields != 0)
        {
            shields--;
            if (shields == 0)
            {
                shieldObj.SetActive(false);
            }
        }
        else
        {
            health--;
            if (health == 2)
            {
                _rightWing.SetActive(true);
            }
            else if (health == 1)
            {
                _leftWing.SetActive(true);
            }
            else
            {
                _leftWing.SetActive(false);
                _rightWing.SetActive(false);
            }
            _uiManager.UpdateLives(health);
            if (health < 1)
            {
                _spawnManager.onPlayerDeath();
                Destroy(this.gameObject);
            }
        }
    }

    void shootLaser()
    {
        _audioSource.Play();
        _canFire = Time.time + _coolDown;

        if (_trippleShot == true)
        {
            //Vector3 offset = new Vector3(0, 0, 0);
            Instantiate(_trippleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Vector3 offset = new Vector3(0, 1.05f, 0);
            Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
        }
        
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.AddScore(_score);
    }

    void calculateMovement()
    {
        float _runSpeed;
        if (_speedBoost == true)
        {
            _runSpeed = _boostSpeed;
        }
        else
        {
            _runSpeed = _speed;
        }
        var moveDirection = moveAction.ReadValue<Vector2>();
        transform.Translate(moveDirection * _runSpeed * Time.deltaTime);

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
}
