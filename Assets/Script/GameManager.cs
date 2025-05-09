using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        private readonly List<GameObject> _cats = new List<GameObject>();
        [SerializeField] List<int> _scaleOrder = new List<int>();

        [SerializeField] private Canvas canvas;
        [SerializeField] List<int> correctScaleOrder;

        private void Start()
        {
            Transform catGroup = GameObject.Find("CatGroup")?.transform;
            if (catGroup != null)
            {
                foreach (Transform child in catGroup)
                {
                    _cats.Add(child.gameObject);
                }
            }
            canvas.gameObject.SetActive(false);
        }

        public bool AreAllCatsFound()
        {
            foreach (GameObject cat in _cats)
            {
                Debug.Log("NotFindAllCat");
                if (cat.activeInHierarchy) return false;
            }
            return true;
        }

        public bool AreScaleOrderCorrect()
        {
            if(correctScaleOrder.Count == 0)
                return true;
            if (correctScaleOrder.Count > _scaleOrder.Count)
                return false;
            for (int i = 0; i < correctScaleOrder.Count; i++)
                if (correctScaleOrder[i] != _scaleOrder[i])
                    return false;
            
            return true;
        }

        public void AddScaleOrder(int _in)
        {
            _scaleOrder.Add(_in);
        }
        public bool StageClear()
        {
            if (AreAllCatsFound() && AreScaleOrderCorrect())
            {
                if (canvas.IsUnityNull()) Debug.LogError("canvas is null");
                else canvas.gameObject.SetActive(true);
                KeyboardButtonSelector.Instance.RefreshButtons();
                return true;
            }
            return false;
        }
    }
}