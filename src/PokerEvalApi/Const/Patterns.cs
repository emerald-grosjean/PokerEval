namespace PokerEvalApi.Const;

public class Patterns
{
    public const string CardPattern = @"[2-9TJQKA][shdc]";

    public const string BoardPattern = $"({CardPattern})" + "{1,5}";

    public const string PocketPattern = $"({CardPattern})" + "{2}";

    public const string WinRatePattern = @"^(0\.\d{4}|1\.0{4})$";
}
