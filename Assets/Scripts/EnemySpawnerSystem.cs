using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Random = UnityEngine.Random;

[BurstCompile]
partial struct EnemySpawnerSystem : ISystem
{
    private EntityManager _entityManager;
    private Entity _enemySpawnerEntity;
    private EnemySpawnerComponent _enemySpawnerComponent;
    private Entity _playerEntity;

    private Unity.Mathematics.Random _random;
    
    // [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerComponent>();
        state.RequireForUpdate<EnemySpawnerComponent>();

        _random = Unity.Mathematics.Random.CreateFromIndex((uint)_enemySpawnerComponent.GetHashCode());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _enemySpawnerEntity = SystemAPI.GetSingletonEntity<EnemySpawnerComponent>();
        _enemySpawnerComponent = _entityManager.GetComponentData<EnemySpawnerComponent>(_enemySpawnerEntity);

        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        
        SpawnEnemies(ref state);
    }

    private void SpawnEnemies(ref SystemState state)
    {
        _enemySpawnerComponent.currentTimeBeforeSpawn -= SystemAPI.Time.DeltaTime;
        if (_enemySpawnerComponent.currentTimeBeforeSpawn <= 0f)
        {
            for (int i = 0; i < _enemySpawnerComponent.numOfEnemiesToSpawnPerSecond; i++)
            {
                EntityCommandBuffer entityCommandBuffer  = new EntityCommandBuffer(Allocator.Temp);
                Entity enemyEntity = _entityManager.Instantiate(_enemySpawnerComponent.enemyPrefabToSpawn);

                LocalTransform enemyTransform = _entityManager.GetComponentData<LocalTransform>(enemyEntity);
                LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

                //random spawn point
                
                float minDistanceSquared = _enemySpawnerComponent.minDistanceFromPlayer *
                                           _enemySpawnerComponent.minDistanceFromPlayer;
                
                float2 randomOffset = _random.NextFloat2Direction() *
                                      _random.NextFloat(_enemySpawnerComponent.minDistanceFromPlayer,
                                          _enemySpawnerComponent.enemySpawnRadius);
                
                float3 playerPos = playerTransform.Position;
                float3 spawnPos = playerPos + new float3(randomOffset.x, 0, randomOffset.y);
                float distanceSquared = math.lengthsq(spawnPos - playerPos);
                
                if (distanceSquared < minDistanceSquared)
                {
                    spawnPos = playerPos + math.normalize(new float3(randomOffset.x, 0, randomOffset.y)) * math.sqrt(minDistanceSquared);
                }
                enemyTransform.Position = spawnPos;

                
                // spawn look direction

                float3 direction = math.normalize(playerTransform.Position - enemyTransform.Position);
                float angle = math.atan2(direction.z, direction.x) - math.PI/2;
                quaternion lookRot = quaternion.AxisAngle(new float3(0,-1,0), angle);
                enemyTransform.Rotation = lookRot;
                
                entityCommandBuffer.SetComponent(enemyEntity, enemyTransform);
                
                
                
                entityCommandBuffer.AddComponent(enemyEntity, new EnemyComponent
                {
                    currentHealth = 100f,
                    enemySpeed = 1.25f,
                });
                
                entityCommandBuffer.Playback(_entityManager);
                entityCommandBuffer.Dispose();
            }

            int desiredEnemiesPerWave = _enemySpawnerComponent.numOfEnemiesToSpawnPerSecond +
                                        _enemySpawnerComponent.numOfEnemiesToSpawnIncrementAmount;
            int enemiesPerWave = math.min(desiredEnemiesPerWave,
                _enemySpawnerComponent.maxNumberOfEnemiesToSpawnPerSecond);

            _enemySpawnerComponent.numOfEnemiesToSpawnPerSecond = enemiesPerWave;

            _enemySpawnerComponent.currentTimeBeforeSpawn = _enemySpawnerComponent.timeBeforeNextSpawn;
        }
        _entityManager.SetComponentData(_enemySpawnerEntity, _enemySpawnerComponent);
    }

    // [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}