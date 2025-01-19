using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    Rigidbody2D SplashRigidbody;
    PlayerMovement player;

    [SerializeField] float XSplashScale;
    [SerializeField] float timeDestroySplash;
    [SerializeField] float splashSpeed = 10.0f;
    Animator myAnimator;
    

    // Start is called before the first frame update
    void Start()
    {
        SplashRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        myAnimator = GetComponent<Animator>();
        XSplashScale = player.transform.localScale.x * splashSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        SplashRigidbody.velocity = new Vector2(XSplashScale, 0);
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag =="Enemy"){
            Destroy(other.gameObject);
        }
        Destroy(this.gameObject,0.5f);
    }
}
