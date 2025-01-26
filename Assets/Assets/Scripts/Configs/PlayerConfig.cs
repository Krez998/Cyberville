using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerConfig
{
    [Header("Character Data")]
    public CameraController CameraController;
    public Animator Animator;
    public CharacterController CharacterController;
    public Transform PlayerTransform;   
    public float MoveSpeed;
    public float RotationSpeed;
    public bool IsGrounded;
    public float YSpeed;
    [Tooltip("������ ����� ������ ���� ����� ������� ���������� ����������� ���������," +
        " ����� �������� ������� � ������� ����� ����� � �������� ������ ���������")]
    public float GroundCheckRadius;
    public Vector3 GroundCheckOffset;
    public LayerMask GroundLayer;
    public List<AttackData> Attacks;

    //public float MoveAmount;
    //public float InterpolatedVerticalInput;
    //public float InterpolatedHorizontalInput;

    [Header("Car Data")]
    public IVehicleDoorControl DoorControl;
    public IMovable Movable;
    public ISteerable Steerable;
    public Transform VehicleTransform;
    public Transform VehicleEntryPosition;
    public List<Seat> VehicleSeats;
}
