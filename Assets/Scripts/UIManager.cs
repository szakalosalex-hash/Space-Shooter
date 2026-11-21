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
    private Sprite[] _liveSprites;

    [SerializeField]
    private Image _liveImage;

    [SerializeField]
    private Text _GameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _GameManager;
    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _GameOverText.gameObject.SetActive(false);
        _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_GameManager == null)
        {
            Debug.Log("_GameManager is null!");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }
    public void UpdateLives(int currentlives)
    {
        _liveImage.sprite = _liveSprites[currentlives];
    }
    public void GameOverText()
    {
        StartCoroutine(GameOverTextFlicker());
    }

    IEnumerator GameOverTextFlicker()
    {
        while (true)
        {
            _GameManager.GameOver();
            _restartText.gameObject.SetActive(true);
            _GameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _GameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }    
}
