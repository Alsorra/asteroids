using UnityEngine;

public class AsteroidManager : MonoBehaviour {

    [Header("Resources")]
    [SerializeField]
    private GameObject mAsteroidPrefab = null;

    [Header("Data")]
    [SerializeField]
    private int[] mAsteroidSizeHealth = new int[] { 25, 50, 100, 200 };

    [SerializeField]
    private int[] mAsteroidScoreHealth = new int[] { 10, 50, 150, 500 };

    [SerializeField]
    private int mMinAsteroids = 10;

    [SerializeField]
    private int mMaxAsteroids = 100;

    [SerializeField]
    private float mActiveRadius = 15.0f;

    [SerializeField]
    private float mAsteroidSpeed = 11.0f;

    [Header("Spawn Data")]
    [SerializeField]
    private float mSpawnRadius = 10.0f;

    [SerializeField]
    private float mSpawnTime = 5.0f;
    private float mSpawnCounter = 0.0f;

    [SerializeField]
    private float mSpawnDirectionVariation = 1.0f;

    private ObjectContainer mAsteroidViews = null;

    public AsteroidView.DestroyedEvent onAsteroidDestroyed { get; } = new AsteroidView.DestroyedEvent();

    private void Start() {
        OnGameStart();

        Game.instance.onGameStart.AddListener(OnGameStart);
    }

    private void FixedUpdate() {
        UpdateAsteroidSpawn();

        CheckActiveArea();
    }

    private void CheckActiveArea() {
        GameObject[] asteroids = mAsteroidViews.lockedObjects;
        foreach (var asteroid in asteroids) {
            if (Vector3.Distance(Vector3.zero, asteroid.transform.position) > mActiveRadius) {
                asteroid.GetComponent<AsteroidView>().Destroy(AsteroidView.DestructionType.OutOfBounds);
            }
        }
    }

    private void UpdateAsteroidSpawn() {
        int asteroidsCount = mAsteroidViews.lockedCount;
        if (asteroidsCount >= mMaxAsteroids) {
            return;
        }

        mSpawnCounter += Time.fixedDeltaTime;
        if (mSpawnCounter >= mSpawnTime || asteroidsCount < mMinAsteroids) {
            mSpawnCounter = 0.0f;

            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            Vector3 spawnPosition = CircleEdgePoint(mSpawnRadius, angle);
            Vector3 direction = (spawnPosition - new Vector3(
                Random.Range(-mSpawnDirectionVariation, mSpawnDirectionVariation), 
                Random.Range(-mSpawnDirectionVariation, mSpawnDirectionVariation),
                spawnPosition.z)).normalized * -1.0f;
            int randomSize = Random.Range(0, mAsteroidSizeHealth.Length);

            SpawnAsteroid(spawnPosition, direction, randomSize);
        }
    }

    private void OnGameStart() {
        mSpawnCounter = 0.0f;

        mAsteroidViews = new ObjectContainer(transform, mAsteroidPrefab);

        while (transform.childCount > 0) {
            GameObject child = transform.GetChild(0).gameObject;
            child.transform.SetParent(null);
            GameObject.Destroy(child);
        }
    }

    private void SpawnAsteroid(Vector3 spawnPosition, Vector3 direction, int size) {
        AsteroidView asteroidView = mAsteroidViews.GetAvailableObject().GetComponent<AsteroidView>();
        asteroidView.Setup(spawnPosition, direction * mAsteroidSpeed, size, mAsteroidSizeHealth[size]);

        asteroidView.onDestroyed.AddListener((AsteroidView.DestructionType destructionType, int size) => {
            OnAsteroidDestroyed(asteroidView, destructionType);
        });
    }

    private void OnAsteroidDestroyed(AsteroidView asteroid, AsteroidView.DestructionType destructionType) {
        onAsteroidDestroyed.Invoke(destructionType, asteroid.size);

        if (asteroid.size > 0 && destructionType == AsteroidView.DestructionType.OutOfHealth) {
            Vector3 position = asteroid.transform.position;
            float scale = asteroid.transform.localScale.x * 0.25f;
            int size = asteroid.size;

            Vector3[] newDirections = new Vector3[] {
                new Vector3(
                    Random.Range(-1.0f, 1.0f),
                    Random.Range(-1.0f, 1.0f),
                    0.0f).normalized,
                new Vector3(
                    Random.Range(-1.0f, 1.0f),
                    Random.Range(-1.0f, 1.0f),
                    0.0f).normalized
            };

            foreach (Vector3 direction in newDirections) {
                SpawnAsteroid(position + (direction * scale), direction, size - 1);
            }
        }

        mAsteroidViews.SetObjectAvailable(asteroid.gameObject);
    }

    public int GetAsteroidScore(int size) {
        Debug.Assert(size >= 0 && size < mAsteroidScoreHealth.Length);
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
