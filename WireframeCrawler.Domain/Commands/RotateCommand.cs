namespace WireframeCrawler.Domain.Commands;
using WireframeCrawler.Domain.Models;

public class RotateCommand : ICommand
{
    private readonly int _direction; // 1 derecha, -1 izquierda

    public RotateCommand(int direction)
    {
        _direction = direction;
    }

    public string Execute(Player player, GameMap map, string[] arguments)
    {
        int currentFacing = (int)player.Facing;
        player.Facing = (Direction)((currentFacing + _direction + 4) % 4);
        
        return _direction > 0 ? "Girando a estribor (Derecha)." : "Girando a babor (Izquierda).";
    }
}