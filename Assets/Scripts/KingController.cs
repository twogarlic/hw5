using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lifeTime = 30f;
    public int health = 5;
    private Rigidbody rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        rb.MovePosition(transform.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(2);  
            }

            health--;

            if (health <= 0)
            {
                GameController gameController = FindObjectOfType<GameController>();
                gameController.AddKill();

                Destroy(gameObject);  
            }
        }
    }

}
