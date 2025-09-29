using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    
    public Image fadeImage;
    public float delayBeforeFade = 2f; 
    public float fadeDuration = 1f;
    [SerializeField] private int levelToLoad;

    private void Start()
    {
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        // Wait before starting fade
        yield return new WaitForSeconds(delayBeforeFade);

        // Fade to black
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        // Load next scene
        SceneManager.LoadScene(levelToLoad);
    }
}
