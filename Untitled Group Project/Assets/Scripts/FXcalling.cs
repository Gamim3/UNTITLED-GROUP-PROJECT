using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXcalling : MonoBehaviour
{
    public GameObject dustParticle;
    public Transform particlePos;
    public ParticleSystem dustDash;
    public void FXCall(string AttackString)
    {
        if(AttackString == "LeftClaw")
        {
            GameObject kaas = Instantiate(dustParticle, particlePos.position, particlePos.rotation);
            Destroy(kaas, 4);
        }
        if (AttackString == "Dash")
        {
            dustDash.Play();
        }
    }
}
