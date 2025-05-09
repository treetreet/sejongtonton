using System.Numerics;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerCtrl : MonoBehaviour
{
    InputSystem_Actions inputsystem;
    InputAction moveAction; // (-1, 0) a (1, 0) d (0, 1) w (0, -1) s
    InputAction lookAction; //-1 q, 1 e
    [SerializeField] Tilemap blockedTilemap;

    void Start()
    {
        inputsystem = new InputSystem_Actions();
        inputsystem.Enable();
        moveAction = inputsystem.Player.Move;
        lookAction = inputsystem.Player.Look;
    }

    void Update()
    {
        Vector3 targetWorldPos = transform.position + (Vector3)moveAction.ReadValue<Vector2>();
        Vector3Int targetCellPos = blockedTilemap.WorldToCell(targetWorldPos);
        if (blockedTilemap.HasTile(targetCellPos))
        {
            Debug.Log("이동 불가! 장애물 있음");
        }
        else 
        {
            transform.position += new Vector3(targetCellPos.x, targetCellPos.y, transform.position.z);
        }

        Debug.Log(moveAction.ReadValue<Vector2>());
        Debug.Log(lookAction.ReadValueAsObject());
    }
}
