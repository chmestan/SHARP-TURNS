using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawMovement : MonoBehaviour
{
    [Header("Saw behavior")]
    [SerializeField] float speed = 3;
    [SerializeField] float rotationSpeed = 180f;
    private Vector2 direction;
    private Collider2D sawCollider;
    private Transform sawSpriteTransform;
    [Header("Sprites")]
    [SerializeField] private Sprite bloodySprite;

    [Header("Particles")]
    [SerializeField] private ParticleSystem activationParticleEffect;

    [Header("Audio")]
    [SerializeField] private AudioSource sawBounceAudioS;
    [SerializeField] AudioSource sawAppearAudioS;  

    public float _Speed   
    {
        get { return speed; }
        set { speed = value; }
    }

    public Vector2 _Direction   
    {
        get { return direction; }
        set { direction = value; }
    }

    void OnEnable()
    {
        sawCollider = GetComponent<Collider2D>();
        sawSpriteTransform = transform.GetChild(0);
    }

    public void ActivateSaw(float delay, float transparency) 
    {
        StartCoroutine(DelayedActivation(delay, transparency));
    }

    private IEnumerator DelayedActivation(float delay, float transparency)
    {
        SetSawTransparency(transparency); 
        sawCollider.enabled = false;
        direction = Vector2.zero;
        
        yield return new WaitForSeconds(delay);

        SetSawTransparency(1f);  
        sawCollider.enabled = true;
        direction = Random.insideUnitCircle.normalized;
        if (activationParticleEffect != null) activationParticleEffect.Play();
        sawAppearAudioS.Play();
    }

    private void SetSawTransparency(float alpha)
    {
        SpriteRenderer spriteRenderer = sawSpriteTransform.GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = alpha; 
        spriteRenderer.color = color;
    }

    void FixedUpdate()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime); //move
        sawSpriteTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime); //rotate visuals
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall_horizontal")) 
        {
            direction.x = -direction.x;
            sawBounceAudioS.Play();
        }

        if (collision.gameObject.CompareTag("wall_vertical")) {
            direction.y = -direction.y;
            sawBounceAudioS.Play();
        }
    }

    private void ChangeToBloodySprite()
    {
        SpriteRenderer spriteRenderer = sawSpriteTransform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = bloodySprite;  // Replace the sprite with the bloody one
    }
}
