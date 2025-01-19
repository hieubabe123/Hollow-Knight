using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoinDestroy : MonoBehaviour
{
    GameObject coin;
    private int point = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        coin = GetComponent<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            Destroy(this.gameObject);
            point++;
        }
    }
}
