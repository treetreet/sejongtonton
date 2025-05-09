using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Script
{
    public class KeyboardButtonSelector : MonoBehaviour
    {
        public Button[] buttons;
        private int _selectedButtonIndex;

        private void Start()
        {
            _selectedButtonIndex = 0;
        }

        void Update()
        {
            // 방향키로 버튼 이동
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _selectedButtonIndex = (_selectedButtonIndex - 1 + buttons.Length) % buttons.Length;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _selectedButtonIndex = (_selectedButtonIndex + 1) % buttons.Length;
            }

            // 엔터나 스페이스바로 버튼 클릭
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                buttons[_selectedButtonIndex].onClick.Invoke();
            }

            // 버튼에 포커스를 맞추기
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == _selectedButtonIndex)
                {
                    buttons[i].Select(); // 버튼을 선택 상태로 변경
                }
            }
        }
    }
}