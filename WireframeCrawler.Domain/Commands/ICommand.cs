namespace WireframeCrawler.Domain.Commands;
using WireframeCrawler.Domain.Models;

public interface ICommand
{
    string Execute(Player player, GameMap map, string[] arguments);
}