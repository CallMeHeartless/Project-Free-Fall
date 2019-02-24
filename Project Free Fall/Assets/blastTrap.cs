using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blastTrap : MonoBehaviour
{
    public float timer;
    public bool On;
    public float cooldown;
    public float Range;
    public float speed;
    public float thrust;
    public GameObject player;
    public Transform prefab;
    // Start is called before the first frame update
    void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
       
            if (On == true)
            {


                if (timer >= 0)
                {
                    timer -= Time.deltaTime;


                }
                else
                {
                    //gameObject.GetComponent<MeshCollider>().enabled = true;
                    On = true;
                    timer = cooldown;
                    Instantiate(prefab, transform.position, Quaternion.identity);
               // prefab.transform.parent = gameObject.transform;
                    prefab.GetComponent<blast>().speed = speed;
                    prefab.GetComponent<blast>().Range = Range;
                    prefab.GetComponent<blast>().thrust = thrust;
            }
            }
           
        }
    
}
