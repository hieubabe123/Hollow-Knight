using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevels : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    public PlayerMovement playerMovement;
    bool isDead;
    // Start is called before the first frame update
    
    void Start(){
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        isDead = playerMovement.isDead;
        
    }
    void OnTriggerEnter2D(Collider2D other) {
       StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel(){
        yield return new WaitForSeconds(levelLoadDelay);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1); 
        
    }
}
