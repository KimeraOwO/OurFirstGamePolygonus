using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar

public class Enemy : MonoBehaviour
{
    public float patrolSpeed = 2f;    // Velocidad al patrullar
    public float chaseSpeed = 3f;     // Velocidad al perseguir al jugador
    public float detectionRange = 5f; // Distancia de detección del jugador
    public Transform pointA, pointB;  // Puntos de patrulla

    private Transform targetPoint;    // Punto de patrulla actual
    private Transform player;         // Referencia al jugador
    private Vector2 startPosition;    // Posición inicial del enemigo
    private Animator anim;            // Controlador de animaciones
    private bool isChasing = false;   // Si está persiguiendo o no

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Encuentra al jugador
        anim = GetComponent<Animator>();  // Obtiene el Animator
        targetPoint = pointA;             // Inicia patrullando hacia el punto A
        startPosition = transform.position; // Guarda su posición inicial
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador está dentro del rango, lo persigue
        if (distanceToPlayer < detectionRange)
        {
            isChasing = true;
            ChasePlayer();
        }
        else if (isChasing) // Si lo pierde de vista, regresa a su posición inicial
        {
            isChasing = false;
            ReturnToStart();
        }
        else // Si no está persiguiendo, patrulla entre los puntos A y B
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, patrolSpeed * Time.deltaTime);

        // Si llega a un punto, cambia de dirección
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = (targetPoint == pointA) ? pointB : pointA;
            Flip(); // Voltear sprite al cambiar de dirección
        }

        anim.SetFloat("EnemyMovement", Mathf.Abs(patrolSpeed)); // Cambia la animación
    }

    void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        FlipTowards(player.position.x); // Gira hacia el jugador
        anim.SetFloat("EnemyMovement", Mathf.Abs(chaseSpeed)); // Cambia la animación
    }

    void ReturnToStart()
    {
        transform.position = Vector2.MoveTowards(transform.position, startPosition, patrolSpeed * Time.deltaTime);

        FlipTowards(startPosition.x); // Gira hacia su punto inicial
        anim.SetFloat("EnemyMovement", Mathf.Abs(patrolSpeed));
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void FlipTowards(float positionX)
    {
        if ((positionX > transform.position.x && transform.localScale.x < 0) ||
            (positionX < transform.position.x && transform.localScale.x > 0))
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) // Si choca con el jugador
        {
            RestartPlayer(); // Llamamos a la función de reinicio
        }
    }

    void RestartPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reinicia la escena
    }
}

