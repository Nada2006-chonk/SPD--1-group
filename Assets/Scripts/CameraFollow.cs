using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
    [SerializeField] private float smoothing = 1.0f;

    [SerializeField] private bool lockX = false;
    [SerializeField] private float lockedXValue = 0f;
    public Vector3 Offset
    {
        get => offset;
        set => offset = value;
    }

    void LateUpdate()
    {
        Vector3 targetPos = target.position + offset;

        if (lockX)
        {
            targetPos.x = lockedXValue;
        }

        // linear interpolation LERP, vi flyttar från en punkt till en annan över en viss tid
        Vector3 newPosition = Vector3.Lerp(transform.position, targetPos, smoothing * Time.deltaTime);
        transform.position = newPosition;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CameraLockZone"))
        {
            lockX = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CameraLockZone"))
        {
            lockX = false;
        }
    }
    public void SetLockX(bool state, float xValue = 0f)
    {
        lockX = state;
        if (state)
            lockedXValue = xValue;
    }


}
