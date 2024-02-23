using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour {

    [SerializeField]
    private Transform mSpawnPoint = null;

    private ObjectContainer mBullets = null;

    private void Awake() {
        BulletView bulletView = GetComponentInChildren<BulletView>(true);
        Debug.Assert(bulletView != null);

        mBullets = new ObjectContainer(transform, bulletView.gameObject);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            BulletView bullet = mBullets.GetFreeObject().GetComponent<BulletView>();

            Vector3 position = mSpawnPoint.position;
            Vector3 direction = (mSpawnPoint.position - mSpawnPoint.parent.position).normalized;

            bullet.Setup(position, direction);
        }
    }
}
