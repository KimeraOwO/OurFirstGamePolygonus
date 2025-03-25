using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    public Vector3 offset = new Vector3(0f, 2f, -10f); // Ajuste de posición

    void LateUpdate()
    {
        if (player != null) // Asegurarse de que el jugador existe
        {
            transform.position = player.position + offset; // Seguir al jugador con un desplazamiento
        }
    }
}
