using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float currentSpeed = 3f;
    Rigidbody2D body;
    public Transform playerTransform;
    // (:-|)
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // ("_"<(ZZZZZ))
    void FixedUpdate()
    {
        
         Move();
    }

    void Move(){
        if(playerTransform == null){
            GetPlayer();
            return;
        }
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        body.MovePosition(body.position + direction * currentSpeed * Time.fixedDeltaTime);
    
    }
    void GetPlayer(){
        playerTransform = GameManager.Instance.GetPlayer.transform;
    }
}
