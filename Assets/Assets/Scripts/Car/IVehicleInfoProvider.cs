using System.Collections.Generic;
using UnityEngine;

public interface IVehicleInfoProvider
{
    public Transform VehicleEntryPosition { get; }

    public List<Seat> Seats { get; }
}
