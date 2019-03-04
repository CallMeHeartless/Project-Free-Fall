using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : AudioController
{
    // Start is called before the first frame update
    public override void Start(){
        base.Start();
    }

    public void PlayerHitAudio() {
        PlaySingleSound("armor hit");
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
        PlaySingleSound("jetpack 4sec");
    }

    public void PlayerMissing()
    {
        switch (Random.Range(1, 3))
        {
            case 1:
                PlaySingleSound("melee miss");
                break;
            case 2:
                PlaySingleSound("melee miss 2");
                break;
            case 3:
                PlaySingleSound("melee hit 3");
                break;
            default:
                break;
        }
        
        
    }
}
