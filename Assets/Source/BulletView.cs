using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour {

    [SerializeField]
    private float mSpeed = 0.1f;
    private float mActiveRadius = 25.0f;

    private Vector3 mDirection = Vector3.zero;

    private void FixedUpdate() {
        Vector3 movement = mDirection * Time.fixedDeltaTime * mSpeed;
        transform.localPosition += movement;

        if (Vector3.Distance(Vector3.zero, transform.position) > mActiveRadius) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid")) {
            gameObject.SetActive(false);
        }
    }

    public void Setup(Vector3 worldPosition, Vector3 direction) {
        transform.position = worldPosition;
        mDirection = direction;
        
        gameObject.SetActive(true);
    }
}
