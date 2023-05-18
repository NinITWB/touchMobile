using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.UI;

enum Direction
{
    RIGHT, RELEASE, LEFT
}

public class playerMovement : MonoBehaviour
{
    [Header("Values For Player")]
    //the most appropriate values follow 8 for velocity and 13 for force with 2.5 gravity scale in rb2D
    [SerializeField] private float velocity;
    [SerializeField] private float force;
    [SerializeField] private LayerMask UILayer;
    


    [Header("References")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    

    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private CapsuleCollider2D capsuleCollider;
    [SerializeField] private Vector3 offset;
    public RaycastHit2D[] hits;

    //specify which direction player will turn
    Direction direction;
    private bool isJumping;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); 
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if (Input.touchCount > 0) SetUpTouch();
        else direction = Direction.RELEASE;

    }

    private void FixedUpdate()
    { 
        Movement();
        animator.SetBool("isWalk", (direction == Direction.LEFT || direction == Direction.RIGHT) && isGrounded());
        
        //Player jump 
        /* There are some errors
         * when i try to use rigidbody2D in FixedUpdate so im still trying to find the way solving it
         */
        jump();

        flipObject();
        
    }

    private void SetUpTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 position = Camera.main.ScreenToWorldPoint(touch.position);
            hits = Physics2D.RaycastAll(position, Vector2.right * .05f, .1f, UILayer);

            foreach (var touching in hits)
            {

                if (touching.collider.name == jumpButton.name && touch.phase == UnityEngine.TouchPhase.Began && 
                    !(touch.phase == UnityEngine.TouchPhase.Stationary) && !(touch.phase == UnityEngine.TouchPhase.Ended))
                {
                    isJumping = true;
                }

                if (touch.phase == UnityEngine.TouchPhase.Began || touch.phase == UnityEngine.TouchPhase.Stationary)
                {
                    if (touching.collider.name == left.name) direction = Direction.LEFT;
                    else if (touching.collider.name == right.name) direction = Direction.RIGHT;
                }
                else if (touch.phase == UnityEngine.TouchPhase.Ended || touch.phase == UnityEngine.TouchPhase.Canceled)
                {
                    direction = Direction.RELEASE;
                  
                }
                //Debug.Log(touching.collider.name);
            }

        }
    }

    private void Movement()
    {
        if (direction == Direction.LEFT)
        {
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
        }
        else if (direction == Direction.RIGHT)
        {
            rb.velocity = new Vector2 (velocity, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    private void jump()
    {
        animator.SetBool("isJump", !isGrounded());
        if (!isGrounded())
        {
            animator.SetTrigger("jump");
            isJumping = false;
        }
        if (isJumping && isGrounded())
        {
            rb.AddForce(Vector2.up * force);
        }
    }


    //adjust the exact direction when we turn player 
    private void flipObject()
    {
        if (direction == Direction.RIGHT)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction == Direction.LEFT)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    //Use to create a box collide for player
    private bool isGrounded()
    {
       
        RaycastHit2D hit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, .1f, groundMask);
        return hit.collider != null;
    }
}
