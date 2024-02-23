using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer {
    private Transform mParent = null;

    private List<GameObject> mObjects = new List<GameObject>();

    public ObjectContainer(Transform parent, GameObject firstObject) {
        mParent = parent;

        mObjects.Add(firstObject);
    }

    public GameObject GetFreeObject() {
        foreach (GameObject gameObject in mObjects) {
            if (!gameObject.activeInHierarchy) {
                return gameObject;
            }
        }

        GameObject newGameObject = GameObject.Instantiate(mObjects[0], mParent.transform);

        mObjects.Add(newGameObject);

        return newGameObject;
    }
}
