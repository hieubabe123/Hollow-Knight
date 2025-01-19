using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    bool checkClimb;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float JumpForce = 7f;
    [SerializeField] GameObject bladeSplash;
    [SerializeField] Transform blade;


    Vector2 deathKick = new Vector2(0f,5f);
    private float gravityScaleOnWall = 0f;
    private float gravityScaleDead = 0f;

    private float gravityScaleOnGround = 8f;
    int jumpTime = 1;
    public bool isDead = false;

    float levelLoadDelay = 1f;

    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollier;
    Animator myAnimator;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAnimator = GetComponent<Animator>();
        myFeetCollier = GetComponent<BoxCollider2D>();
    }
    void OnMove(InputValue value){
        if(isDead){
            return;
        }
        moveInput = value.Get<Vector2>();
    }
    void Update()
    {
        if(isDead){
            return;
        }
        Run();
        FlipSprite();
        Jump();
        ReturnJumpTime();
        Climb();
        AnimationClimb();
        Dead();
    }
    void Run(){
        // Player don't stuck between Ground and Wall
        if((myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Wall")))) {
            return;
        }
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x ) > Mathf.Epsilon ;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        
    }

    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x ) > Mathf.Epsilon ;
        if(playerHasHorizontalSpeed){
            Vector2 PlayerScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);
            this.transform.localScale = PlayerScale;
        }
    }

    void OnJump(InputValue value){
        if(jumpTime <= 0 || isDead == true){
            return;
        }
        if(value.isPressed){
            Vector2 playerJump = new Vector2(0f,JumpForce);
            myRigidbody.velocity += playerJump;
            jumpTime = jumpTime -1 ;
        }
        
    }

    void Jump(){
        bool checkJump = Mathf.Abs(myRigidbody.velocity.y ) > Mathf.Epsilon ;
        myAnimator.SetBool("isJumping", checkJump);

    }

    void ReturnJumpTime(){
        if(myFeetCollier.IsTouchingLayers(LayerMask.GetMask("Ground"))){
            jumpTime = 1;
        }
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Wall"))){
            jumpTime = 1 ;
        }
    }
    void Climb(){
        if(!myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Wall"))){
            myRigidbody.gravityScale = gravityScaleOnGround;
            checkClimb = false;
            return;
        }
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Wall"))){
            myRigidbody.gravityScale = gravityScaleOnWall;
            checkClimb = true;
        }
        myAnimator.SetBool("isClimbing",checkClimb);
    }

    void AnimationClimb(){
        myAnimator.SetBool("isClimbing",checkClimb);
    }

    public void Dead(){
        if(myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || myFeetCollier.IsTouchingLayers(LayerMask.GetMask("Enemy"))|| myFeetCollier.IsTouchingLayers(LayerMask.GetMask("Dead Zone"))){
            isDead = true;
            myAnimator.SetTrigger("isDead");
            myRigidbody.velocity = deathKick;
            myRigidbody.gravityScale = gravityScaleDead;
            StartCoroutine(LoadCurrentLevel());
        }
    }

    void OnFire(InputValue value){
        if(isDead){
            return;
        }
        Instantiate(bladeSplash,blade.position,transform.rotation);
    }

    IEnumerator LoadCurrentLevel(){
        yield return new WaitForSeconds(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        
    }   
}
