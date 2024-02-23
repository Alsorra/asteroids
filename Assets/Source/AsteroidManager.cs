using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidManager : MonoBehaviour {

    [SerializeField]
    private int[] mAsteroidSizeHealth = new int[] { 25, 50, 100, 200 };

    [SerializeField]
    private float mSpawnRadius = 10.0f;

    [SerializeField]
    private float mActiveRadius = 15.0f;

    [SerializeField]
    private float mAsteroidSpeed = 11.0f;

    [SerializeField]
    private float mSpawnTime = 5.0f;
    private float mSpawnCounter = 0.0f;

    private List<Asteroid> mAsteroids = new List<Asteroid>();

    private ObjectContainer mAsteroidViews = null;

    private void Awake() {
        AsteroidView asteroidView = GetComponentInChildren<AsteroidView>(true);
        Debug.Assert(asteroidView != null);

        mAsteroidViews = new ObjectContainer(transform, asteroidView.gameObject);
    }

    private void Update() {
        UpdateAsteroidSpawn();
    }

    private void UpdateAsteroidSpawn() {
        mSpawnCounter += Time.deltaTime;
        if (mSpawnCounter >= mSpawnTime) {
            mSpawnCounter = 0.0f;

            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            Vector3 spawnPosition = CircleEdgePoint(mSpawnRadius, angle);
            Vector3 direction = spawnPosition.normalized * -1.0f;
            int randomSize = Random.Range(0, mAsteroidSizeHealth.Count());

            SpawnAsteroid(spawnPosition, direction, randomSize);
        }
    }

    private void SpawnAsteroid(Vector3 spawnPosition, Vector3 direction, int size) {
        float speed = mAsteroidSpeed * (mAsteroidSpeed / (size + 1));

        Asteroid asteroid = new Asteroid(size, mAsteroidSizeHealth[size]);
        mAsteroids.Add(asteroid);

        asteroid.onDestroyed.AddListener(() => {
            OnAsteroidDestroyed(asteroid);
        });

        AsteroidView asteroidView = mAsteroidViews.GetFreeObject().GetComponent<AsteroidView>();
        asteroidView.Setup(asteroid, spawnPosition, direction * 100.0f);
    }

    private void OnAsteroidDestroyed(Asteroid asteroid) {
        mAsteroids.Remove(asteroid);

        /*if (asteroid.size > 0) {
            float directionAngle = Mathf.Acos(-asteroid.direction.x);
            float directionAngleDifference = Mathf.Deg2Rad * 25.0f;
            float[] newDirections = new float[] {
                directionAngle + directionAngleDifference,
                directionAngle - directionAngleDifference
            };

            foreach (float angle in newDirections) {
                Vector3 direction = CircleEdgePoint(1.0f, angle).normalized * -1.0f;
                SpawnAsteroid(asteroid.worldPosition, direction, asteroid.size - 1);
            }
        }*/
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, mSpawnRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, mActiveRadius);
    }

    private static Vector3 CircleEdgePoint(float radius, float radianAngle) {
        return new Vector3(radius * Mathf.Cos(radianAngle), radius * Mathf.Sin(radianAngle), 0.0f);
    }
}
