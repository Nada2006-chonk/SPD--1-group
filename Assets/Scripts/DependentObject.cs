using UnityEngine;

public class DependentObject : MonoBehaviour
{
    [SerializeField] private Destructible[] supports;

    private int destroyedSupports = 0;
    private BreakablePillar pillar;

    void Start()
    {
        pillar = GetComponent<BreakablePillar>();

        foreach (var support in supports)
        {
            if (support != null)
            {
                support.OnDestroyed += SupportDestroyed;
            }
        }
    }

    private void SupportDestroyed()
    {
        destroyedSupports++;

        if (destroyedSupports >= supports.Length && pillar != null)
        {
            pillar.Break();
        }
    }
}
