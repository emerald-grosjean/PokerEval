using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PokerEvalApi.Models;

public class PokerOddsRequest : IValidatableObject
{
    public required List<PokerOddsRequestItem> Pockets { get; set; }

    [RegularExpression(Const.Patterns.BoardPattern)]
    public string? Board { get; set; }

    private const string ErrorPrefix = "Invalid";

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Pockets.Count < 2)
        {
            yield return new ValidationResult($"{ErrorPrefix} Two or more {nameof(Pockets)} are required.");
        }

        foreach (var pocket in Pockets)
        {
            if (!Regex.IsMatch(pocket.Pocket, Const.Patterns.PocketPattern))
            {
                yield return new ValidationResult($"{nameof(Pockets)} contains not {Const.Patterns.PocketPattern} pattern.");
                break;
            }

            if (ConstainsSameCard(pocket.Pocket))
            {
                yield return new ValidationResult($"{nameof(Pockets)} contains same card.");
                break;
            }
        }

        if (!string.IsNullOrEmpty(Board))
        {
            if (ConstainsSameCard(Board))
            {
                yield return new ValidationResult($"{nameof(Pockets)} contains same card.");
            }
        }
    }

    bool ConstainsSameCard(string pocket)
    {
        if (pocket.Length == 0) return false;

        var cards = SplitCard(pocket);

        if (cards.Count() < 2) return false;

        return cards.GroupBy(x => x).Any(g => g.Count() > 1);
    }

    IEnumerable<string> SplitCard(string pocket)
    {
        if (pocket.Length == 0) return [];

        const int chunkSize = 2;

        return pocket
            .Select((c, i) => i % chunkSize == 0 ? pocket.Substring(i, Math.Min(chunkSize, pocket.Length - i)) : null)
            .Where(s => s != null)
            .ToArray();
    }
}


public class PokerOddsRequestItem
{
    [RegularExpression(Const.Patterns.PocketPattern)]
    public required string Pocket { get; set; }
}
