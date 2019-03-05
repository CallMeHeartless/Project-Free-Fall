using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    GameObject endUI;

    [SerializeField]
    private float roundOverTimeDelay = 3.0f;
    private bool warning = false;

    [SerializeField]
    AudioSource[] sounds;

    [SerializeField]
    bool shake = true;
    bool CurrentShake = false;
    public float scale;
    private Vector3 changes = new Vector3(0, 0, 0);
    public GameObject[] cameras = null;
    public float shakeLength;

    // Start is called before the first frame update
    void Start() {
        // To force testing the arena without going through the full loop
        if (TestMode) {
            GameManager.SetPlayerCount(TestPlayerCount);
        }

        InitialisePlayers();
        InitialiseCameras();

        int PlayerCount = 0;
        bool[] players = GameManager.GetReadyStatus();
        for(int i = 0; i < 4; ++i) {
            if (players[i]) {
                ++PlayerCount;
            }
        }
        if (EnableVictoryOrb || PlayerCount > 2) {
            SpawnVictoryOrb();
        }

        GameObject.Find("InGameScoreUI").GetComponent<spawnScore>().setScore();
        //endUI = GameObject.Find("EndOfRoundUI");

        // Start music (triggers once)
        GameObject music = GameObject.Find("GameMusic");
        if (music != null) {
            music.GetComponent<GameMusicController>().StartMusic();


        }
        Invoke("starting", .3f);

        
    }

    void starting(){
        cameras = GameObject.FindGameObjectsWithTag("cam");   
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
            GameObject winner = GameObject.FindGameObjectWithTag("Player");
            if(winner != null) {
                GameManager.AddToPlayerScore(winner.GetComponent<PlayerController>().GetPlayerID());
                // Show round over
                SetRoundOverText(winner.GetComponent<PlayerController>().GetPlayerID());
            }
            // End of Round check
            StartCoroutine(EndOfRound());
        }

        if (shake)
        {
            screenshake();
        }
    }

    // Removes players characters with no corresponding player
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

    // Ensure the correct camera display for the number of players 
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
            // Delete cameras that are not in use
            int[] camerasUsed = { 4, 4 };
            for(int i = 0; i < 4; ++i) {
                if (!playersReady[i]) {
                    Destroy(cameras.GetChild(i).gameObject);
                } else {
                    if(camerasUsed[0] == 4) {
                        camerasUsed[0] = i;
                    }else {
                        camerasUsed[1] = i;
                    }
                }
            }

            cameras.GetChild(camerasUsed[0]).GetComponent<Camera>().rect = new Rect(new Vector2(0.0f, 0.5f), new Vector2(1.0f, 1.0f));
            cameras.GetChild(camerasUsed[1]).GetComponent<Camera>().rect = new Rect(new Vector2(0.0f, -0.5f), new Vector2(1.0f, 1.0f));
        }
    }

    // Checks to see if only one player remains alive
    private bool CheckForLastStanding() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        return players.Length <= 1;
    }

    // Performs end of round checks, starting a new round or returning to main menu
    IEnumerator EndOfRound() {
        yield return new WaitForSeconds(roundOverTimeDelay);
        endUI.SetActive(false);

        if (GameManager.CheckForGameOver()) {
            // Move these to be resolved after a 'game over' screen?
            SceneManager.LoadScene("EndGameMenu");
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
                //Debug.Log("hot");
                warning = true;
               
                sounds[0].Play(0);
            }
            

        }
        // Update countdown text or other visual effects
    }

    // Issue drop command to ring
    void DetachRing() {
        //Debug.Log("Detaching ring: " + ringCount.ToString());
        GameObject steamBurst = GameObject.Find("VFX_ArenaStage0" + ringCount.ToString()).transform.GetChild(0).gameObject;
        steamBurst.GetComponent<ParticleSystem>().Play();
        GameObject ring = GameObject.Find("ArenaFloor_Stage0" + ringCount.ToString());
        ring.GetComponent<ArenaRingController>().DropRing();
        ++ringCount;
    }

    // Creates a victory orb at the designated spawn point
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

    // Handles the event that a player wins via victory orb
    public void VictoryOrbWin(int _playerID) {
        roundWon = true;
        Debug.Log("Player " + _playerID + " has won the round with the victory orb");
        SetRoundOverText(_playerID);
        GameManager.AddToPlayerScore(_playerID);
        StartCoroutine(EndOfRound());
    }

    // Returns an instance of round manager (there should only ever be one in scene, singleton not enforced)
    public static GameObject GetManager() {
        return GameObject.Find("RoundManager");
    }

    public void SetRoundOverText(int winningPlayerID) {
        
        if(endUI == null) {
            Debug.LogError("ERROR: EndOfRoundUI null reference exception");
            return;
        }
        // turn text on 
        endUI.SetActive(true);
        endUI.GetComponentInChildren<Text>().text = "PLAYER " + (winningPlayerID + 1).ToString() + " WINS THE ROUND";
        endUI.transform.GetChild(0).GetComponentInChildren<scoreEnd>().endscore();
    }
    void screenshake()
    {
        StartCoroutine(Shaking());

    }
    private IEnumerator Shaking() {

        yield return null;

        CurrentShake = true;
        Debug.Log("herre");
        int same = cameras.Length;
        if (shakeLength <= 0)
        {

            for (int i = 0; i < same; i++)
            {
                cameras[i].gameObject.transform.localPosition -= changes;
                Debug.Log("finish");
            }
            changes = new Vector3(0, 0, 0);
            shake = false;
        }
        else
        {
            if (shakeLength > 0)
            {
                changes = changes + new Vector3(Random.Range(-100, 100), Random.Range(-100, 1000), Random.Range(-100, 100));
                for (int i = 0; i < same; i++)
                {
                    cameras[i].gameObject.transform.localPosition += changes;
                    Debug.Log("move");
                }

                shakeLength -= Time.deltaTime;
            }
        }
        CurrentShake = false;
       
    }

}
