namespace WireframeCrawler.Domain.Commands;
using WireframeCrawler.Domain.Models;

public class MoveCommand : ICommand
{
    private readonly int _steps;

    public MoveCommand(int steps)
    {
        _steps = steps;
    }

    public string Execute(Player player, GameMap map, string[] arguments)
    {
        int fwdX = 0, fwdY = 0;
        
        switch (player.Facing)
        {
            case Direction.North: fwdY = -1; break;
            case Direction.East: fwdX = 1; break;
            case Direction.South: fwdY = 1; break;
            case Direction.West: fwdX = -1; break;
        }

        int targetX = player.X + (fwdX * _steps);
        int targetY = player.Y + (fwdY * _steps);

        if (map.IsWall(targetX, targetY))
        {
            return "[ALERTA] Colisión inminente detectada. Movimiento abortado.";
        }
        
        player.X = targetX;
        player.Y = targetY;
        return _steps > 0 ? "Avanzando coordenadas." : "Retrocediendo coordenadas.";
    }
}