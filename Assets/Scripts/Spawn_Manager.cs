using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        _asteroid = Instantiate(_asteroid, new Vector3(0, 4, 0), Quaternion.identity);
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        while (_stopspawning == false)
        {
            Vector3 SpawnPos = new Vector3(Random.Range(-8.5f, 8.5f), 7, 0);

            GameObject newEnemy = Instantiate(EnemyPrefab, SpawnPos + new Vector3(0, 4f, 0), Quaternion.identity);
            newEnemy.transform.parent = EnemyContainer.transform;
            yield return new WaitForSeconds(5.0f);    
        }
    }
    public void OnPlayerDeath()
    {
        _stopspawning = true;
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        while (_stopspawning == false)
        {
            float cooldown = Random.Range(3f, 8f);

            yield return new WaitForSeconds(cooldown);

            if(_stopspawning)
            {
                yield break;
            }

            int randomPowerUp = Random.Range(0, powerUps.Length);

            Vector3 SpawnPos = new Vector3(Random.Range(-8.5f, 8.5f), 7, 0);

            GameObject newTripleShot = Instantiate(powerUps[randomPowerUp], SpawnPos + new Vector3(0, + 4f, 0), Quaternion.identity);
        }
    }

}
