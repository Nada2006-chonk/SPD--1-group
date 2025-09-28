using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private int levelToLoad;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }



    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
