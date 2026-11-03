using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoardGame.Models;

namespace BoardGame.Tests
{
    [TestClass]
    public class DiceTests
    {
        [TestMethod]
        public void TestRoll_ReturnsValueInRange()
        {
            var dice = new Dice(6);
            for (int i = 0; i < 100; i++)
            {
                int result = dice.Roll();
                Assert.IsTrue(result >= 1 && result <= 6);
            }
        }
    }
}