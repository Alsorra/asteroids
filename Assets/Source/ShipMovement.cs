using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    [SerializeField]
    private float mSpeed = 1.0f;

    private void Start() {
        Game.instance.onGameStart.AddListener(() => {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        });
    }

    private void FixedUpdate() {
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition() {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
            Vector3 movement = new Vector3(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"),
                0.0f);

            movement = movement * mSpeed * Time.fixedDeltaTime;

            transform.position += movement;
        }
    }

    private void UpdateRotation() {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        worldPosition.z = transform.position.z;

        Vector3 direction = (worldPosition - transform.position).normalized;
        transform.right = direction;

        transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
    }
}
