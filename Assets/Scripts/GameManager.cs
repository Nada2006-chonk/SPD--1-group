using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

   
    [SerializeField] private bool reloadSceneOnDeath = false;

    private Vector3 respawnPoint;
//ser till att endast en gamemanager finns och att den inte destroyas när man resettar scenen
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Vector3 pos)
    {
        respawnPoint = pos;
    }

    public Vector3 GetRespawnPoint()
    {
        if (respawnPoint == Vector3.zero)
        {
            GameObject defaultSpawn = GameObject.FindWithTag("SpawnPoint");
            if (defaultSpawn != null) return defaultSpawn.transform.position;
        }
        return respawnPoint;
    }

    public void HandlePlayerDeath(GameObject player)
    {
        if (reloadSceneOnDeath)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(RespawnAfterReload());
        }
        else
        {
            RespawnPlayer(player);
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.ResetHealth();
            player.transform.position = GetRespawnPoint();
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
    }

    private IEnumerator RespawnAfterReload()
    {
        yield return null;
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            player.transform.position = GetRespawnPoint();
    }
}
