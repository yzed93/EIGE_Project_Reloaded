using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(Rigidbody))]
public class SpearBehaviour : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Hindernis")
        {
            Debug.Log("Treffer");
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            this.GetComponent<Rigidbody>().AddTorque((Vector3.up) * -100, ForceMode.Impulse);
            this.GetComponent<Rigidbody>().detectCollisions = false;

            


        }

    }
}
