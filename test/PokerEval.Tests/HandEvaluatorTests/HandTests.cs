namespace HoldemHand.Tests;

[TestClass()]
public class HandTests
{
    private const int DeckCards = 52;
    private const int BoardCards = 5;
    private const int RateDigits = 4;

    [TestMethod()]
    [DataRow(new string[] { "AsAh", "KsKh" }, "", "", new double[] { 0.8264, 0.1736 }, new long[] { 1410336, 292660 }, 9308)]
    [DataRow(new string[] { "AsAh", "KsKh" }, "AdAcKd", "", new double[] { 1.0000, 0.0000 }, new long[] { 990, 0 }, 0)]
    [DataRow(new string[] { "AsAh", "KsKh" }, "AdKdKc", "", new double[] { 0.0444, 0.9556 }, new long[] { 44, 946 }, 0)]
    [DataRow(new string[] { "AsAh", "KsKh" }, "KdKcQd", "", new double[] { 0.0010, 0.9990 }, new long[] { 1, 989 }, 0)]
    [DataRow(new string[] { "AsAh", "KsKh" }, "", "Ad", new double[] { 0.8084, 0.1916 }, new long[] { 1235829, 289688 }, 8422)]
    public void HandOddsTest(string[] pockets, string board, string dead, double[] expectedWinRates, long[] expectedWins, long expectedTie)
    {
        var wins = new long[pockets.Length];
        var losses = new long[pockets.Length];
        var ties = new long[pockets.Length];
        long totalHands = 0;

        Hand.HandOdds(
            pockets: pockets,
            board: board,
            dead: dead,
            wins: wins,
            losses: losses,
            ties: ties,
            totalHands: ref totalHands);

        var expectedTotalHands = Comb(DeckCards - (pockets.Length * 2) - CardLengthFromSHDC(board + dead), BoardCards - CardLengthFromSHDC(board));

        Assert.AreEqual(expectedTotalHands, totalHands);
        Assert.AreEqual(expectedTotalHands, wins.Sum() + ties.First());

        CollectionAssert.AreEqual(expectedWins, wins);

        Assert.IsTrue(ties.All(x => x == expectedTie));

        for (int i = 0; i < pockets.Length; i++)
        {
            var winRate = Math.Round((wins[i] + ties[i] / (double)pockets.Length) / totalHands, RateDigits);

            Assert.AreEqual(expectedWinRates[i], winRate);
        }
    }

    [TestMethod()]
    [DataRow(new string[] { "AsAh", "KsKh" }, "", new double[] { 0.8264, 0.1736 })]
    [DataRow(new string[] { "AsAh", "2s7h" }, "", new double[] { 0.8898, 0.1102 })]
    [DataRow(new string[] { "AsAh", "7d8d" }, "", new double[] { 0.7698, 0.2302 })]
    [DataRow(new string[] { "KhTd", "Jc9s" }, "", new double[] { 0.6276, 0.3724 })]
    [DataRow(new string[] { "KhTd", "9c6s" }, "", new double[] { 0.6527, 0.3473 })]
    [DataRow(new string[] { "KhTd", "6s7s" }, "", new double[] { 0.5874, 0.4126 })]
    [DataRow(new string[] { "KhTd", "Kc9s" }, "", new double[] { 0.7172, 0.2828 })]
    [DataRow(new string[] { "8h8d", "Jc7s" }, "", new double[] { 0.7060, 0.2940 })]
    [DataRow(new string[] { "8h8d", "Js7s" }, "", new double[] { 0.6696, 0.3304 })]
    [DataRow(new string[] { "8h8d", "9s8s" }, "", new double[] { 0.6100, 0.3900 })]
    [DataRow(new string[] { "8h8d", "TcTs" }, "", new double[] { 0.1906, 0.8094 })]
    [DataRow(new string[] { "8h8d", "6c7s" }, "", new double[] { 0.8485, 0.1515 })]
    [DataRow(new string[] { "8h8d", "AcKs" }, "", new double[] { 0.5557, 0.4443 })]
    [DataRow(new string[] { "8h8d", "KsQs" }, "", new double[] { 0.5111, 0.4889 })]
    [DataRow(new string[] { "8h8d", "5s4s" }, "", new double[] { 0.7815, 0.2185 })]
    [DataRow(new string[] { "AsAh", "KsKh", "QsQh" }, "", new double[] { 0.6767, 0.1723, 0.1510 })]
    [DataRow(new string[] { "AsAh", "KhKd", "QdQc" }, "", new double[] { 0.6687, 0.1744, 0.1569 })]
    [DataRow(new string[] { "AsAh", "KhKd", "QdQc" }, "QsJsTs", new double[] { 0.3311, 0.1307, 0.5382 })]
    [DataRow(new string[] { "AsAh", "KhKd", "QdQc" }, "QsJsTs9h", new double[] { 0.2143, 0.5476, 0.2381 })]
    [DataRow(new string[] { "AsAh", "KhKd", "QdQc" }, "QsJsTs9hKs", new double[] { 1.0000, 0.0000, 0.0000 })]
    public void HandOddsTest_WinRates(string[] pockets, string board, double[] expectedWinRates)
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

        for (int i = 0; i < pockets.Length; i++)
        {
            var winRate = Math.Round((wins[i] + ties[i] / (double)pockets.Length) / totalHands, RateDigits);

            Assert.AreEqual(expectedWinRates[i], winRate);
        }
    }

    private static int Comb(int n, int r)
    {
        if (r == 0 || r == n)
        {
            return 1;
        }

        return Comb(n - 1, r - 1) + Comb(n - 1, r);
    }

    private static int CardLengthFromSHDC(string value)
        => value.Length / 2;
}
