using System.ComponentModel.DataAnnotations;

namespace PokerEvalApi.Models;

public class PocketModel
{
    [RegularExpression(Const.Patterns.PocketPattern)]
    public string Pocket { get; set; }
}
