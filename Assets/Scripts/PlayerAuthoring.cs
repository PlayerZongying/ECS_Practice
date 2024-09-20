using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public int numberOfBulletToSpawn = 50;
    [Range(0f,10f)]public float bulletSpreadWidth = 5f;
    [Range(0f,180f)]public float bulletSpreadAngle = 20f;
    
    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(playerEntity, new PlayerComponent
            {
                moveSpeed = authoring.moveSpeed,
                bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                numberOfBulletToSpawn = authoring.numberOfBulletToSpawn,
                bulletSpreadWidth = authoring.bulletSpreadWidth,
                bulletSpreadAngle = authoring.bulletSpreadAngle
            });
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
