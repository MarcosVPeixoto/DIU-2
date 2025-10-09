using UnityEngine;
using Platformer.Mechanics;
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 20f;
    public float lifetime = 3f;
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
        Debug.Log($"{transform.position.x}");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //transform = GetComponent<Transform>();

        if (rb == null)
        {
            Debug.LogError("ERRO: Rigidbody2D não encontrado na bala!");
            return;
        }

        // Move a bala para frente
        rb.linearVelocity = new Vector2((direction * speed) / 4, 0);

        Debug.Log("Bala disparada! Velocidade: " + rb.linearVelocity);

        // Destroi a bala após o tempo de vida
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log("Bala colidiu com: " + hitInfo.gameObject.name + " (Tag: " + hitInfo.tag + ")");

        // Ignora colisões com Player, câmera e outros objetos que não devem destruir a bala
        if (
            hitInfo.CompareTag("MainCamera") ||
            hitInfo.gameObject.name.Contains("CinemachineConfiner") 
            )
        {
            Debug.Log("Ignorando colisão com: " + hitInfo.gameObject.name);
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