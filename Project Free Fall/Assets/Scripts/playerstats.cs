using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerstats : MonoBehaviour
{
    public enum CurrentAction
    {
        move,
        stun,
        preDash,
        dashing,
        melee
    };

    public float stunTime;
    public float preDashMaxTime;
    public float dashingLength;
    public float meleeTime;
    public float HP;




}
