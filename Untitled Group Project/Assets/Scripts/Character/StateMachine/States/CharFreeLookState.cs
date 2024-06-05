using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFreeLookState : CharBaseState
{
    public CharFreeLookState(CharStateMachine currentContext, CharStateFactory charachterStateFactory) : base(currentContext, charachterStateFactory)
    {
        StateName = "FreeLook";
    }

    public override void EnterState()
    {
        // Debug.Log("Enter FreeLook");

        InitializeSubState();

        Ctx.IsFreeLookState = true;

        // Ctx.FreeLookCam.gameObject.SetActive(true);
    }

    public override void ExitState()
    {
        Ctx.IsFreeLookState = false;

        // Ctx.FreeLookCam.gameObject.SetActive(false);
    }



    public override void UpdateState()
    {
        // Debug.Log("Update FreeLook");

        if (Ctx.PlayerInput.currentActionMap == Ctx.PlayerInput.actions.FindActionMap("Menu"))
        {
            return;
        }

        Ctx.Orientation.forward = Ctx.PlayerObj.forward.normalized;

        float mouseY = Ctx.IsCamAction.y * Ctx.MouseSensitivity * Time.deltaTime;
        float mouseX = Ctx.IsCamAction.x * Ctx.MouseSensitivity * Time.deltaTime;

        Ctx.YRotation += mouseX;
        Ctx.XRotation -= mouseY;

        Ctx.XRotation = Mathf.Clamp(Ctx.XRotation, -Ctx.MinXRotation, Ctx.MaxXRotation);

        Ctx.CamTarget.rotation = Quaternion.Euler(Ctx.XRotation, Ctx.YRotation, 0);

        Vector3 viewDir = Ctx.transform.position - new Vector3(Ctx.PlayerCam.transform.position.x, Ctx.transform.position.y, Ctx.PlayerCam.transform.position.z);
        Ctx.Orientation.forward = viewDir.normalized;

        Vector3 inputDir = Ctx.Orientation.forward * Ctx.CurrentMovementInput.y + Ctx.Orientation.right * Ctx.CurrentMovementInput.x;

        Quaternion lookRotation = Quaternion.LookRotation(inputDir, Vector3.up);
        Ctx.PlayerObj.transform.rotation = Quaternion.Slerp(Ctx.PlayerObj.transform.rotation, lookRotation, Time.deltaTime * Ctx.PlayerRotationSpeed);

        CheckSwitchStates();
    }

    public override void FixedUpdateState() { }

    public override void InitializeSubState()
    {
        if (Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SetSubState(Factory.Grounded());
        }
        else if (Ctx.IsSloped)
        {
            SetSubState(Factory.Sloped());
        }
        else if (!Ctx.IsGrounded && !Ctx.IsSloped && !Ctx.IsJumpAction)
        {
            SetSubState(Factory.Airborne());
        }
        else if (Ctx.IsJumpAction && Ctx.IsGrounded || Ctx.IsJumpAction && Ctx.IsSloped)
        {
            SetSubState(Factory.Jumping());
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsTargetingAction)
        {
            SwitchState(Factory.Target());
        }
    }
}
