using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsController : MonoBehaviour
{

    private Transform dashBox;
    public Transform swordBox;
    public GameObject swordTrail;

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
        if(swordTrail != null) {
            swordTrail.SetActive(true);
        }
    }

    public void SwordBoxOff() {
        swordBox.gameObject.SetActive(false);
        transform.parent.transform.GetChild(4).gameObject.GetComponent<PlayerAudioController>().PlayerMissing();
        if(swordTrail != null) {
            swordTrail.SetActive(false);
        }

    }
}
