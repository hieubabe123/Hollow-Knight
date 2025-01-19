using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 8f;
    float rollSpeed = -10f;
    bool checkRoll;
    int timeRoll =2;
    
    bool isRolling;

    Rigidbody2D myRigidbody;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myAheadCollider;
    Animator myAnimator;
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myBodyCollider = GetComponent<CapsuleCollider2D> ();
        myAheadCollider = GetComponent<BoxCollider2D> ();
        myAnimator = GetComponent<Animator> ();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move(){
        if(checkRoll == true){
            myRigidbody.velocity = new Vector2(rollSpeed,myRigidbody.velocity.y);
        }else{
            myRigidbody.velocity = new Vector2(moveSpeed,myRigidbody.velocity.y);
        }
        myAnimator.SetBool("isRolling",checkRoll);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(timeRoll > 0){
            checkRoll = true;
            timeRoll--;
        }else{
            checkRoll = false;
            timeRoll =2;
        }
        moveSpeed = -moveSpeed;
        rollSpeed = -rollSpeed;
        FlipSprite();
    }

    void FlipSprite(){
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x ) > Mathf.Epsilon ;
        if(playerHasHorizontalSpeed){
            Vector2 PlayerScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x),1f);
            this.transform.localScale = PlayerScale;
        }
    }
}
