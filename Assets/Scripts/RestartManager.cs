using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class RestartManager : MonoBehaviour
{
    [SerializeField] GameObject restartScreen; 
    [SerializeField] AudioSource selectAudioSource;  
    private bool isRestarting = false;

    void Start()
    {
        restartScreen.SetActive(false);
    }

    void Update()
    {
        if (isRestarting && Input.anyKeyDown)
        {
            StartCoroutine(PlaySelectAndRestart());
        }
    }

    public void ShowRestartScreen()
    {
        Time.timeScale = 0; 
        restartScreen.SetActive(true); 
        isRestarting = true; 
    }

    private IEnumerator PlaySelectAndRestart()
    {
        if (selectAudioSource != null)
        {
            selectAudioSource.Play();
            yield return new WaitForSecondsRealtime(selectAudioSource.clip.length);
        }
        RestartGame();
    }

    private void RestartGame()
    {
        Time.timeScale = 1; 
        restartScreen.SetActive(false); 
        isRestarting = false; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }
}
