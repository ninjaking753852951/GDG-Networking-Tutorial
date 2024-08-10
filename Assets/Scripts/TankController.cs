using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TankController : NetworkBehaviour
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
        if (!IsLocalPlayer)
            this.enabled = false;   
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
                    ShootRPC();
                break;
            case 1:
                moveInput = new Vector2(Input.GetAxisRaw("Horizontal1"), Input.GetAxisRaw("Vertical1"));
                if(Input.GetKeyDown(KeyCode.Space))
                    ShootRPC();
                break;
        }
    }

    [Rpc(SendTo.Server)]
    void ShootRPC()
    {
        GameObject bulletClone = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bulletClone.GetComponent<NetworkObject>().Spawn();
    }

    void Move()
    {
        rb.MovePosition(transform.position + transform.up * moveInput.y * Time.fixedDeltaTime * moveSpeed);

        curRotation += moveInput.x * Time.fixedDeltaTime * turnSpeed * -1;
        rb.SetRotation(Quaternion.Euler(0,0,curRotation));
    }

    [Rpc(SendTo.Everyone)]
    public void SetActiveRPC(bool active)
    {
        gameObject.SetActive(active);
    }

    [Rpc(SendTo.Owner)]
    public void TeleportRPC(Vector3 pos)
    {
        transform.position = pos;
    }
}
