using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    void Start()
    {
        GameObject player = FindPlayer();
        if (player == null) return;

        player.SetActive(true);

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

    private static GameObject FindPlayer()
    {
        MovementScript[] found = FindObjectsByType<MovementScript>(FindObjectsInactive.Include);
        return found.Length > 0 ? found[0].gameObject : null;
    }
}
