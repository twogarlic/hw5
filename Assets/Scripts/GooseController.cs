using UnityEngine;

public class GooseController : MonoBehaviour
{
    public float moveSpeed = 5f;  
    public float changeDirectionInterval = 3f;  

    private Rigidbody rb;
    private Vector3 moveDirection;
    private float timeToChangeDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        ChangeDirection();
    }

    void Update()
    {
        timeToChangeDirection -= Time.deltaTime;
        if (timeToChangeDirection <= 0)
        {
            ChangeDirection();  
        }
    }

    void FixedUpdate()
    {
        Move();  
    }

    void Move()
    {
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    void ChangeDirection()
    {
        float randomAngle = Random.Range(0f, 360f);  
        moveDirection = new Vector3(Mathf.Cos(randomAngle * Mathf.Deg2Rad), 0f, Mathf.Sin(randomAngle * Mathf.Deg2Rad));  

        timeToChangeDirection = changeDirectionInterval;
    }

}
