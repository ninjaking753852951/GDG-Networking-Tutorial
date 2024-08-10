using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;

public class Projectile : NetworkBehaviour
{
    public float speed = 5f; // Speed of the projectile
    public int maxBounces = 5;
    public GameObject explosionEffect;
    private Rigidbody2D rb;

    Vector2 curDir;
    int curBounces;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ApplyDirection(transform.up);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(!IsServer)
            return;
        
        TankController hitTank = collision.gameObject.GetComponent<TankController>();
            
        curBounces++;
        Vector2 reflectDirection = Vector2.Reflect(curDir.normalized, collision.GetContact(0).normal);
        ApplyDirection(reflectDirection);
        
        if (curBounces >= maxBounces || hitTank != null)
            Detonate(hitTank);
    }

    void ApplyDirection(Vector2 dir)
    {
        curDir = dir;
        rb.velocity = dir.normalized * speed;
        
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void Detonate(TankController player = null)
    {
        if (player != null)
        {
            player.SetActiveRPC(false);
            GameManager.instance.TankDestroyed((int)player.NetworkObject.OwnerClientId);
        }
        
        Instantiate(explosionEffect, transform.position, quaternion.identity);
        
        Destroy(gameObject);
    }
}
