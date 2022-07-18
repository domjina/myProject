using System.Configuration;
using UnityEngine;

/// <summary>
/// Handles the Ghost Trigger Tile.
/// </summary>
public class GhostTrigger : MonoBehaviour {

    /// <summary>
    /// Number of ghost the ghost spawner should create.
    /// </summary>
    private byte numberOfGhosts = 5;

    /// <summary>
    /// How many frames should the trigger wait till creating all ghosts.
    /// </summary>
    public int framesTillSpawn = 30;

    /// <summary>
    /// How many frames should past between each ghost spawn.
    /// </summary>
    public int framesBetweenSpawn = 20;

    /// <summary>
    /// How long should the ghost follow the player.
    /// </summary>
    public int framesToFollow = 600;
    
    /// <summary>
    /// Begin the spawning of ghosts.
    /// </summary>
    public void BeginSpawning() {
        for (int i = 0; i < numberOfGhosts; i++) {
            CreateGhost(framesBetweenSpawn*i);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// Creates a ghost.
    /// </summary>
    /// <param name="framesBehind">How many frames later should the ghost spawn</param>
    private void CreateGhost(int framesBehind) {
        GameObject ghost = (GameObject)Instantiate(
            Resources.Load("Objects/Ghost", typeof(GameObject)));
        ghost.GetComponent<Ghost>().framesBehind = framesBehind;
        ghost.GetComponent<Ghost>().framesToFollow = framesToFollow;
    }

}
