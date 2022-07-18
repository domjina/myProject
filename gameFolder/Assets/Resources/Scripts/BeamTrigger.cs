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
    /// Max width of the beam in terms of scale.
    /// </summary>
    public float maxWidth = 0.75f;

    /// <summary>
    /// <para>Minimum width of the beam in terms of scale.</para>
    /// <para>Needed to dispay the beam before increasing its size.</para>
    /// </summary>
    public float minWidth = 0.05f;

    /// <summary>
    /// Time delay in seconds for creating a small beam.
    /// </summary>
    public float timeToCreate = 0.5f;

    /// <summary>
    /// Time delay in seconds for stretching the beam.
    /// </summary>
    public float timeToExtend = 0.5f;

    /// <summary>
    /// Time to hold the beam at its max size.
    /// </summary>
    public float timeToHooold = 2.0f;

    /// <summary>
    /// Time to shrink the beam after it has reached its max size.
    /// </summary>
    public float timeToShrink = 0.5f;

    /// <summary>
    /// The beamTrigger Tile as GameObject.
    /// </summary>
    private GameObject beamTrigger;

    /// <summary>
    /// List of all beams.
    /// </summary>
    private List<GameObject> beams = new();

    /// <summary>
    /// <para>Describes if the beams should be vertical or horizontal.</para>
    /// <para>0 for horizontal, 1 for vertical.</para>
    /// </summary>
    private bool mode;

    void Start()
    {

        // Require a util method for random false or true values.
        mode = false;
        beamTrigger = gameObject;
    }

    /// <summary>
    /// List of animations instructions.
    /// </summary>
    /// <returns>Nothing but delay</returns>
    IEnumerator Routine() {
        
        byte randomNumber = (byte)Random.Range(0, 3);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < randomNumber; i++) {
            CreateBeam(mode);
        }
        yield return new WaitUntil(() => AnimateSecondExtend());
        foreach (GameObject beam in beams) {
            beam.GetComponent<BoxCollider2D>().enabled = true;
        }
        yield return new WaitForSeconds(3.0f);
        foreach (GameObject beam in beams) {
            beam.GetComponent<BoxCollider2D>().enabled = false;
        }
        yield return new WaitUntil(() => AnimateExit());
        foreach (GameObject beam in beams) {
            Destroy(beam);
        }
        beams.Clear();
    }

    /// <summary>
    /// Activate the beam animation sequence
    /// </summary>
    public void Activate() {
        StartCoroutine(Routine());
    }

    /// <summary>
    /// Animates the second extend animation
    /// </summary>
    /// <returns>True once done</returns>
    private bool AnimateSecondExtend() {
        if (!mode) {
            while (beams[0].transform.localScale.y < maxWidth) {
                foreach (GameObject beam in beams) {
                    beam.transform.localScale += new Vector3(
                        20.0f, (maxWidth - minWidth) * (Time.deltaTime / timeToExtend), 1.0f);
                }
            }
        } else {
            while (beams[0].transform.localScale.x < maxWidth) {
                foreach (GameObject beam in beams) {
                    beam.transform.localScale += new Vector3(
                        (maxWidth - minWidth) * (Time.deltaTime / timeToExtend), 20.0f, 1.0f);
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Animates the exit animation
    /// </summary>
    /// <returns>True once donce</returns>
    private bool AnimateExit() {
        if (!mode) {
            while (beams[0].transform.localScale.y < maxWidth) {
                foreach (GameObject beam in beams) {
                    beam.transform.localScale -= new Vector3(
                        20.0f, maxWidth * (Time.deltaTime / timeToExtend), 1.0f);
                }
            }
        } else {
            while (beams[0].transform.localScale.x < maxWidth) {
                foreach (GameObject beam in beams) {
                    beam.transform.localScale -= new Vector3(
                        maxWidth * (Time.deltaTime / timeToExtend), 20.0f, 1.0f);
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Creates a beam
    /// </summary>
    /// <param name="mode">Mode, 0 for vertical, 1 for horizontal mode</param>
    private void CreateBeam(bool mode) {

        GameObject beam = (GameObject)Instantiate(
                Resources.Load("Objects/Beam", typeof(GameObject)));
        beam.transform.position = new Vector3(
                Random.Range(-5, 5),
                Random.Range(-7, 7) + beamTrigger.transform.position.y,
                0);

        if (!mode) {
            beam.transform.localScale = new Vector3(20.0f, minWidth, 1.0f);
        } else {
            beam.transform.localScale = new Vector3(minWidth, 20.0f, 1.0f);
        }
        beams.Add(beam);
    }
}
