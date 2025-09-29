using UnityEngine;
using System.Collections;

public class LastSceneAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationTrigger;
    public float delay = 2f;
    private void Start()
    {
        StartCoroutine(PlayAfterDelay());
    }

    private IEnumerator PlayAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(animationTrigger);
    }
}