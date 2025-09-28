using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private bool reloadSceneOnDeath = false;
    [SerializeField] private Transform firstCheckpoint;

    private Vector3 respawnPoint;


    private void Start()
    {
        // Try to find the player at scene start
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = GetRespawnPoint();
        }
    }

    //ser till att endast en gamemanager finns och att den inte destroyas när man resettar scenen
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // set initial respawn point to SpawnPoint at scene start
            GameObject defaultSpawn = GameObject.FindWithTag("SpawnPoint");
            if (firstCheckpoint != null)
                respawnPoint = firstCheckpoint.position;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //hämtar transform från checkpoint script och sätter respawnPoint
    public void SetCheckpoint(Vector3 pos)
    {
        respawnPoint = pos;
    }

  public Vector3 GetRespawnPoint()
    {
        return respawnPoint;
    }

    
    public void HandlePlayerDeath(GameObject player, bool killedByLight)
    {
        //dör av ljus och reload scene är på
        if (reloadSceneOnDeath && killedByLight)
        {
            StartCoroutine(ReloadAfterDeathAnim(player));
        }
        //dör inte av ljus och reload scene är på
        else if (reloadSceneOnDeath)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            StartCoroutine(RespawnAfterReload());
        }
        //dör av antingen ljus eller inte ljus men reload scene är av
        else
        {
            PlayerMovement pm = player.GetComponent<PlayerMovement>();
            if (pm != null)
            {
                if (killedByLight)
                    pm.StartCoroutine(pm.HandleDeath());
                else
                    RespawnPlayer(player);
            }
        }
    }

    

    public void RespawnPlayer(GameObject player)
    {
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            pm.ResetHealth();
            pm.ResetVelocity();
            player.transform.position = GetRespawnPoint();
        }
    }

    private IEnumerator RespawnAfterReload()
    {
        yield return null;
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
            player.transform.position = GetRespawnPoint();
        
    }

    private IEnumerator ReloadAfterDeathAnim(GameObject player)
    {
        PlayerMovement pm = player.GetComponent<PlayerMovement>();
        if (pm != null)
        {
            yield return pm.StartCoroutine(pm.HandleDeath());
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
