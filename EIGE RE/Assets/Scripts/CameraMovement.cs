using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Sprite Crosshair;
    public Vector3 targetPosition;
    public Jarid_BasicMovement jarid;

    [SerializeField]
    public Transform followedObject;

    private float smooth;
    private float heightOverPlayer;
    private float distanceBehindPlayer;

    [SerializeField]
    private float aimingOverRightShoulder;
    [SerializeField]
    private float aimingOverPlayer;
    [SerializeField]
    private float aimingBehindPlayer;
    [SerializeField]
    private int turningAmount;

    [SerializeField]
    private int smoothnessOfDoom;
    

    void Start()
    {
        heightOverPlayer = 3;
        distanceBehindPlayer = 5;
        smooth = 100;
        aimingBehindPlayer = 1.6f;
        aimingOverPlayer = 0.3f;
        aimingOverRightShoulder = 0.8f;
        turningAmount = 30;
        oldActionState = jarid.getActionState();
    }

    public void transition()
    {
        this.smooth = 100;
    }

    public void transitionOver()
    {
        this.smooth = 0;
    }

    private ActionState oldActionState;

    void LateUpdate()
    {   
        if (jarid.getActionState() == ActionState.DEAD)
        {
            this.transform.position += Vector3.up * 0.01f;
            transform.LookAt(jarid.transform);
        }
        if (jarid.getActionState() == ActionState.SWINGING || jarid.getActionState() == ActionState.GETTINGTHEHANGOFIT) {
            transform.LookAt(jarid.transform);
            /*targetPosition = jarid.getReckstange().transform.position;
            targetPosition += new Vector3(followedObject.rotation.y / 10, 0, followedObject.rotation.y-90 / 10);
            transform.position = targetPosition;
            
            transform.LookAt(jarid.transform);*/
        }
        else if (jarid.getActionState() == ActionState.AIMING) {
            targetPosition = followedObject.position + aimingOverPlayer * Vector3.up - aimingBehindPlayer * followedObject.forward + aimingOverRightShoulder * followedObject.right;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
            transform.LookAt(followedObject, Vector3.up * 30);
            transform.Rotate(0, turningAmount, 0, Space.World);

        } else if (jarid.getActionState() == ActionState.RUNNING) {

            targetPosition = followedObject.position + heightOverPlayer * Vector3.up - distanceBehindPlayer * followedObject.forward;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
            transform.LookAt(followedObject, Vector3.up * 30);
        }
        oldActionState = jarid.getActionState();
        if (transform.position.y < followedObject.position.y) transform.position = new Vector3(transform.position.x, followedObject.position.y, transform.position.z);
    }
}
