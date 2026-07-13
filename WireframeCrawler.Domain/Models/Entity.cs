namespace WireframeCrawler.Domain.Models;

public enum EntityType
{
    FoundObject,       // Se dibujará como una 'X'
    Passage,           // Se dibujará como un Círculo
    InteractableZone   // Se dibujará como un Cuadrado
}

public class Entity
{
    public int X { get; set; }
    public int Y { get; set; }
    public EntityType Type { get; set; }

    public Entity(int x, int y, EntityType type)
    {
        X = x; Y = y; Type = type;
    }
}