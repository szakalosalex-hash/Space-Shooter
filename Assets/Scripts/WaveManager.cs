using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private Spawn_Manager _spawnManager;

    [SerializeField]
    private UIManager _uiManager;

    private bool _isWaveSpawning = false;
    private bool _isFirstWaveSpawning = false;

    private bool _isWaveCooldownActive = false;

    private int _currentWave = 0;
    private int _waveEnemies = 5;

    private int _enemiesAlive;

    private void Start()
    {
        Debug.Log("Wave manager initialized");
    }
    IEnumerator WaveCooldownRoutine()
    {
        if (_isWaveSpawning || _isWaveCooldownActive)
        {
            yield break;
        }

        _isWaveCooldownActive = true;

        yield return new WaitForSeconds(5f);

        _isWaveCooldownActive = false;

        StartNextWave();
    }

    public void StartFirstWave()
    {
        if (_currentWave > 0 || _isFirstWaveSpawning) return;

        StartCoroutine(FirstWaveStartRoutine());
    }
    private void StartNextWave()
    {
        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager reference is missing!", this);
            return;
        }

        _isWaveSpawning = true;
        _enemiesAlive = 0;
        _currentWave++;
        AddWave();

        _waveEnemies = 5 + (_currentWave - 1) * 2;

        Debug.Log("Wave " + _currentWave + " started! Enemies: " + _waveEnemies);

        _spawnManager.SpawnWave(_waveEnemies);
    }
    public void EnemyDefeated()
    {
        if (_enemiesAlive <= 0) return;

        _enemiesAlive--;
        _uiManager.UpdateEnemies(_enemiesAlive, _waveEnemies);


        if (_enemiesAlive == 0 && _isWaveSpawning == false)
        {
            StartCoroutine(WaveCooldownRoutine());
        }
    }
    public void EnemySpawned()
    {
        _enemiesAlive++;
        _uiManager.UpdateEnemies(_enemiesAlive, _waveEnemies);
    }
    public void WaveSpawningFinished()
    {
        _isWaveSpawning = false;
        Debug.Log("Wave spawning finished");

        if (_enemiesAlive == 0)
        {
            StartCoroutine(WaveCooldownRoutine());
        }
    }

    IEnumerator FirstWaveStartRoutine()
    {
        _isFirstWaveSpawning = true;

        yield return new WaitForSeconds(1.5f);

        StartNextWave();
    }

    public void AddWave()
    {
        if (_uiManager != null)
        {
            _uiManager.UpdateWave(_currentWave);
        }
        else
        {
            Debug.LogError("UIManager reference is missing!", this);
        }
    }
}
