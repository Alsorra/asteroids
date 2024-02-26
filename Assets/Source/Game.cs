using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Game : MonoBehaviour {
    public static Game instance { get; private set; } = null;

    public enum State {
        Active,
        Paused,
        EndGame
    }

    [Header("Resources")]
    [SerializeField]
    private GameObject mPauseMenuPrefab = null;
    [SerializeField]
    private GameObject mGameEndMenuPrefab = null;

    [Header("References")]
    [SerializeField]
    private MenuManager mMenuManager = null;
    [SerializeField]
    private AsteroidManager mAsteroidManager = null;

    private int mScore = 0;

    public int score { get { return mScore; } private set { mScore = value; onScoreUpdated.Invoke(mScore); } }

    public State gameState { get; private set; } = State.Active;

    public Ship ship { get; private set; } = new Ship();

    public BasicEvents.Void onGameStart { get; } = new BasicEvents.Void();
    public BasicEvents.Void onGameEnd { get; } = new BasicEvents.Void();
    public BasicEvents.Integer onScoreUpdated { get; } = new BasicEvents.Integer();

    private void Awake() {
        instance = this;

        ship.onHealthUpdated.AddListener((int health) => {
            if (health <= 0) {
                OnGameEnd();
            }
        });
    }

    private void Start() {
        mAsteroidManager.onAsteroidDestroyed.AddListener((AsteroidView.DestructionType type, int asteroidSize) => {
            if (type == AsteroidView.DestructionType.OutOfHealth) {
                score += mAsteroidManager.GetAsteroidScore(asteroidSize);
            } else if (type == AsteroidView.DestructionType.HitShip) {
                score += mAsteroidManager.GetAsteroidScore(asteroidSize) / 2;
            }
        });
    }

    private void Update() {
        UpdatePauseCheck();
    }

    private void UpdatePauseCheck() {
        bool pauseInput = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P);
        if (pauseInput && gameState == State.Active) {
            if (mMenuManager.TryOpenMenu(mPauseMenuPrefab, out Menu menu)) {
                menu.GetButton("Continue").onClick.AddListener(() => {
                    menu.Close();
                });

                menu.GetButton("Quit").onClick.AddListener(() => {
                    Application.Quit();
                });

                menu.onMenuClosed.AddListener(() => {
                    gameState = State.Active;
                    Time.timeScale = 1.0f;
                });

                gameState = State.Paused;
                Time.timeScale = 0.0f;
            }
        } else if (pauseInput && gameState == State.Paused) {
            mMenuManager.CloseCurrentMenu();
        }
    }

    private void OnGameEnd() {
        gameState = State.EndGame;
        Time.timeScale = 0.0f;

        onGameEnd.Invoke();

        if (mMenuManager.TryOpenMenu(mGameEndMenuPrefab, out Menu menu)) {
            menu.GetButton("Restart").onClick.AddListener(Restart);
        }
    }

    private void Restart() {
        mMenuManager.CloseCurrentMenu();

        gameState = State.Active;
        Time.timeScale = 1.0f;

        score = 0;

        ship.Restart();

        onGameStart.Invoke();
    }
}
