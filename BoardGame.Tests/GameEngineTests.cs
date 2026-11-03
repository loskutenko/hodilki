using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoardGame.Models;
using System.Collections.Generic;
using System.IO;

namespace BoardGame.Tests
{
    [TestClass]
    public class GameEngineTests
    {
        [TestMethod]
        public void TestStartGame_ValidPlayers()
        {
            var engine = new GameEngine();
            var players = new List<Player>
            {
                new Player("Игрок 1", "Красный"),
                new Player("Игрок 2", "Синий")
            };
            engine.StartGame(players);

            Assert.AreEqual(2, engine.Players.Count);
            Assert.AreEqual(50, engine.Board.TotalCells);
            Assert.IsFalse(engine.IsGameOver);
        }

        [TestMethod]
        public void TestStartGame_LessThanTwoPlayers_ThrowsException()
        {
            var engine = new GameEngine();
            var players = new List<Player> { new Player("Один", "Красный") };

            Assert.ThrowsException<System.ArgumentException>(() => engine.StartGame(players));
        }

        [TestMethod]
        public void TestSaveAndLoadGame()
        {
            var engine = new GameEngine();
            var players = new List<Player>
            {
                new Player("Игрок 1", "Красный"),
                new Player("Игрок 2", "Синий")
            };
            engine.StartGame(players);
            engine.Players[0].MoveTo(10);
            engine.Players[1].MoveTo(5);
            engine.CurrentPlayerIndex = 1;

            string filePath = "test_save.json";

            engine.SaveGame(filePath);
            Assert.IsTrue(File.Exists(filePath));

            var loadedEngine = GameEngine.LoadGame(filePath);

            Assert.AreEqual(engine.Players[0].Position, loadedEngine.Players[0].Position);
            Assert.AreEqual(engine.Players[1].Position, loadedEngine.Players[1].Position);
            Assert.AreEqual(engine.CurrentPlayerIndex, loadedEngine.CurrentPlayerIndex);
            Assert.AreEqual(engine.Board.TotalCells, loadedEngine.Board.TotalCells);

            var loadedCell = loadedEngine.Board.GetCell(10);
            Assert.IsInstanceOfType(loadedCell, typeof(BoardGame.Models.Cells.MoveCell));

            File.Delete(filePath);
        }
    }
}