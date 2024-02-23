using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipView : MonoBehaviour {
    private void Update() {
        Vector2 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        worldPosition.z = transform.position.z;

        Vector3 direction = (worldPosition - transform.position).normalized;
        transform.right = direction;

        transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));
    }
}