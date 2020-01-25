using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearThrow : MonoBehaviour
{
    public Transform waffenSpawn;
    public Jarid_BasicMovement jarid;
    public Rigidbody speer;
    [SerializeField]
    [Range(10, 100)]
    public float throwForce;
    public bool bewaffnet;
    

     void Update()
    {
        if (jarid.getActionState()==ActionState.AIMING && Input.GetButtonDown("Fire1") && bewaffnet == true)
        {
            
            Throw();
            SpearSpawn();
        }
    }
    void SpearSpawn()
    {
        Debug.Log("Spawn geht");
        Rigidbody neuerSpeer = new Rigidbody();
        neuerSpeer = Instantiate(speer, waffenSpawn);
    
        
        

    }
    void Throw()
    {
       
        {


            speer.transform.parent = null;
            speer.isKinematic = false;
            speer.AddForce(Camera.main.transform.TransformDirection(Vector3.forward) * throwForce, ForceMode.Impulse);
            speer.AddTorque(speer.transform.TransformDirection(Vector3.up) * 100, ForceMode.Impulse);
            bewaffnet = false;
        }
    }


}
