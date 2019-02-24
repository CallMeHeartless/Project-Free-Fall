using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{

    private Transform dashBox;

    private void Start() {
        dashBox = transform.parent.Find("DashHitBox");
    }

    public void DashHitboxOn() {
        dashBox.gameObject.SetActive(true);
    }


    public void DashHitboxOff() {
        dashBox.gameObject.SetActive(false);
        //Debug.Log("hitbox off");
        transform.parent.GetComponent<PlayerController>().StopPlayer();
    }
}
