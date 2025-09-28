using UnityEngine;

public class CameraLock : MonoBehaviour
{
    [SerializeField] private float lockedXValue = 0f;

    public float LockedXValue => lockedXValue;
}
