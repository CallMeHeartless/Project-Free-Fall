using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlLVL : MonoBehaviour
{
    public GameObject[] circles;
    public float timer;
    int counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = circles.Length -1;
        timer = 10;
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
                
               
                circles[counter].GetComponent<Drop>().destoying = true;
             
                   
                timer -= Time.deltaTime;
                timer = 10;
                counter--;
            }

        }
        else
        {
            timer--;
        }
    }
}
