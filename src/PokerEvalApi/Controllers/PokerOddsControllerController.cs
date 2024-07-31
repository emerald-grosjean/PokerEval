using HoldemHand;
using Microsoft.AspNetCore.Mvc;

namespace PokerEvalApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PokerOddsController() : ControllerBase
{
    private const int RateDigits = 4;

    [HttpGet]
    public IEnumerable<double> Get([FromQuery] string[] pockets, [FromQuery] string? board)
    {
        var wins = new long[pockets.Length];
        var losses = new long[pockets.Length];
        var ties = new long[pockets.Length];
        long totalHands = 0;

        Hand.HandOdds(
            pockets: pockets,
            board: board,
            dead: string.Empty,
            wins: wins,
            losses: losses,
            ties: ties,
            totalHands: ref totalHands);

        var items = new List<double>();

        for (int i = 0; i < pockets.Length; i++)
        {
            var winRate = Math.Round((wins[i] + ties[i] / (double)pockets.Length) / totalHands, RateDigits);
            items.Add(winRate);
        }

        return items;
    }
}
