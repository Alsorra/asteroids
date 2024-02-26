using UnityEngine;
using UnityEngine.Events;

public static class BasicEvents {
    public class Void : UnityEvent { }

    public class Boolean : UnityEvent<bool> { }

    public class Integer : UnityEvent<int> { }

    public class Float2 : UnityEvent<Vector2> { }
    public class Float3 : UnityEvent<Vector3> { }

    public class UnityTransform : UnityEvent<Transform> { }
}
