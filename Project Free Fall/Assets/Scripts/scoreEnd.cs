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

        for (int i = 0; i < PlayerCount; i++)
        {
            player4s[i * 2+1].GetComponent<Text>().text = data[i].ToString();



        }
      
        switch (PlayerCount)
        {
            case 2:
                player4s[0].gameObject.SetActive(true);
                player4s[1].gameObject.SetActive(true);
                player4s[2].gameObject.SetActive(true);
                player4s[3].gameObject.SetActive(true);
                break;
            case 3:
                
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
