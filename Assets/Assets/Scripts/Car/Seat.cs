using System;
using UnityEngine;

[Serializable]
public class Seat
{
    public SeatType SeatType;

    public Transform Transform;
}

public enum SeatType
{
    DriversSeat,
    FrontRightPassengerSeat,
    RareLeftPassengerSeat,
    RareRightPassengerSeat
}