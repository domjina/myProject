using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Handles the Beam trigger sequence.</para>
/// <para>The Animation goes as follows:</para>
/// <para>1. Increase the beams scale to a thin beam, indicating a beam is coming.</para>
/// <para>2. Wait for player to realise beam indication.</para>
/// <para>3. Increase the beams scale to a full beam, and enable hitbox.</para>
/// <para>4. Hold the beam till...</para>
/// <para>5. Decrease the beam and kill the GameObject.</para>
/// </summary>
public class BeamTrigger : MonoBehaviour
{
    /// <summary>
    /// The beamTrigger Tile as GameObject.
    /// </summary>
    private GameObject beamTrigger;

    void Start()
    {
        beamTrigger = gameObject;
    }

    public void Trigger() {
        CreateBeams();
        Destroy(beamTrigger);
    }

    /// <summary>
    /// Creates beams
    /// </summary>
    private void CreateBeams() {

        GameObject beam = (GameObject)Instantiate(
                Resources.Load("Objects/Beam", typeof(GameObject)));
        beam.transform.position = new Vector3(0, Random.Range(-7, 7) + beamTrigger.transform.position.y, 0);
    }
}
