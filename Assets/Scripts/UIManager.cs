using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
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

    // Update is called once per frame
    void Update()
    {

    }

    public void AddScore(int points)
    {
        scoreText.text = "Score: " + points;
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
