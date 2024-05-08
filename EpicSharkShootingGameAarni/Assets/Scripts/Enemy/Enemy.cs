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
    public float attackCooldown = 2f;
    private bool isDashing = false;
    public float attackTimer = 0f;
    Rigidbody2D body;
    public Transform playerTransform;

    Vector2 direction;
    // (:-|)
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }
    void onEnable() {
        currentHP = MaxHP;
        isDashing = false;
    }
    void OnDisable() {
        StopAllCoroutines();
    }
    // ("_"<(ZZZZZ))
    void FixedUpdate()
    {
        
         Move();
         Attack();
           }

private void Attack(){
    if(playerTransform == null)
    {
        return;
    }
    if(attackTimer > 0){
        attackTimer -= Time.fixedDeltaTime;
    }
    else if(!isDashing && Vector2.Distance(transform.position, playerTransform.position) < attackRange)
    {
        StartCoroutine(DashAttack());
    }

    IEnumerator DashAttack(){

        isDashing = true;
        float startTime = Time.time;
        while(Time.time < startTime + dashDuration){
            body.velocity = direction * dashSpeed;
            yield return null;
        }
        

        
        body.velocity = Vector2.zero;
        isDashing = false;
        attackTimer= attackCooldown;
    }
}
    void Move(){
        if(playerTransform == null){
            GetPlayer();
            return;
            
        }
        if(isDashing){
            return;
        }
        direction = (playerTransform.position - transform.position).normalized;
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
        if(other.CompareTag("Player") && isDashing){
            other.GetComponent<IDamageable>().TakeDamage(1);
        }
        }
    }

