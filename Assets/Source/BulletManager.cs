using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    [Header("Resources")]
    [SerializeField]
    private GameObject mBulletPrefab = null;

    [Header("Data")]
    [SerializeField]
    private Transform mSpawnPoint = null;

    private ObjectContainer mBullets = null;

    private void Awake() {
        mBullets = new ObjectContainer(transform, mBulletPrefab);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            BulletView bullet = mBullets.GetAvailableObject().GetComponent<BulletView>();
            bullet.onDestroyed.AddListener(() => {
                mBullets.SetObjectAvailable(bullet.gameObject);
            });

            Vector3 position = mSpawnPoint.position;
            Vector3 direction = (mSpawnPoint.position - mSpawnPoint.parent.position).normalized;

            bullet.Setup(position, direction);
        }
    }
}
