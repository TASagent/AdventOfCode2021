using AoCTools;

const string inputFile = @"../../../../input23.txt";

Console.WriteLine("Day 23 - Amphipod");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

Dictionary<Point2D, Room> roomLookup = new Dictionary<Point2D, Room>();
Dictionary<Flavor, Room[]> targetRooms = new Dictionary<Flavor, Room[]>();

AddRoom(new Room((0, 0), Flavor.Room));
AddRoom(new Room((1, 0), Flavor.Room));
AddRoom(new Room((2, 0), Flavor.Doorway));
AddRoom(new Room((3, 0), Flavor.Room));
AddRoom(new Room((4, 0), Flavor.Doorway));
AddRoom(new Room((5, 0), Flavor.Room));
AddRoom(new Room((6, 0), Flavor.Doorway));
AddRoom(new Room((7, 0), Flavor.Room));
AddRoom(new Room((8, 0), Flavor.Doorway));
AddRoom(new Room((9, 0), Flavor.Room));
AddRoom(new Room((10, 0), Flavor.Room));

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

targetRooms.Add(Flavor.A, new[] { roomLookup[(2, 1)], roomLookup[(2, 2)] });
targetRooms.Add(Flavor.B, new[] { roomLookup[(4, 1)], roomLookup[(4, 2)] });
targetRooms.Add(Flavor.C, new[] { roomLookup[(6, 1)], roomLookup[(6, 2)] });
targetRooms.Add(Flavor.D, new[] { roomLookup[(8, 1)], roomLookup[(8, 2)] });


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

int bestSolution = int.MaxValue;

Solve(0);

Console.WriteLine($"The answer is: {bestSolution}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();
int output2 = 0;




Console.WriteLine($"The answer is: {output2}");



Console.WriteLine();
Console.ReadKey();



void Solve(int cumulativeCost)
{
    if (cumulativeCost >= bestSolution)
    {
        return;
    }

    //Check for completion HERE
    if (amphipods.All(x => x.CurrentRoom.flavor == x.flavor))
    {
        //Done
        bestSolution = cumulativeCost;
        return;
    }

    List<Amphipod> movableAmphipods = MovableAmphipods().ToList();

    foreach (Amphipod amphipod in movableAmphipods)
    {
        Room initialRoom = amphipod.CurrentRoom;

        List<Room> destinationRooms = ValidDestinations(amphipod).ToList();
        foreach (Room destinationRoom in destinationRooms)
        {
            Solve(cumulativeCost + amphipod.Move(destinationRoom, false));
            amphipod.Move(initialRoom, true);
        }
    }
}

IEnumerable<Amphipod> MovableAmphipods()
{
    Amphipod immediateTarget = amphipods.FirstOrDefault(x => x.CurrentRoom.coordinates.y == 0 &&
        CanPathTo(x.CurrentRoom, targetRooms[x.flavor][0]) &&
        targetRooms[x.flavor][0].Occupant is null &&
        (targetRooms[x.flavor][1].Occupant is null || targetRooms[x.flavor][1].Occupant.flavor == x.flavor));

    if (immediateTarget is not null)
    {
        yield return immediateTarget;
        yield break;
    }

    foreach (Amphipod amphipod in amphipods)
    {
        if (amphipod.CurrentRoom.coordinates.y == 2)
        {
            if (!amphipod.HasMoved &&
                roomLookup[amphipod.CurrentRoom.coordinates - Point2D.YAxis].Occupant is null)
            {
                yield return amphipod;
            }
        }
        else if (amphipod.CurrentRoom.coordinates.y == 1)
        {
            if (!amphipod.HasMoved)
            {
                yield return amphipod;
            }
        }
        else
        {
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
        Point2D position = (amphipod.CurrentRoom.coordinates.x + 1, 0);
        while (roomLookup.ContainsKey(position) && roomLookup[position].Occupant is null)
        {
            if (roomLookup[position].flavor == Flavor.Room)
            {
                yield return roomLookup[position];
            }
            position += Point2D.XAxis;
        }

        position = (amphipod.CurrentRoom.coordinates.x - 1, 0);
        while (roomLookup.ContainsKey(position) && roomLookup[position].Occupant is null)
        {
            if (roomLookup[position].flavor == Flavor.Room)
            {
                yield return roomLookup[position];
            }
            position -= Point2D.XAxis;
        }
    }
    else
    {
        if (targetRooms[amphipod.flavor][0].Occupant is not null ||
            !CanPathTo(amphipod.CurrentRoom, targetRooms[amphipod.flavor][0]))
        {
            yield break;
        }

        if (targetRooms[amphipod.flavor][1].Occupant is not null)
        {
            yield return targetRooms[amphipod.flavor][0];
        }
        else
        {
            yield return targetRooms[amphipod.flavor][1];
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
            if (CurrentRoom.flavor == Flavor.Room)
            {
                HasMoved = false;
            }
        }
        else
        {
            HasMoved = true;
        }

        int distance = (CurrentRoom.coordinates - destinationRoom.coordinates).TaxiCabLength;

        CurrentRoom.Occupant = null;

        CurrentRoom = destinationRoom;
        destinationRoom.Occupant = this;

        return distance * GetMovementCost();
    }

    public override string ToString() => $"{flavor}: {CurrentRoom.coordinates}";
}

enum Flavor
{
    A = 0,
    B,
    C,
    D,
    Doorway,
    Room,
    MAX
}