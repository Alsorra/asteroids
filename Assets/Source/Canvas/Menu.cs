using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    [SerializeField]
    private List<string> mButtonKeys = new List<string>();

    [SerializeField]
    private List<Button> mButtonValues = new List<Button>();

    private Dictionary<string, Button> mButtons = new Dictionary<string, Button>();

    public BasicEvents.Void onMenuClosed { get; } = new BasicEvents.Void();

    private void Awake() {
        int count = mButtonKeys.Count;

        for (int i = 0; i < count; ++i) {
            string key = mButtonKeys[i];
            Button button = mButtonValues[i];

            if (button != null) {
                mButtons.Add(key, button);
            }
        }

        mButtonKeys.Clear();
        mButtonValues.Clear();
    }

    public Button GetButton(string key) {
        if (mButtons.TryGetValue(key, out Button value)) {
            return value;
        }
        return null;
    }

    public void Close() {
        onMenuClosed.Invoke();
    }
}
