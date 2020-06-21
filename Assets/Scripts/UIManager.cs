using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
[RequireComponent(typeof(PlayerInput))]
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text ammoText;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private GameObject _gameOverText;
    [SerializeField]
    private GameObject _restartText;
    private bool _gameOverBool = false;
    private gameManager _gameManager;

    [SerializeField]
    private Image pauseImage;
    [SerializeField]
    private Image fireImage;
    [SerializeField]
    private Image thrusterImage;
    [SerializeField]
    private Image pauseImage1;
    [SerializeField]
    private Image fireImage1;
    [SerializeField]
    private Image thrusterImage1;

    // refs to your sprites
    public Sprite gamepadPauseImage;
    public Sprite keyboardPauseImage;
    public Sprite gamepadFireImage;
    public Sprite keyboardFireImage;
    public Sprite gamepadThrusterImage;
    public Sprite keyboardThrusterImage;


    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + 0;
        _gameOverText.SetActive(false);
        _restartText.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<gameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is Null");
        }

    }


    public void updateButtonImage(string schemeName)
    {
        // assuming you have only 2 schemes: keyboard and gamepad
        if (schemeName.Equals("Keyboard"))
        {
            pauseImage.sprite = keyboardPauseImage;
            fireImage.sprite = keyboardFireImage;
            thrusterImage.sprite = keyboardThrusterImage;

        }
        else if (schemeName.Equals("Gamepad"))
        {
            pauseImage.sprite = gamepadPauseImage;
            fireImage.sprite = gamepadFireImage;
            thrusterImage.sprite = gamepadThrusterImage;
        }
    }

    public void AddScore(int points)
    {
        scoreText.text = "Score: " + points;
    }

    public void SetAmmo(int points,int max)
    {
        ammoText.text = points + "/" + max;
    }

    public void UpdateLives(int currentlives)
    {
        _livesImage.sprite = _liveSprites[currentlives];
        if (currentlives == 0)
        {
            _restartText.SetActive(true);
            _gameManager.isGameOver();
            StartCoroutine(GameOver());
        }

    }

    private IEnumerator GameOver()
    {
        while (true)
        {

            _gameOverBool = !_gameOverBool;
            _gameOverText.SetActive(_gameOverBool);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
