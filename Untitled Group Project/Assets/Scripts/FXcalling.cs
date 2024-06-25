using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXcalling : MonoBehaviour
{
    public ParticleSystem dustFX;
    public void FXCall(string AttackString)
    {
        if(AttackString == "LeftClaw")
        {
            dustFX.Play();
        }
    }
}
