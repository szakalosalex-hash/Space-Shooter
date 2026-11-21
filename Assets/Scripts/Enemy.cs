using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is null!");
        }
        _enemyDeath = GetComponent<Animator>();

        if (_enemyDeath == null)
        {
            Debug.LogError("Animator is null!");
        }

        _audiosource = GetComponent<AudioSource>();

        if (_audiosource == null)
        {
            Debug.LogError("Audiosource is null!");
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
        _enemyDeath.SetTrigger("OnEnemyDeath");
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    private void Die()
    {
        if (_isdead) return;

        _isdead = true;
        _speed = 0;

        GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DestroyAfterAnim());
        _audiosource.PlayOneShot(_explosion);
    }
}
