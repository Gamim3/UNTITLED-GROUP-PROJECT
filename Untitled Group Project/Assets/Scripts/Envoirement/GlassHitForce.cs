using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassHitForce : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PropSword")
        {
            float direction = Random.Range(1, 5);

            if(direction == 1)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 50,ForceMode.Impulse);
            }
            else if (direction == 2)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 50, ForceMode.Impulse);
            }
            else if (direction == 1)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 50, ForceMode.Impulse);
            }
            else if (direction == 1)
            {
                collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.left * 50, ForceMode.Impulse);
            }
        }
    }
}
