using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    private EntityManager _entityManager;

    private Entity _playerEntity;

    private Vector3 _playerPos;

    [SerializeField]private Camera camera;

    [Range(0f, 100f)] public float camaraHeight = 10f;
    [Range(0f, 100f)] public float camaraFollowSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _playerEntity = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<PlayerComponent>())
            .GetSingletonEntity();

    }

    // Update is called once per frame
    void Update()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _playerEntity = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<PlayerComponent>())
            .GetSingletonEntity();
        _playerPos = _entityManager.GetComponentData<LocalTransform>(_playerEntity).Position;
        camera.transform.position = Vector3.Lerp(camera.transform.position, _playerPos + Vector3.up * camaraHeight,
            camaraFollowSpeed * Time.deltaTime);
    }
}