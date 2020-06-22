using DigitalRuby.LightningBolt;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class Player : MonoBehaviour, PlayerInputActions.IPlayerActions
{
    // Player Variables
    [SerializeField]
    private float shakeTime = 0.5f;
    [SerializeField]
    private float shakeAmount = 0.1f;
    private int _health;
    [SerializeField]
    private int _max_health = 3;
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
    private int _ammoMax = 15;
    [SerializeField]
    private int _ammo = 15;
    [SerializeField]
    private int _ammoPowerupAmount;
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
    private bool _lightningShot = false;
    [SerializeField]
    private GameObject _trippleShotPrefab;
    [SerializeField]
    private int _shields = 0;
    private GameObject _shieldObj;
    private Color _color_Green = new Color(0f, 1f, 0.1039953f, 1f);
    private Color _color_Yellow = new Color(1f, 0.8287185f, 0f, 1f);
    private Color _color_Red = new Color(1f, 0.1072823f, 0f, 1f);
    private PlayerInputActions _playerControls;
    private ButtonControl _buttonControl;
    private spawnManager _spawnManager;
    private int _score;
    private UIManager _uiManager;
    [SerializeField]
    private GameObject _rightWing;
    [SerializeField]
    private GameObject _leftWing;
    private AudioSource _audioSource;
    float _isPressed = 0;
    private bool _neg_Powerup_Brain = false;
    Vector2 moveDirection;

    // Start is called before the first frame update

    private void Awake()
    {
        _playerControls = new PlayerInputActions();
        _playerControls.player.SetCallbacks(this);
        _buttonControl = (ButtonControl)_playerControls.player.trusters.controls[0];
    }
    private void OnEnable()
    {
        _playerControls.player.Enable();

    }
    private void OnDisable()
    {
        _playerControls.player.Disable();

    }


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _health = _max_health;
        _shieldObj = transform.Find("Shields").gameObject;
        _shieldObj.SetActive(false);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<spawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _thrustspeed = _speed * 2;
        _ammo = _ammoMax;
        _uiManager.SetAmmo(_ammo, _ammoMax);
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
            if (_isPressed > 0 && _thrusterTime > 0f)
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

        if (_lightningShot == true)
        {
            this.GetComponentInChildren<LightningBoltScript>().Trigger();
        }

    }


    void ThrustersPressed()
    {
        _thrusters = true;
        _thrusterTime -= Time.deltaTime;
        _thrusterBar.fillAmount = _thrusterTime / 10;
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
            _thrusterBar.fillAmount = _thrusterTime / 10;
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

    public void powerUp_LightningShot()
    {
        if (_lightningShot == true)
        {
            StopCoroutine(LightningShotTimer());
        }
        _lightningShot = true;
        StartCoroutine(LightningShotTimer());
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

    public void powerUp_Ammo()
    {
        _ammo += _ammoPowerupAmount;
        if (_ammo > _ammoMax) _ammo = _ammoMax;
        _uiManager.SetAmmo(_ammo, _ammoMax);
    }

    public void powerUp_Health()
    {
        if (_health < _max_health)
        {
            _health++;
            _uiManager.UpdateLives(_health);
        }
    }

    public void Neg_Powerup_Brain()
    {
        _neg_Powerup_Brain = true;
        StartCoroutine(negBrainTimer());
    }

    private IEnumerator negBrainTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _neg_Powerup_Brain = false;
    }

    private IEnumerator TrippleShotTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _trippleShot = false;
    }

    private IEnumerator LightningShotTimer()
    {
        yield return new WaitForSeconds(5.0f);
        _lightningShot = false;
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
            switch (_shields)
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
            CameraShake.Shake(shakeTime, shakeAmount);
            StartCoroutine(Controlerrumble());
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
                _spawnManager.Explode(transform.position);
                InputSystem.ResetHaptics();
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator Controlerrumble()
    {
        Gamepad.current.SetMotorSpeeds(0.25f, 0.75f);
        yield return new WaitForSeconds(shakeTime);
        InputSystem.ResetHaptics();
    }

    void shootLaser()
    {
        if (_ammo > 0)
        {
            --_ammo;
            _uiManager.SetAmmo(_ammo, _ammoMax);
            _audioSource.Play();
            _canFire = Time.time + _laserCoolDown;

            if (_trippleShot == true)
            {
                //Vector3 offset = new Vector3(0, 0, 0);
                Instantiate(_trippleShotPrefab, transform.position, Quaternion.identity);
            }
            else if (_lightningShot == true)
            {

                this.GetComponentInChildren<LightningBoltScript>().Shoot();
            }
            else
            {
                Vector3 offset = new Vector3(0, 1.05f, 0);
                Instantiate(_laserPrefab, transform.position + offset, Quaternion.identity);
            }

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

        if (_neg_Powerup_Brain == true)
        {
            transform.Translate(-moveDirection * _runSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(moveDirection * _runSpeed * Time.deltaTime);
        }

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -11.0f)
        {
            transform.position = new Vector3(transform.position.x, -11.0f, 0);
        }

        if (transform.position.x > 21.0)
        {
            transform.position = new Vector3(-21.0f, transform.position.y, 0);
        }
        else if (transform.position.x < -21.0f)
        {
            transform.position = new Vector3(21.0f, transform.position.y, 0);
        }
    }


    public void OnMovement(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        var scheme = InputControlScheme.FindControlSchemeForDevice(device, context.action.actionMap.controlSchemes);
        if (scheme.HasValue)
        {
            _uiManager.updateButtonImage(scheme.Value.name);
        }
        else

        {
            _uiManager.updateButtonImage("none");
        }

        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        var scheme = InputControlScheme.FindControlSchemeForDevice(device, context.action.actionMap.controlSchemes);
        if (scheme.HasValue)
        {
            _uiManager.updateButtonImage(scheme.Value.name);
        }
        else

        {
            _uiManager.updateButtonImage("none");
        }
        if (Time.timeScale != 0)
        {
            if (context.performed && Time.time > _canFire)
            {
                shootLaser();
            }
        }
    }

    public void OnTrusters(InputAction.CallbackContext context)
    {
        var device = context.control.device;
        var scheme = InputControlScheme.FindControlSchemeForDevice(device, context.action.actionMap.controlSchemes);
        if (scheme.HasValue)
        {
            _uiManager.updateButtonImage(scheme.Value.name);
        }
        else

        {
            _uiManager.updateButtonImage("none");
        }

        _isPressed = context.ReadValue<float>();

    }
}
