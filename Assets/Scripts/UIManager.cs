using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Text _waveText;

    [SerializeField]
    private Text _enemyNumberText;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _liveImage;

    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _GameManager;

    private WaveManager _waveManager;

    private Coroutine _gameOverCoroutone;
    // Start is called before the first frame update
    void Start()
    {
        if (_enemyNumberText != null)
        {
            _enemyNumberText.text = "Enemies: 0";
        }
        else
        {
            Debug.LogError("Enemies Text is missing!", this);
        }

        if (_scoreText != null)
        {
            _scoreText.text = "Score: 0";
        }
        else
        {
            Debug.LogError("Score Text is missing!", this);
        }

        if (_waveText != null)
        {
            _waveText.text = "Wave: 0";
        }
        else
        {
            Debug.LogError("Wave Text is missing!", this);
        }

        if (_GameOverText != null)
        {
            _GameOverText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("Game Over text is missing!", this);
        }

        GameObject gameManagerObject = GameObject.Find("GameManager");

        if (gameManagerObject != null)
        {
            _GameManager = gameManagerObject.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManager reference is missing!", this);
        }

        GameObject waveManager = GameObject.Find("Wave_Manager");

        if (waveManager != null)
        {
            _waveManager = waveManager.GetComponent<WaveManager>();
        }
        else
        {
            Debug.LogError("WaveManager reference is missing!", this);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int score)
    {
        if (_scoreText == null)
        {
            Debug.LogError("Score Text is missing!", this);
            return;
        }

        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateWave(int wave)
    {
        if (_waveText == null)
        {
            Debug.LogError("Wave Text is missing!", this);
            return;
        }

        _waveText.text = "Wave: " + wave.ToString();
    }

    public void UpdateEnemies(int alive, int total)
    {
       if (_enemyNumberText == null)
        {
            Debug.LogError("Enemies Text is missing!", this);
            return;
        }
       _enemyNumberText.text = "Enemies: " + alive + "/" + total;
    }
    public void UpdateLives(int currentlives)
    {
        if (_liveImage == null)
        {
            Debug.LogError("Live Image is missing!", this);
            return;
        }

        if (_liveSprites == null ||
            currentlives < 0 ||
            currentlives >= _liveSprites.Length)
        {
            Debug.LogError("Invalid lives index: " + currentlives, this);
            return;
        }

        _liveImage.sprite = _liveSprites[currentlives];
    }
    public void GameOverText()
    {
        if (_gameOverCoroutone != null)
        {
            return;
        }

        if (_GameOverText == null || _restartText == null)
        {
            Debug.LogError("Game Over UI references are missing!", this);
            return;
        }

        _gameOverCoroutone = StartCoroutine(GameOverTextFlicker());
    }

    IEnumerator GameOverTextFlicker()
    {
        if (_GameManager != null)
        {
            _GameManager.GameOver();
        }

        _restartText.gameObject.SetActive(true);

        while (true)
        {
            _GameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            _GameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }    
}
