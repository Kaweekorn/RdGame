using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            // Get input from player
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            Debug.Log("This is input.x"+ input.x);
            Debug.Log("This is input.y" + input.y);

            

            if (input.x != 0) input.y = 0;
            // Check if there's any input
            if (input != Vector2.zero)
            {

                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                // Normalize the input so diagonal movement isn't faster
                input = input.normalized;

                // Calculate the target position based on input
                Vector3 targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                // Start moving towards the target position
                StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);

    }

    // Coroutine to handle smooth movement towards target position
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        // Continue moving until the object reaches the target position
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // Set the position exactly at the target and mark as not moving
        transform.position = targetPos;
        isMoving = false;
    }
}
