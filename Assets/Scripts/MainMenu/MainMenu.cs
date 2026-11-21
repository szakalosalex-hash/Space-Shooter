using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private int _gameSceneBuildIndex = 1;

    private bool _isloading = false;

    public void LoadGame()
    {
       if (_isloading) return;

       if (!Application.CanStreamedLevelBeLoaded(_gameSceneBuildIndex))
        {
            Debug.LogError("Game scene cannot be loaded, check Build settings!", this);
            return;
        }

        _isloading = true;
        SceneManager.LoadScene(_gameSceneBuildIndex);
    }
}
