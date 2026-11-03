using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BoardGame.Models;

namespace BoardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            string savePath = "savegame.json";

            GameEngine engine = null;

            if (File.Exists(savePath))
            {
                Console.Write("Найдено сохранение. Загрузить игру? (y/n): ");
                if (Console.ReadLine()?.ToLower() == "y")
                {
                    try
                    {
                        engine = GameEngine.LoadGame(savePath);
                        Console.WriteLine("Игра успешно загружена!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка загрузки: {ex.Message}. Начинаем новую игру.");
                        engine = SetupGame();
                    }
                }
                else
                {
                    engine = SetupGame();
                }
            }
            else
            {
                engine = SetupGame();
            }

            if (engine == null) return;

            // Игровой цикл
            while (!engine.IsGameOver)
            {
                Player current = engine.GetCurrentPlayer();

                // Если текущий игрок — бот, ход делается автоматически
                if (current.IsBot)
                {
                    Console.WriteLine($"\nБот {current.Name} думает...");
                    System.Threading.Thread.Sleep(1000);
                    string botLog = engine.MakeTurn();
                    if (!string.IsNullOrEmpty(botLog)) Console.WriteLine(botLog);
                }
                else
                {
                    Console.WriteLine($"\nСейчас ход: {current.Name}");
                    Console.WriteLine("Нажмите Enter, чтобы бросить кубик, 'S' - сохранить и выйти.");
                    string input = Console.ReadLine();

                    if (input?.ToUpper() == "S")
                    {
                        engine.SaveGame(savePath);
                        Console.WriteLine("Игра сохранена. Выход.");
                        return;
                    }

                    string turnLog = engine.MakeTurn();
                    if (!string.IsNullOrEmpty(turnLog)) Console.WriteLine(turnLog);
                }

                Console.WriteLine("\nТекущие позиции:");
                foreach (var p in engine.Players)
                {
                    string status = p.IsFinished ? " (Финиш)" : "";
                    string type = p.IsBot ? "Бот " : "";
                    Console.WriteLine($"  {type}{p.Name} (цвет {p.Color}): клетка {p.Position}{status}");
                }
            }

            Console.WriteLine("\nИгра завершена! Итоговые места:");
            for (int i = 0; i < engine.Winners.Count; i++)
            {
                string type = engine.Winners[i].IsBot ? "Бот " : "";
                Console.WriteLine($"  {i + 1} место: {type}{engine.Winners[i].Name}");
            }

            File.Delete(savePath);
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static GameEngine SetupGame()
        {
            Console.WriteLine("\nНастройка новой игры");

            int playerCount = 0;
            while (playerCount < 2 || playerCount > 4)
            {
                Console.Write("Введите количество игроков (от 2 до 4): ");
                if (!int.TryParse(Console.ReadLine(), out playerCount) || playerCount < 2 || playerCount > 4)
                {
                    Console.WriteLine("Ошибка! Введите число от 2 до 4.");
                    playerCount = 0;
                }
            }

            List<string> availableColors = new List<string> { "Красный", "Синий", "Зеленый", "Желтый" };
            List<string> botNames = new List<string> { "Джо", "Боб", "Макс", "Лео", "Сэм", "Ник" };
            Random rnd = new Random();

            List<Player> players = new List<Player>();

            for (int i = 0; i < playerCount; i++)
            {
                Console.WriteLine($"\nНастройка игрока {i + 1}");

                bool isBot = false;
                while (true)
                {
                    Console.Write("Это бот? (y/n): ");
                    string botInput = Console.ReadLine()?.ToLower();
                    if (botInput == "y") { isBot = true; break; }
                    if (botInput == "n") { isBot = false; break; }
                    Console.WriteLine("Ошибка! Введите 'y' (да) или 'n' (нет).");
                }

                string name;
                if (isBot)
                {
                    int nameIndex = rnd.Next(botNames.Count);
                    name = botNames[nameIndex];
                    botNames.RemoveAt(nameIndex);

                    Console.WriteLine($"Имя бота: {name}");
                }
                else
                {
                    Console.Write("Введите имя: ");
                    name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name)) name = $"Игрок {i + 1}";
                }

                string color = "";
                if (availableColors.Count == 1)
                {
                    color = availableColors[0];
                    Console.WriteLine($"Оставшийся цвет назначается автоматически: {color}");
                }
                else
                {
                    Console.WriteLine("Доступные цвета:");
                    for (int c = 0; c < availableColors.Count; c++)
                    {
                        Console.WriteLine($"  {c + 1}. {availableColors[c]}");
                    }

                    while (string.IsNullOrEmpty(color))
                    {
                        Console.Write("Выберите цвет (номер): ");
                        if (int.TryParse(Console.ReadLine(), out int colorChoice) && colorChoice >= 1 && colorChoice <= availableColors.Count)
                        {
                            color = availableColors[colorChoice - 1];
                        }
                        else
                        {
                            Console.WriteLine("Ошибка! Введите корректный номер цвета.");
                        }
                    }
                }

                availableColors.Remove(color);
                players.Add(new Player(name, color, isBot));
            }

            var engine = new GameEngine();
            engine.StartGame(players);
            Console.WriteLine("\nИгра началась! Нажмите Enter для первого хода.");
            Console.ReadLine();

            return engine;
        }
    }
}