using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraDirection
{
    Left, Right
}

public enum ViewMode
{
    HipFire, Aim
}

public class Pilot : CharacterManager
{
    [SerializeField] private CameraDirection currentCameraDirection;
    [SerializeField] private PilotAnimatorController pilotAnimatorController;
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    
    [SerializeField] private Vector3 hipCameraTargetPos;
    [SerializeField] private Vector3 aimLeftCameraTargetPos;
    [SerializeField] private Vector3 aimRightCameraTargetPos;
    [SerializeField] private bool canJump = true;

    private void Start()
    {
        movementController.OnSprintChange += OnSprintChange;
    }

    private void OnCameraRecoil(Vector2 vector)
    {
        cameraController.AddRecoil(vector);
    }

    private void Update()
    {
        if (canControl)
        {
            movementController.OnMove(InputManager.Instance.GetMovementInput());

            if (InputManager.Instance.GetSprintInput()) movementController.OnSprint();

            if (canJump) movementController.OnJump(InputManager.Instance.GetJumpInput());

            cameraController.UpdateInput(InputManager.Instance.GetCameraInput());

            if(Input.GetKeyDown(KeyCode.T))
            {
                switch (currentCameraDirection)
                {
                    case CameraDirection.Left:
                        ChangeCameraDirection(CameraDirection.Right);
                        break;
                    case CameraDirection.Right:
                        ChangeCameraDirection(CameraDirection.Left);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void ChangeCameraDirection(CameraDirection direction)
    {
        currentCameraDirection = direction;
        CameraManager.Instance.ChangeCameraDirection(currentCameraDirection);
    }

    public void OnPick()
    {
        movementController.StopMovement();
        characterController.enabled = false;
        movementController.enabled = false;
        cameraController.enabled = false;
        animator.SetBool("IsPicked", true);
    }

    public void OnDrop()
    {
        animator.SetBool("IsPicked", false);
        characterController.enabled = true;
        movementController.enabled = true;
        cameraController.enabled = true;
    }

    private void OnSprintChange(bool state)
    {
        switch (state)
        {
            case true:
                CameraManager.Instance.ChangeFollowFOV(50, .5f);
                break;
            case false:
                CameraManager.Instance.ChangeFollowFOV(40, .5f);
                break;
        }
    }

    private void OnAimModeChanged(ViewMode mode)
    {
        movementController.StopSprint();

        switch (mode)
        {
            case ViewMode.HipFire:
                aimVirtualCamera.gameObject.SetActive(false);
                movementController.SetRotateOnMove(true);
                break;
            case ViewMode.Aim:
                aimVirtualCamera.gameObject.SetActive(true);
                movementController.SetRotateOnMove(false);
                break;
        }
    }

    private void OnFire()
    {
        movementController.StopSprint();
    }

    public override void SetShowWeapon(bool show)
    {
        base.SetShowWeapon(show);
    }

    public override void ResetActions()
    {
        base.ResetActions();
    }
}
