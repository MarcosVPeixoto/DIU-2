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
            Debug.LogError("SpriteRenderer do player n�o encontrado!");
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
        if (ScoreManager.instance.currentScore <= 0) return;
        ScoreManager.instance.AddScore(-20);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        float direction = playerSpriteRenderer.flipX ? -1f : 1f;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction);
        }
    }
}