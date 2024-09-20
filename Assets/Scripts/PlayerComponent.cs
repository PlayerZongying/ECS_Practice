using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public float moveSpeed;
    public Entity bulletPrefab;
    public int numberOfBulletToSpawn;
    public float bulletSpreadWidth;
    public float bulletSpreadAngle;
    
}
