using UnityEngine;
using TMPro; // Use TextMeshPro para texto moderno

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton
    public int currentScore = 0;
    public TextMeshProUGUI scoreText; // arraste o texto aqui no Inspector

    void Awake()
    {
        // Garante que só exista um ScoreManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Mantém o ScoreManager ao trocar de cena (opcional)
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
    }
}
