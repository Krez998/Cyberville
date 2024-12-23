using UnityEngine;

public class PlayerController : MonoBehaviour, IPlayerInteraction
{
    public void EnterVehicle()
    {
        throw new System.NotImplementedException();
    }

    public void ExitVehicle()
    {
        throw new System.NotImplementedException();
    }

    public void Jump()
    {
        throw new System.NotImplementedException();
    }

    public void Move(Vector3 direction)
    {
        throw new System.NotImplementedException();
    }
}


public interface IPlayerInteraction
{
    void Move(Vector3 direction);
    void Jump();
    void EnterVehicle();
    void ExitVehicle();
}

public interface IVehicleInteraction
{
    Transform VehicleEntryPosition { get; }
    void OpenDoor();
    void StartEngine();
    void StopEngine();
}

public interface IPlayerState
{
    void HandleInput(InputController inputController);
}

public class InputController : MonoBehaviour
{
    public IPlayerInteraction Player { get; private set; }
    private IPlayerState currentState;

    void Start()
    {
        Player = FindFirstObjectByType<PlayerController>();
        currentState = new PlayerOnFootState(Player); // Начальное состояние
    }

    public void HandleInput()
    {
        currentState.HandleInput(this);
    }

    public void ChangeState(IPlayerState newState)
    {
        currentState = newState;
    }
}

public class PlayerOnFootState : IPlayerState
{
    private IPlayerInteraction player;

    public PlayerOnFootState(IPlayerInteraction player)
    {
        this.player = player;
    }

    public void HandleInput(InputController inputController)
    {
        if (Input.GetKey(KeyCode.W))
        {
            player.Move(Vector3.forward);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            player.EnterVehicle();
            //inputController.ChangeState(new PlayerInVehicleState(player));
        }
    }
}

public class PlayerInVehicleState : IPlayerState
{
    private IVehicleInteraction vehicle;

    public PlayerInVehicleState(IVehicleInteraction vehicle)
    {
        this.vehicle = vehicle;
    }

    public void HandleInput(InputController inputController)
    {
        if (Input.GetKey(KeyCode.W))
        {
            vehicle.StartEngine();
            // Здесь можно вызвать анимацию вождения
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            vehicle.OpenDoor();
        }
        if (Input.GetKeyDown(KeyCode.X)) // Пример выхода из автомобиля
        {
            inputController.ChangeState(new PlayerOnFootState(inputController.Player));
            inputController.Player.ExitVehicle();
        }
    }
}
