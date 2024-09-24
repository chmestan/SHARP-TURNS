using System.Collections;
using TMPro;
using UnityEngine;

public class SawLifetime : MonoBehaviour
{
    [Header ("Lifetime")]
    [SerializeField] float disableTime; 
    [SerializeField] float destroyTime;
    [Header ("Fade")]
    [SerializeField] float fadeDuration = 2.0f; 
    [Header ("Reference")]
    [SerializeField] SpriteRenderer spriteRenderer; 

    public float _DisableTime
    {
        get { return disableTime; }
        set { disableTime = value; }
    }
    public float _DestroyTime  
    {
        get { return destroyTime; }
        set { destroyTime = value; }
    }

    void OnEnable()
    {
        StartCoroutine(HandleSawLifetime());
    }

    IEnumerator HandleSawLifetime()
    {
        yield return new WaitForSeconds(disableTime);

        gameObject.layer = LayerMask.NameToLayer("NoCollision");
        yield return StartCoroutine(FadeOut());

        yield return new WaitForSeconds(fadeDuration);

        Destroy(gameObject); //once fade is over
    }

    IEnumerator FadeOut()
    {
        float fadeElapsedTime = 0f;
        Color originalColor = spriteRenderer.color;
        
        while (fadeElapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, fadeElapsedTime / fadeDuration); 

            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            fadeElapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
