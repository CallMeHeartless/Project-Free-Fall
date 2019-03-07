using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : AudioController
{
    bool playerHit = false;
    // Start is called before the first frame update
    public override void Start(){
        base.Start();
        
    }

    public void PlayerHitAudio() {
        PlaySingleSound("armor hit");
        playerHit = true;
    }

    public void PlayerWallHitAudio()
    {
        PlaySingleSound("hitting wall");
        playerHit = true;
    }

    public void PlayerArmourLostAudio() {
        PlaySingleSound("armor going off");
    }

    public void PlayerChargingAudio()
    {
        PlaySingleSound("Charge up 5sec");
    }

    public void StopPlayerChargingAudio()
    {
        StopSingleSound("Charge up 5sec");
    }

    public void PlayerdashingAudio()
    {
        PlaySingleSound("jetpack 2.5s");
    }

    public void StopdashingAudio()
    {
        StopSingleSound("jetpack 2.5s");
    }

    public void PlayerHasCollectedOrb() {
        PlaySingleSound("OrbPickedUp");
    }

    public void PlayerMissing()
    {
        if (playerHit == false)
        {


            switch (Random.Range(1, 2))
            {
                case 1:
                    PlaySingleSound("melee miss");
                    break;
                case 2:
                    PlaySingleSound("melee miss Vup");
                    break;
                case 3:
                    // PlaySingleSound("melee hit 3");
                    break;
                default:
                    break;
            }

        }
        else
        {
            playerHit = false;
        }  
    }
}
