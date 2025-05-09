using System.Collections;
using UnityEngine;

public class SFXTestScript : MonoBehaviour
{
    bool play;
    void Start()
    {
        play = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (play)
        {
            StartCoroutine(playSFX());
        }
    }

    IEnumerator playSFX()
    {
        play = false;
        SoundManager.Instance.PlaySFX("air");
        yield return new WaitForSeconds(1f);
        play = true;
    }
}
