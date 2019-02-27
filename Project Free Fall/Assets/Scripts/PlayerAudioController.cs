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


}
