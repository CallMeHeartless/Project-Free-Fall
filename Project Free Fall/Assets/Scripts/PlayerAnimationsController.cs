using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{

    private Transform dashBox;
    public Transform swordBox;

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

    public void SwordBoxOn() {
        swordBox.gameObject.SetActive(true);
    }

    public void SwordBoxOff() {
        swordBox.gameObject.SetActive(false);
        transform.parent.transform.GetChild(4).gameObject.GetComponent<PlayerAudioController>().PlayerMissing();
    }
}
