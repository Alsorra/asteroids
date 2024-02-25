using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

public class Ship {
    public static readonly int MaxHealth = 100;

    private int mHealth = 0;

    public int health { get { return mHealth; } set { mHealth = value; onHealthUpdated.Invoke(mHealth); } }
    public BasicEvents.Integer onHealthUpdated { get; } = new BasicEvents.Integer();

    public Ship() {
        mHealth = MaxHealth;
    }

    public void OnCollision(Collider2D collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Asteroid")) {
            health -= 10;
        }
    }

    public void Restart() {
        health = MaxHealth;
    }
}
