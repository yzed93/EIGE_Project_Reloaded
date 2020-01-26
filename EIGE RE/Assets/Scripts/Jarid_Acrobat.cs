using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jarid_Acrobat
{
    public float chillWinkel = 3;
    public float initialRotateSpeed = 3;
    public float acceleration = 20f;
    public float deceleration = 25f;
    public float overallSlowness = 5;
    public float absprungPower = 2.5f;

    private float maxWinkel;
    private float hangWinkel;
    private float hangWinkelAlt;
    private bool runterwärts;
    private float rotateAmount;

    private Jarid_BasicMovement basicMovement;
    private Rigidbody playerRigidbody;
    private GameObject playerGameObject;
    private Collider other;

    private Transform reckstange;
    private Transform Achse;
    private Transform targetTransformation;

    public void hangOntoReck(Collider other)
    {
        this.other = other;
        reckstange = other.transform.GetChild(0);
        Achse = other.transform;
        hangWinkel = 0;
        
        reckstange.LookAt(playerGameObject.transform.position);
        //Debug.Log("x1=" + reckstange.rotation.x + " x2=" + Achse.rotation.x + " y1=" + reckstange.rotation.x + " y2=" + Achse.rotation.x + " z1=" + reckstange.rotation.z + " z2=" + Achse.rotation.z);
        float winkel = Vector3.Angle(Achse.forward, reckstange.forward);
        //Debug.Log("Winkel: " + winkel);
        playerGameObject.transform.LookAt(new Vector3(reckstange.position.x, reckstange.position.y, reckstange.position.z));
        playerGameObject.transform.RotateAround(playerGameObject.transform.position, playerGameObject.transform.right, 90);
        playerGameObject.transform.SetParent(reckstange);
        
        playerRigidbody.transform.RotateAround(playerRigidbody.transform.position, playerRigidbody.transform.up, -90);
        targetTransformation = Achse;
        //targetTransformation.position -= new Vector3(0, 0.4f, 0);
        
    }
    public Jarid_Acrobat(Jarid_BasicMovement basicMovement)
    {
        this.basicMovement = basicMovement;
        this.playerGameObject = basicMovement.gameObject;

        this.playerRigidbody = playerGameObject.GetComponent<Rigidbody>();
        
    }

    public void gettingToPosition()
    {
        reckstange.transform.rotation = Quaternion.Lerp(reckstange.rotation, targetTransformation.rotation, Time.deltaTime*5);
        
        if (Quaternion.Angle(reckstange.rotation, targetTransformation.rotation) < 0.5)
        {
            basicMovement.changeActionState(ActionState.SWINGING);
        }

    }

    public void ForwardSwinging(float forwardInput)
    { 
        rotateAmount = 0;
        if (maxWinkel == chillWinkel && forwardInput != 0)
        {
            maxWinkel += acceleration / 4;
        }
        if (hangWinkelAlt * hangWinkel < 0)
        {
            runterwärts = !runterwärts;
            if (forwardInput == 0)
            {
                if (maxWinkel >= chillWinkel + deceleration)
                {
                    maxWinkel -= deceleration;
                }
                else
                {
                    maxWinkel = chillWinkel;
                }
            } else {
                maxWinkel += acceleration * (maxWinkel > 0 ? 1 : -1);
                if (maxWinkel > 100) maxWinkel = 100;
            }
        } else if (Mathf.Abs(Mathf.Abs(hangWinkel) - Mathf.Abs(maxWinkel)) < 2 || Mathf.Abs(hangWinkel) - Mathf.Abs(maxWinkel) > 0) {
            runterwärts = true;
            if (maxWinkel < chillWinkel) maxWinkel = chillWinkel;
        }

        if (maxWinkel == chillWinkel) {
            rotateAmount = 0;
        } else if (runterwärts) {
            rotateAmount = ((Mathf.Abs(maxWinkel) - Mathf.Abs(hangWinkel)) / overallSlowness) * (hangWinkel > 0 ? -1 : 1);
            if (rotateAmount == 0) rotateAmount = (hangWinkel > 0 ? -0.1f : 0.1f);
        } else {
            rotateAmount = ((Mathf.Abs(hangWinkel) - Mathf.Abs(maxWinkel)) / overallSlowness) * (hangWinkel > 0 ? -1 : 1);
            if (rotateAmount == 0) rotateAmount = (hangWinkel > 0 ? 0.1f : -0.1f);
        }

        //Debug.Log((runterwärts? "Runter" : "Hinauf") + ",\thangWinkel=" + hangWinkel + ",\tmaxWinkel=" + maxWinkel + ",\tamount=" + rotateAmount);
        hangWinkelAlt = hangWinkel;
        hangWinkel += rotateAmount;

        reckstange.RotateAround(Achse.position, Achse.up, rotateAmount);
    }



    public ActionState JumpSwinging()
    {
        playerGameObject.transform.SetParent(null);

        playerRigidbody.isKinematic = false;
        playerRigidbody.useGravity = true;
        //reckstange.transform.rotation = Quaternion.Euler(180, 0, 0);
            
        Vector3 absprungRichtung = playerGameObject.transform.forward;
        Debug.Log(rotateAmount * absprungPower);
        playerRigidbody.velocity = absprungRichtung * rotateAmount * absprungPower;
        rotateAmount = 0;
        maxWinkel = chillWinkel;
        return ActionState.RUNNING;
    }
}