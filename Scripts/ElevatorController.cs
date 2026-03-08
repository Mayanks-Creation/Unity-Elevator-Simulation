using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float speed = 2f;
    public float floorHeight = 3f;

    public int currentFloor = 0;

    private List<int> requests = new List<int>();

    private bool isMoving = false;
    private bool doorBusy = false;

    private int direction = 0; // 1 = up, -1 = down, 0 = idle

    public Animator animator;
    public float doorOpenTime = 2f;

    public System.Action<int> OnFloorServiced;

    void Update()
    {
        if (!isMoving && !doorBusy && requests.Count > 0)
        {
            int targetFloor = GetNextFloor();
            requests.Remove(targetFloor);

            if (targetFloor > currentFloor)
                direction = 1;
            else if (targetFloor < currentFloor)
                direction = -1;

            StartCoroutine(MoveToFloor(targetFloor));
        }
    }

    int GetNextFloor()
    {
        int bestFloor = requests[0];
        int bestDistance = int.MaxValue;

        foreach (int floor in requests)
        {
            int distance = Mathf.Abs(floor - currentFloor);

            if (direction == 1 && floor >= currentFloor)
            {
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestFloor = floor;
                }
            }
            else if (direction == -1 && floor <= currentFloor)
            {
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestFloor = floor;
                }
            }
        }

        if (bestDistance == int.MaxValue)
        {
            bestFloor = requests[0];
            bestDistance = Mathf.Abs(bestFloor - currentFloor);

            foreach (int floor in requests)
            {
                int distance = Mathf.Abs(floor - currentFloor);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestFloor = floor;
                }
            }
        }

        return bestFloor;
    }

    IEnumerator MoveToFloor(int targetFloor)
    {
        isMoving = true;

        float targetY = targetFloor * floorHeight;

        while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(transform.position.x, targetY, transform.position.z),
                speed * Time.deltaTime
            );

            currentFloor = Mathf.RoundToInt(transform.position.y / floorHeight);

            yield return null;
        }

        transform.position = new Vector3(
            transform.position.x,
            targetY,
            transform.position.z
        );

        currentFloor = targetFloor;

        isMoving = false;

        yield return StartCoroutine(OpenDoorRoutine());

        direction = 0;

        OnFloorServiced?.Invoke(currentFloor);
    }

    IEnumerator OpenDoorRoutine()
    {
        doorBusy = true;

        if (animator != null)
            animator.SetTrigger("OpenDoor");

        yield return new WaitForSeconds(doorOpenTime);

        if (animator != null)
            animator.SetTrigger("CloseDoor");

        yield return new WaitForSeconds(doorOpenTime);

        doorBusy = false;
    }

    public void AddRequest(int floor)
    {
        if (!requests.Contains(floor))
            requests.Add(floor);
    }

    public int GetCurrentFloor()
    {
        return currentFloor;
    }

    public int GetDirection()
    {
        return direction;
    }

    public bool IsMoving()
    {
        return isMoving || doorBusy;
    }

    public bool IsMovingVertically()
    {
        return isMoving;
    }

    public void OpenDoorInstant()
    {
        StartCoroutine(OpenDoorInstantRoutine());
    }

    IEnumerator OpenDoorInstantRoutine()
    {
        yield return StartCoroutine(OpenDoorRoutine());
        OnFloorServiced?.Invoke(currentFloor);
    }
}