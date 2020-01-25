using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Jarid_BasicMovement : MonoBehaviour
{
    private Collider altereckstange;
    private ActionState doing;
    public MoveSettings moveSettings;
    public InputSettings inputSettings;
    public ActionState getActionState() {
        return doing;
    }

    /* ++++++++++++++++++++++++++++++++ */
    /*         ++++++++++++++++         */
    /*          INITIALISATION          */
    /*         ++++++++++++++++         */
    /* ++++++++++++++++++++++++++++++++ */

    void Awake() {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        velocity = Vector3.zero;
        forwardInput = sidewaysInput = lookupInput = turnInput = jumpInput = 0;
        targetRotation = transform.rotation;
        moving = false;
        doing = ActionState.RUNNING;
        acrobaticSkills = new Jarid_Acrobat(this);
    }

    /* +++++++++++++++ */
    /*     +++++++     */
    /*      INPUT      */
    /*     +++++++     */
    /* +++++++++++++++ */

    private float forwardInput, sidewaysInput, turnInput, lookupInput, jumpInput;
    private bool mouseOnePressed, mouseTwoPressed, mouseTwoHeld, mouseTwoReleased;

    void GetInput() {
        if (inputSettings.FORWARD_AXIS.Length != 0) {
            forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
            //Debug.Log("ForwardInput detected: " + forwardInput);
        }

        if (inputSettings.SIDEWAYS_AXIS.Length != 0) {
            sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);
            //Debug.Log("sidewaysInput detected: " + sidewaysInput);
        }

        if (inputSettings.JUMP_AXIS.Length != 0)
            jumpInput = Input.GetAxis(inputSettings.JUMP_AXIS);
        if (inputSettings.LOOKUP_AXIS.Length != 0)
            lookupInput = Input.GetAxis(inputSettings.LOOKUP_AXIS);
        if (inputSettings.TURN_AXIS.Length != 0)
            turnInput = Input.GetAxis(inputSettings.TURN_AXIS);

        mouseOnePressed = Input.GetMouseButtonDown(0);
        mouseTwoHeld = Input.GetMouseButton(1);
        mouseTwoPressed = Input.GetMouseButtonDown(1);
        mouseTwoReleased = Input.GetMouseButtonUp(1);
    }

    /* ++++++++++++++++++++ */
    /*      ++++++++++      */
    /*       MOVEMENT       */
    /*      ++++++++++      */
    /* ++++++++++++++++++++ */

    private Rigidbody playerRigidbody;
    private Vector3 velocity;
    private Quaternion targetRotation;
    private bool moving;


    private Jarid_Acrobat acrobaticSkills;


    public void changeActionState(ActionState newActionState)
    {
        this.doing = newActionState;
    }

    public bool isMoving() {
        return moving;
    }
    
    void Forward() {
        switch (doing)
        {
            case (ActionState.RUNNING):
                if (Grounded())
                {
                    //Debug.Log("geht: " + forwardInput + " " + sidewaysInput);
                    velocity.z = forwardInput * moveSettings.runSpeed * ((doing == ActionState.AIMING) ? 0.5f : 1f);
                    velocity.x = sidewaysInput * moveSettings.runSpeed * ((doing == ActionState.AIMING) ? 0.5f : 1f);
                    velocity.y = playerRigidbody.velocity.y;
                    //Debug.Log("x" + velocity.x + " y" + velocity.y + " z" + velocity.z);
                    playerRigidbody.velocity = transform.TransformDirection(velocity);
                }
                break;
            case (ActionState.SWINGING):
                acrobaticSkills.ForwardSwinging(forwardInput);
                break;
            case (ActionState.GETTINGTHEHANGOFIT):
                acrobaticSkills.gettingToPosition();
                break;
        
        }
    }

    private Quaternion reckstangeUrsprung;

    void Jump() {
        if (jumpInput != 0) {
            if (doing == ActionState.RUNNING) {
                if (Grounded()) {
                    playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, moveSettings.jumpVelocity, playerRigidbody.velocity.z);
                }
            } else if (doing == ActionState.SWINGING) {
                this.doing = acrobaticSkills.JumpSwinging();
            }
        }
    }


    /* ++++++++++++++++ */
    /*     ++++++++     */
    /*      CAMERA      */
    /*     ++++++++     */
    /* ++++++++++++++++ */

    public Transform viewDirection;
    public Transform viewVertical;

    void Turn() {
        if (doing == ActionState.RUNNING || doing == ActionState.AIMING)
        {
            
            float turningAmount = moveSettings.rotateVelocity * turnInput * Time.deltaTime;
            if (Mathf.Abs(turnInput) > 0)
            {
                targetRotation *= Quaternion.AngleAxis(turningAmount, Vector3.up);
            }
            transform.rotation = targetRotation;

            float lookupAmount = moveSettings.lookupVelocity * lookupInput * Time.deltaTime * (-1);
            viewVertical.Rotate(lookupAmount, 0, 0, Space.Self);

        }
    }

    /* +++++++++++++++++++++++++++++++++++++++++++++++++++ */
    /*              +++++++++++++++++++++++++              */
    /*               ENVIRONMENT INTERACTION               */
    /*              +++++++++++++++++++++++++              */
    /* +++++++++++++++++++++++++++++++++++++++++++++++++++ */
    //private Quaternion reckstangeUrsprung;
    
    bool Grounded() {
        if (Physics.Raycast(transform.position, Vector3.down, moveSettings.distanceToGround, moveSettings.ground))
        {
            if (altereckstange != null) altereckstange.attachedRigidbody.detectCollisions = true;

            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Reckstange")) {
            doing = ActionState.GETTINGTHEHANGOFIT;
            playerRigidbody.useGravity = false;
            playerRigidbody.isKinematic = true;
            acrobaticSkills.hangOntoReck(other);
            altereckstange = other;
        }
    }

    /* +++++++++++++++++++ */
    /*      +++++++++      */
    /*       UPDATES       */
    /*      +++++++++      */
    /* +++++++++++++++++++ */

    void Update() {
        GetInput();
        Turn();
    }

    void FixedUpdate() {
        Forward();
        Jump();
    }
}

[System.Serializable]
public class MoveSettings {
    public float runSpeed = 5;
    public float rotateVelocity = 150;
    public float lookupVelocity = 150;
    public float jumpVelocity = 12;
    public float distanceToGround = 1.3f;
    public LayerMask ground;
}

[System.Serializable]
public class InputSettings {
    
    public string FORWARD_AXIS = "Vertical";
    public string SIDEWAYS_AXIS = "Horizontal";
    public string TURN_AXIS = "Mouse X";
    public string LOOKUP_AXIS = "Mouse Y";
    public string JUMP_AXIS = "Jump";
}

public enum ActionState
{
    RUNNING, SWINGING, AIMING, GETTINGTHEHANGOFIT
}
