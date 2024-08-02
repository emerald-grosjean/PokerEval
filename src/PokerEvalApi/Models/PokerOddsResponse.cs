using System.ComponentModel.DataAnnotations;

namespace PokerEvalApi.Models;

public class PokerOddsResponse
{
    public required IEnumerable<PokerOddsResponseItem> Pockets { get; set; }

    [RegularExpression(Const.Patterns.BoardPattern)]
    public string? Board { get; set; }
}

public class PokerOddsResponseItem
{
    [RegularExpression(Const.Patterns.PocketPattern)]
    public required string Pocket { get; set; }

    public double WinRate { get; set; }
}
