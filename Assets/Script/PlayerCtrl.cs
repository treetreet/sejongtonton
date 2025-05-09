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
    bool canRotate = true;
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
        if (!canRotate) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            Vector3 targetWorldPos = transform.position;

            if (input.y == 1)
            {
                targetWorldPos += transform.up;
            }
            else if (input.y == -1)
            {
                targetWorldPos -= transform.up;
            }
            else if (input.x != 0)
            {
                Vector3 vec = Vector3.zero;

                if (Mathf.Approximately(transform.up.y, 1f))
                    vec = Vector3.left;
                else if (Mathf.Approximately(transform.up.x, -1f))
                    vec = Vector3.down;
                else if (Mathf.Approximately(transform.up.y, -1f))
                    vec = Vector3.right;
                else if (Mathf.Approximately(transform.up.x, 1f))
                    vec = Vector3.up;

                targetWorldPos += vec * -input.x;  // 방향 반대로 보정
            }

            Vector3Int targetCellPos = blockedTilemap.WorldToCell(targetWorldPos);

            if (blockedTilemap.HasTile(targetCellPos))
            {
                Debug.Log("이동 불가! 장애물 있음");
            }
            else
            {
                StartCoroutine(PlayerMove(targetCellPos));
            }
        }

        float lookinput = lookAction.ReadValue<float>();
        if (lookinput != 0)
        {
            StartCoroutine(PlayerRotate(lookinput));
        }
    }

    IEnumerator PlayerMove(Vector3Int targetCellPos)
    {
        canMove = false;

        transform.position = blockedTilemap.GetCellCenterWorld(targetCellPos);

        yield return new WaitForSeconds(0.3f);
        canMove = true;
    }

    IEnumerator PlayerRotate(float input)
    {
        canRotate = false;
        Vector3 rotateStart = transform.eulerAngles;
        float rotationtimer = 0f;
        while (rotationtimer <= 0.25f)
        {
            rotationtimer += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(rotateStart, rotateStart + new Vector3(0, 0, input * -90f), rotationtimer * 4f);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.25f);
        canRotate = true;
    }
}

