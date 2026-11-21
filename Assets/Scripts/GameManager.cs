using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool _isgameOver;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isgameOver)
        {
            SceneManager.LoadScene(1, LoadSceneMode.Single); //current game scene
            _isgameOver = false;
        }
    }

    public void GameOver()
    {
        _isgameOver = true;
    }
}
