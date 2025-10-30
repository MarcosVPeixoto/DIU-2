
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private string _nomeCena = "SampleScene";
    
    public void Jogar()
    {
        SceneManager.LoadScene(_nomeCena);
        Debug.Log("Reiniciando o jogo...");

    }
}
