using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform enemyParent;
    private PlayerStats player;

    NativeList<EnemyData> enemyDataList;
    NativeArray<float2> newPositions;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        enemyDataList = new NativeList<EnemyData>(Allocator.Persistent);
    }

    private void Update()
    {
        enemyDataList.Clear();
        

        foreach (var enemy in enemyParent.GetComponentsInChildren<Enemy>())
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                Transform enemyTransform = enemy.transform;

                EnemyData enemyData = new EnemyData
                {
                    position = enemyTransform.position,
                    playerPosition = player.transform.position,
                    speed = enemy.GetSpeed(),
                    isSingleDirectional = enemy.GetMoveMethod() ? 1 : 0,
                    moveDirection = enemy.GetMoveTarget()
                    //moveDirection = 
                };

                enemyDataList.Add(enemyData);
            }
        }
        newPositions = new NativeArray<float2>(enemyDataList.Length, Allocator.Persistent);

        EnemyJob enemyJob = new EnemyJob
        {
            inputData = enemyDataList.AsArray(),
            newPositions = newPositions,
            deltaTime = Time.deltaTime
        };

        JobHandle jobHandle = enemyJob.Schedule(enemyDataList.Length, 64);
        jobHandle.Complete();

        // Apply result;
        int index = 0;
        foreach (var enemy in enemyParent.GetComponentsInChildren<Enemy>())
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                enemy.transform.position = (Vector2)newPositions[index];

                index++;
            }
        }
    }

    private void OnDisable()
    {
        enemyDataList.Dispose();
        newPositions.Dispose();
    }
}

public struct EnemyData
{
    public Vector2 position;
    public Vector2 moveDirection;       // for single-directional
    public Vector2 playerPosition;
    public float speed;
    public int isSingleDirectional;
}

[BurstCompile]
public struct EnemyJob : IJobParallelFor
{
    [ReadOnly] public NativeArray<EnemyData> inputData;
    public NativeArray<float2> newPositions;
    public float deltaTime;


    public void Execute(int index)
    {
        EnemyData enemy = inputData[index];

        float2 direction;
        if (enemy.isSingleDirectional == 0)     // if not single directional
        {
            direction = math.normalize(enemy.playerPosition - enemy.position);
        }
        else   // if single directional
        {
            direction = math.normalize(enemy.moveDirection);
        }

        float2 newPosition = (float2)enemy.position + direction * enemy.speed * deltaTime;

        newPositions[index] = newPosition;
    }
}