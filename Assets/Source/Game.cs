using System.Collections;
using System.Collections.Generic;
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
    private GameObject mPauseMenu = null;

    [Header("References")]
    [SerializeField]
    private MenuManager mMenuManager = null;

    public State gameState { get; private set; } = State.Active;

    public Ship ship { get; private set; } = new Ship();

    public BasicEvents.Void onGameStart { get; } = new BasicEvents.Void();
    public BasicEvents.Void onGameEnd { get; } = new BasicEvents.Void();

    private void Awake() {
        instance = this;

        ship.onHealthUpdated.AddListener((int health) => {
            if (health <= 0) {
                OnGameEnd();
            }
        });
    }

    private void Update() {
        if (gameState == State.Active && Input.GetKeyDown(KeyCode.P)) {
            if (mMenuManager.TryOpenMenu(mPauseMenu, out Menu menu)) {
                menu.GetButton("Continue").onClick.AddListener(() => {
                    menu.Close();
                });

                menu.onMenuClosed.AddListener(() => {
                    gameState = State.Active;
                    Time.timeScale = 1.0f;
                });

                gameState = State.Paused;
                Time.timeScale = 0.0f;
            }
        }
    }

    private void OnGameEnd() {
        gameState = State.EndGame;

        onGameEnd.Invoke();
    }
}
