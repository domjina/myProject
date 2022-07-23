using UnityEngine;

/// <summary>
/// Handles Beam GameObject and its animation
/// </summary>
public class Beam : MonoBehaviour
{

    /// <summary>
    /// Internal timer.
    /// </summary>
    private float timer = 0;

    /// <summary>
    /// The GameObject itself.
    /// </summary>
    private GameObject beam;

    /// <summary>
    /// If the beam is lethal to the player or not.
    /// Required to distinguish the different phases of the beam.
    /// </summary>
    private bool lethal = false;

    /// <summary>
    /// Time for the beam to increase. It is not lethal yet.
    /// </summary>
    public float timeToIncrease = 2;

    /// <summary>
    /// How long the beam should remain. It is lethal by now.
    /// </summary>
    public float timeToStay = 3;

    void Start()
    {
        beam = gameObject;
        beam.transform.localScale =
            new Vector3(10, 0, 1);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < timeToIncrease) {
            beam.transform.localScale += ((1 / timeToIncrease) * Time.deltaTime * new Vector3(0, 1, 0));
        } else if (timer > timeToIncrease && timer < timeToIncrease + timeToStay) {
            lethal = true;
        } else if (timer > timeToIncrease + timeToStay) {
            Destroy(beam);
        }
    }

    /// <summary>
    /// Shows if a beam is lethal to the player or not.
    /// </summary>
    /// <returns>True if lethal, false if not</returns>
    public bool GetLethal() {
        return lethal;
    }
}
