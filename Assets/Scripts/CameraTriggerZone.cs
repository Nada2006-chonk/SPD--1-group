using UnityEngine;

public class CameraTriggerZone : MonoBehaviour
{
    [SerializeField] private Vector3 newOffset = new Vector3(0, 0, -15f);
    [SerializeField] private bool revertOnExit = true;

    private CameraFollow cameraFollow;
    private Vector3 originalOffset;

    void Start()
    {
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cameraFollow != null)
        {
            
            originalOffset = cameraFollow.Offset;
            cameraFollow.Offset = newOffset;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && cameraFollow != null && revertOnExit)
        {
            cameraFollow.Offset = originalOffset;
        }
    }
}
