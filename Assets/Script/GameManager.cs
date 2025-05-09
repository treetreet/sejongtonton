using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        private readonly List<GameObject> _cats = new List<GameObject>();
    
        private Canvas canvas;
        

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
            
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            canvas.gameObject.SetActive(false);
        }

        public bool AreAllCatsFound()
        {
            foreach (GameObject cat in _cats)
            {
                if (cat.activeInHierarchy) return false;
            }

            return true;
        }

        public void StageClear()
        {
            canvas.gameObject.SetActive(true);
            KeyboardButtonSelector.Instance.RefreshButtons();
        }
    }
}