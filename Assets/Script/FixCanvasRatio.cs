using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // 씬 로드를 다루기 위한 네임스페이스

namespace Script
{
    public class FixCanvasRatio : MonoBehaviour
    {
        private static FixCanvasRatio _instance;
        public static FixCanvasRatio Instance => _instance;
        
        private Canvas _canvas;

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

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetResolution();
            SetCameraToAspectRatio();
        }

        /* 해상도 설정하는 함수 */
        private void SetResolution()
        {
            int setWidth = 1920; // 사용자 설정 너비
            int setHeight = 1080; // 사용자 설정 높이

            int deviceWidth = Screen.width; // 기기 너비 저장
            int deviceHeight = Screen.height; // 기기 높이 저장

            // 해상도 변경
            Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // 기기의 해상도 비가 더 큰 경우
                {
                    float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
                    mainCamera.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // 새로운 Rect 적용
                }
                else // 게임의 해상도 비가 더 큰 경우
                {
                    float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이
                    mainCamera.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // 새로운 Rect 적용
                }
            }
            else
            {
                Debug.LogWarning("Main Camera not found! Make sure the camera is tagged with 'MainCamera'.");
            }

            // Canvas 찾기
            if (_canvas == null)
            {
                _canvas = GameObject.Find("ButtonGroup").GetComponent<Canvas>(); // 씬에서 Canvas를 찾기
            }

            // Canvas Scaler의 Scale Factor 조정
            if (_canvas != null)
            {
                CanvasScaler scaler = _canvas.GetComponent<CanvasScaler>();
                if (scaler != null)
                {
                    float scaleFactor = (float)deviceWidth / setWidth; // 비율 계산 (해상도 비율에 맞춰 스케일링)
                    scaler.scaleFactor = scaleFactor;
                }
            }
        }
        
        private void SetCameraToAspectRatio()
        {
            float targetAspect = 16f / 9f;
            float windowAspect = Screen.width / (float)Screen.height;
            float scaleHeight = windowAspect / targetAspect;

            Camera mainCamera = Camera.main;

            if (mainCamera != null)
            {

                if (scaleHeight < 1.0f)
                {
                    // 화면이 세로로 길다
                    Rect rect = mainCamera.rect;

                    rect.width = 1.0f;
                    rect.height = scaleHeight;
                    rect.x = 0;
                    rect.y = (1.0f - scaleHeight) / 2.0f;

                    mainCamera.rect = rect;
                }
                else
                {
                    // 화면이 가로로 길다 (letterbox)
                    float scaleWidth = 1.0f / scaleHeight;

                    Rect rect = mainCamera.rect;

                    rect.width = scaleWidth;
                    rect.height = 1.0f;
                    rect.x = (1.0f - scaleWidth) / 2.0f;
                    rect.y = 0;

                    mainCamera.rect = rect;
                }
            }
            else
            {
                Debug.LogWarning("Main Camera not found!");
            }
        }
    }
}