using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[BurstCompile]

public partial class InputSystem : SystemBase
{
    private PlayerControls _controls;



    [BurstCompile]
    protected override void OnCreate()
    {
        base.OnCreate();
        if (!SystemAPI.TryGetSingleton(out InputComponent input))
        {
            EntityManager.CreateEntity(typeof(InputComponent));
        }

        _controls = new PlayerControls();
        _controls.Enable();
    }
    
    [BurstCompile]
    protected override void OnUpdate()
    {
        Vector2 moveVector = _controls.Player.Move.ReadValue<Vector2>();
        Vector2 mousePos = _controls.Player.MousePos.ReadValue<Vector2>();

        bool shoot = _controls.Player.Shoot.IsPressed();
        
        SystemAPI.SetSingleton(new InputComponent
        {
            movement = moveVector,
            mousePosition =  mousePos,
            bShoot = shoot
        });
    }
}