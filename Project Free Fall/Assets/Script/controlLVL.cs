using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlLVL : MonoBehaviour
{
    public GameObject[] circles;
    public int timer;
    public int counter;
    // Start is called before the first frame update
    void Start()
    {
        timer = 60;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer<= 0)
        {
            if (counter == 0)
            {

            }
            else
            {
                Debug.Log("hello");
                circles[counter].GetComponent<Drop>().destoying = true;
                Debug.Log("good");
                counter--;
                timer = 40;

            }

        }
        else
        {
            timer--;
        }
    }
}
