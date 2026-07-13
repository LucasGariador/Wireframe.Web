namespace WireframeCrawler.Domain.Commands;
using WireframeCrawler.Domain.Models;

public class InteractCommand : ICommand
{
    public string Execute(Player player, GameMap map, string[] arguments)
    {
        // 1. Buscamos directamente en la posición actual de la nave
        Entity? currentEntity = map.GetEntityAt(player.X, player.Y);

        if (currentEntity == null)
        {
            return "Escáner negativo. No hay objetivos interactuables en tus coordenadas actuales.";
        }

        // 2. Resolvemos la interacción sobre lo que estamos pisando
        return currentEntity.Type switch
        {
            EntityType.FoundObject => HandleFoundObject(currentEntity, map),
            EntityType.Passage => HandlePassage(currentEntity),
            EntityType.InteractableZone => HandleInteractableZone(currentEntity),
            _ => "Entidad desconocida. Interacción fallida."
        };
    }

    private string HandleFoundObject(Entity item, GameMap map)
    {
        map.Entities.Remove(item); // Lo quitamos del mapa
        return "[ÉXITO] Has recogido un registro de datos antiguo. Entidad borrada del radar.";
    }

    private string HandlePassage(Entity door)
    {
        return "[SISTEMA] Iniciando secuencia de descenso por la escotilla... (Nivel no implementado).";
    }

    private string HandleInteractableZone(Entity terminal)
    {
        // Aquí podrías validar si el jugador tiene cierto objeto en el inventario antes de funcionar
        return "[TERMINAL] Conexión establecida. Introduce el código de anulación para continuar.";
    }
}