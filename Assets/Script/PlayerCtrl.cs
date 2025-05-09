using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerCtrl : MonoBehaviour
{
    InputSystem_Actions inputsystem;
    InputAction moveAction; // (-1, 0) a (1, 0) d (0, 1) w (0, -1) s
    InputAction lookAction; //-1 q, 1 e

    void Start()
    {
        inputsystem = new InputSystem_Actions();
        inputsystem.Enable();
        moveAction = inputsystem.Player.Move;
        lookAction = inputsystem.Player.Look;
    }

    void Update()
    {
        Debug.Log(moveAction.ReadValue<Vector2>());
        Debug.Log(lookAction.ReadValueAsObject());
    }
}
