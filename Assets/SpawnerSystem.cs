using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct SpawnerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
    }
    public void OnDestroy(ref SystemState state)
    {
    }
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        
        foreach (RefRW<Spawner> spawner in SystemAPI.Query<RefRW<Spawner>>())
        {
            if (spawner.ValueRO.nextSpawnTime < SystemAPI.Time.ElapsedTime)
            {
                Entity newEntity = entityCommandBuffer.Instantiate(spawner.ValueRO.prefab);
                float3 pos = new float3(spawner.ValueRO.spawnPosition.x, spawner.ValueRO.spawnPosition.y, 0);
                entityCommandBuffer.SetComponent(newEntity, LocalTransform.FromPosition(pos));
                spawner.ValueRW.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + spawner.ValueRO.spawnRate;
            }
        }
        
        entityCommandBuffer.Playback(state.EntityManager);
    }
}
