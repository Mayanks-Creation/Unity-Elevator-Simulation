using UnityEngine;
using TMPro;

public class FloorDisplay : MonoBehaviour
{
    public ElevatorController elevator;
    public TextMeshPro textDisplay;

    void Update()//Updates the floor Display on Elevator while moving
    {
        int floor = elevator.GetCurrentFloor();

        if (floor == 0)
            textDisplay.text = "G";
        else
            textDisplay.text = floor.ToString();
    }
}