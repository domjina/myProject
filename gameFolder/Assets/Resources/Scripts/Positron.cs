using UnityEngine;

/// <summary>
/// <para>Controls the behaviour of the positron.</para>
/// <para>A positron moves into a direction with
/// a velocity and dies after it reached a range.</para>
/// </summary>
public class Positron : MonoBehaviour {

    /// <summary>
    /// The direction of the positron moving.
    /// </summary>
    private Vector3 direction;

    /// <summary>
    /// The initial position of the positron.
    /// </summary>
    private Vector3 startPos;

    /// <summary>
    /// The velocity of the positron.
    /// </summary>
    private float velocity;

    /// <summary>
    /// The distance where the positron will despawn.
    /// </summary>
    private float range = 10.0f;

    /// <summary>
    /// The positron as GameObject itself.
    /// </summary>
    private GameObject positron;

    public void Start() {

        positron = gameObject;
        startPos = positron.transform.position;
        // Z Coordinate determines layer rendering. Set abritary to 3.
        startPos.z = 3;
    }

    public void Update() {

        positron.transform.position +=
            Time.deltaTime * velocity * direction;

        if (Vector3.Magnitude(startPos - positron.transform.position) > range) {
            Destroy(positron);
        }
    }

    /// <summary>
    /// Sets properties of the positron.
    /// </summary>
    /// <param name="dir">The supposed direction of the positron</param>
    /// <param name="vel">The supposed velocity of the positron</param>
    /// <param name="rang">The supposed range of the positron</param>
    public void SetProperties(Vector3 dir, float vel, float rang) {
        direction = dir;
        velocity = vel;
        range = rang;
    }

}
