using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXcalling : MonoBehaviour
{
    public ParticleSystem[] particleFX;
    public void FXCall(string AttackString)
    {
        if(AttackString == "LeftClaw")
        {
            particleFX[0].Play();
        }
        if (AttackString == "Dash")
        {
            particleFX[1].Play();
        }
    }
}
