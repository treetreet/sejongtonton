using System.Collections;
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
    InputAction moveAction;
    InputAction lookAction;

    [SerializeField] Tilemap blockedTilemap;
    bool canMove = true;

    void Start()
    {
        inputsystem = new InputSystem_Actions();
        inputsystem.Enable();
        moveAction = inputsystem.Player.Move;
        lookAction = inputsystem.Player.Look;
    }

    void Update()
    {
        if (!canMove) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            Vector3 targetWorldPos = transform.position + (Vector3)input;
            Vector3Int targetCellPos = blockedTilemap.WorldToCell(targetWorldPos);

            if (blockedTilemap.HasTile(targetCellPos))
            {
                Debug.Log("이동 불가! 장애물 있음");
            }
            else
            {
                StartCoroutine(PlayerMove(targetCellPos));
            }

            Debug.Log(input);
            Debug.Log(lookAction.ReadValueAsObject());
        }
    }

    IEnumerator PlayerMove(Vector3Int targetCellPos)
    {
        canMove = false;

        transform.position = blockedTilemap.GetCellCenterWorld(targetCellPos);

        yield return new WaitForSeconds(0.3f);
        canMove = true;
    }
}

