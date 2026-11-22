using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 2f;

    [SerializeField]
    private float horizontalLimit = 8.5f;

    [SerializeField]
    private float _targetY = 3.5f;

    [SerializeField]
    private float _entryspeed = 3f;

    [SerializeField]
    private float _health = 50;

    private float _currentHealth;

    private bool _movingRight = true;
    private bool _isentering = true;
    private bool _isdying = false;

    [SerializeField]
    private GameObject _bossLaserPrefab;

    [SerializeField]
    private float _fireRate = 2f;

    private float _nextFireTime = 0f;

    [SerializeField]
    private Transform _leftFirePoint1;

    [SerializeField]
    private Transform _leftFirePoint2;

    [SerializeField]
    private Transform _rightFirePoint1;

    [SerializeField]
    private Transform _rightFirePoint2;

    private UIManager UIManager;

    private WaveManager WaveManager;

    //X axis limits: -8.5f, 8.5f
    //Y axis limits: 3f, 3.8f

    //spawn position: X0,Y8

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _health;

        GameObject waveManagerObject = GameObject.Find("Wave_Manager");

        if (waveManagerObject != null)
        {
            WaveManager = waveManagerObject.GetComponent<WaveManager>();

            if (WaveManager == null)
            {
                Debug.LogError("WaveManager component is missing!");
            }
        }
        else
        {
            Debug.LogError("WaveManager GameObject is missing!");
        }

        GameObject canvasObject = GameObject.Find("Canvas");

        if (canvasObject != null)
        {
            UIManager = canvasObject.GetComponent<UIManager>();
            
            if (UIManager == null)
            {
                Debug.LogError("UIManager component is missing!");
            }
        }
        else
        {
            Debug.LogError("Canvas GameObject is missing!");
        }
        if (UIManager != null)
        {
            UIManager.ShowBossHP(_currentHealth, _health);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (_isentering)
        {
            transform.Translate(Vector3.down * _entryspeed * Time.deltaTime);
            pos = transform.position;
            if (pos.y <= _targetY)
            {
                pos.y = _targetY;
                transform.position = pos;
                _isentering = false;
            }
        }
        else if (_movingRight)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
            pos = transform.position;
            if (pos.x >= horizontalLimit)
            {
                _movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
            pos = transform.position;

            if (pos.x <= -horizontalLimit)
            {
                _movingRight = true;
            }
        }

        if (!_isentering)
        {
            FireLaser();
        }
    }

    public void Damage()
    {
        if (_currentHealth <= 0 || _isdying) return;

        _currentHealth--;

        if (UIManager == null)
        {
            Debug.LogError("UIManager component is missing!");
        }
        else
        {
            UIManager.UpdateBossHP(_currentHealth, _health);
        }

        if (_currentHealth <= 0)
        {
            if (UIManager == null)
            {
                Debug.LogError("UIManager component is missing!");
            }
            else
            {
                UIManager.HideBossHPBar();
            }
            Die();
        }
    }
    public void Die()
    {
        if (_isdying) return;

        _isdying = true;
        Destroy(gameObject);

        if (WaveManager == null) return;

        WaveManager.WaveSpawningFinished();
    }

    private void FireLaser()
    {
        if (Time.time < _nextFireTime)
            return;

        _nextFireTime = Time.time + _fireRate;

        if (_bossLaserPrefab == null)
        {
            Debug.LogError("Boss Laser prefab is missing!", this);
            return;
        }

        int leftIndex = Random.Range(0, 2);
        int rightIndex = Random.Range(0, 2);

        Transform leftPoint = leftIndex == 0 ? _leftFirePoint1 : _leftFirePoint2;
        Transform rightPoint = rightIndex == 0 ? _rightFirePoint1 : _rightFirePoint2;

        Instantiate(_bossLaserPrefab, leftPoint.position, Quaternion.identity);
        Instantiate(_bossLaserPrefab, rightPoint.position, Quaternion.identity);
    }
}
