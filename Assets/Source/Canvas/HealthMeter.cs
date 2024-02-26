using UnityEngine;

public class HealthMeter : MonoBehaviour {

    private void Start() {
        Game.instance.ship.onHealthUpdated.AddListener(OnHealthUpdated);
    }

    private void OnHealthUpdated(int health) {
        float t = Mathf.Clamp((float)health / (float)Ship.MaxHealth, 0.0f, 1.0f);
        transform.localScale = new Vector3(t, 1.0f, 1.0f);
    }
}
