using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private WaveManager waveManager;
    
    private float _speed = 4.0f;
    [SerializeField]
    private Player _player;

    private bool _isdead = false;

    [SerializeField]
    private Animator _enemyDeath;

    [SerializeField]
    private AudioSource _audiosource;
    [SerializeField]
    private AudioClip _explosion;
     void Start()
    {
        //player init
        GameObject playerObject = GameObject.Find("Player");

        if (playerObject != null)
        {
            _player = playerObject.GetComponent<Player>();
        }
        else
        {
            Debug.LogError("Player is null!");
        }

        _enemyDeath = GetComponent<Animator>();

        //enemy anim check
        if (_enemyDeath == null)
        {
            Debug.LogError("Enemy animator is null!", this);
        }

        _audiosource = GetComponent<AudioSource>();

        //audio check
        if (_audiosource == null)
        {
            Debug.LogError("Audiosource is null!", this);
        }
        if (_explosion == null)
        {
            Debug.LogError("Enemy explosion Audio is missing!", this);
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed *  Time.deltaTime);
        Vector3 pos = transform.position;
        if (pos.y < -6.6f)
        {
            pos.y = 6.6f;
            pos.x = UnityEngine.Random.Range(-8.5f, 8.5f);
            transform.position = pos;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isdead) return;

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();

            }
            Die();
        }

        if (other.CompareTag("Laser"))
        {
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Die();
            Destroy(other.gameObject);
        }
    }

    IEnumerator DestroyAfterAnim()
    {
        if (_enemyDeath != null)
        {
            _enemyDeath.SetTrigger("OnEnemyDeath");
        }
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    private void Die()
    {
       
        if (_isdead) return;

        _isdead = true;

        if (waveManager != null)
        {
            waveManager.EnemyDefeated();
        }
        else
        {
            Debug.LogError("WaveManager reference is null!", this);
        }

        _speed = 0;

        Collider2D enemyCollider = GetComponent<Collider2D>();

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (_audiosource != null && _explosion != null)
        {
            _audiosource.PlayOneShot(_explosion);
        }
        StartCoroutine(DestroyAfterAnim());
    }
    public void SetWaveManager(WaveManager manager)
    {
        waveManager = manager;
    }
}
