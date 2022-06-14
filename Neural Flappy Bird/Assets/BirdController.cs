using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour{
    public float jumpForce = 10.0f;
    public Rigidbody2D rb;
    public bool isAlive = true;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    void Update(){
        if(!isAlive) return;
        Rotate();
        CheckDeathScreen();
    }

    public void Jump(){
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    //rotate based on the vertical speed
    void Rotate(){
        float angle = Mathf.Lerp(0, 70, rb.velocity.y / 5);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void CheckDeathScreen(){
        if(transform.position.y < -4f || transform.position.y > 6f){
            isAlive = false;
        }
    }

    

    void OnTriggerEnter2D(Collider2D c){
        if(c.gameObject.tag == "Obstacle"){
            print("DEAD");
            isAlive = false;
        }
    }
}