using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        public InputSystem_Actions inputSystem;
        InputAction _restartAction;
        private readonly List<GameObject> _cats = new List<GameObject>();
        [SerializeField] List<int> scaleOrder = new List<int>();

        [SerializeField] private GameObject clearPanel;
        [SerializeField] private GameObject gameoverPanel;
        [SerializeField] List<int> correctScaleOrder;
        private PlayerCtrl _player;
        private void Start()
        {
            inputSystem = new InputSystem_Actions();
            inputSystem.Enable();
            _restartAction = inputSystem.Player.Restart;
            Transform catGroup = GameObject.Find("CatGroup")?.transform;
            if (catGroup != null)
            {
                foreach (Transform child in catGroup)
                {
                    _cats.Add(child.gameObject);
                }
            }
            clearPanel.SetActive(false);
            gameoverPanel.SetActive(false);
            
            _player = GameObject.FindWithTag("Player").GetComponent<PlayerCtrl>();
        }

        private bool AreAllCatsFound()
        {
            foreach (GameObject cat in _cats)
            {
                Debug.Log("NotFindAllCat");
                if (cat.activeInHierarchy) return false;
            }
            return true;
        }

        private bool AreScaleOrderCorrect()
        {
            if(correctScaleOrder.Count == 0)
                return true;
            if (correctScaleOrder.Count > scaleOrder.Count)
                return false;
            for (int i = 0; i < correctScaleOrder.Count; i++)
                if (correctScaleOrder[i] != scaleOrder[i])
                    return false;
            
            return true;
        }

        public void AddScaleOrder(int @in)
        {
            if (scaleOrder.Count == correctScaleOrder.Count)
                scaleOrder.RemoveAt(0);
            scaleOrder.Add(@in);
        }
        public bool StageClear()
        {
            if (AreAllCatsFound() && AreScaleOrderCorrect())
            {
                if (clearPanel.IsUnityNull()) Debug.LogError("clearPanel is null");
                else clearPanel.SetActive(true);
                _player.enabled = false;
                KeyboardButtonSelector.Instance.RefreshButtons();
                return true;
            }
            return false;
        }

        public void GameOver()
        {
            if (gameoverPanel.IsUnityNull()) Debug.LogError("gameoverPanel is null");
            _player.enabled = false;
            gameoverPanel.SetActive(true);
            KeyboardButtonSelector.Instance.RefreshButtons();
        }

        private void Update()
        {
            if(_restartAction.triggered)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}