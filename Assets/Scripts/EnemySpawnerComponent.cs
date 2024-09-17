using Unity.Entities;

public struct EnemySpawnerComponent : IComponentData
{
    public Entity enemyPrefabToSpawn;

    public int numOfEnemiesToSpawnPerSecond;
    public int numOfEnemiesToSpawnIncrementAmount;
    public int maxNumberOfEnemiesToSpawnPerSecond;
    public float enemySpawnRadius;
    public float minDistanceFromPlayer;

    public float timeBeforeNextSpawn;
    public float currentTimeBeforeSpawn;
}
