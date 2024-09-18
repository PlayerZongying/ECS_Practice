using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEditor.Experimental.Rendering;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject enemyPrefabToSpawn;

    public int numOfEnemiesToSpawnPerSecond = 50;
    public int numOfEnemiesToSpawnIncrementAmount = 2;
    public int maxNumberOfEnemiesToSpawnPerSecond = 200;
    public float enemySpawnRadius = 40f;
    public float minDistanceFromPlayer = 5f;

    public float timeBeforeNextSpawn = 2f;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity enemySpawnerEntity = GetEntity(TransformUsageFlags.None);

            AddComponent(enemySpawnerEntity, new EnemySpawnerComponent
            {
                enemyPrefabToSpawn = GetEntity(authoring.enemyPrefabToSpawn, TransformUsageFlags.Dynamic),
                numOfEnemiesToSpawnPerSecond = authoring.numOfEnemiesToSpawnPerSecond,
                maxNumberOfEnemiesToSpawnPerSecond = authoring.maxNumberOfEnemiesToSpawnPerSecond,
                enemySpawnRadius = authoring.enemySpawnRadius,
                minDistanceFromPlayer = authoring.minDistanceFromPlayer,
                timeBeforeNextSpawn = authoring.timeBeforeNextSpawn,
            });
        }
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}