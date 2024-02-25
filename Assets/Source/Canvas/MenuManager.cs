using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    private GameObject mPlayAreaPanel = null;

    private Menu mCurrentMenu = null;

    public bool menuOpen { get { return mCurrentMenu != null; } }

    public bool TryOpenMenu(GameObject menuPrefab, out Menu menu) {
        menu = null;

        if (menuOpen || menuPrefab == null) {
            return false;
        }

        GameObject menuObject = GameObject.Instantiate(menuPrefab, transform);
        if (menuObject.TryGetComponent<Menu>(out mCurrentMenu)) {
            mPlayAreaPanel.SetActive(true);

            mCurrentMenu.onMenuClosed.AddListener(() => {
                mCurrentMenu = null;

                mPlayAreaPanel.SetActive(false);

                GameObject.Destroy(menuObject);
            });

            menu = mCurrentMenu;

            return true;
        }

        GameObject.Destroy(menuObject);
        return false;
    }

    public void CloseCurrentMenu() {
        if (!menuOpen) {
            return;
        }

        mCurrentMenu.Close();
    }
}
