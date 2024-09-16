using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;

        NativeArray<Entity> allEntities = entityManager.GetAllEntities();

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
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
