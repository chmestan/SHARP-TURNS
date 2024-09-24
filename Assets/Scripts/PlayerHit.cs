using System.Collections;
using UnityEngine;
using UnityEngine.UI; 

public class PlayerHit : MonoBehaviour
{
    [Header("Number of lives")]
    [SerializeField] int maxLives = 3;
    [SerializeField] int currentLives;

    [Header("Invulnerability period")]
    [SerializeField] float invulnerabilityDuration = 1.0f; 
    [SerializeField] bool isInvulnerable = false; //for debugging

    [Header("UI Hearts")]
    [SerializeField] Image[] heartImages; 

    [Header("References")]
    [SerializeField] Transform spriteTransform; 
    [SerializeField] RestartManager restartScreenManager;
    private SpriteRenderer spriteRenderer;

    [Header ("Particles")]
    [SerializeField] ParticleSystem bloodParticles; 

    [Header("Audio")]
    [SerializeField] AudioSource damageAudioSource;
    [SerializeField] AudioSource loseAudioSource;
    [SerializeField] AudioSource inGameMusicSource;  
    [SerializeField] float musicFadeDuration = 1.0f;

    void OnEnable()
    {
        spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
        currentLives = maxLives;

        UpdateHeartsUI();
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.CompareTag("saw") || collider.CompareTag("bullet")) && !isInvulnerable)
        {
            if (damageAudioSource != null) damageAudioSource.Play();
            collider.SendMessage("ChangeToBloodySprite", SendMessageOptions.DontRequireReceiver);
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        if (bloodParticles != null) bloodParticles.Play(); 

        currentLives--;
        UpdateHeartsUI(); 

        if (currentLives <= 0){
            if (inGameMusicSource != null) StartCoroutine(FadeOutMusic());
            StartCoroutine(DelayedRestart());
        }
        else StartCoroutine(InvulnerabilityPeriod());
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentLives) heartImages[i].enabled = true; 
            else heartImages[i].enabled = false; 
            
        }
    }

    IEnumerator InvulnerabilityPeriod()
    {
        isInvulnerable = true;
        float elapsedTime = 0f;
        float flashInterval = 0.1f; 

        while (elapsedTime < invulnerabilityDuration)
        {
            //flicker
            SetAlpha(spriteRenderer.color.a == 1f ? 0.5f : 1f);
            
            yield return new WaitForSeconds(flashInterval);
            elapsedTime += flashInterval;
        }

        SetAlpha(1f); //to make sure that alpha is 1 at the end
        isInvulnerable = false;
    }

    void SetAlpha(float alphaValue)
    {
        Color newColor = spriteRenderer.color;
        newColor.a = alphaValue;
        spriteRenderer.color = newColor;
    }

    IEnumerator DelayedRestart()
    {
        Time.timeScale = 0f; 
        
        yield return new WaitForSecondsRealtime(0.5f); 
        Time.timeScale = 1f; 
        restartScreenManager.ShowRestartScreen();
    }
    IEnumerator FadeOutMusic()
    {
        float startVolume = inGameMusicSource.volume;

        for (float t = 0; t < musicFadeDuration; t += Time.unscaledDeltaTime)
        {
            inGameMusicSource.volume = Mathf.Lerp(startVolume, 0, t / musicFadeDuration);
            yield return null;
        }
        inGameMusicSource.volume = 0;
        inGameMusicSource.Stop();
        if (loseAudioSource != null) loseAudioSource.Play();
    }

}
