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
        
        // 1. Efectuamos el movimiento a la nueva casilla
        player.X = targetX;
        player.Y = targetY;
        
        // 2. Preparamos el mensaje base de confirmación de movimiento
        string response = _steps > 0 ? "Avanzando coordenadas." : "Retrocediendo coordenadas.";

        // 3. Revisamos qué hay debajo de nosotros en la nueva posición
        Entity? steppedEntity = map.GetEntityAt(player.X, player.Y);

        if (steppedEntity != null)
        {
            // El trigger solo informa, no interactúa automáticamente
            response += steppedEntity.Type switch
            {
                EntityType.FoundObject => "\n[SENSOR] Detectas un objeto en el suelo. Usa 'interact' para examinarlo.",
                EntityType.Passage => "\n[SENSOR] Escotilla o pasaje detectado en estas coordenadas.",
                EntityType.InteractableZone => "\n[SENSOR] Zona interactuable detectada (Requiere hardware compatible).",
                _ => "\n[SENSOR] Anomalía bajo la nave."
            };
        }

        return response;
    }
}