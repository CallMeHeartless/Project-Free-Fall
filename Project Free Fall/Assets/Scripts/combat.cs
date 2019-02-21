using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{
    public enum CurrentAction
    {
        move,
        stun,
        dashing,
        melee
    };

    public Vector3 spawnPos;
    public float spawnRot;
    public float spawnDistance;
    public float hitDelay;
    public GameObject range;
    public bool grounded;
    public bool invincablty;
    float dashtimer;
    public float[] timersForCurrentAction;
    public CurrentAction game = CurrentAction.move;


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

            game = CurrentAction.melee;
            hitDelay = timersForCurrentAction[3];
        }
        //reset
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = spawnPos;
            transform.localEulerAngles = new Vector3(0, spawnRot, 0);

            game = CurrentAction.move;
            hitDelay = 0;
        }
        //dash
        if (Input.GetKeyDown(KeyCode.Z))
        {
           

            Vector3 vector = Quaternion.Euler(0, transform.rotation.y, 0) * transform.forward;
            gameObject.GetComponent<Rigidbody>().velocity = vector * 10;

            dashtimer = 3;
            invincablty = true;
            //Instantiate(range, playerPos, playerRotation);

            game = CurrentAction.dashing;
            hitDelay = timersForCurrentAction[2];
        }

        ////dash action
        //if (dashtimer <= 0)
        //{
        //    dashtimer -= Time.deltaTime;
        //}
        //else
        //{
        //    if (dashtimer >= 1)
        //    {
        //        gameObject.GetComponent<Rigidbody>().useGravity = true;
        //    }
        //    else
        //    {
        //        invincablty = false;
        //    }
        //}

        // before player can recover
        if (hitDelay >= 0)
        {
            //can't move or reduce movement (need to be added)

            hitDelay -= Time.deltaTime;

        }
        else
        {
            game = CurrentAction.move;
        }
    }
        //dash hit 
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (game == CurrentAction.dashing)
                {
                    if (collision.gameObject.GetComponent<combat>().invincablty == false)
                    {
                        float thrust = 2;
                        collision.rigidbody.velocity = (transform.forward * thrust) + new Vector3(0, 1, 0);
                        game = CurrentAction.stun;
                        collision.gameObject.GetComponent<combat>().hitDelay = 3;
                    }
                }

            }
        }

       
}