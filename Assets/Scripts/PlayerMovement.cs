using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionAsset inputActions;
    [Header ("Movement")]
    [SerializeField] float speed = 9f;
    [SerializeField] float acceleration = 30f;
    [SerializeField] float deceleration = 80f;
    [SerializeField] float turnSpeed = 1f;
    private Vector2 currentVelocity = Vector2.zero;
    
    [Header("Reference")]
    [SerializeField] Transform spriteTransform; 
    
    [Header ("Juice")]
    [SerializeField] float stretchFactor = 1.1f; 
    [SerializeField] float smoothTime = 0.1f; 
    private Vector3 originalScale;
    private Vector3 targetScale; // Target scale to lerp to

    private InputAction move;

    void Awake()
    {
        move = inputActions.FindActionMap("player").FindAction("move");
        originalScale = spriteTransform.localScale;
        targetScale = originalScale;
    }

    private void OnEnable()
    {
        inputActions.FindActionMap("player").Enable();
    }

    void FixedUpdate()
    {
        Vector2 moveAmount = move.ReadValue<Vector2>();
        Vector2 targetVelocity = moveAmount.normalized * speed;

        bool isChangingDirection = Vector2.Dot(currentVelocity, targetVelocity) < 0;
        if (isChangingDirection) turnSpeed = 30f;
        else turnSpeed = 1f;

        currentVelocity = Vector2.MoveTowards(currentVelocity, targetVelocity,
            (moveAmount.magnitude > 0 ? acceleration : deceleration) * Time.deltaTime * turnSpeed);

        transform.Translate(currentVelocity * Time.deltaTime);

        if (Mathf.Abs(moveAmount.x) > 0) 
        {
            targetScale = new Vector3(originalScale.x * stretchFactor, originalScale.y, originalScale.z);
        }
        else if (Mathf.Abs(moveAmount.y) > 0) 
        {
            targetScale = new Vector3(originalScale.x, originalScale.y * stretchFactor, originalScale.z);
        }
        else 
        {
            targetScale = originalScale;
        }

        spriteTransform.localScale = Vector3.Lerp(spriteTransform.localScale, targetScale, smoothTime);
    }

    private void OnDisable()
    {
        inputActions.FindActionMap("player").Disable();
    }
}
