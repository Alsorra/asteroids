using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AsteroidManager : MonoBehaviour {

    [Header("Resources")]
    [SerializeField]
    private GameObject mAsteroidPrefab = null;

    [Header("Data")]
    [SerializeField]
    private int[] mAsteroidSizeHealth = new int[] { 25, 50, 100, 200 };

    [SerializeField]
    private int[] mAsteroidScoreHealth = new int[] { 5, 50, 150, 500 };

    [SerializeField]
    private float mSpawnRadius = 10.0f;

    [SerializeField]
    private float mActiveRadius = 15.0f;

    [SerializeField]
    private float mAsteroidSpeed = 11.0f;

    [SerializeField]
    private float mSpawnTime = 5.0f;
    private float mSpawnCounter = 0.0f;

    private ObjectContainer mAsteroidViews = null;

    public BasicEvents.Integer onAsteroidDestroyed { get; } = new BasicEvents.Integer();

    private void Awake() {
        mAsteroidViews = new ObjectContainer(transform, mAsteroidPrefab);
    }

    private void Update() {
        if (Game.instance.gameState != Game.State.Active) {
            return;
        }

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

        AsteroidView asteroidView = mAsteroidViews.GetAvailableObject().GetComponent<AsteroidView>();
        asteroidView.Setup(spawnPosition, direction * 100.0f, size, mAsteroidSizeHealth[size]);

        asteroidView.onDestroyed.AddListener((int destructionType) => {
            OnAsteroidDestroyed(asteroidView, (AsteroidView.DestructionType)destructionType);
        });
    }

    private void OnAsteroidDestroyed(AsteroidView asteroid, AsteroidView.DestructionType type) {
        mAsteroidViews.SetObjectAvailable(asteroid.gameObject);

        onAsteroidDestroyed.Invoke(asteroid.size);

        if (asteroid.size > 0 && type == AsteroidView.DestructionType.OutOfHealth) {
            Vector3 position = asteroid.transform.position;
            Vector3 velocity = asteroid.velocity;
            int size = asteroid.size;

            asteroid = null;

            float directionAngle = Mathf.Acos(-velocity.x);
            float directionAngleDifference = Mathf.Deg2Rad * 25.0f;

            float[] newDirections = new float[] {
                directionAngle + directionAngleDifference,
                directionAngle - directionAngleDifference
            };

            foreach (float angle in newDirections) {
                Vector3 direction = CircleEdgePoint(1.0f, angle).normalized * -1.0f;
                SpawnAsteroid(position + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0.0f), direction, size - 1);
            }
        }
    }

    public int GetAsteroidScore(int size) {
        Debug.Assert(size >= 0 && size < mAsteroidScoreHealth.Count());
        return mAsteroidScoreHealth[size];
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
