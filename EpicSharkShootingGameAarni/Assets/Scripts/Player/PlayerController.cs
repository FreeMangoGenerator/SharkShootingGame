using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public Transform gunTransform;
    public float moveSpeed = 5f;

    private Master controls;

    private Rigidbody2D body;

    private Vector2 moveInput;

    void Awake(){
        controls = new Master();
        body = GetComponent<Rigidbody2D>();
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
        
    }

    void Update(){
        Shoot();
    }

    private void Shoot(){
        if(controls.Player.Fire.triggered){
            Debug.Log("this is an easter egg. --> 0 <-- (literally)");
            GameObject bullet = BulletPoolManager.Instance.GetBullet();
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
}
