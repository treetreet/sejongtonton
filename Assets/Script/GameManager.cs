using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance => _instance;
        private readonly List<GameObject> _cats = new List<GameObject>();
    
        [SerializeField] private Canvas canvas;
        
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

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
        }
    }
}