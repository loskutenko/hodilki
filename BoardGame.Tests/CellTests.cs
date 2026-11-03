using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoardGame.Models;
using BoardGame.Models.Cells;

namespace BoardGame.Tests
{
    [TestClass]
    public class CellTests
    {
        [TestMethod]
        public void TestMoveCell_MovesForward()
        {
            var player = new Player("Тест", "Красный");
            player.MoveTo(5);
            var cell = new MoveCell(10, "Мост", 3);

            var engine = new GameEngine();
            engine.Board.InitializeDefaultBoard();

            cell.ApplyEffect(player, engine);
            Assert.AreEqual(8, player.Position);
        }

        [TestMethod]
        public void TestSkipTurnCell_SetsFlag()
        {
            var player = new Player("Тест", "Красный");
            var cell = new SkipTurnCell(10, "Красный свет");
            var engine = new GameEngine();

            cell.ApplyEffect(player, engine);
            Assert.IsTrue(player.SkipNextTurn);
        }

        [TestMethod]
        public void TestFinishCell_SetsFinished()
        {
            var player = new Player("Тест", "Красный");
            var cell = new FinishCell(49, "Финиш");
            var engine = new GameEngine();

            cell.ApplyEffect(player, engine);
            Assert.IsTrue(player.IsFinished);
        }
    }
}