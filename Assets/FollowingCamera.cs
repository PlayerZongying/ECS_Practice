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

    private Camera _camera;

    [Range(0f, 100f)] public float camaraHeight = 10f;
    [Range(0f, 100f)] public float camaraFollowSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        _playerEntity = _entityManager.CreateEntityQuery(ComponentType.ReadOnly<PlayerComponent>())
            .GetSingletonEntity();

        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _playerPos = _entityManager.GetComponentData<LocalTransform>(_playerEntity).Position;
        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _playerPos + Vector3.up * camaraHeight,
            camaraFollowSpeed * Time.deltaTime);
    }
}