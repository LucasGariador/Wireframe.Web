namespace WireframeCrawler.Domain.Models;

public class GameMap
{
    // Una matriz: 1 = Pared, 0 = Pasillo
    public int[,] Grid { get; private set; }
    
    public int Width => Grid.GetLength(0);
    public int Height => Grid.GetLength(1);

    public GameMap()
    {
        // Un mapa hardcodeado de 5x5 para empezar a probar
        Grid = new int[,]
        {
            { 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 0, 1, 0, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1 }
        };
    }

    public bool IsWall(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height) return true; // Fuera del mapa es pared
        return Grid[x, y] == 1;
    }
}