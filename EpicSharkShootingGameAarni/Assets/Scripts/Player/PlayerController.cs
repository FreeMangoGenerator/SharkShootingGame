using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour, IDamageable //🥶💀
{
    public GameObject deathParticle;
    public int maxHP = 10;
    private int currentHP;
    public Transform gunTransform;
    public float moveSpeed = 5f;
    public Sprite sideSprite;
    public Sprite topSprite;
    private SpriteRenderer spriteRenderer;
    private Master controls;
    private Rigidbody2D body;
    private Vector2 moveInput;
    private Vector2 aimInput;

    void Awake(){
        controls = new Master();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void OnEnable() {
        controls.Enable();
         }

    void OnDisable() {
        controls.Disable();
         }

// hello you have found my player controller script, please die now
    void Start()
    {
        GameManager.Instance.GetPlayer = this;
        currentHP = maxHP;
    }
     
    
    void Update(){
        Shoot();
        Aim();
        UpdSprDir();
    }
    void UpdSprDir(){
        if(moveInput.sqrMagnitude > 0.1f){
            if(Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)){
                spriteRenderer.sprite = sideSprite;
                spriteRenderer.flipX = moveInput.x < 0;
                spriteRenderer.flipY = false;
            }
            else{
                spriteRenderer.sprite = topSprite;
                spriteRenderer.flipY = moveInput.y < 0;
                spriteRenderer.flipX = false;
            }
        }
    }
    private void Aim(){
        aimInput = controls.Player.Aim.ReadValue<Vector2>();
        if(aimInput.sqrMagnitude > 0.1){
            Vector2 aimDirection = Vector2.zero;
            if(UsingMouse()){
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                mousePosition.z = 0;
                aimDirection = mousePosition - gunTransform.position;
            }
            else{
                aimDirection = aimInput;

            }

            float angle = Mathf.Atan2(aimDirection.x, -aimDirection.y) * Mathf.Rad2Deg;
            gunTransform.rotation = Quaternion.Euler(0,0,angle);
        }

    }
    private bool UsingMouse(){
        if(Mouse.current.delta.ReadValue().sqrMagnitude > 0.1){
            return true;
        }
        return false;
    }

    private void Shoot(){
        if(controls.Player.Fire.triggered){
            Debug.Log("=)");
            GameObject bullet = BulletPoolManager.Instance.GetBullet();
            if(bullet == null){
                return;
            }
            bullet.transform.position = gunTransform.position;
            bullet.transform.rotation = gunTransform.rotation;
        }
    }

    void FixedUpdate()
    {
       Move();
    }
    
    void Move(){
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        Vector2 movement = new Vector2(moveInput.x, moveInput.y) * moveSpeed * Time.fixedDeltaTime;
        body.MovePosition(body.position + movement);
    }
    public void TakeDamage(int damage){
        currentHP -= damage;
        if(currentHP <= 0){
            Die();
        }
    }
    public void Die(){
        Debug.Log("I love goat cheese but sadly I have to eat Diarrhea"); // True
        Instantiate(deathParticle,transform.position, Quaternion.identity);
        GameManager.Instance.ChangeGameState(GameState.GameOver);
    }

}
