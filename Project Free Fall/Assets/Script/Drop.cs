using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public int timer;
    public bool destoying = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            destoying = true;
        }
        if (destoying == true)
        {
            if (timer !=0)
            {
                timer--;
                gameObject.transform.Translate(0, -0.1f, 0);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
