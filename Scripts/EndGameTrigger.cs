using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Verifica si el jugador tocó el trigger
        {
            SceneManager.LoadScene("CreditsScene"); // Carga la escena de créditos
        }
    }
}
