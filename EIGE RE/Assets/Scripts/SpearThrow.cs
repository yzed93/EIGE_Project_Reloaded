using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrow : MonoBehaviour
{
   
    public Rigidbody speer;
    [SerializeField]
    [Range(10, 50)]
    public float throwForce;
    

     void Awake()
    {
        speer.transform.parent = null;
        speer.isKinematic = false;
        speer.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * throwForce, ForceMode.Impulse);
        speer.AddTorque(speer.transform.TransformDirection(Vector3.up) * 100, ForceMode.Impulse);
    }


}
