namespace WireframeCrawler.Domain.Models;

public enum Direction { North, East, South, West }

public class Player
{
    public int X { get; set; }
    public int Y { get; set; }
    public Direction Facing { get; set; }

    public Player(int startX, int startY, Direction startFacing)
    {
        X = startX;
        Y = startY;
        Facing = startFacing;
    }
}