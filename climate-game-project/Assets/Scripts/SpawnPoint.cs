using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        MovementScript movement = player.GetComponent<MovementScript>();
        if (movement != null) movement.enabled = true;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.position = (Vector2)transform.position;
        }
        else
        {
            player.transform.position = transform.position;
        }
    }
}
