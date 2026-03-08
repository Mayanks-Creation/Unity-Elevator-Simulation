using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public ElevatorController[] elevators;
    public FloorButton[] floorButtons;

    void Start()
    {
        foreach (ElevatorController elevator in elevators)
        {
            elevator.OnFloorServiced += HandleFloorServiced;
        }
    }

    public void RequestElevator(int floor)// Calculates the Possible correct solution on request made
    {
        // Case 1: elevator already on the floor
        foreach (ElevatorController elevator in elevators)
        {
            if (elevator.GetCurrentFloor() == floor && !elevator.IsMovingVertically())
            {
                elevator.OpenDoorInstant();
                return;
            }
        }

        ElevatorController bestElevator = null;
        int minDistance = int.MaxValue;

        // STEP 1: elevators already moving toward request
        foreach (ElevatorController elevator in elevators)
        {
            if (!elevator.IsMovingVertically())
                continue;

            int direction = elevator.GetDirection();
            int currentFloor = elevator.GetCurrentFloor();
            int distance = Mathf.Abs(currentFloor - floor);

            bool movingTowardFloor =
                (direction == 1 && floor >= currentFloor) ||
                (direction == -1 && floor <= currentFloor);

            if (movingTowardFloor && distance < minDistance)
            {
                minDistance = distance;
                bestElevator = elevator;
            }
        }

        // STEP 2: closest elevator not moving vertically (idle OR door animation)
        if (bestElevator == null)
        {
            foreach (ElevatorController elevator in elevators)
            {
                if (elevator.IsMovingVertically())
                    continue;

                int distance = Mathf.Abs(elevator.GetCurrentFloor() - floor);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestElevator = elevator;
                }
            }
        }

        // STEP 3: fallback closest
        if (bestElevator == null)
        {
            foreach (ElevatorController elevator in elevators)
            {
                int distance = Mathf.Abs(elevator.GetCurrentFloor() - floor);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestElevator = elevator;
                }
            }
        }

        if (bestElevator != null)
        {
            bestElevator.AddRequest(floor);
        }
    }

    void HandleFloorServiced(int floor)
    {
        if (floorButtons != null && floor < floorButtons.Length)
        {
            floorButtons[floor].ResetButton();
        }
    }
}