using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSquare : MonoBehaviour
{

    private bool dragging = false;
    float changeInX, changeInY;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0){
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            switch(touch.phase){
                case TouchPhase.Began:
                    dragging = true;
                    changeInX = touchPosition.x - transform.position.x;
                    changeInY = touchPosition.y - transform.position.y;
                    break;

                case TouchPhase.Moved:
                    rigidBody.MovePosition(new Vector2(touchPosition.x - changeInX, touchPosition.y - changeInY));
                    break;

                case TouchPhase.Ended:
                    dragging = false;
                    break;
            }
        }
    }
}
