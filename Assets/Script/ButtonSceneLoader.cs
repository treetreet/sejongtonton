using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Script
{
    public class ButtonSceneLoader : MonoBehaviour
    {

        void Start()
        {
            this.GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            string buttonName = gameObject.name;
            string sceneName = buttonName.Replace(" Button", "");

            SceneManager.LoadScene(sceneName);
        }
    }
}