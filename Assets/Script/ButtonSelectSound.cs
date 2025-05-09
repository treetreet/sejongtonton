using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(AudioSource))]
public class ButtonSoundPlayer : MonoBehaviour, ISelectHandler
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        // 현재 선택된 오브젝트 이름
        string buttonName = gameObject.name;
        string audioName = buttonName.Replace(" Button", "");

        // Resources/Audio/폴더에서 이름과 같은 오디오 클립 로드
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioName);
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("사운드 파일을 찾을 수 없습니다: " + audioName);
        }
    }
}