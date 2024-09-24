using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] private AudioSource selectAudioSource; 
    private bool isKeyPressed = false; 

    void Update()
    {
        if (Input.anyKeyDown && !isKeyPressed)
        {
            isKeyPressed = true;  
            StartCoroutine(PlaySelectAndLoadScene());
        }
    }

    private IEnumerator PlaySelectAndLoadScene()
    {
        if (selectAudioSource != null)
        {
            selectAudioSource.Play();
            yield return new WaitForSeconds(selectAudioSource.clip.length);
        }
        SceneManager.LoadScene("Main");
    }
}
