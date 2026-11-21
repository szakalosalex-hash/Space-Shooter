using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Spawn_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject EnemyPrefab;
    [SerializeField]
    private GameObject EnemyContainer;
    private bool _stopspawning = false;
    [SerializeField]
    private GameObject TripleShot;
    [SerializeField]
    private GameObject speedUp;
    [SerializeField]
    private GameObject[] powerUps;
    [SerializeField]
    private GameObject _asteroid;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _poweruppickup;

    private bool _isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        if (_asteroid != null)
        {
            _asteroid = Instantiate(_asteroid,
                new Vector3(0, 4, 0),
                Quaternion.identity);
        }
        else
        {
            Debug.LogError("Asteroid prefab is missing!", this);
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null )
        {
            Debug.LogError("Audio Source is missing!", this);
        }
    }

    public void StartSpawning()
    {
        if (_isSpawning || _stopspawning)
        {
            return;
        }

        _isSpawning=true;

        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemyRoutine()
    {

        if (EnemyPrefab == null)
        {
            Debug.LogError("EnemyPrefab is not assigned!", this);
            yield break;
        }

        if (EnemyContainer == null)
        {
            Debug.LogError("EnemyContainer is not assigned!", this);
            yield break;
        }

        yield return new WaitForSeconds(1.5f);
        while (!_stopspawning)
        {
            Vector3 SpawnPos = new Vector3(
                Random.Range(-8.5f, 8.5f),7, 0);

            GameObject newEnemy = Instantiate(
                EnemyPrefab,
                SpawnPos + new Vector3(0, 4f, 0),
                Quaternion.identity);

            newEnemy.transform.SetParent(EnemyContainer.transform);
            yield return new WaitForSeconds(5.0f);    
        }
    }
    public void OnPlayerDeath()
    {
        _stopspawning = true;
    }

    IEnumerator SpawnPowerUpRoutine()
    {

        if (powerUps == null || powerUps.Length == 0)
        {
            Debug.LogError("PowerUps array is empty or missing!", this);
            yield break;
        }

        yield return new WaitForSeconds(1.5f);

        while (!_stopspawning)
        {
            float cooldown = Random.Range(3f, 8f);

            yield return new WaitForSeconds(cooldown);

            if(_stopspawning)
            {
                yield break;
            }

            int randomPowerUp = Random.Range(0, powerUps.Length);

            if (powerUps[randomPowerUp] == null)
            {
                Debug.LogError("Selected Powerup prefab is missing!", this);
                continue;
            }

            Vector3 SpawnPos = new Vector3(
                Random.Range(-8.5f, 8.5f), 7, 0);

             Instantiate(
                 powerUps[randomPowerUp],
                 SpawnPos + new Vector3(0, + 4f, 0),
                 Quaternion.identity);
        }
    }

}
