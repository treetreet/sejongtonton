using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Script
{
    public class PlayerCtrl : MonoBehaviour
    {
        public delegate void PlayerMoveHandler();
        public static event PlayerMoveHandler OnPlayerMove;
        
        
        InputSystem_Actions _inputSystemActions;
        InputAction _moveAction;
        InputAction _lookAction;
        
        [SerializeField] private Tilemap blockedTilemap;
        [SerializeField] private float distance = 0.5f;
        [SerializeField] private GameManager gameManager;
        bool _canMove = true;
        bool _canRotate = true;
        void Start()
        {
            _inputSystemActions = new InputSystem_Actions();
            _inputSystemActions.Enable();
            _moveAction = _inputSystemActions.Player.Move;
            _lookAction = _inputSystemActions.Player.Look;
        }

        private void OnDestroy()
        {
            if (_inputSystemActions != null)
            {
                _inputSystemActions.Disable();
                _inputSystemActions.Dispose();
            }
        }

        void Update()
        {
            if (!_canMove) return;
            if (!_canRotate) return;

            Vector2 input = _moveAction.ReadValue<Vector2>();
            if (input != Vector2.zero)
            {
                Vector3 targetWorldPos = transform.position;

                if (input.y != 0)
                {
                    targetWorldPos += transform.up * input.y;
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

                    targetWorldPos += vec * -input.x; // 방향 반대로 보정
                }

                Vector3Int targetCellPos = blockedTilemap.WorldToCell(targetWorldPos);

                if (blockedTilemap.HasTile(targetCellPos))
                {
                    CheckRayCast(input);
                    SoundManager.Instance.PlaySFX("hit");
                    StartCoroutine(PlayerCanMove());
                    Debug.Log("이동 불가! 장애물 있음");
                }
                else
                {
                    StartCoroutine(PlayerMove(targetCellPos));
                }
            }

            float lookDirection = _lookAction.ReadValue<float>();
            if (lookDirection != 0)
            {
                StartCoroutine(PlayerRotate(lookDirection));
            }
        }

        void CheckRayCast(Vector2 input)
        {
            Vector2 direction = GetDirection(input);  // 플레이어가 바라보는 방향

            Vector2 rayOrigin = (Vector2)transform.position + direction.normalized * 0.3f;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, distance);
            
            if (!hit.collider.IsUnityNull() && hit.collider.CompareTag("exitPoint"))
            {
                if (gameManager.AreAllCatsFound())
                {
                    Debug.Log("clear");
                    gameManager.StageClear();
                }
                else
                {
                    SoundManager.Instance.PlaySFX("hit");
                }
            }
        }

        Vector2 GetDirection(Vector2 input)
        {
            Vector2 direction = Vector2.zero;
            if (input.y > 0)
            {
                direction = transform.up;  // 위쪽
            }
            else if (input.y < 0)
            {
                direction = -transform.up; // 아래쪽
            }
            else if (input.x > 0)
            {
                direction = transform.right;  // 오른쪽
            }
            else if (input.x < 0)
            {
                direction = -transform.right; // 왼쪽
            }
            return direction;
        }

        private void PlayerMoveEnd()
        {
            OnPlayerMove?.Invoke();
        }

        public Vector3 getPosition()
        {
            return transform.position;
        }

        IEnumerator PlayerMove(Vector3Int targetCellPos)
        {
            StartCoroutine(PlayerCanMove());
            transform.position = blockedTilemap.GetCellCenterWorld(targetCellPos);
            SoundManager.Instance.PlaySFX("walking");
            yield return null;
        }

        IEnumerator PlayerCanMove()
        {
            _canMove = false;
            yield return new WaitForSeconds(0.3f);
            PlayerMoveEnd();
            _canMove = true;
        }
        IEnumerator PlayerRotate(float input)
        {
            _canRotate = false;
            Vector3 rotateStart = transform.eulerAngles;
            float rotationTimer = 0f;
            while (rotationTimer <= 0.25f)
            {
                rotationTimer += Time.deltaTime;
                transform.eulerAngles = Vector3.Lerp(rotateStart, rotateStart + new Vector3(0, 0, input * -90f),
                    rotationTimer * 4f);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.25f);
            _canRotate = true;
        }
        
        /*
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, GetDirection(_moveAction.ReadValue<Vector2>()) * distance);
        }
        */
    }
}