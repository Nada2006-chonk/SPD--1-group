using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    [SerializeField] private float fallingSpeed = 3f;

    void Update()
    {
    
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

       
        transform.Translate(0f, -fallingSpeed * Time.deltaTime, 0f, Space.World);
    }
}

