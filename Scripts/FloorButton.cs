using UnityEngine;
using UnityEngine.UI;

public class FloorButton : MonoBehaviour
{
    public int floorNumber;

    public ElevatorManager elevatorManager;

    public Image buttonImage;

    public Color normalColor = Color.white;
    public Color activeColor = Color.red;

    private bool requestActive = false;

    void Start()//button set to normal color once at start
    {
        buttonImage.color = normalColor;
    }

    public void RequestElevator()//Called when button is Pressed ,send request to Elevator Manager and button color is changed
    {
        if (requestActive)
            return;

        elevatorManager.RequestElevator(floorNumber);

        buttonImage.color = activeColor;
        requestActive = true;
    }

    public void ResetButton()//Resets Button back to normal
    {
        buttonImage.color = normalColor;
        requestActive = false;
    }
}