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

    [SerializeField]
    private float mBulletSpeed = 3.0f;

    private ObjectContainer mBullets = null;

    private void Start() {
        OnGameStart();

        Game.instance.onGameStart.AddListener(OnGameStart);
    }

    private void FixedUpdate() {
        if (Input.GetMouseButtonDown(0)) {
            BulletView bullet = mBullets.GetAvailableObject().GetComponent<BulletView>();
            bullet.onDestroyed.AddListener(() => {
                mBullets.SetObjectAvailable(bullet.gameObject);
            });

            Vector3 position = mSpawnPoint.position;
            Vector3 direction = (mSpawnPoint.position - mSpawnPoint.parent.position).normalized;

            bullet.Setup(position, direction, mBulletSpeed);
        }
    }

    private void OnGameStart() {
        mBullets = new ObjectContainer(transform, mBulletPrefab);

        while (transform.childCount > 0) {
            GameObject child = transform.GetChild(0).gameObject;
            child.transform.SetParent(null);
            GameObject.Destroy(child);
        }
    }
}
