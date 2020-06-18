using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Player Variables
    [SerializeField]
    private int _health = 3;
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _boostSpeed = 5.0f;
    private bool _speedBoost = false;
    bool _thrusters;
    float _thrusterTime = 10f;
    private float _thrustspeed;
    [SerializeField]
    private GameObject _thrusterEffect;
    [SerializeField]
    private Image _thrusterBar;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _laserCoolDown = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private AudioClip _LaserFireClip;
    [SerializeField]
    private bool _trippleShot = false;
    [SerializeField]
    private GameObject _trippleShotPrefab;
    [SerializeField]
    private int _shields = 0;
    private GameObject _shieldObj;
    private Color _color_Green = new Color(0f,1f,0.1039953f,1f);
    private Color _color_Yellow = new Color(1f, 0.8287185f, 0f, 1f);
    private Color _color_Red = new Color(1f, 0.1072823f, 0f, 1f);
    private PlayerInputActions _playerControls;
    private ButtonControl _buttonControl;
    private spwanManager _spawnManager;
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightWing;
    [SerializeField]
    private GameObject _leftWing;
    private AudioSource _audioSource;

    // Start is called before the first frame update

    private void Awake()
    {
        _playerControls = new PlayerInputActions();
        _buttonControl = (ButtonControl)_playerControls.player.trusters.controls[0];
    }
    private void OnEnable()
    {
        _playerControls.Enable();
    }
    private void OnDisable()
    {
        _playerControls.Disable();
    }
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _shieldObj = transform.Find("Shields").gameObject;
        _shieldObj.SetActive(false);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<spwanManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _thrustspeed = _speed * 2;
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
            if (_playerControls.player.fire.triggered && Time.time > _canFire)
            {
                shootLaser();
            }

            if (_buttonControl.isPressed && _thrusterTime > 0f)
            {
                if (_thrusters == false && _thrusterTime < 10f)
                {
                    ThrustersNotPressed();
                }
                else
                {
                    ThrustersPressed();
                }
                
            }
            else
            {
                ThrustersNotPressed();
            }
        }
    }

    void ThrustersPressed()
    {
        _thrusters = true;
        _thrusterTime -= Time.deltaTime;
        _thrusterBar.fillAmount = _thrusterTime/10;
        if (_thrusterEffect.activeSelf != true)
        _thrusterEffect.SetActive(true);
    }

    void ThrustersNotPressed()
    {
        if (_thrusterTime < 10f)
        {
            _thrusters = false;
            if (_thrusterEffect.activeSelf != false)
                _thrusterEffect.SetActive(false);
            _thrusterTime += Time.deltaTime;
            _thrusterBar.fillAmount = _thrusterTime/10;
        }
        else
        {
            _thrusterTime = 10f;
        }

    }

    public void powerUp_TripleShot()
    {
        if (_trippleShot == true)
        {
            StopCoroutine(TrippleShotTimer());
        }
        _trippleShot = true;
        StartCoroutine(TrippleShotTimer());
    }

    public void powerUp_Speed()
    {
        if (_speedBoost == true)
        {
            StopCoroutine(SpeedTimer());
        }
        _speedBoost = true;
        StartCoroutine(SpeedTimer());

    }

    public void powerUp_Shield()
    {
        _shields = 3;
        _shieldObj.GetComponent<SpriteRenderer>().color = _color_Green;
        _shieldObj.SetActive(true);
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
        if (_shields != 0)
        {
            _shields--;
            switch(_shields)
            {
                case 2:
                    _shieldObj.GetComponent<SpriteRenderer>().color = _color_Yellow;
                    break;
                case 1:
                    _shieldObj.GetComponent<SpriteRenderer>().color = _color_Red;
                    break;
                case 0:
                    _shieldObj.SetActive(false);
                    break;
            }
        }
        else
        {
            _health--;
            if (_health == 2)
            {
                _rightWing.SetActive(true);
            }
            else if (_health == 1)
            {
                _leftWing.SetActive(true);
            }
            else
            {
                _leftWing.SetActive(false);
                _rightWing.SetActive(false);
            }
            _uiManager.UpdateLives(_health);
            if (_health < 1)
            {
                _spawnManager.onPlayerDeath();
                Destroy(this.gameObject);
            }
        }
    }

    void shootLaser()
    {
        _audioSource.Play();
        _canFire = Time.time + _laserCoolDown;

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
        float _runSpeed = 0f;
        if (_thrusters == true)
        {
            _runSpeed += _thrustspeed;
        }
        else
        {
            _runSpeed += _speed;
        }
        if (_speedBoost == true)
        {
            _runSpeed += _boostSpeed;
        }

        var moveDirection = _playerControls.player.movement.ReadValue<Vector2>();
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
