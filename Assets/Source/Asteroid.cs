using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid {

    private int mHealth = 0;

    public int size { get; private set; } = 0;
    public int health { get { return mHealth; } private set { mHealth = value; OnHealthUpdated(); } }
    public bool active { get; private set; } = true;

    public BasicEvents.Void onDestroyed { get; } = new BasicEvents.Void();
    public BasicEvents.Integer onHealthUpdated { get; } = new BasicEvents.Integer();

    public Asteroid(int size, int health) {
        this.size = size;
        mHealth = health;
    }

    public void OnCollision(int layerMask) {
        if (active && layerMask == LayerMask.NameToLayer("Bullet")) {
            health -= 10;
        }
    }

    public void Destroy() {
        if (active) {
            active = false;
            onDestroyed.Invoke();
        }
    }

    private void OnHealthUpdated() {
        if (mHealth > 0) {
            onHealthUpdated.Invoke(mHealth);
        } else {
            Destroy();
        }
    }
}
