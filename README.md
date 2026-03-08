Unity-Elevator-Simulation
Elevator Simulation Game

Unity Version
Unity 6.3 LTS

Features
- Multi elevator system
- Direction aware dispatch
- Nearest elevator selection
- Door open / close animations
- Request queue handling
- UI floor panel

Dispatch Logic
1. Idle closest elevator
2. Moving elevator already heading toward request
3. Fallback closest elevator

Controls
Press UI floor buttons to call elevators.

Elevator Dispatch Algorithm

The system manages multiple elevators and assigns floor requests using a priority-based
selection algorithm.

Selection Priority:
1. Idle closest elevator
   - If an elevator is idle, the system chooses the one closest to the requested floor.

2. Elevator moving toward the request
   - If an elevator is already moving in the direction of the requested floor
     and will pass that floor, it is assigned the request.

3. Fallback closest elevator
   - If no idle or suitable moving elevator is found, the nearest elevator
     is selected regardless of direction.

Additional Behaviour:
- Elevators wait for door open/close animations to finish before moving.
- Each elevator maintains its own request queue.
- Direction is updated dynamically during movement to ensure correct request assignment.
