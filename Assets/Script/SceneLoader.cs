using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace Script
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader _instance;
        public static SceneLoader Instance => _instance;

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

        // 버튼 클릭 시 호출될 메서드 (매개변수로 씬 이름 받음)
        public void LoadNewScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}