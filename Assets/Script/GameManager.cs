using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        private readonly List<GameObject> _cats = new List<GameObject>();
    
        [SerializeField] private Canvas canvas;
        

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

        public void StageClear()
        {
            if(canvas.IsUnityNull()) Debug.LogError("canvas is null");
            else canvas.gameObject.SetActive(true);
            KeyboardButtonSelector.Instance.RefreshButtons();
        }
    }
}