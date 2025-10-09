using UnityEngine;
using UnityEngine.InputSystem;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    private PlayerInput playerInput;
    private InputAction m_shoot;

    private SpriteRenderer playerSpriteRenderer;


    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            m_shoot = playerInput.actions["Fire1"];
            m_shoot.Enable();
        }
        else
        {
            Debug.LogError("PlayerInput component not found!");
        }

        playerSpriteRenderer = GetComponentInParent<SpriteRenderer>();

        if (playerSpriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer do player não encontrado!");
        }
    }

    void Update()
    {
        if (m_shoot != null && m_shoot.WasPressedThisFrame())
        {
            Shoot();
            Debug.Log("Shot fired!");
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // Determina a direção baseado no flipX do player
        float direction = playerSpriteRenderer.flipX ? -1f : 1f;

        // Passa a direção para a bala
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }
}