using UnityEngine;
using UnityEngine.Rendering.Universal;
[RequireComponent(typeof(Light2D))]
public class TorchLight : MonoBehaviour
{
    private Light2D flickeringLight;

    [SerializeField, Range(0f, 10f)] private float minIntensity = 3f;
    [SerializeField, Range(0f, 10f)] private float maxIntensity = 5f;
    [SerializeField, Min(0f)] private float flickeringSpeed = 0.1f;

    private float currentTimer;

    void Awake()
    {
        flickeringLight = GetComponent<Light2D>();
        IntensityBounds();
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer < flickeringSpeed)
            return;

        flickeringLight.intensity = Random.Range(minIntensity, maxIntensity);
        currentTimer = 0f;
    }

    private void IntensityBounds()
    {
        if (minIntensity > maxIntensity)
        {
            (minIntensity, maxIntensity) = (maxIntensity, minIntensity);
        }
    }
}
