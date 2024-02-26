using UnityEngine;

public class ShipView : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        Game.instance.ship.OnCollision(collision);
    }
}
