using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    private GameObject mPlayAreaPanel = null;

    public bool menuOpen { get; private set; } = false;

    public bool TryOpenMenu(GameObject menuPrefab, out Menu menu) {
        menu = null;

        if (menuOpen || menuPrefab == null) {
            return false;
        }

        GameObject menuObject = GameObject.Instantiate(menuPrefab, transform);
        if (menuObject.TryGetComponent<Menu>(out menu)) {
            menuOpen = true;

            mPlayAreaPanel.SetActive(true);

            menu.onMenuClosed.AddListener(() => {
                menuOpen = false;

                mPlayAreaPanel.SetActive(false);

                GameObject.Destroy(menuObject);
            });

            return true;
        }

        GameObject.Destroy(menuObject);
        return false;
    }
}
