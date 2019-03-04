using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{
    public Text winnerText;
    
    // Start is called before the first frame update
    void Start()
    {
        int winner = 0;
        
        int[] scores = GameManager.GetPlayerScores();
        int lead = scores[0];
        for(int i = 1; i < 4; ++i) {
            if(scores[i] > lead) {
                winner = i;
                lead = scores[i];
            }
        }

        winnerText.text = "P L A Y E R " + (winner + 1).ToString() + " W I N S";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
