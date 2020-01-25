using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jarid_Acrobat
{
    public float chillWinkel = 3;
    public float initialRotateSpeed = 3;
    public float acceleration = 0.1f;
    public float deceleration = 0.3f;
    public float overallSlowness = 15;
    public float absprungPower = 3;

    private float maxWinkel;
    private float nullWinkel;
    private float hangWinkel;
    private bool runterwärts;
    private float rotateAmount;

    private Jarid_BasicMovement basicMovement;
    private Rigidbody playerRigidbody;
    private GameObject playerGameObject;

    private Transform reckstange;
    private Transform Achse;

    public void hangOntoReck(Collider other)
    {
        reckstange = other.transform.GetChild(0);
        Achse = other.transform;
        hangWinkel = 0;
        


        
        
        reckstange.LookAt(playerGameObject.transform.position);
        Debug.Log("x1=" + reckstange.rotation.x + " x2=" + Achse.rotation.x + " y1=" + reckstange.rotation.x + " y2=" + Achse.rotation.x + " z1=" + reckstange.rotation.z + " z2=" + Achse.rotation.z);
        float winkel = Vector3.Angle(Achse.forward, reckstange.forward);
        //Debug.Log("Winkel: " + winkel);
        playerGameObject.transform.LookAt(new Vector3(reckstange.position.x, reckstange.position.y, reckstange.position.z));
        //reckstange.RotateAround(reckstange.position, Vector3.up, winkel);
        //playerGameObject.transform.RotateAround(playerGameObject.transform.position, reckstange.up, winkel > 90? -winkel : winkel);

        //transform.rotation = Quaternion.LookRotation(reckstange.rotation.eulerAngles);
        //Quaternion.LookRotation(reckstange.right, reckstange.forward);
        //                transform.RotateAround(transform.position, reckstange.up, 90);
        //playerGameObject.transform.RotateAround(playerGameObject.transform.position, reckstange.up, (ankathete > 0 ? winkelOfDoom : -winkelOfDoom));
        //transform.RotateAround(transform.position, reckstange.up, (ankathete > 0 ? 90 : -90));

        /*
        reckstange.Rotate(0, 0, winkelOfDoom + (ankathete > 0 ? 0 : -90));
        playerGameObject.transform.SetParent(reckstange);

        maxWinkel = winkelOfDoom < 3 ? 3 : (winkelOfDoom + (ankathete > 0 ? 0 : 90));
        nullWinkel = -winkelOfDoom;
        runterwärts = true;*/
    }
    public Jarid_Acrobat(Jarid_BasicMovement basicMovement)
    {
        this.basicMovement = basicMovement;
        this.playerGameObject = basicMovement.gameObject;

        this.playerRigidbody = playerGameObject.GetComponent<Rigidbody>();
        
    }

    public void ForwardSwinging(float forwardInput)
    { /*
        rotateAmount = 0;

        float hangWinkelAlt = hangWinkel;
        hangWinkel = reckstange.rotation.eulerAngles.z - 180;
        if (hangWinkelAlt * hangWinkel < 0)
        {
            runterwärts = !runterwärts;
            if (forwardInput == 0)
            {
                if (maxWinkel >= chillWinkel + deceleration)
                {
                    maxWinkel -= (deceleration + maxWinkel) / 8f;
                }
                else
                {
                    maxWinkel = chillWinkel;
                }
            }
        }
        else if (Mathf.Abs(Mathf.Abs(hangWinkel) - Mathf.Abs(maxWinkel)) < 2 || Mathf.Abs(hangWinkel) - Mathf.Abs(maxWinkel) > 0)
        {
            runterwärts = true;
            if (forwardInput != 0)
                maxWinkel += acceleration * (maxWinkel > 0 ? 1 : -1);

            if (maxWinkel > 130) maxWinkel = 130;
            if (maxWinkel < chillWinkel) maxWinkel = chillWinkel;
        }


        if (runterwärts)
        {

            rotateAmount = ((Mathf.Abs(maxWinkel) - Mathf.Abs(hangWinkel)) / overallSlowness) * (hangWinkel > 0 ? -1 : 1);
            if (rotateAmount == 0) rotateAmount = (hangWinkel > 0 ? -0.1f : 0.1f);
        }
        else
        {
            rotateAmount = ((Mathf.Abs(hangWinkel) - Mathf.Abs(maxWinkel)) / overallSlowness) * (hangWinkel > 0 ? -1 : 1);
            if (rotateAmount == 0) rotateAmount = (hangWinkel > 0 ? 0.1f : -0.1f);
        }

        //            Debug.Log(rotateAmount);
        //Debug.Log((runterwärts? "Runter" : "Hinauf") + ",\thangWinkel=" + hangWinkel + ",\tmaxWinkel=" + maxWinkel + ",\tnullWinkel=" + nullWinkel + ",\tamount=" + rotateAmount);
        reckstange.Rotate(new Vector3(0, rotateAmount, 0));
        */
    }



    public ActionState JumpSwinging()
    {
        playerGameObject.transform.SetParent(null);

        playerRigidbody.isKinematic = false;
        playerRigidbody.useGravity = true;
        reckstange.transform.rotation = Quaternion.Euler(180, 0, 0);

        Vector3 absprungRichtung = playerRigidbody.transform.forward;
        Debug.Log(rotateAmount);
        playerRigidbody.velocity = absprungRichtung * rotateAmount * -absprungPower;
        return ActionState.RUNNING;
    }
}