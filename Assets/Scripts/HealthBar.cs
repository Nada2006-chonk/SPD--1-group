using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private Image image;

    private Material materialInstance;

    private void Awake()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        if (image != null)
        {

            materialInstance = Instantiate(image.material);
            image.material = materialInstance;
        }

    }
    public void SetHealth(float healthPercent)
    {
        if (materialInstance != null)
        {
            materialInstance.SetFloat("_AmountOfLiquid", Mathf.Clamp01(healthPercent));
        }
    }
}
