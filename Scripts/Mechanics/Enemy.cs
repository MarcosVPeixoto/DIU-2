using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int maxHealth = 50;
    public int currentHealth;
    public int damageToPlayer = 10;
    public bool isBoss = false;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float patrolDistance = 5f;
    public bool canMove = true;

    [Header("Combat")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float detectionRange = 10f;
    public float shootCooldown = 2f;
    public LayerMask playerLayer;

    [Header("Effects")]
    public GameObject deathEffect;
    public Color damageColor = Color.red;

    [Header("Loot")]
    public GameObject lootDrop;
    public int scoreValue = 10;
    [Header("Jump Settings")]
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isJumper = true;
    private bool canJumpAgain = true;
    public float jumpCooldown = 2.5f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Vector3 startPosition;
    private float direction = 1f;
    private Rigidbody2D rb;
    private Transform player;
    private float lastShootTime;
    private bool playerDetected = false;


    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        startPosition = transform.position;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        DetectPlayer();

        if (playerDetected && player != null)
        {
            StopAndShoot();
        }
        else if (canMove)
        {
            Patrol();
        }
        if (playerDetected && canJumpAgain && isJumper)
        {
            Jump();
        }
    }

    void DetectPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {

            playerDetected = true;

            if (player.position.x > transform.position.x && direction < 0)
            {
                Flip();
            }
            else if (player.position.x < transform.position.x && direction > 0)
            {
                Flip();
            }
        }
        else
        {
            playerDetected = false;
        }
    }
    void Jump()
    {
        if (rb == null) return;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        canJumpAgain = false;
        Invoke(nameof(UpdateJumpStatus), jumpCooldown);
    }
    void UpdateJumpStatus()
    {
       canJumpAgain = true;
    }
    void StopAndShoot()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        if (Time.time >= lastShootTime + shootCooldown)
        {

            Shoot();
            lastShootTime = Time.time;
        }
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            return;
        }

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Define a direção da bala
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
            bulletScript.SetOwner("enemy");
        }

    }

    void Patrol()
    {
        float distance = Vector3.Distance(startPosition, transform.position);

        if (distance >= patrolDistance)
        {
            Flip();
        }

        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
        }
    }

    void Flip()
    {
        direction *= -1;

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (spriteRenderer != null)
        {
            StartCoroutine(DamageFlash());
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator DamageFlash()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void Die()
    {

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        if (lootDrop != null)
        {
            Instantiate(lootDrop, transform.position, Quaternion.identity);
        }
        Debug.Log("Adicionando pontuação: " + ScoreManager.instance.currentScore);
        ScoreManager.instance.AddScore(100);
        Destroy(gameObject);
    }



}