using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    private bool _gameOver;
    [SerializeField]
    private InputAction _restartgame;
    [SerializeField]
    private InputAction _quitgame;
    [SerializeField]
    private InputAction _pausegame;
    private bool _paused = false;
    [SerializeField]
    private GameObject _pauseMenu;


    void Start()
    {
        _gameOver = false;
        _restartgame.Enable();
        _quitgame.Enable();
        _pausegame.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (_restartgame.triggered && _gameOver == true)
        {
            SceneManager.LoadScene("Game");
        }
        if (_quitgame.triggered)
        {
            Application.Quit();
        }
        if (_pausegame.triggered)
        {
            GameState();
        }
    }

    public void isGameOver()
    {
        _gameOver = true;
    }

    void GameState()
    {
        
        if (_gameOver == false && _paused == true)
        {
            ResumeGame();
        }
        else if (_gameOver == false && _paused == false)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        _paused = true;
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        _paused = false;
        _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
