using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{

    [Header("Shoot Settings")]
    public Transform shootPoint;
    public GameObject bulletPrefab;
    
    [Header("Move Settings")]
    public float moveSpeed = 2;
    public float turnSpeed = 360;

    public int playerID; // 0 = wasd, 1 = arrow keys
    
    Vector2 moveInput;
    Rigidbody2D rb;
    float curRotation;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GetInput()
    {
        switch (playerID)
        {
            case 0:
                moveInput = new Vector2(Input.GetAxisRaw("Horizontal0"), Input.GetAxisRaw("Vertical0"));
                if(Input.GetKeyDown(KeyCode.E))
                    Shoot();
                break;
            case 1:
                moveInput = new Vector2(Input.GetAxisRaw("Horizontal1"), Input.GetAxisRaw("Vertical1"));
                if(Input.GetKeyDown(KeyCode.Space))
                    Shoot();
                break;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
    }

    void Move()
    {
        rb.MovePosition(transform.position + transform.up * moveInput.y * Time.fixedDeltaTime * moveSpeed);

        curRotation += moveInput.x * Time.fixedDeltaTime * turnSpeed * -1;
        rb.SetRotation(Quaternion.Euler(0,0,curRotation));
    }
}
