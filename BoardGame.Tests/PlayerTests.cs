using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoardGame.Models;
using System;

namespace BoardGame.Tests
{
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void TestPlayerCreation_ValidData()
        {
            var player = new Player("Иван", "Красный", false);
            Assert.AreEqual("Иван", player.Name);
            Assert.AreEqual("Красный", player.Color);
            Assert.AreEqual(0, player.Position);
            Assert.IsFalse(player.IsFinished);
        }

        [TestMethod]
        public void TestPlayerCreation_EmptyName_ThrowsException()
        {
            Assert.ThrowsException<ArgumentException>(() => new Player("", "Синий"));
        }

        [TestMethod]
        public void TestMoveTo_ChangesPosition()
        {
            var player = new Player("Иван", "Красный");
            player.MoveTo(15);
            Assert.AreEqual(15, player.Position);
        }
    }
}