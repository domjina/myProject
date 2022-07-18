using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles player control and PvE interactions.
/// </summary>
public class Player : MonoBehaviour
{

    /// <summary>
    /// Player velocity.
    /// </summary>
    private float velocity;

    /// <summary>
    /// The electron as GameObject itself.
    /// </summary>
    private GameObject playerObject;

    /// <summary>
    /// <para>The forward vector of the electron.</para>
    /// <para>This is not the same as transform.forward!</para>
    /// </summary>
    private Vector3 forwardVector;

    /// <summary>
    /// The buttons that controll player movement.
    /// </summary>
    public Button   uiButtonUp,
                    uiButtonDown,
                    uiButtonLeft,
                    uiButtonRight;

    /// <summary>
    /// The display to show the amount of batterys a player obtained.
    /// </summary>
    public Text     uiBatteryDisplay;

    /// <summary>
    /// <para>Makes the player invulnerable against anything.</para>
    /// <para> Even fucking walls! Walls man!!!</para>
    /// </summary>
    public bool invulnerable = false;

    void Start()
    {
        velocity = 2.0f;
        playerObject = GameObject.Find("Player");
        forwardVector = new Vector3(0, 1, 0);

        AddInputListener();
    }

    void Update()
    {
        MoveForward();
    }

    /// <summary>
    /// <para>Moves the electron forward.</para>
    /// Method is applied for one frame.
    /// Call method every frame to make it move smoothly.
    /// </summary>
    private void MoveForward() {

        // Time.deltaTime is the time for one frame.
        // This makes sure that the object doesnt move erratically,
        // if the framerate should drop.
        playerObject.transform.position +=
            Time.deltaTime * velocity * forwardVector;
    }

    /// <summary>
    /// <para>Checks if the user tappes one of the controller buttons.</para>
    /// <para>Turns the electron accordingly.</para>
    /// </summary>
    private void AddInputListener() {
        uiButtonUp.onClick.AddListener(delegate { forwardVector = Vector3.up; });
        uiButtonDown.onClick.AddListener(delegate { forwardVector = Vector3.down; });
        uiButtonRight.onClick.AddListener(delegate { forwardVector = Vector3.right; });
        uiButtonLeft.onClick.AddListener(delegate { forwardVector = Vector3.left; });
    }

    /// <summary>
    /// Removes the user input. Needed for the LED tile.
    /// </summary>
    private void RemoveInputListener() {
        uiButtonUp.onClick.RemoveListener(delegate { forwardVector = Vector3.up; });
        uiButtonDown.onClick.RemoveListener(delegate { forwardVector = Vector3.down; });
        uiButtonRight.onClick.RemoveListener(delegate { forwardVector = Vector3.right; });
        uiButtonLeft.onClick.RemoveListener(delegate { forwardVector = Vector3.left; });
    }

    /// <summary>
    /// <para>Checks if the player GameObject entered a trigger box.</para>
    /// Currently supports Resistance and Coils.
    /// </summary>
    /// <param name="powerup">The powerup that has been hit</param>
    private void OnTriggerEnter2D(Collider2D powerup) {

        GameObject puobj = powerup.gameObject;

        if(puobj.CompareTag("Resistance")) {
            velocity = 0.5f * velocity;
        } else if (puobj.CompareTag("Coil")){
            velocity = 2f * velocity;
        } else if (puobj.CompareTag("Wall") && invulnerable == false) {
            Kill();
        } else if (puobj.CompareTag("Battery")) {
            Destroy(puobj);
            // Increase number for the battery display
            uiBatteryDisplay.text = (int.Parse(uiBatteryDisplay.text) + 1).ToString();
        } else if (puobj.CompareTag("BrokenCircuit")) {
            Destroy(puobj);
            velocity /= 2;
        } else if (puobj.CompareTag("BeamTrigger")) {
            puobj.GetComponent<BeamTrigger>().Activate();
        } else if (puobj.CompareTag("Shield")) {
            Destroy(puobj);
            StartCoroutine(ShieldRoutine());
        } else if (puobj.CompareTag("LED")) {
            forwardVector = puobj.transform.eulerAngles;
            RemoveInputListener();
        } else if (puobj.CompareTag("GhostTrigger")) {
            puobj.GetComponent<GhostTrigger>().BeginSpawning();
        }
    }

    private void OnTriggerExit2D(Collider2D powerup) {
        GameObject puobj = powerup.gameObject;

        if (puobj.CompareTag("LED")) {
            AddInputListener();
        }
    }

    /// <summary>
    /// Handles the animations for the Shield Collectible.
    /// </summary>
    /// <returns></returns>
    IEnumerator ShieldRoutine() {

        invulnerable = true;
        playerObject.transform.localScale = Vector3.one;

        yield return new WaitForSeconds(10);

        playerObject.transform.localScale = 0.5f * Vector3.one;
        invulnerable = false;

    }

    /// <summary>
    /// Kills the player.
    /// </summary>
    void Kill() {
        Destroy(playerObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // I left following methods in case we want to add
    // a powerup tile that changes the players movement set.

    /// <summary>
    /// Rotates the forward vector by an angle. Left in.
    /// Could be useful in the future.
    /// </summary>
    /// <param name="angle">The angle in degrees,
    /// by which the forward vector should be turned.</param>
    /*private void RotateFwdVecBy(float angle) {
        rotation += angle;
        RotateFwdVec();
    }*/

    /// <summary>
    /// Rotates the forward vector to the rotation. Left in.
    /// Could be useful in the future.
    /// </summary>
    /*private void RotateFwdVec() {
        forwardVector.x = Mathf.Sin(Mathf.Deg2Rad * rotation);
        forwardVector.y = Mathf.Cos(Mathf.Deg2Rad * rotation);
    }*/

    /// <summary>
    /// Rotates the forward vector to the rotation. Left in.
    /// Could be useful in the future.
    /// </summary>
    /// <param name="angle">The angle in degrees to rotate to</param>
    /*private void RotateFwdVec(float angle) {
        forwardVector.x = Mathf.Sin(Mathf.Deg2Rad * angle);
        forwardVector.y = Mathf.Cos(Mathf.Deg2Rad * angle);
    }*/

}
