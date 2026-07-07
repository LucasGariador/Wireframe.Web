using System.Collections.Generic;
using WireframeCrawler.Domain.Models;

namespace WireframeCrawler.Domain;

public class Engine
{
    private readonly int _screenWidth;
    private readonly int _screenHeight;
    private readonly double _fovScale = 400.0;

    public Engine(int screenWidth, int screenHeight)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public List<Quad2D> RenderView(GameMap map, Player player)
    {
        var lines = new List<Quad2D>();
        
        int maxDepth = 4; // Cuántas casillas hacia adelante dibujamos
        int viewWidth = 2; // Cuántas casillas hacia la izquierda/derecha dibujamos

        // 1. Obtener vectores de dirección según hacia dónde mira el jugador
        (int dirX, int dirY) forward = GetForwardVector(player.Facing);
        (int dirX, int dirY) right = (-forward.dirY, forward.dirX); // Vector perpendicular a la derecha

        // 2. Escanear el mapa de atrás hacia adelante (algoritmo del pintor conceptual)
        for (int z = maxDepth; z >= 0; z--)
        {
            for (int x = -viewWidth; x <= viewWidth; x++)
            {
                // Traducir las coordenadas locales (relativas a la cámara) a coordenadas absolutas del mapa
                int worldX = player.X + (forward.dirX * z) + (right.dirX * x);
                int worldY = player.Y + (forward.dirY * z) + (right.dirY * x);

                if (map.IsWall(worldX, worldY))
                {
                    // Si hay una pared, generamos las líneas del "cubo wireframe"
                    lines.AddRange(GenerateCubeFaces(x, z));
                }
            }
        }

        return lines;
    }

    private (int dirX, int dirY) GetForwardVector(Direction facing)
    {
        return facing switch
        {
            Direction.North => (0, -1),
            Direction.East  => (1, 0),
            Direction.South => (0, 1),
            Direction.West  => (-1, 0),
            _ => (0, -1)
        };
    }

private List<Quad2D> GenerateCubeFaces(double localX, double localZ)
    {
        var quads = new List<Quad2D>();

        // EL SECRETO 1: Desplazamiento de la cámara (Z-Offset).
        // Al sumar 1.0, colocamos nuestros "ojos" en el borde trasero de la casilla actual.
        // Así, las paredes laterales de nuestra propia casilla quedan por delante de la cámara y se dibujan.
        double zOffset = 1.0; 

        double left = localX - 0.5;
        double right = localX + 0.5;
        double top = -0.5;    
        double bottom = 0.5;
        
        double front = localZ + zOffset - 0.5;
        double back = localZ + zOffset + 0.5;

        // Si el bloque entero está a nuestra espalda, lo ignoramos por completo
        if (back <= 0.1) return quads;

        // Clip de seguridad: Si la parte frontal de la pared atraviesa la cámara, 
        // la frenamos a 0.1 unidades para que no divida por cero y la línea se fugue hacia el borde de la pantalla.
        double safeFront = front < 0.1 ? 0.1 : front;

        var vFrontTopLeft = Project(left, top, safeFront);
        var vFrontTopRight = Project(right, top, safeFront);
        var vFrontBottomLeft = Project(left, bottom, safeFront);
        var vFrontBottomRight = Project(right, bottom, safeFront);
        
        var vBackTopLeft = Project(left, top, back);
        var vBackTopRight = Project(right, top, back);
        var vBackBottomLeft = Project(left, bottom, back);
        var vBackBottomRight = Project(right, bottom, back);

        // EL SECRETO 2: Dejamos de dibujar el cubo entero y solo dibujamos los PLANOS visibles.
        
        // 1. Pared Derecha del pasillo (Si el bloque está a nuestra derecha, solo vemos su cara Izquierda)
        if (localX > 0) {
            quads.Add(new Quad2D(vBackTopLeft.X, vBackTopLeft.Y, vFrontTopLeft.X, vFrontTopLeft.Y, vFrontBottomLeft.X, vFrontBottomLeft.Y, vBackBottomLeft.X, vBackBottomLeft.Y)); 
        }
        // 2. Pared Izquierda del pasillo (Si el bloque está a nuestra izquierda, solo vemos su cara Derecha)
        else if (localX < 0) {
            quads.Add(new Quad2D(vFrontTopRight.X, vFrontTopRight.Y, vBackTopRight.X, vBackTopRight.Y, vBackBottomRight.X, vBackBottomRight.Y, vFrontBottomRight.X, vFrontBottomRight.Y)); 
        }

        // 3. Pared Frontal (Solo la dibujamos si realmente está frente a nosotros)
        if (front >= 0.1) {
            quads.Add(new Quad2D(vFrontTopLeft.X, vFrontTopLeft.Y, vFrontTopRight.X, vFrontTopRight.Y, vFrontBottomRight.X, vFrontBottomRight.Y, vFrontBottomLeft.X, vFrontBottomLeft.Y));
        }

        return quads;
    }

    private (double X, double Y) Project(double x, double y, double z)
    {
        if (z <= 0) z = 0.01; 

        double screenX = (x / z) * _fovScale + (_screenWidth / 2.0);
        double screenY = (y / z) * _fovScale + (_screenHeight / 2.0);

        return (screenX, screenY);
    }
}