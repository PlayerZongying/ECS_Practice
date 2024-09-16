using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bulletPrefab;
    public int numberOfBulletToSpawn = 50;
    [Range(0f,10f)]public float bulletSpread = 5f;
    
    public class PlayerBaker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            Entity playerEntity = GetEntity(TransformUsageFlags.None);
            AddComponent(playerEntity, new PlayerComponent
            {
                moveSpeed = authoring.moveSpeed,
                bulletPrefab = GetEntity(TransformUsageFlags.None),
                numberOfBulletToSpawn = authoring.numberOfBulletToSpawn,
                bulletSpread = authoring.bulletSpread
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
