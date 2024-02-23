using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class AsteroidView : MonoBehaviour {

    private Rigidbody2D mRigidbody = null;

    public int size { get; private set; } = 0;
    public int health { get; private set; } = 0;

    public BasicEvents.Void onDestroyed { get; } = new BasicEvents.Void();

    public Vector3 velocity { get { return mRigidbody ? mRigidbody.velocity : Vector3.zero; } }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet")) {
            health -= 10;

            if (health <= 0) {
                Destroy();
            }
        }
    }

    public void Setup(Vector3 position, Vector3 startingForce, int size, int health) {
        gameObject.SetActive(true);

        this.size = size;
        this.health = health;

        transform.position = position;
        transform.localScale = Vector3.one * (size + 1) * 0.25f;

        if (mRigidbody == null) {
            mRigidbody = GetComponent<Rigidbody2D>();
        }

        mRigidbody.velocity = Vector3.zero;
        mRigidbody.mass = 2 * (size + 1);
        mRigidbody.AddForce(startingForce, ForceMode2D.Force);
    }

    private void Destroy() {
        if (gameObject.activeInHierarchy) {
            gameObject.SetActive(false);
            onDestroyed.Invoke();

            onDestroyed.RemoveAllListeners();
        }
    }
}
