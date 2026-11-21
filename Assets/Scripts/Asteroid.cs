
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotationspeed = 75f;

    private Player _player;

    [SerializeField]
    private int _lives = 4;

    [SerializeField]
    private GameObject _explosionPrefab;

    private bool _isdead = false;

    private Spawn_Manager _spawnManager;
    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        //player init
        if (playerObject != null)
        {
            _player = playerObject.GetComponent<Player>();
        }

        if (_player == null)
        {
            Debug.LogError("Player is null!");
        }

        //spawnManager init
        GameObject spawnObject = GameObject.Find("Spawn_Manager");
        if (spawnObject != null)
        {
            _spawnManager = spawnObject.GetComponent<Spawn_Manager>();
        }
        else
        {
            Debug.LogError("SpawnManager is missing!");
        }
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotationspeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isdead) return;

        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Damage();
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Damage();
        }
    }

    private void Damage()
    {
        if (_isdead) return;

        _lives--;

        if (_lives <= 0)
        {
            Die();
            if (_spawnManager != null)
            {
                _spawnManager.StartSpawning();
            }
        }
    }

    private void Die()
    {
        if (_isdead) return;

        _isdead = true;
        _rotationspeed = 0;

        Collider2D asteroidCollider = GetComponent<Collider2D>();

        if (asteroidCollider != null)
        {
            asteroidCollider.enabled = false;
        }
        else
        {
            Debug.LogError("Asteroid Collider is missing!", this);
        }

        if (_explosionPrefab != null)
        {
            Instantiate(_explosionPrefab,
                transform.position,
                Quaternion.identity);
        }
        else
        {
            Debug.LogError("Explosion prefab is missing!", this);
        }

        Destroy(gameObject);
    }
}
