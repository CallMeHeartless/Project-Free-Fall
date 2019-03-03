using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour
{
    // Test variables, delete for final build
    [Header("Arena Test Variables")]
    public bool TestMode = false;
    public int TestPlayerCount = 4;

    [Header("Arena Variables")]
    [SerializeField]
    private float ringFallInterval = 25.0f;
    private float ringTimer = 0.0f;
    private int ringCount = 1;
    private bool roundWon = false;
    [SerializeField]
    private bool EnableVictoryOrb = false;

    [SerializeField]
    private float roundOverTimeDelay = 3.0f;
    private bool warning = false;

    [SerializeField]
    AudioSource[] sounds;
    // Start is called before the first frame update
    void Start(){
        // To force testing the arena without going through the full loop
        if (TestMode) {
            GameManager.SetPlayerCount(TestPlayerCount);
        }

        InitialisePlayers();
        InitialiseCameras();
        if (EnableVictoryOrb) {
            SpawnVictoryOrb();
        }

        GameObject.Find("InGameScoreUI").GetComponent<spawnScore>().setScore();
    }

    // Update is called once per frame
    void Update(){
        if (roundWon) {
            return;
        }

        // Detach rings as needed
        if(ringCount < 5) {
            ProcessRingTimer();
        }

        // End of round check
        if (CheckForLastStanding()) {
            roundWon = true;
            // Award the player one point
            GameManager.AddToPlayerScore(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetPlayerID());

            // Round over text

            // End of Round check
            StartCoroutine(EndOfRound());
        }
    }

    void InitialisePlayers() {
        // Iterate through registered ready players and ready prefab instances for them
        bool[] readyStatus = GameManager.GetReadyStatus();
        for (int i = 0; i < 4; ++i) {
            // Currently assuming players exist in level prior to game
            GameObject player = GameObject.Find("Player" + i.ToString());
            if (readyStatus[i]) {
                // Assign id to player instance
                player.GetComponent<PlayerController>().AssignPlayerID(i);
            } else {
                if (player != null) {
                    Destroy(player);
                }
            }
        }
    }

    void InitialiseCameras() {
        // Obtain number of ready players
        int playerCount = 0;
        bool[] playersReady = GameManager.GetReadyStatus();
        for(int i = 0; i < 4; ++i) {
            if (playersReady[i]) {
                ++playerCount;
            }
        }

        Debug.Log("Player count: " + playerCount);

        // Define camera setup accordingly
        Transform cameras = GameObject.Find("Cameras").GetComponent<Transform>();
        if(playerCount > 2) {
            cameras.GetChild(0).GetComponent<Camera>().rect = new Rect(new Vector2(-0.5f, 0.5f), new Vector2(1.0f, 1.0f));
            cameras.GetChild(1).GetComponent<Camera>().rect = new Rect(new Vector2(0.5f, 0.5f), new Vector2(1.0f, 1.0f));
            cameras.GetChild(2).GetComponent<Camera>().rect = new Rect(new Vector2(-0.5f, -0.5f), new Vector2(1.0f, 1.0f));
            cameras.GetChild(3).GetComponent<Camera>().rect = new Rect(new Vector2(0.5f, -0.5f), new Vector2(1.0f, 1.0f));
        } else {
            Destroy(cameras.GetChild(2).GetComponent<GameObject>());
            Destroy(cameras.GetChild(3).GetComponent<GameObject>());
        }
    }

    // Checks to see if only one player remains alive
    private bool CheckForLastStanding() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length == 1;
    }

    // Performs end of round checks, starting a new round or returning to main menu
    IEnumerator EndOfRound() {
        yield return new WaitForSeconds(roundOverTimeDelay);

        if (GameManager.CheckForGameOver()) {
            // Move these to be resolved after a 'game over' screen?
            GameManager.ClearReadyStatus();
            GameManager.ClearPlayerScores();
            SceneManager.LoadScene(0);
        } else {
            SceneManager.LoadScene("lvl_Arena_One");
        }
    }

    // Updates the ring timer, dropping and resetting as needed
    void ProcessRingTimer() {
        ringTimer += Time.deltaTime;
        if (ringTimer >= ringFallInterval)
        {
            ringTimer = 0.0f;
            warning = false;
            DetachRing();
        }
        else if (ringTimer >= ringFallInterval - 2.5f)
        {
            ParticleSystem steam = GameObject.Find("VFX_ArenaStage0" + ringCount.ToString()).GetComponent<ParticleSystem>();
            if (steam)
            {
                if (!steam.isPlaying)
                {
                    steam.Play();
                   
                    sounds[1].Play(0);
                    sounds[2].PlayDelayed(2);
                    //gameObject.GetComponentInChildren<AudioSource>().Play(0);
                }
            }
           
        }
        else if (ringTimer >= ringFallInterval - 5.5f)
        {
            if (warning != true)
            {
                Debug.Log("hot");
                warning = true;
               
                sounds[0].Play(0);
            }
            

        }
        // Update countdown text or other visual effects
    }

    void DetachRing() {
        Debug.Log("Detaching ring: " + ringCount.ToString());
        GameObject ring = GameObject.Find("ArenaFloor_Stage0" + ringCount.ToString());
        ring.GetComponent<ArenaRingController>().DropRing();
        ++ringCount;
    }

    public static void SpawnVictoryOrb() {
        GameObject spawnPoint = GameObject.Find("VictoryOrbSpawnPoint");
        if (spawnPoint) {
            GameObject victoryOrb = Instantiate(Resources.Load("VictoryOrb", typeof(GameObject))) as GameObject;
            if (victoryOrb) {
                victoryOrb.transform.position = spawnPoint.transform.position;
            } else {
                Debug.LogError("ERROR: Path to victory orb does not exist");
            }
        } else {
            Debug.LogError("ERROR: No spawn point was found");
        }
    }

    public void VictoryOrbWin(int _playerID) {
        roundWon = true;
        Debug.Log("Player " + _playerID + " has won the round with the victory orb");
        GameManager.AddToPlayerScore(_playerID);
        StartCoroutine(EndOfRound());
    }

    public static GameObject GetManager() {
        return GameObject.Find("RoundManager");
    }
}
