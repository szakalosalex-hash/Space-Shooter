using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private float _firerate = 0.15f;
    private float nextFire = 0.0f;

    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private int _score;

    private Spawn_Manager _spawnManager;
    private UIManager UIManager;

    private bool _tSActive = false;
    private bool _speedActive = false;
    private bool _shieldActive = false;

    [SerializeField]
    private GameObject tSPrefab;
    [SerializeField]
    private GameObject playerShield;
    [SerializeField]
    private GameObject laserPrefab;
    [SerializeField]
    private GameObject[] damaged;

    [SerializeField]
    private AudioSource _audiosource;
    [SerializeField]
    private AudioClip _laser;
    [SerializeField]
    private AudioClip _explosion;
    [SerializeField]
    private AudioClip _powerup;

    private Coroutine _tripleshotcoroutine;
    private Coroutine _speedcoroutine;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, -3.5f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<Spawn_Manager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawnmanager is null!");
        }
        playerShield.SetActive(false);
        UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput;
        float verticalInput;

        #if UNITY_ANDROID && !UNITY_EDITOR

         horizontalInput = SimpleInput.GetAxis("Horizontal");
         verticalInput = SimpleInput.GetAxis("Vertical");
        #else
             horizontalInput = Input.GetAxis("Horizontal");
             verticalInput = Input.GetAxis("Vertical");
        #endif
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_speedActive == true)
        {
            transform.Translate(direction * _speed * _speedMultiplier * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * _speed * Time.deltaTime);

        }
        Vector3 pos = transform.position;

        if (Mathf.Abs(pos.y) > 4.7f)
            pos.y = Mathf.Clamp(pos.y, -4.7f, 6.5f);

        if (Mathf.Abs(pos.x) > 11.3f)
            pos.x = -pos.x;

        transform.position = pos;
    }

    public void FireLaser()
    {
        if (Time.time < nextFire)
            return;

            nextFire = Time.time + _firerate;

        if (_tSActive == true)
        {
            Instantiate(tSPrefab, transform.position + new Vector3(0, 1.03f, 0), Quaternion.identity);
        }
        else 
        {
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1.03f, 0), Quaternion.identity);
        }
        _audiosource.PlayOneShot(_laser);
    }
    public void Damage()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            playerShield.SetActive(false);
            return;
        }

        _lives--;
        UIManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            damaged[0].SetActive(true);
        }
        else if (_lives == 1)
        {
            damaged[1].SetActive(true);
        }

        if (_lives <= 0)
        {
            _spawnManager.OnPlayerDeath();
            UIManager.GameOverText();
            Destroy(gameObject);
            _audiosource.PlayOneShot(_explosion);
        }
    }
    public void TsActive()
    {
        _tSActive = true;

        if (_tripleshotcoroutine != null)
        {
            StopCoroutine(_tripleshotcoroutine);
        }
        _tripleshotcoroutine = StartCoroutine(TripleShotPowerDownRoutine());
    }
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _tSActive = false;
        _tripleshotcoroutine = null;
    }
    public void SpeedActive()
    {
        _speedActive = true;

        if (_speedcoroutine != null)
        {

            StopCoroutine(_speedcoroutine);
        }
        _speedcoroutine = StartCoroutine(SpeedPowerDownroutine());
    }
    IEnumerator SpeedPowerDownroutine()
    {
        yield return new WaitForSeconds(5f);
        _speedActive = false;
        _speedcoroutine = null;
    }

    public void ShieldActive()
    {
        if (_shieldActive != true)
        {
            _shieldActive = true;
            playerShield.SetActive(true);
        }
    }
    public void AddScore(int points)
    {
        _score += points;
        UIManager.UpdateScore(_score);

    }
    public void PlayPowerUpSound()
    {
        _audiosource.PlayOneShot(_powerup);
    }
}
