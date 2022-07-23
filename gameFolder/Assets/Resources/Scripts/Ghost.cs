using UnityEngine;

/// <summary>
/// The Ghost GameObject follows the player.
/// </summary>
public class Ghost : MonoBehaviour
{

    /// <summary>
    /// The GameObject to follow,
    /// here it is the player.
    /// </summary>
    private GameObject player;

    /// <summary>
    /// The ghost GameObject is the gameObject itself.
    /// </summary>
    private GameObject ghost;

    /// <summary>
    /// This array keeps track of all the players Position.
    /// </summary>
    private Vector3[] playerPos;

    /// <summary>
    /// Iterative Counter.
    /// </summary>
    private int currentFrame = 0;

    /// <summary>
    /// How many frames should the ghost follow the player.
    /// </summary>
    public int framesToFollow = 600;

    /// <summary>
    /// <para>How many frames should the ghost be behind the player.</para>
    /// <para>See it as how far away the ghost should be away from the player.</para>
    /// </summary>
    public int framesBehind = 60;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        ghost = gameObject;

        // Not render the ghost and disable the hitbox
        // Still does not work, and I do not know why
        ghost.GetComponent<SpriteRenderer>().enabled = false;
        ghost.GetComponent<BoxCollider2D>().isTrigger = false;
        playerPos = new Vector3[framesToFollow+framesBehind];
    }

    void Update()
    {

        // Destroy the ghost after existing for too long.
        if (currentFrame == framesToFollow + framesBehind) {
            Destroy(ghost);
            return;
        }

        // Only after the ghost waited enough, begin following the player.
        if (currentFrame > framesBehind) {
            ghost.transform.position = playerPos[currentFrame - framesBehind];
            ghost.GetComponent<SpriteRenderer>().enabled = true;
            ghost.GetComponent<BoxCollider2D>().isTrigger = true;
        }

        // Record the players movement.
        if (player != null) playerPos[currentFrame] = player.transform.position;
        currentFrame++;
    }
}
