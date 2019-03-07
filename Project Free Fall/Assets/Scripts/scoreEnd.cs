using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreEnd : MonoBehaviour
{
    public GameObject[] player4s;
    //public GameObject menus;
    // Start is called before the first frame update
    public void endscore()
    {

        int[] data = GameManager.GetPlayerScores();

        int PlayerCount = 0;
        bool[] players = GameManager.GetReadyStatus();
        for (int i = 0; i < 4; ++i)
        {
            if (players[i])
            {
                ++PlayerCount;
            }
        }



        int j = 0;
        for (int i = 0; i < PlayerCount; i++)
        {
            if (players[i] == true)
            {
                player4s[j * 2 + 1].GetComponent<Text>().text = data[i].ToString();
                j++;
            }
            



        }

        j = 0;
        switch (PlayerCount)
        {
            case 2:
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] == true)
                    {
                        player4s[j * 2].GetComponent<Text>().text = "player " + (i + 1);
                        j++;

                    }
                }
                player4s[0].gameObject.SetActive(true);
                player4s[1].gameObject.SetActive(true);
                player4s[2].gameObject.SetActive(true);
                player4s[3].gameObject.SetActive(true);
                break;
            case 3:

                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] == true)
                    {
                        player4s[j*2].GetComponent<Text>().text = "player " + (i + 1);
                        j++;

                    }
                }
                player4s[0].gameObject.SetActive(true);
                player4s[1].gameObject.SetActive(true);
                player4s[2].gameObject.SetActive(true);
                player4s[3].gameObject.SetActive(true);
                player4s[4].gameObject.SetActive(true);
                player4s[5].gameObject.SetActive(true);
               
                break;
            case 4:
               
                player4s[0].gameObject.SetActive(true);
                player4s[1].gameObject.SetActive(true);
                player4s[2].gameObject.SetActive(true);
                player4s[3].gameObject.SetActive(true);
                player4s[4].gameObject.SetActive(true);
                player4s[5].gameObject.SetActive(true);
                player4s[6].gameObject.SetActive(true);
                player4s[7].gameObject.SetActive(true);
               
                break;
            default:
                break;
        }
    }

}
