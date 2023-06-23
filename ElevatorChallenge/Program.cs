//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using System.Collections.Generic;
using System.Drawing;

Start:
Console.WriteLine("How tall is the building that this elevator will be in?");

int floor; string floorInput; Elevator elevator;
floorInput = Console.ReadLine();

if (Int32.TryParse(floorInput, out floor))
    elevator = new Elevator(floor);
else
{
    Console.WriteLine("Kindly input a valid floor number");
    Console.Beep();
    Thread.Sleep(2000);
    Console.Clear();
    goto Start;
}
string input = string.Empty;

//}

var maxPeople = 5;
var minPeople = 1;
var numOnBoard = 0;
var maxWeight = 150;



while (true)
{
    Console.WriteLine("\nElevator Console");
    Console.WriteLine("1. Select Floor");
    Console.WriteLine("2. Open Door");
    Console.WriteLine("3. Close Door");
    Console.WriteLine("4. Display Status");
    Console.WriteLine("5. Quit");

    Console.Write("Enter your choice: ");
    string choice = Console.ReadLine();

    if (choice == "1")
    {
        Console.WriteLine($"Please enter number of people to use the lift a minimum of {minPeople} and a maximum of {maxPeople} and a max weight of {maxWeight} Kilograms.");
        int peopleNumber = int.Parse(Console.ReadLine());
        if (peopleNumber < minPeople || peopleNumber > maxPeople)
        {
            Console.WriteLine($"The lift can only carry {minPeople} to {maxPeople} people");
           
            return;
        }

      


        Thread.Sleep(2000);
         Console.Write("Please press which floor you would like to go to: ");
         floor = int.Parse(Console.ReadLine());
        Thread.Sleep(2000);
        elevator.LiftPosition(floor);
        elevator.OpenDoor();
        elevator.CloseDoor();
        elevator.FloorPress(floor);
        numOnBoard = peopleNumber;
    }
    else if (choice == "2")
    {
        elevator.OpenDoor();
    }
    else if (choice == "3")
    {
        elevator.CloseDoor();
    }
    else if (choice == "4")
    {
        elevator.DisplayStatus(numOnBoard);
    }
    else if (choice == "5")
    {
        Console.WriteLine("Exiting the elevator console.");
        break;
    }
    else
    {
        Console.WriteLine("Invalid choice. Please try again.");
    }
}







public class Elevator
{

    private bool[] floorReady;
    public int CurrentFloor = 0;
    private int topfloor;
    public ElevatorStatus Status = ElevatorStatus.STOPPED;
    private bool isDoorOpen;

    public int LiftA = 0;
    public int LiftB = 0;
    public int LiftC = 0;
    public int LiftD = 0;


    public Elevator(int NumberOfFloors = 10)
    {
        floorReady = new bool[NumberOfFloors + 1];
        topfloor = NumberOfFloors;
    }

    private void Stop(int floor)
    {
        Status = ElevatorStatus.STOPPED;
        CurrentFloor = floor;
        floorReady[floor] = false;
        Console.WriteLine("Stopped at floor {0}", floor);
        OpenDoor();
        Thread.Sleep(2000);
        CloseDoor();
        Thread.Sleep(2000);
    }

    private void Descend(int floor)
    {
        Console.WriteLine("Going Down");
        for (int i = CurrentFloor; i >= 1; i--)
        {
            if (floorReady[i])
            {
                Stop(floor);
                break;
            }
            else
            {
                Console.WriteLine($"Floor number {i}");
                Thread.Sleep (1000);
                continue;
            }
                
        }

        Status = ElevatorStatus.STOPPED;
    }

    private void Ascend(int floor)
    {
        Console.WriteLine("Going Up");
        for (int i = CurrentFloor; i <= topfloor; i++)
        {
            if (floorReady[i])
            {
                Stop(floor);
                break;
            }
            else
            {
                Console.WriteLine($"Floor number {i}");
                Thread.Sleep (1000);
                continue;
            }
                
        }

        Status = ElevatorStatus.STOPPED;

    }

    void StayPut()
    {
        Console.WriteLine($"You are already on  floor  {CurrentFloor}.");
    }

    public void FloorPress(int floor)
    {
        if (floor > topfloor)
        {
            Console.WriteLine("We only have {0} floors", topfloor);
            return;
        }

        floorReady[floor] = true;

        switch (Status)
        {

            case ElevatorStatus.DOWN:
                Descend(floor);
                break;

            case ElevatorStatus.STOPPED:
                if (CurrentFloor < floor)
                    Ascend(floor);
                else if (CurrentFloor == floor)
                    StayPut();
                else
                    Descend(floor);
                break;

            case ElevatorStatus.UP:
                Ascend(floor);
                break;

            default:
                break;
        }


    }

    public void OpenDoor()
    {
        if (isDoorOpen)
        {
            Console.WriteLine("The door is already open.");
        }
        else
        {
            Console.WriteLine("Door Opening.");
            isDoorOpen = true;
        }
    }

    public void CloseDoor()
    {
        if (!isDoorOpen)
        {
            Console.WriteLine("The door is already closed.");
        }
        else
        {
            Console.WriteLine("Door Closing.");
            isDoorOpen = false;
        }
    }

    public void DisplayStatus(int numOnBoard)
    {
        Console.WriteLine($"Current floor: {CurrentFloor} and the lift is {Status} and has {numOnBoard} people on board");
        Console.WriteLine("Door status: " + (isDoorOpen ? "Open" : "Closed"));
    }

    public void LiftPosition(  int floorSelected)
    {
        var min = 0;
        var spaceInfo = new Dictionary<string, int>();
        spaceInfo.Add("LiftA", LiftA);
        spaceInfo.Add("LiftB", LiftB);
        spaceInfo.Add("LiftC", LiftC);
        spaceInfo.Add("LiftD", LiftD);


        var itemKey = "";
        var itemValue = 0;



        for (int index = 0; index < spaceInfo.Count; index++)
        {
            var item = spaceInfo.ElementAt(index);
             itemKey = item.Key;
             itemValue = item.Value;

            var tempResult = itemValue - floorSelected;
            if (tempResult < min)
                tempResult = min;

            if (itemValue >= 0)
            {
                min = tempResult;
            }
        }

        //var lift = spaceInfo[itemKey];

       //lift = itemValue + floorSelected;

        spaceInfo = spaceInfo.ToDictionary(kvp => kvp.Key, kvp => kvp.Value + floorSelected);

        spaceInfo.Remove(itemKey);
        spaceInfo.Add(itemKey, itemValue);

        Console.WriteLine($"Kindly use lift {itemKey}");

    }


    public enum ElevatorStatus
    {
        UP,
        STOPPED,
        DOWN
    }
}