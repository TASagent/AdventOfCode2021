using AoCTools;
using System.Text;

const string inputFile = @"../../../../input23.txt";

Console.WriteLine("Day 23 - Amphipod");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

Dictionary<Point2D, Room> roomLookup = new Dictionary<Point2D, Room>();
Dictionary<Flavor, Room[]> targetRooms = new Dictionary<Flavor, Room[]>();
Dictionary<string, int> costMap = new Dictionary<string, int>();

AddRoom(new Room((0, 0), Flavor.Hallway));
AddRoom(new Room((1, 0), Flavor.Hallway));
AddRoom(new Room((2, 0), Flavor.Doorway));
AddRoom(new Room((3, 0), Flavor.Hallway));
AddRoom(new Room((4, 0), Flavor.Doorway));
AddRoom(new Room((5, 0), Flavor.Hallway));
AddRoom(new Room((6, 0), Flavor.Doorway));
AddRoom(new Room((7, 0), Flavor.Hallway));
AddRoom(new Room((8, 0), Flavor.Doorway));
AddRoom(new Room((9, 0), Flavor.Hallway));
AddRoom(new Room((10, 0), Flavor.Hallway));

AddRoom(new Room((2, 1), Flavor.A));
AddRoom(new Room((2, 2), Flavor.A));

AddRoom(new Room((4, 1), Flavor.B));
AddRoom(new Room((4, 2), Flavor.B));

AddRoom(new Room((6, 1), Flavor.C));
AddRoom(new Room((6, 2), Flavor.C));

AddRoom(new Room((8, 1), Flavor.D));
AddRoom(new Room((8, 2), Flavor.D));

for (int x = 0; x < 10; x++)
{
    Room leftRoom = roomLookup[(x, 0)];
    ConnectRooms(leftRoom, roomLookup[(x + 1, 0)]);

    if ((x > 0 && x < 10) && (x % 2 == 0))
    {
        ConnectRooms(leftRoom, roomLookup[(x, 1)]);
        ConnectRooms(roomLookup[(x, 1)], roomLookup[(x, 2)]);
    }
}

targetRooms[Flavor.A] = new[] { roomLookup[(2, 1)], roomLookup[(2, 2)] };
targetRooms[Flavor.B] = new[] { roomLookup[(4, 1)], roomLookup[(4, 2)] };
targetRooms[Flavor.C] = new[] { roomLookup[(6, 1)], roomLookup[(6, 2)] };
targetRooms[Flavor.D] = new[] { roomLookup[(8, 1)], roomLookup[(8, 2)] };


List<Amphipod> amphipods = new List<Amphipod>();

for (int sideRoom = 0; sideRoom < 4; sideRoom++)
{
    Amphipod first = Parse(lines[2][3 + 2 * sideRoom]);
    Amphipod second = Parse(lines[3][3 + 2 * sideRoom]);

    amphipods.Add(first);
    amphipods.Add(second);

    roomLookup[(2 + sideRoom * 2, 1)].Occupant = first;
    first.CurrentRoom = roomLookup[(2 + sideRoom * 2, 1)];

    roomLookup[(2 + sideRoom * 2, 2)].Occupant = second;
    second.CurrentRoom = roomLookup[(2 + sideRoom * 2, 2)];
}

