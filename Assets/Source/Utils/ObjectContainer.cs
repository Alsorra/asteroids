using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer {
    private Transform mParent = null;
    private GameObject mPrefab = null;

    private List<GameObject> mAvailableObject = new List<GameObject>();
    private List<GameObject> mLockedObjects = new List<GameObject>();

    public int lockedCount { get { return mLockedObjects.Count; } }

    public GameObject[] availableObjects { get { return mAvailableObject.ToArray(); } }
    public GameObject[] lockedObjects { get { return mLockedObjects.ToArray(); } }

    public ObjectContainer(Transform parent, GameObject prefab) {
        mParent = parent;
        mPrefab = prefab;
    }

    public GameObject GetAvailableObject() {
       GameObject freeObject = mAvailableObject.Count == 0 ?
            GameObject.Instantiate(mPrefab, mParent.transform) :
            mAvailableObject[0];

        mAvailableObject.Remove(freeObject);
        mLockedObjects.Add(freeObject);

        return freeObject;
    }

    public void SetObjectAvailable(GameObject lockedObject) {
        if (mLockedObjects.Contains(lockedObject)) {
            mLockedObjects.Remove(lockedObject);
            mAvailableObject.Add(lockedObject);
        }
    }
}
