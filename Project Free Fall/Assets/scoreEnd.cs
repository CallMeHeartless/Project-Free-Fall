using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreEnd : MonoBehaviour
{
    public GameObject[] player4s;
    public GameObject menus;
    // Start is called before the first frame update
   public void endscore()
    {
        Debug.Log("hello");
        int[] data = GameManager.GetPlayerScores();
        for (int i = 0; i < menus.AddComponent<spawnScore>().playercount; i++)
        {
            player4s[i*2].GetComponent<Text>().text = data[0].ToString();



        }
        Debug.Log("show");
        switch (menus.AddComponent<spawnScore>().playercount)
        {
            case 2:
                player4s[0].gameObject.SetActive(true);
                player4s[1].gameObject.SetActive(true);
                player4s[2].gameObject.SetActive(true);
                player4s[3].gameObject.SetActive(true);
                break;
            case 3:
                Debug.Log("here");
                player4s[0].gameObject.SetActive(true);
                player4s[1].gameObject.SetActive(true);
                player4s[2].gameObject.SetActive(true);
                player4s[3].gameObject.SetActive(true);
                player4s[4].gameObject.SetActive(true);
                player4s[5].gameObject.SetActive(true);
                Debug.Log("ho");
                break;
            case 4:
                Debug.Log("here");
                player4s[0].gameObject.SetActive(true);
                player4s[1].gameObject.SetActive(true);
                player4s[2].gameObject.SetActive(true);
                player4s[3].gameObject.SetActive(true);
                player4s[4].gameObject.SetActive(true);
                player4s[5].gameObject.SetActive(true);
                player4s[6].gameObject.SetActive(true);
                player4s[7].gameObject.SetActive(true);
                Debug.Log("ho");
                break;
            default:
                break;
        }
    }

}
