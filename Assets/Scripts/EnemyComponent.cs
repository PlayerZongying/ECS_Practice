using Unity.Entities;

public struct EnemyComponent : IComponentData
{
    public float currentHealth;
    public float enemySpeed;
}
