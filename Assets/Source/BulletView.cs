using UnityEngine;

public class BulletView : MonoBehaviour {

    private float mSpeed = 0.0f;
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

    public void Setup(Vector3 worldPosition, Vector3 direction, float speed) {
        transform.position = worldPosition;
        mDirection = direction;
        mSpeed = speed;

        transform.right = direction;
        transform.Rotate(new Vector3(0.0f, 0.0f, -90.0f));

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
