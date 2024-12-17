using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum WavePattern { Random, Circular, Horizontal, Vertical, Square}

public class WaveManager : MonoBehaviour
{
    PlayerStats player;
    [SerializeField] HealthBarManager healthBarManager;

    Dictionary<string, ObjectPool<Enemy>> enemyPool;
    [SerializeField] Enemy[] enemiesPrefab;
    [SerializeField] Transform enemyParent;

    [SerializeField] Wave[] enemyWave;

    //Events
    public static Action<Enemy> OnEnemyDieE;
    public static Action OnEnemySpawnedE;

    private void OnEnable()
    {
        GameManager.OnUpdateTimer += CheckTime;
        OnEnemyDieE += KillEnemy;
    }

    private void OnDisable()
    {
        GameManager.OnUpdateTimer -= CheckTime;
        OnEnemyDieE -= KillEnemy;
    }

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    private void Start()
    {
        enemyPool = new();
        foreach (Enemy e in enemiesPrefab)
        {
            ObjectPool<Enemy> _objectPool = new(() =>
            {
                return Instantiate(e, enemyParent);
            }, enemy =>
            {
                enemy.gameObject.SetActive(true);
                enemy.Initialize(player, healthBarManager);
            }, enemy =>
            {
                enemy.gameObject.SetActive(false);
            }, enemy =>
            {
                Destroy(enemy.gameObject);
            }, false, 100, 500);

            enemyPool.Add(e.GetName(), _objectPool);
        }
    }

    public void CheckTime(int time)
    {
        // Start Wave
        for (int i = 0; i < enemyWave.Length; i++)
        {
            if (time > enemyWave[i].startSeconds && time < enemyWave[i].endSeconds && !enemyWave[i].isRunning)
            {
                RunWave(enemyWave[i]);
                continue;
            }

            if (time > enemyWave[i].endSeconds)
            {
                StopWave(enemyWave[i]);
            }
        }
    }

    public void RunWave(Wave _wave)
    {
        StartCoroutine(RunWaveCoroutine(_wave));
    }

    public void StopWave(Wave _wave)
    {
        _wave.isRunning = false;
    }

    public IEnumerator RunWaveCoroutine(Wave _wave)
    {
        Debug.Log("Wave Running");

        _wave.isRunning = true;
        int enemyRandomCount = UnityEngine.Random.Range((int)_wave.enemyCountRange.min, (int)_wave.enemyCountRange.max);
        float enemyRandomTime;
        if (_wave.isSingleSpawn)
        {
            /*for (int i = 0; i < enemyRandomCount; i++)
            {
                var _enemy = SpawnEnemy(_wave.enemyPrefab);
                _enemy.transform.position = RandomizePosition();
            }*/

            SpawnEnemiesInPattern(_wave);

            yield break;
        }

        while (_wave.isRunning)
        {
            enemyRandomTime = UnityEngine.Random.Range(_wave.spawnIntervalRange.min, _wave.spawnIntervalRange.max);

            // Random Pattern
            /*for (int i = 0; i < enemyRandomCount; i++)
            {
                var _enemy = SpawnEnemy(_wave.enemyPrefab);
                _enemy.transform.position = RandomizePosition();
            }*/

            SpawnEnemiesInPattern(_wave);

            yield return new WaitForSeconds(enemyRandomTime);
        }

        Debug.Log("Wave Finished");

        yield return null;
    }

    public Enemy SpawnEnemy(Enemy enemy)
    {
        OnEnemySpawnedE?.Invoke();

        return enemyPool[enemy.GetName()].Get();
    }

    public void SpawnEnemiesInPattern(Wave wave)
    {
        switch (wave.pattern) 
        {
            case WavePattern.Random:
                int enemyRandomCount = UnityEngine.Random.Range((int)wave.enemyCountRange.min, (int)wave.enemyCountRange.max);
                for (int i = 0; i < enemyRandomCount; i++)
                {
                    var _enemy = SpawnEnemy(wave.enemyPrefab);
                    _enemy.transform.position = RandomizePosition();
                }
                break;

            case WavePattern.Circular:
                float angleStep = 360f / wave.enemyCount;
                for (int i = 0; i < wave.enemyCount; i++)
                {
                    Vector2 centerPosition = player.transform.position;

                    float angle = i * angleStep;

                    float angleRad = Mathf.Deg2Rad * angle;

                    float x = centerPosition.x + Mathf.Cos(angleRad) * wave.radius;
                    float y = centerPosition.y + Mathf.Sin(angleRad) * wave.radius;

                    Vector2 spawnPosition = new Vector2(x, y);

                    var _enemy = SpawnEnemy(wave.enemyPrefab);
                    _enemy.transform.position = spawnPosition;
                }
                break;

            case WavePattern.Horizontal:
                for (int i = 0; i < wave.enemyCount; i++)
                {
                    Vector2 startPosition = player.transform.position;
                    startPosition.y += wave.radius;

                    float extendFactor = i % 2 == 0 ? -i : i;
                    Vector2 spawnPosition = startPosition + new Vector2(extendFactor * wave.spacing, 0);

                    var _enemy = SpawnEnemy(wave.enemyPrefab);
                    _enemy.transform.position = spawnPosition;
                }
                break;

            case WavePattern.Vertical:
                for (int i = 0; i < wave.enemyCount; i++)
                {
                    Vector2 startPosition = player.transform.position;
                    startPosition.x += wave.radius;

                    float extendFactor = i % 2 == 0 ? -i : i;
                    Vector2 spawnPosition = startPosition + new Vector2(0, extendFactor * wave.spacing);

                    var _enemy = SpawnEnemy(wave.enemyPrefab);
                    _enemy.transform.position = spawnPosition;
                }
                break;
            case WavePattern.Square:
                break;
        }
    }

    public void KillEnemy(Enemy enemy)
    {
        enemyPool[enemy.GetName()].Release(enemy);
    }

    public Vector2 RandomizePosition()
    {
        Vector2 randomPos = new(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(-15f, 15f));
        while (Vector2.Distance(randomPos, player.transform.position) < 10f)
        {
            randomPos = new(UnityEngine.Random.Range(-15f, 15f), UnityEngine.Random.Range(-15f, 15f));
        }
        return randomPos;
    }
}

[Serializable]
public class Wave
{
    public bool isSingleSpawn;

    public int startSeconds;
    public int endSeconds;

    public Enemy enemyPrefab;
    
    public Range enemyCountRange;
    public Range spawnIntervalRange;

    public bool isRunning;

    public WavePattern pattern;

    [Header("For non random pattern")]
    public float radius;
    public float spacing;
    public int enemyCount;
}

[Serializable]
public class Range
{
    public float min;
    public float max;
}