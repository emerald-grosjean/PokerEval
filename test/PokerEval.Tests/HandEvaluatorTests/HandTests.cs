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
    [DataRow(new string[] { "AsAh", "KsKh" }, new double[] { 0.8264, 0.1736 })]
    [DataRow(new string[] { "AsAh", "KsKh", "QsQh" }, new double[] { 0.6767, 0.1723, 0.1510 })]
    public void HandOddsTest_WinRates(string[] pockets, double[] expectedWinRates)
    {
        var wins = new long[pockets.Length];
        var losses = new long[pockets.Length];
        var ties = new long[pockets.Length];
        long totalHands = 0;

        Hand.HandOdds(
            pockets: pockets,
            board: string.Empty,
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
