using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script
{
    public class KeyboardButtonSelector : MonoBehaviour
    {
        private static KeyboardButtonSelector _instance;
        public static KeyboardButtonSelector Instance => _instance;
        
        [SerializeField] private readonly List<Button> _buttons = new List<Button>();
        private int _selectedButtonIndex;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            // 이벤트 등록 해제 (메모리 누수 방지)
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RefreshButtons();
        }

        public void RefreshButtons()
        {
            _buttons.Clear();
            _selectedButtonIndex = 0;

            Transform buttonGroup = GameObject.Find("ButtonGroup")?.transform;
            if (buttonGroup != null)
            {
                foreach (Transform child in buttonGroup)
                {
                    Button button = child.GetComponent<Button>();
                    if (!button.IsUnityNull())
                    {
                        _buttons.Add(button);
                    }
                }
            }
        }

        void Update()
        {
            _buttons.RemoveAll(b => b.IsUnityNull()); // Destroy된 버튼 제거
            if (_buttons.Count == 0) return;
            
            // 방향키로 버튼 이동
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _selectedButtonIndex = (_selectedButtonIndex - 1 + _buttons.Count) % _buttons.Count;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _selectedButtonIndex = (_selectedButtonIndex + 1) % _buttons.Count;
            }

            // 엔터나 스페이스바로 버튼 클릭
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _buttons[_selectedButtonIndex].onClick.Invoke();
            }

            // 버튼에 포커스를 맞추기
            for (int i = 0; i < _buttons.Count; i++)
            {
                if (i == _selectedButtonIndex)
                {
                    _buttons[i].Select(); // 버튼을 선택 상태로 변경
                }
            }
        }
    }
}