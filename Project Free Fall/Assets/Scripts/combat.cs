﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class combat : MonoBehaviour
{
    public enum CurrentAction
    {
        move,
        stun,
        preDash,
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
    float dashCharge;
    float dashtimer;
    public float[] timersForCurrentAction;
    public CurrentAction game = CurrentAction.move;

    public float hp;

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
        
        if (Physics.Raycast(groundRay, out ray, .55f))
        {
           
            grounded = true;
            
            gameObject.GetComponent<Rigidbody>().useGravity = false;
           
            gameObject.transform.parent = ray.transform;
            
        }
        else
        {
            
            grounded = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
            transform.parent = null;
        }

        //Controls

        //swing
        if (Input.GetKeyDown(KeyCode.Q))
        {
          

            Vector3 spawnPos = transform.position + transform.forward * spawnDistance;

            Instantiate(range, spawnPos, transform.rotation);

            game = CurrentAction.melee;
            hitDelay = timersForCurrentAction[4];
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
            dashCharge = 0;
            game = CurrentAction.preDash;
        }

        if (Input.GetKey(KeyCode.Z))
        {
            if (dashCharge == 100)
            {
                //charge done add effects
                dashCharge++;
            }
            else
            {
                if (dashCharge == 101)
                {
                    //charge done

                }
                dashCharge++;
            }

        }

        if (Input.GetKeyUp(KeyCode.Z))
        {


            Vector3 vector = Quaternion.Euler(0, transform.rotation.y, 0) * transform.forward;
            gameObject.GetComponent<Rigidbody>().velocity = vector * 10;

            dashtimer = 3;
            invincablty = true;
            //Instantiate(range, playerPos, playerRotation);

            game = CurrentAction.dashing;
            hitDelay = timersForCurrentAction[3];
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
            if ((game == CurrentAction.dashing))
            {
                if (collision.gameObject.GetComponent<combat>().invincablty == false)
                {
                    float thrust = 2;
                    collision.rigidbody.velocity = (transform.forward * thrust) + new Vector3(0, Random.Range(0.5f, 1.0f), 0);
                    game = CurrentAction.stun;
                    collision.gameObject.GetComponent<combat>().hitDelay = 3;
                }
            }

        }
    }

    Vector3 randomDamage(float baseKnockback)
    {
        float dummy = Random.Range(0.5f, 1.0f);
        if ((hp<= 0)&& (hp >= 25))
        {
            
            //(transform.forward * thrust) + new Vector3(0, Random.Range(0.5f, 1.0f), 0);
        }
        return (transform.forward * 2) + new Vector3(0, Random.Range(0.5f, 1.0f), 0);
    } 
}