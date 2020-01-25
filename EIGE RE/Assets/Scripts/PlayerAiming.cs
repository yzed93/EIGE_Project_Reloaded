using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public Jarid_BasicMovement spieler;

    void Update()
    {
        if (spieler.getActionState() == ActionState.RUNNING && Input.GetMouseButtonDown(1))
        {
            spieler.changeActionState(ActionState.AIMING);

        }
        if (spieler.getActionState() == ActionState.AIMING && Input.GetMouseButtonUp(1))
        {
            spieler.changeActionState(ActionState.RUNNING);
        }
    }
}
