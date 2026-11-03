using System;

namespace BoardGame.Models
{
    public class Player
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Position { get; set; }
        public bool IsBot { get; set; } = false;
        public bool SkipNextTurn { get; set; } = false;

        public bool IsFinished { get; set; } = false;

        public Player() { }

        public Player(string name, string color, bool isBot = false)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Имя не может быть пустым.");
            Name = name;
            Color = color;
            IsBot = isBot;
            Position = 0;
            IsFinished = false;
        }

        public void MoveTo(int newPosition)
        {
            Position = newPosition;
        }
    }
}