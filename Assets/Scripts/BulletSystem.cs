using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;
using Unity.Mathematics;

[BurstCompile]
partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PhysicsWorldSingleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();

        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        

        foreach (Entity entity in allEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity) && entityManager.HasComponent<BulletLifeTimeComponent>(entity))
            {
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);

                // move bullet
                bulletTransform.Position += bulletComponent.speed * SystemAPI.Time.DeltaTime * bulletTransform.Forward();
                entityManager.SetComponentData(entity, bulletTransform);
                
                // reduce lifetime
                BulletLifeTimeComponent bulletLifeTimeComponent =
                    entityManager.GetComponentData<BulletLifeTimeComponent>(entity);
                bulletLifeTimeComponent.remainingLifeTime -= SystemAPI.Time.DeltaTime;

                if (bulletLifeTimeComponent.remainingLifeTime <= 0f)
                {
                    entityManager.DestroyEntity(entity);
                    continue;
                }
                entityManager.SetComponentData(entity, bulletLifeTimeComponent);
                
                //physics
                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
                float3 point1 = new float3(bulletTransform.Position - bulletTransform.Forward() * 0.015f);
                float3 point2 = new float3(bulletTransform.Position + bulletTransform.Forward() * 0.015f);

                physicsWorldSingleton.CapsuleCastAll(point1, point2, bulletComponent.size / 20, float3.zero, 1f,
                    ref hits, new CollisionFilter
                    {
                        BelongsTo = (uint)CollisionLayer.Default,
                        CollidesWith = (uint)CollisionLayer.Wall
                    });

                if (hits.Length > 0)
                {
                    entityManager.DestroyEntity(entity);
                }

                hits.Dispose();

            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
