using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassHitForce : MonoBehaviour
{
    public float launchForce = 50;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "PropSword")
        {
            float direction = Random.Range(1, 5);

            if(direction == 1)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * launchForce,ForceMode.Impulse);
            }
            else if (direction == 2)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * launchForce, ForceMode.Impulse);
            }
            else if (direction == 1)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * launchForce, ForceMode.Impulse);
            }
            else if (direction == 1)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * launchForce, ForceMode.Impulse);
            }
        }
    }
}
