using UnityEngine;
using Platformer.Mechanics;
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 20f;
    public float lifetime = 0.5f;
    public int damage = 10;

    [Header("Effects")]
    public GameObject hitEffect;

    private Rigidbody2D rb;
    //private Transform transform;

    private float direction = 1f;

    private string owner = "player";

    public void SetDirection(float dir)
    {
        direction = dir;
    }

    public void SetOwner(string own)
    {
        owner = own;
    }

    void Update()
    {
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            return;
        }

        rb.linearVelocity = new Vector2((direction * speed) / 4, 0);

        if (owner == "player")
        {
            Invoke(nameof(DestroyBullet), 1f);

        }
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log("Bala colidiu com: " + hitInfo.gameObject.name + " (Tag: " + hitInfo.tag + ")");

        if (
            hitInfo.CompareTag("MainCamera") ||
            hitInfo.gameObject.name.Contains("CinemachineConfiner")
            )
        {
            return;
        }
        if (hitInfo.gameObject.name.Contains("Bullet"))
        {
            return;
        }
        if (hitInfo.CompareTag("Player") && owner == "player" || hitInfo.CompareTag("Enemy") && owner == "enemy")
        {
            return;
        }

        if (hitInfo.CompareTag("Enemy") && owner != "enemy")
        {
            Enemy enemy = hitInfo.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        if (hitInfo.CompareTag("Player") && owner != "player")
        {
            Debug.Log("Player atingido");

            PlayerController player = hitInfo.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        // Cria efeito de impacto
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }

        Debug.Log("Bala destruída por colisão!");
        // Destroi a bala
        Destroy(gameObject);
    }
}