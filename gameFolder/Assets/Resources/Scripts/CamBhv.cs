using UnityEngine;

/// <summary>
/// Handles the camera movement in levels.
/// </summary>
public class CamBhv : MonoBehaviour
{
    
    /// <summary>
    /// Camera object itself.
    /// </summary>
    private Camera cam ;

    /// <summary>
    /// Object to be focused on, usually the player.
    /// </summary>
    public GameObject focusObject;

    /// <summary>
    /// If the camera should only move along the x axis.
    /// </summary>
    public bool followX = false;

    void Start() {
        cam = Camera.main;
        cam.orthographicSize = 3.5f / cam.aspect;
    }

    void Update()
    {
        if (focusObject != null) {
            FollowGameObject(focusObject, cam.transform.position.z, followX);
        }
    }

    /// <summary>
    /// Follows a GameObject.
    /// </summary>
    /// <param name="focus">The object that
    /// should be focused to</param>
    /// <param name="h">The height of the camera</param>
    /// <param name="fx">True if the camera should also follow
    /// the object along the x axis</param>
    private void FollowGameObject(GameObject focus, float h, bool fx) {
        if (fx) {
            cam.transform.position = new Vector3(focus.transform.position.x,
                focus.transform.position.y, h);
        } else {
            cam.transform.position = new Vector3(cam.transform.position.x,
                focus.transform.position.y, h);
        }
    }
}
