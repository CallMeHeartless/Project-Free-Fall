using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{
    public Vector3 spawnPos;
    public float spawnRot;
    public float spawnDistance;
    public float hitDelay;
    public GameObject range;
    public bool grounded;
    float dashtimer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //grounded
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * .55f, Color.yellow);

        RaycastHit ray;
        Ray groundRay = new Ray(transform.position, Vector3.down);
        Debug.Log("call");
        if (Physics.Raycast(groundRay, out ray, .55f))
        {
            Debug.Log("hit");
            grounded = true;
            Debug.Log("hit1");
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            Debug.Log("hit22");
            gameObject.transform.parent = ray.transform;
            Debug.Log("hit333");
        }
        else
        {
            Debug.Log("bye");
            grounded = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            transform.parent = null;
        }

        //Controls

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
            Vector3 playerPos = transform.position;
            Quaternion playerRotation = transform.rotation;
           
            Vector3 vector = Quaternion.Euler(0, playerRotation.y, 0) * transform.forward;
            gameObject.GetComponent<Rigidbody>().velocity = vector * 10;

            dashtimer = 3;
            //Instantiate(range, playerPos, playerRotation);

        }
        if (dashtimer <= 0)
        {
            dashtimer -= Time.deltaTime;
        }
        else {
            if(dashtimer >= 1)
            {
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
        if (hitDelay >= 0)
        {
            if (hitDelay >= 1)
            {
                hitDelay -= Time.deltaTime;
                gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
            else
            {
                hitDelay -= Time.deltaTime;
            }
           
        } 
        
        
    }
        //void OnCollisionStay(Collision collision)
        //{
        //    Debug.Log("landing");
        //    if (collision.gameObject.tag == "ground")
        //    {
        //        RaycastHit ray;
        //        Vector2 positionToCheck = transform.position;
        //        if(Physics.Raycast(transform.position, Vector3.down, 1))
        //        {
        //            grounded = true;
        //            gameObject.GetComponent<Rigidbody>().useGravity = false;
        //            gameObject.transform.parent = collision.transform;


        //        }

        //    }
        //}

        ////call when jumping
        //void OnCollisionExit(Collision collision)
        //{
        //    Debug.Log("bye");
        //    grounded = false;
        //    gameObject.GetComponent<Rigidbody>().useGravity = true;
        //    transform.parent = null;
        //}


    }
