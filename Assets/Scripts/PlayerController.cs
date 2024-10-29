using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    public float rotationSpeed = 10f;
    public float bulletSpeed = 5f;
    public GameObject bulletPrefab;
    public float firePointYOffset = 1.5f;

    public int maxHealth = 10;
    private int currentHealth;
    public float invincibilityDuration = 2f;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;

    private Rigidbody rb;
    private Transform firePoint;
    private bool isDead = false;

    public Material playerMaterial;
    public Color glowColor = Color.red; 
    public float glowIntensity = 2.0f; 
    private Color originalColor; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        firePoint = new GameObject("FirePoint").transform;
        firePoint.position = new Vector3(transform.position.x, transform.position.y + firePointYOffset, transform.position.z);
        firePoint.parent = transform;

        if (playerMaterial != null)
        {
            originalColor = playerMaterial.GetColor("_GlowColor");
            playerMaterial.SetFloat("_GlowIntensity", 0f); 
        }
    }

    void Update()
    {
        Move();
        Shoot();

        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                DisableGlowEffect(); 
            }
        }
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 moveDirection = new Vector3(moveX, 0.0f, moveZ);

        if (moveDirection.magnitude > 0)
        {
            rb.velocity = moveDirection.normalized * currentSpeed + new Vector3(0.0f, rb.velocity.y, 0.0f);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            Vector3 targetPosition;

            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
            }
            else
            {
                targetPosition = ray.GetPoint(1000f);
            }

            Vector3 direction = (targetPosition - firePoint.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            bulletRb.useGravity = false;

            bulletRb.AddForce(direction * bulletSpeed, ForceMode.Impulse);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || isDead)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            SceneManager.LoadScene("LoseScene");
        }

        isInvincible = true;
        invincibilityTimer = invincibilityDuration;

        EnableGlowEffect(); 
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("King"))
        {
            TakeDamage(1);
        }

        if (other.gameObject.CompareTag("HealthPickup"))
        {
            Heal(3);
            Destroy(other.gameObject);
        }
    }

    private void EnableGlowEffect()
    {
        if (playerMaterial != null)
        {
            playerMaterial.SetColor("_GlowColor", glowColor);
            playerMaterial.SetFloat("_GlowIntensity", glowIntensity);
        }
    }

    private void DisableGlowEffect()
    {
        if (playerMaterial != null)
        {
            playerMaterial.SetColor("_GlowColor", originalColor);
            playerMaterial.SetFloat("_GlowIntensity", 0f);
        }
    }
}
