using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletView : MonoBehaviour {

    [SerializeField]
    private float mSpeed = 0.1f;
    private float mActiveRadius = 25.0f;

    private Vector3 mDirection = Vector3.zero;

    public BasicEvents.Void onDestroyed { get; } = new BasicEvents.Void();

    private void FixedUpdate() {
        Vector3 movement = mDirection * Time.fixedDeltaTime * mSpeed;
        transform.localPosition += movement;

        if (Vector3.Distance(Vector3.zero, transform.position) > mActiveRadius) {
            Destroy();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Asteroid")) {
            Destroy();
        }
    }

    public void Setup(Vector3 worldPosition, Vector3 direction) {
        transform.position = worldPosition;
        mDirection = direction;
        
        gameObject.SetActive(true);
    }

    public void Destroy() {
        if (gameObject.activeInHierarchy) {
            gameObject.SetActive(false);
            onDestroyed.Invoke();

            onDestroyed.RemoveAllListeners();
        }
    }
}
