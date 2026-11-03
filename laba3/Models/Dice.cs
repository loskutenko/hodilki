using System;

namespace BoardGame.Models
{
    public class Dice
    {
        // Количество граней (по умолчанию 6)
        public int Faces { get; set; } = 6;

        // Для сериализации нужен пустой конструктор
        public Dice() { }

        public Dice(int faces)
        {
            if (faces < 1) throw new ArgumentException("Кубик должен иметь хотя бы одну грань.");
            Faces = faces;
        }

        // Бросок кубика
        public int Roll()
        {
            Random rnd = new Random();
            return rnd.Next(1, Faces + 1);
        }
    }
}