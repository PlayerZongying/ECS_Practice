using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;


partial struct PlayerSystem : ISystem
{
    private EntityManager _entityManager;

    private Entity _playerEntity;
    private Entity _inputEntity;
    
    private PlayerComponent _playerComponent;
    private InputComponent _inputComponent;
    
    
    // [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InputComponent>();
        state.RequireForUpdate<PlayerComponent>();
    }

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        _inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();

        _playerComponent = _entityManager.GetComponentData<PlayerComponent>(_playerEntity);
        _inputComponent = _entityManager.GetComponentData<InputComponent>(_inputEntity);
        
        
        Move(ref state);
        Shoot(ref state);

    }

    private void Move(ref SystemState state)
    {
        LocalTransform playerTransform = state.EntityManager.GetComponentData<LocalTransform>(_playerEntity);
        playerTransform.Position +=
            new float3(_inputComponent.movement.x, 0, _inputComponent.movement.y) * _playerComponent.moveSpeed * SystemAPI.Time.DeltaTime;
        
        
        Vector2 dir = (Vector2)_inputComponent.mousePosition -
                      (Vector2)Camera.main.WorldToScreenPoint(playerTransform.Position);
        float angle = math.degrees(math.atan2(dir.y, dir.x)) ;
        
        playerTransform.Rotation = Quaternion.AngleAxis(angle, Vector3.up);
        state.EntityManager.SetComponentData(_playerEntity,playerTransform);

        // Debug.Log("Dir: " + _inputComponent.mousePosition.ToString());

    }
    
    private void Shoot(ref SystemState state)
    {
        
    }

    // [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
