using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour {

    [SerializeField]
    private float mSpeed = 1.0f;

    private void Update() {
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
            Vector3 movement = new Vector3(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical"),
                0.0f);

            movement = movement * mSpeed * Time.deltaTime;

            transform.position += movement;
        }
    }
}
