using System.Collections.Generic;
using WireframeCrawler.Domain.Commands;
using WireframeCrawler.Domain.Models;

namespace WireframeCrawler.Domain;

public class CommandParser
{
    private readonly Dictionary<string, ICommand> _commandRegistry;

    public CommandParser()
    {
        // El diccionario busca comandos en tiempo constante O(1)
        _commandRegistry = new Dictionary<string, ICommand>
        {
            { "forward", new MoveCommand(1) },
            { "w", new MoveCommand(1) },
            { "backward", new MoveCommand(-1) },
            { "s", new MoveCommand(-1) },
            { "left", new RotateCommand(-1) },
            { "a", new RotateCommand(-1) },
            { "right", new RotateCommand(1) },
            { "d", new RotateCommand(1) },
            { "interact", new InteractCommand() }

        };
    }

    public string ParseAndExecute(string rawInput, Player player, GameMap map)
    {
        if (string.IsNullOrWhiteSpace(rawInput)) return "";

        // Separamos el comando de sus posibles argumentos (ej: "scan sector")
        string[] parts = rawInput.Trim().ToLower().Split(' ');
        string verb = parts[0];
        
        // TryGetValue es extremadamente rápido y no lanza excepciones si falla
        if (_commandRegistry.TryGetValue(verb, out ICommand? command))
        {
            // Pasamos el resto de las palabras como argumentos
            return command.Execute(player, map, parts[1..]); 
        }

        return $"ERROR: Comando '{verb}' no reconocido en los sistemas.";
    }
}