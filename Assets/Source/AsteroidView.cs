using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidView : MonoBehaviour {

    private Asteroid mAsteroid = null;
    private Rigidbody mRigidbody = null;

    private void OnTriggerEnter(Collider other) {
        if (mAsteroid != null) {
            mAsteroid.OnCollision(other.gameObject.layer);
        } else {
            OnAsteroidDestroyed();
        }
    }

    public void Setup(Asteroid asteroid, Vector3 position, Vector3 startingForce) {
        gameObject.SetActive(true);

        mAsteroid = asteroid;
        mAsteroid.onDestroyed.AddListener(OnAsteroidDestroyed);

        transform.position = position;
        transform.localScale = Vector3.one * (asteroid.size + 1) * 0.25f;

        if (mRigidbody == null) {
            mRigidbody = GetComponent<Rigidbody>();
        }

        mRigidbody.velocity = Vector3.zero;
        mRigidbody.mass = 2 * (asteroid.size + 1);
        mRigidbody.AddForce(startingForce, ForceMode.Force);
    }

    private void OnAsteroidDestroyed() {
        gameObject.SetActive(false);
        mAsteroid = null;
    }
}
