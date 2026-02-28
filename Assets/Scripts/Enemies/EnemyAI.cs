using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float speed = 3f;
    public float detectionRange = 10f;

    public float separationDistance = 1.2f;
    public float separationForce = 2f;

    public float pushForce = 20f;

    public float dashSpeed = 12f;
    public float dashRange = 3f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 2f;

    private bool isDashing = false;
    private float dashEndTime = 0f;
    private float nextDashTime = 0f;

    public float spawnFadeTime = 0.5f;

    private SpriteRenderer sr;
    private bool spawning = true;


    public float maxLifeTime = 8f;      // tiempo antes de reaparecer
    public float respawnRadius = 10f;   // radio donde reaparece
    private float lifeTimer = 0f;

   
    private float currentLifeTime; // tiempo único para cada enemigo

    private Transform player;
    private Rigidbody2D rb;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        StartCoroutine(SpawnEffect());
    }



    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Tiempo de vida del enemigo
        lifeTimer += Time.fixedDeltaTime;

        if (lifeTimer >= currentLifeTime)
        {
            Respawn();
            lifeTimer = 0f;

            // Nuevo tiempo aleatorio para el siguiente respawn
            currentLifeTime = Random.Range(maxLifeTime * 0.5f, maxLifeTime * 1.5f);
        }

        // DASH
        if (!isDashing && Time.time >= nextDashTime && distanceToPlayer < dashRange)
        {
            isDashing = true;
            dashEndTime = Time.time + dashDuration;
            nextDashTime = Time.time + dashCooldown;

            Vector2 dashDir = (player.position - transform.position).normalized;
            rb.linearVelocity = dashDir * dashSpeed;
            return; // saltamos el movimiento normal mientras dura el dash
        }

        if (spawning)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }


        // Fin del dash
        if (isDashing)
        {
            if (Time.time >= dashEndTime)
            {
                isDashing = false;
            }
            else
            {
                return; // seguimos en dash
            }
        }

        // Movimiento normal
        Vector2 movement = Vector2.zero;

        if (distanceToPlayer < detectionRange)
        {
            Vector2 dirToPlayer = (player.position - transform.position).normalized;
            movement += dirToPlayer;
        }

        // Separación entre enemigos
        // Separación entre enemigos (versión fuerte)
GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
foreach (GameObject e in enemies)
{
    if (e == this.gameObject) continue;

    float dist = Vector2.Distance(transform.position, e.transform.position);

    if (dist < separationDistance)
    {
        Vector2 away = (transform.position - e.transform.position).normalized;

        // Fuerza de separación exponencial: cuanto más cerca, más fuerte
        float force = separationForce * (1f / Mathf.Max(dist, 0.1f));

        movement += away * force;
    }
}


        rb.linearVelocity = movement.normalized * speed;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Quitar vida
            collision.collider.GetComponent<PlayerHealth>().TakeDamage();

            // Empuje físico REAL
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            Vector2 pushDir = (collision.collider.transform.position - transform.position).normalized;
            playerRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Player"))
    {
        // Quitar vida
        collision.GetComponent<PlayerHealth>().TakeDamage();

        // Separar al jugador
        Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            float separationDistance = 2f; // distancia mínima deseada
            Vector2 pushDir = (collision.transform.position - transform.position).normalized;

            // Nueva posición del jugador, alejado del enemigo
            Vector2 newPos = (Vector2)transform.position + pushDir * separationDistance;

            // Teletransporte suave (sin romper física)
            playerRb.position = newPos;
        }
    }

}
    void Respawn()
    {
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        Vector2 newPos = (Vector2)player.position + randomDir * respawnRadius;

        // Asegurar que no reaparece demasiado cerca
        if (Vector2.Distance(newPos, player.position) < 3f)
            newPos = (Vector2)player.position + randomDir * (respawnRadius + 3f);

        rb.position = newPos;
        rb.linearVelocity = Vector2.zero;
    }
    IEnumerator SpawnEffect()
    {
        spawning = true;
        rb.linearVelocity = Vector2.zero;

        Color c = sr.color;
        c.a = 0f;
        sr.color = c;

        float t = 0f;
        while (t < spawnFadeTime)
        {
            t += Time.deltaTime;
            c.a = t / spawnFadeTime;
            sr.color = c;
            yield return null;
        }

        spawning = false;
    }

    IEnumerator SpawnFlash()
    {
        spawning = true;
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < 6; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        sr.enabled = true;
        spawning = false;
    }


}
