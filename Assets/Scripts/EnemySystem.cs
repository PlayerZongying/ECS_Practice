using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial struct EnemySystem : ISystem
{
    private EntityManager _entityManager;
    private Entity _playerEntity;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();

        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

        NativeArray<Entity> allEntities = _entityManager.GetAllEntities();

        foreach (Entity entity in allEntities)
        {
            if (_entityManager.HasComponent<EnemyComponent>(entity))
            {
                LocalTransform enemyTransform = _entityManager.GetComponentData<LocalTransform>(entity);
                EnemyComponent enemyComponent = _entityManager.GetComponentData<EnemyComponent>(entity);
                
                float3 moveDir = math.normalize(playerTransform.Position - enemyTransform.Position);
                
                enemyTransform.Position += enemyComponent.enemySpeed * SystemAPI.Time.DeltaTime * moveDir;
                
                
                float3 direction = math.normalize(playerTransform.Position - enemyTransform.Position);
                float angle = math.atan2(direction.z, direction.x) - math.PI/2;
                quaternion lookRot = quaternion.AxisAngle(new float3(0,-1,0), angle);
                enemyTransform.Rotation = lookRot;
                
                _entityManager.SetComponentData(entity,enemyTransform);
            }
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
