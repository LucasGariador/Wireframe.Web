using System.Collections.Generic;

namespace WireframeCrawler.Domain.Models;

public class GameMap
{
    public int[,] Grid { get; private set; }
    
    // Lista donde guardamos todos los objetos del nivel
    public List<Entity> Entities { get; private set; } 
    
    public int Width => Grid.GetLength(1);
    public int Height => Grid.GetLength(0);

    public GameMap()
    {
        Grid = new int[,]
        {
            { 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1 }
        };

        // Colocamos entidades de prueba con los nuevos tipos
        Entities = new List<Entity>
        {
            new Entity(1, 1, EntityType.FoundObject),       // La 'X' arriba a la izquierda
            new Entity(3, 1, EntityType.InteractableZone),  // El Cuadrado arriba a la derecha
            new Entity(2, 2, EntityType.Passage)            // El Círculo en el centro
        };
    }

    public bool IsWall(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return true;
        return Grid[y, x] == 1; 
    }

    public Entity? GetEntityAt(int x, int y)
    {
        foreach (var entity in Entities)
        {
            if (entity.X == x && entity.Y == y)
            {
                return entity;
            }
        }
        return null;
    }
}