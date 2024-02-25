using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreText : MonoBehaviour {
    void Start() {
        TMPro.TMP_Text text = GetComponent<TMPro.TMP_Text>();
        Game.instance.onScoreUpdated.AddListener((int score) => {
            text.text = score.ToString();
        });

        text.text = Game.instance.score.ToString();
    }
}
