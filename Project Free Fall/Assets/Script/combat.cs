using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{
    public Vector3 spawnPos;
    public float spawnRot;
    public float spawnDistance;
    public GameObject range;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //swing
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 playerPos = transform.position;
            Vector3 playerDirection = transform.forward;
            Quaternion playerRotation = transform.rotation;
            

            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

            Instantiate(range, spawnPos, playerRotation);

            
        }
        //reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spawnPos;
            transform.localEulerAngles = new Vector3(0, spawnRot, 0);
        }
        //dash
        if (Input.GetKeyDown(KeyCode.Z))
        {

        }
    }
}
