using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spawnScore : MonoBehaviour
{
   public GameObject[] player2s;
   public GameObject[] player4s;
   int playercount;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("starting", .3f);
    }
    void starting()
    {
        bool[] activePlayers = GameManager.GetReadyStatus();
        int total = 0;
        for (int i = 0; i < activePlayers.Length; i++)
        {
            if (activePlayers[i] == true)
            {
                total++;
            }
        }
        //find player count if 3 then call player3
        if (total == 3)
        {
            player3();
        }
        playercount = total;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            seeScore(1);
        }
    }
    //call to make menu show up
    public void seeScore(int playerID)
    {
        Debug.Log(playercount);
        if (playercount == 2)
        {
            player2s[playerID].gameObject.active = !player2s[playerID].gameObject.active;
           
        }
        else
        {
            player4s[playerID].gameObject.active = !player4s[playerID].gameObject.active;
        }

    }
    //call at the start of the next round
    public void setScore()
    {
        int[] data = GameManager.GetPlayerScores();

        switch (playercount)
        {
            case 2:

                player2s[0].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player2s[0].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();

                player2s[1].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player2s[1].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();
                break;

            case 3:

                player4s[0].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player4s[0].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();
                player4s[0].transform.GetChild(5).GetComponent<Text>().text = data[2].ToString();
               
                player4s[1].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player4s[1].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();
                player4s[1].transform.GetChild(5).GetComponent<Text>().text = data[2].ToString();

                player4s[2].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player4s[2].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();
                player4s[2].transform.GetChild(5).GetComponent<Text>().text = data[2].ToString();
                break;

            case 4:

                player4s[0].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player4s[0].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();
                player4s[0].transform.GetChild(5).GetComponent<Text>().text = data[2].ToString();
                player4s[0].transform.GetChild(7).GetComponent<Text>().text = data[3].ToString();

                player4s[1].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player4s[1].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();
                player4s[1].transform.GetChild(5).GetComponent<Text>().text = data[2].ToString();
                player4s[1].transform.GetChild(7).GetComponent<Text>().text = data[3].ToString();

                player4s[2].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player4s[2].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();
                player4s[2].transform.GetChild(5).GetComponent<Text>().text = data[2].ToString();
                player4s[2].transform.GetChild(7).GetComponent<Text>().text = data[3].ToString();

                player4s[3].transform.GetChild(1).GetComponent<Text>().text = data[0].ToString();
                player4s[3].transform.GetChild(3).GetComponent<Text>().text = data[1].ToString();
                player4s[3].transform.GetChild(5).GetComponent<Text>().text = data[2].ToString();
                player4s[3].transform.GetChild(7).GetComponent<Text>().text = data[3].ToString();
                break;
            default:
                break;
        }
    }
    //call if their is only 3 players
    void player3()
    {
        player4s[0].transform.GetChild(6).gameObject.SetActive(false);
        player4s[0].transform.GetChild(7).gameObject.SetActive(false);

        player4s[1].transform.GetChild(6).gameObject.SetActive(false);
        player4s[1].transform.GetChild(7).gameObject.SetActive(false);

        player4s[2].transform.GetChild(6).gameObject.SetActive(false);
        player4s[2].transform.GetChild(7).gameObject.SetActive(false);

    }
}
