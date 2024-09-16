using Unity.Entities;
using Unity.Mathematics;

public struct InputComponent : IComponentData
{
    public float2 movement;
    public float2 mousePosition;
    public bool bShoot;
}
