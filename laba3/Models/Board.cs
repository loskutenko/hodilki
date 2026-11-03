using System;
using System.Collections.Generic;
using BoardGame.Models.Cells;

namespace BoardGame.Models
{
    public class Board
    {
        // Список всех клеток на поле
        public List<Cell> Cells { get; set; } = new List<Cell>();

        public int TotalCells => Cells.Count;

        public Board() { }

        // Метод для удобной генерации поля
        public void InitializeDefaultBoard()
        {
            Cells.Clear();

            // Поле на 50 клеток
            for (int i = 0; i < 50; i++)
            {
                if (i == 0)
                    Cells.Add(new NormalCell(i, "Старт"));
                else if (i == 49)
                    Cells.Add(new FinishCell(i, "Финиш"));

                // 1. Пропуски хода (всего 6 штук)
                else if (i == 7)
                    Cells.Add(new SkipTurnCell(i, "Красный свет"));
                else if (i == 13)
                    Cells.Add(new SkipTurnCell(i, "Заправился"));
                else if (i == 21)
                    Cells.Add(new SkipTurnCell(i, "Штрафной"));
                else if (i == 32)
                    Cells.Add(new SkipTurnCell(i, "Ремонт дороги"));
                else if (i == 41)
                    Cells.Add(new SkipTurnCell(i, "Проверка документов"));
                else if (i == 46)
                    Cells.Add(new SkipTurnCell(i, "Обед"));

                // 2. Сдвиги вперед (всего 5 штук)
                else if (i == 4)
                    Cells.Add(new MoveCell(i, "Попутный ветер (вперед на 4)", 4));
                else if (i == 16)
                    Cells.Add(new MoveCell(i, "Зеленый свет (вперед на 3)", 3));
                else if (i == 27)
                    Cells.Add(new MoveCell(i, "Попутка (вперед на 6)", 6));
                else if (i == 35)
                    Cells.Add(new MoveCell(i, "Скоростное шоссе (вперед на 5)", 5));
                else if (i == 44)
                    Cells.Add(new MoveCell(i, "Быстрое такси (вперед на 2)", 2));

                // 3. Сдвиги назад (всего 6 штук)
                else if (i == 10)
                    Cells.Add(new MoveCell(i, "Водопад (назад на 3)", -3));
                else if (i == 18)
                    Cells.Add(new MoveCell(i, "Прокол колеса (назад на 4)", -4));
                else if (i == 24)
                    Cells.Add(new MoveCell(i, "Пробка (назад на 2)", -2));
                else if (i == 30)
                    Cells.Add(new MoveCell(i, "Эвакуатор (назад на 5)", -5));
                else if (i == 38)
                    Cells.Add(new MoveCell(i, "Заблудился (назад на 6)", -6));
                else if (i == 47)
                    Cells.Add(new MoveCell(i, "Разворот (назад на 3)", -3));

                // Все остальные — обычные
                else
                    Cells.Add(new NormalCell(i, $"Клетка {i}"));
            }
        }

        public Cell GetCell(int index)
        {
            if (index < 0 || index >= TotalCells)
                throw new ArgumentOutOfRangeException(nameof(index), "Индекс клетки вне диапазона.");

            return Cells[index];
        }
    }
}