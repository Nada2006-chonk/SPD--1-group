using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image image; // assign the Image in inspector

    private Material materialInstance;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>(); // automatically grab the Image if not assigned
        }

        if (image != null)
        {
            // Make a unique material instance so changes only affect this health bar
            materialInstance = Instantiate(image.material);
            image.material = materialInstance;
        }
        else
        {
            Debug.LogError("HealthBar requires an Image component.");
        }
    }

    /// <summary>
    /// Set the health amount (0 to 1) for the shader
    /// </summary>
    public void SetHealth(float healthPercent)
    {
        if (materialInstance != null)
        {
            materialInstance.SetFloat("_AmountOfLiquid", Mathf.Clamp01(healthPercent));
        }
    }
}
