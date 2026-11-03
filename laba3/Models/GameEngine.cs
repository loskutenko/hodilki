using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BoardGame.Models.Cells;

namespace BoardGame.Models
{
    public class GameEngine
    {
        public Board Board { get; set; } = new Board();
        public List<Player> Players { get; set; } = new List<Player>();
        public Dice Dice { get; set; } = new Dice();

        public int CurrentPlayerIndex { get; set; } = 0;
        public bool IsGameOver { get; set; } = false;
        public List<Player> Winners { get; set; } = new List<Player>();

        public GameEngine() { }

        public void StartGame(List<Player> players)
        {
            if (players == null || players.Count < 2)
                throw new ArgumentException("Для игры нужно минимум 2 игрока.");

            Players = players;
            Board.InitializeDefaultBoard();
            CurrentPlayerIndex = 0;
            IsGameOver = false;
            Winners.Clear();
        }

        public Player GetCurrentPlayer() => Players[CurrentPlayerIndex];

        public string MakeTurn()
        {
            if (IsGameOver) return "Игра уже завершена.";

            Player currentPlayer = GetCurrentPlayer();

            if (currentPlayer.IsFinished)
            {
                NextPlayer();
                return "";
            }

            // Проверка на пропуск хода
            if (currentPlayer.SkipNextTurn)
            {
                currentPlayer.SkipNextTurn = false;
                string type = currentPlayer.IsBot ? "Бот " : "";
                string msgSkip = $">>> {type}{currentPlayer.Name} ПРОПУСКАЕТ ХОД (скип)! <<<";
                NextPlayer();
                return msgSkip;
            }

            int roll = Dice.Roll();
            string typeName = currentPlayer.IsBot ? "Бот " : "";
            string log = $"{typeName}{currentPlayer.Name} бросает кубик и выпадает {roll}. ";

            int newPos = currentPlayer.Position + roll;

            if (newPos >= Board.TotalCells - 1)
            {
                newPos = Board.TotalCells - 1;
            }

            currentPlayer.MoveTo(newPos);
            log += $"Ходит на клетку {newPos}. ";

            Cell cell = Board.GetCell(newPos);
            log += cell.ApplyEffect(currentPlayer, this);

            if (currentPlayer.IsFinished && !Winners.Contains(currentPlayer))
            {
                Winners.Add(currentPlayer);
                log += $" {typeName}{currentPlayer.Name} занимает {Winners.Count} место!";
            }

            CheckGameOver();

            if (!IsGameOver)
            {
                NextPlayer();
            }

            return log;
        }

        private void NextPlayer()
        {
            int startIndex = CurrentPlayerIndex;
            do
            {
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            }
            while (Players[CurrentPlayerIndex].IsFinished && CurrentPlayerIndex != startIndex);
        }

        private void CheckGameOver()
        {
            var activePlayers = Players.Where(p => !p.IsFinished).ToList();

            if (activePlayers.Count == 1)
            {
                Player lastPlayer = activePlayers[0];
                lastPlayer.IsFinished = true;
                Winners.Add(lastPlayer);
                IsGameOver = true;
            }
            else if (activePlayers.Count == 0)
            {
                IsGameOver = true;
            }
        }

        private static JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions { WriteIndented = true, IncludeFields = true };
        }

        public void SaveGame(string path)
        {
            string json = JsonSerializer.Serialize(this, GetJsonOptions());
            File.WriteAllText(path, json);
        }

        public static GameEngine LoadGame(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Файл сохранения не найден.");

            string json = File.ReadAllText(path);
            var engine = JsonSerializer.Deserialize<GameEngine>(json, GetJsonOptions());

            if (engine == null) throw new Exception("Ошибка загрузки игры.");
            return engine;
        }
    }
}