Console.WriteLine($"The answer is: {Solve()}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

//Inserting:
//    #D#C#B#A#
//    #D#B#A#C#

Flavor[] secondRow = new[] { Flavor.D, Flavor.C, Flavor.B, Flavor.A };
Flavor[] thirdRow = new[] { Flavor.D, Flavor.B, Flavor.A, Flavor.C };


AddRoom(new Room((2, 3), Flavor.A));
AddRoom(new Room((2, 4), Flavor.A));

AddRoom(new Room((4, 3), Flavor.B));
AddRoom(new Room((4, 4), Flavor.B));

AddRoom(new Room((6, 3), Flavor.C));
AddRoom(new Room((6, 4), Flavor.C));

AddRoom(new Room((8, 3), Flavor.D));
AddRoom(new Room((8, 4), Flavor.D));

for (int targetZone = 0; targetZone < 4; targetZone++)
{
    ConnectRooms(roomLookup[(2 + 2 * targetZone, 2)], roomLookup[(2 + 2 * targetZone, 3)]);
    ConnectRooms(roomLookup[(2 + 2 * targetZone, 3)], roomLookup[(2 + 2 * targetZone, 4)]);
}


targetRooms[Flavor.A] = new[] { roomLookup[(2, 1)], roomLookup[(2, 2)], roomLookup[(2, 3)], roomLookup[(2, 4)] };
targetRooms[Flavor.B] = new[] { roomLookup[(4, 1)], roomLookup[(4, 2)], roomLookup[(4, 3)], roomLookup[(4, 4)] };
targetRooms[Flavor.C] = new[] { roomLookup[(6, 1)], roomLookup[(6, 2)], roomLookup[(6, 3)], roomLookup[(6, 4)] };
targetRooms[Flavor.D] = new[] { roomLookup[(8, 1)], roomLookup[(8, 2)], roomLookup[(8, 3)], roomLookup[(8, 4)] };

foreach (Room room in roomLookup.Values)
{
    room.Occupant = null;
}


amphipods.Clear();

for (int sideRoom = 0; sideRoom < 4; sideRoom++)
{
    Amphipod[] newAmphipods = new[]
    {
        Parse(lines[2][3 + 2 * sideRoom]),
        new Amphipod(secondRow[sideRoom]),
        new Amphipod(thirdRow[sideRoom]),
        Parse(lines[3][3 + 2 * sideRoom])
    };

    for (int i = 0; i < 4; i++)
    {
        amphipods.Add(newAmphipods[i]);

        roomLookup[(2 + sideRoom * 2, i + 1)].Occupant = newAmphipods[i];
        newAmphipods[i].CurrentRoom = roomLookup[(2 + sideRoom * 2, i + 1)];
    }
}

Console.WriteLine($"The answer is: {Solve()}");

Console.WriteLine();
Console.ReadKey();

string PrintState()
{
    StringBuilder builder = new StringBuilder();
    
    for (int x = 0; x < 11; x++)
    {
        builder.Append(PrintRoom(roomLookup[(x, 0)]));
    }

    for (int depth = 0; depth < targetRooms[0].Length; depth++)
    {
        for (int sideRoom = 0; sideRoom < 4; sideRoom++)
        {
            builder.Append(PrintRoom(targetRooms[(Flavor)sideRoom][depth]));
        }
    }

    return builder.ToString();
}

static char PrintRoom(Room room)
{
    if (room.Occupant is not null)
    {
        return (char)(room.Occupant.flavor + 'A');
    }
    else
    {
        return '.';
    }
}


int Solve()
{
    string boardState = PrintState();

    if (costMap.ContainsKey(boardState))
    {
        return costMap[boardState];
    }

    //Check for completion HERE
    if (amphipods.All(x => x.CurrentRoom.flavor == x.flavor))
    {
        //Done
        return 0;
    }

    int bestSolution = int.MaxValue;

    foreach (Amphipod amphipod in MovableAmphipods())
    {
        Room initialRoom = amphipod.CurrentRoom;

        foreach (Room destinationRoom in ValidDestinations(amphipod))
        {
            int moveCost = amphipod.Move(destinationRoom, false);
            int solution = Solve();

            if (solution != int.MaxValue)
            {
                bestSolution = Math.Min(bestSolution, moveCost + solution);
            }

            //Roll back move
            amphipod.Move(initialRoom, true);
        }
    }

    costMap[boardState] = bestSolution;

    return bestSolution;
}


IEnumerable<Amphipod> MovableAmphipods()
{
    //Initial check for easy free choices
    foreach (Amphipod amphipod in amphipods)
    {
        if (amphipod.CurrentRoom.coordinates.y == 0 || !amphipod.HasMoved)
        {
            for (int depth = targetRooms[amphipod.flavor].Length - 1; depth >= 0; depth--)
            {
                if (targetRooms[amphipod.flavor][depth].Occupant is null)
                {
                    if (CanPathTo(amphipod.CurrentRoom, targetRooms[amphipod.flavor][depth]))
                    {
                        yield return amphipod;
                        yield break;
                    }
                    else
                    {
                        break;
                    }
                }
                else if (targetRooms[amphipod.flavor][depth].Occupant.flavor != amphipod.flavor)
                {
                    break;
                }
            }
        }
    }

    //Proper Scan
    foreach (Amphipod amphipod in amphipods)
    {
        if (amphipod.CurrentRoom.coordinates.y > 0)
        {
            //Amphipod in a Target Room
            if (!amphipod.HasMoved &&
                CanPathTo(amphipod.CurrentRoom, roomLookup[(amphipod.CurrentRoom.coordinates.x, 0)]))
            {
                yield return amphipod;
            }
        }
        else
        {
            //Amphipod in the Hallway
            if (CanPathTo(amphipod.CurrentRoom, targetRooms[amphipod.flavor][0]))
            {
                yield return amphipod;
            }
        }
    }
}

bool CanPathTo(Room startingRoom, Room endingRoom)
{
    Point2D position = startingRoom.coordinates;

    while (position.y > 0)
    {
        position -= Point2D.YAxis;

        if (roomLookup[position].Occupant is not null)
        {
            return false;
        }

        if (position == endingRoom.coordinates)
        {
            return true;
        }
    }

    Point2D diff;

    if (position.x < endingRoom.coordinates.x)
    {
        diff = Point2D.XAxis;
    }
    else
    {
        diff = -Point2D.XAxis;
    }

    while (position.x != endingRoom.coordinates.x)
    {
        position += diff;

        if (roomLookup[position].Occupant is not null)
        {
            return false;
        }

        if (position == endingRoom.coordinates)
        {
            return true;
        }
    }

    while (position != endingRoom.coordinates)
    {
        position += Point2D.YAxis;

        if (roomLookup[position].Occupant is not null)
        {
            return false;
        }

        if (position == endingRoom.coordinates)
        {
            return true;
        }
    }

    return true;
}


IEnumerable<Room> ValidDestinations(Amphipod amphipod)
{
    if (amphipod.CurrentRoom.coordinates.y > 0)
    {
        //If Amphipod is in a side-room
        Point2D position = (amphipod.CurrentRoom.coordinates.x + 1, 0);
        while (roomLookup.ContainsKey(position) && roomLookup[position].Occupant is null)
        {
            if (roomLookup[position].flavor == Flavor.Hallway)
            {
                yield return roomLookup[position];
            }
            position += Point2D.XAxis;
        }

        position = (amphipod.CurrentRoom.coordinates.x - 1, 0);
        while (roomLookup.ContainsKey(position) && roomLookup[position].Occupant is null)
        {
            if (roomLookup[position].flavor == Flavor.Hallway)
            {
                yield return roomLookup[position];
            }
            position -= Point2D.XAxis;
        }
    }
    else
    {
        //Amphipod is in the hallway
        if (targetRooms[amphipod.flavor][0].Occupant is not null ||
            !CanPathTo(amphipod.CurrentRoom, targetRooms[amphipod.flavor][0]))
        {
            yield break;
        }

        for (int i = targetRooms[amphipod.flavor].Length - 1; i >= 0; i--)
        {
            if (targetRooms[amphipod.flavor][i].Occupant is null)
            {
                yield return targetRooms[amphipod.flavor][i];
                yield break;
            }
        }
    }
}

void AddRoom(Room room)
{
    roomLookup.Add(room.coordinates, room);
}

void ConnectRooms(Room room1, Room room2)
{
    room1.connectedRooms.Add(room2);
    room2.connectedRooms.Add(room1);
}

Amphipod Parse(char input)
{
    switch (input)
    {
        case 'A': return new Amphipod(Flavor.A);
        case 'B': return new Amphipod(Flavor.B);
        case 'C': return new Amphipod(Flavor.C);
        case 'D': return new Amphipod(Flavor.D);

        default: throw new Exception();
    }
}

class Room
{
    public readonly Point2D coordinates;

    public readonly List<Room> connectedRooms = new List<Room>();

    public readonly Flavor flavor;
    public Amphipod Occupant { get; set; } = null;

    public Room(in Point2D coordinates, Flavor flavor)
    {
        this.coordinates = coordinates;
        this.flavor = flavor;
    }

    public override string ToString() => $"{coordinates}: {(Occupant?.flavor.ToString() ?? "Empty")}";
}

class Amphipod
{
    public bool HasMoved { get; set; } = false;

    public readonly Flavor flavor;

    public Room CurrentRoom { get; set; } = null;

    public Amphipod(Flavor flavor)
    {
        this.flavor = flavor;
    }

    public int GetMovementCost() => flavor switch
    {
        Flavor.A => 1,
        Flavor.B => 10,
        Flavor.C => 100,
        Flavor.D => 1000,
        _ => throw new Exception()
    };

    /// <summary>
    /// Move the amphipod to the room, return the cost
    /// </summary>
    public int Move(Room destinationRoom, bool reverse)
    {
        if (reverse)
        {
            if (IsTargetRoom(destinationRoom.flavor))
            {
                HasMoved = false;
            }
        }
        else
        {
            HasMoved = true;
        }

        int distance = Math.Abs(CurrentRoom.coordinates.x - destinationRoom.coordinates.x) +
            CurrentRoom.coordinates.y +
            destinationRoom.coordinates.y;

        CurrentRoom.Occupant = null;

        CurrentRoom = destinationRoom;
        destinationRoom.Occupant = this;

        return distance * GetMovementCost();
    }

    public static bool IsTargetRoom(Flavor flavor) => flavor switch
    {
        Flavor.A or Flavor.B or Flavor.C or Flavor.D => true,
        _ => false
    };

    public override string ToString() => $"{flavor}: {CurrentRoom.coordinates}";
}

enum Flavor
{
    A = 0,
    B,
    C,
    D,
    Doorway,
    Hallway,
    MAX
}