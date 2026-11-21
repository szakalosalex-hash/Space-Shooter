using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool _isgameOver;

    [SerializeField]
    private string _gameSceneName = "Game";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isgameOver)
        {
            RestartGame();
        }
    }

    public void GameOver()
    {
        _isgameOver = true;
    }

    private void RestartGame()
    {
        if (string.IsNullOrEmpty(_gameSceneName))
        {
            Debug.LogError("Game scene name is missing!", this);
            return;
        }

        if (!Application.CanStreamedLevelBeLoaded(_gameSceneName))
        {
            Debug.LogError("Game scene cannot be loaded: " + _gameSceneName, this);
            return;
        }

        SceneManager.LoadScene(_gameSceneName, LoadSceneMode.Single);
    }
}
