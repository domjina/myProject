using UnityEngine;

/// <summary>
/// <para>Controls the spawn behaviour of the positron spawner.</para>
/// <para>A positron spawner sends out positrons in two (opposing) directions.</para>
/// </summary>
public class PositronSpawner : MonoBehaviour
{

    /// <summary>
    /// The positron spawner as GameObject itself.
    /// </summary>
    private GameObject positronSpawner;

    /// <summary>
    /// Iterative timer; Needed to shoot positrons after
    /// some time interval.
    /// </summary>
    private float time = 0.0f;

    // velocity and range could be set, but I wanted to have the option to change
    // them on Runtime when randomly generate them.

    /// <summary>
    /// Velocity of the positrons.
    /// </summary>
    public float velocity = 2.0f;

    /// <summary>
    /// Range of the positrons.
    /// </summary>
    public float range = 15.0f;

    /// <summary>
    /// <para>Every time interval that passes, a positron spawns.</para>
    /// <para>E.g. timeInterval of 1.0f seconds means
    /// every second a positron spawns.</para>
    /// </summary>
    public float timeInterval = 1.0f;

    /// <summary>
    /// Standard direction of the positron spawner.
    /// </summary>
    public Vector3 direction = Vector3.right;

    void Start()
    {
        positronSpawner = gameObject;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > timeInterval) {
            time = 0;
            Shoot();
        }
    }

    /// <summary>
    /// Shoots positrons on both sides 
    /// </summary>
    private void Shoot() {
        GameObject positron1 = (GameObject)Instantiate(
            Resources.Load("Objects/Positron", typeof(GameObject)), positronSpawner.transform);
        positron1.GetComponent<Positron>().SetProperties(direction, velocity, range);
        GameObject positron2 = (GameObject)Instantiate(
            Resources.Load("Objects/Positron", typeof(GameObject)), positronSpawner.transform);
        positron2.GetComponent<Positron>().SetProperties(-direction, velocity, range);
    }

    /// <summary>
    /// Set the direction of the spawner. Note that on both sides positrons spawn
    /// </summary>
    /// <param name="vector3"></param>
    public void SetDirection(Vector3 vector3) {
        direction = vector3;
    }
}
