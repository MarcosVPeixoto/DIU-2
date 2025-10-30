
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalManager : MonoBehaviour
{
    [SerializeField] private string _nomeCena = "SampleScene";
    
    public void Jogar()
    {
        SceneManager.LoadScene(_nomeCena);
        Debug.Log("Iniciando o jogo...");

    }
}
