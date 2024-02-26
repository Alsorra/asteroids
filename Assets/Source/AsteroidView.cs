using UnityEngine;
using UnityEngine.Events;

public class AsteroidView : MonoBehaviour {

    public enum DestructionType {
        HitShip = 0,
        OutOfBounds = 1,
        OutOfHealth = 2
    }

    public class DestroyedEvent : UnityEvent<DestructionType, int> { }

    private Rigidbody2D mRigidbody = null;

    public int size { get; private set; } = 0;
    public int health { get; private set; } = 0;

    public DestroyedEvent onDestroyed { get; private set; } = new DestroyedEvent();

    public Vector3 velocity { get { return mRigidbody ? mRigidbody.velocity : Vector3.zero; } }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet")) {
            health -= 10;

            if (health <= 0) {
                Destroy(DestructionType.OutOfHealth);
            }
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Ship")) {
            Destroy(DestructionType.OutOfBounds);
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
        mRigidbody.mass = 5 * (size + 1);
        mRigidbody.AddForce(startingForce, ForceMode2D.Force);
    }

    public void Destroy(DestructionType type) {
        if (gameObject.activeInHierarchy) {
            onDestroyed.Invoke(type, size);
            onDestroyed.RemoveAllListeners();

            gameObject.SetActive(false);
        }
    }
}
