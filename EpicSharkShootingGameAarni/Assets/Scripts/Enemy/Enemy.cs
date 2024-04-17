using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    float currentSpeed = 3f;
    public int MaxHP = 3;

    private int currentHP = 0;
    public float attackRange = 5f;
    public int attackPower = 1;
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    private bool isDashing = false;
    public float attackTimer = 0f;
    Rigidbody2D body;
    public Transform playerTransform;
    // (:-|)
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    void onEnable() {
        currentHP = MaxHP;
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
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0){
            Die();
        }
    }
    public void Die()
    {
        EnemyPoolManager.Instance.ReturnEnemy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D other){
        IDamageable damageable = other.GetComponent<IDamageable>();
        if(damageable != null){
            damageable.TakeDamage(1);
        }
    }
}
