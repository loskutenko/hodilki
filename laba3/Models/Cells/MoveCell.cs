namespace BoardGame.Models.Cells
{
    // Клетка, которая переносит игрока (например, "Водопад" назад или "Мост" вперед)
    public class MoveCell : Cell
    {
        public int Offset { get; set; } // На сколько клеток двигать (минус = назад)

        public MoveCell() : base() { }
        public MoveCell(int index, string description, int offset) : base(index, description)
        {
            Offset = offset;
        }

        public override string ApplyEffect(Player player, GameEngine engine)
        {
            int oldPos = player.Position;
            int newPos = player.Position + Offset;

            // Проверка, чтобы не улететь за пределы поля
            if (newPos < 0) newPos = 0;
            if (newPos >= engine.Board.TotalCells) newPos = engine.Board.TotalCells - 1;

            player.MoveTo(newPos);
            string direction = Offset > 0 ? "вперед" : "назад";
            return $"Клетка '{Description}': игрок {player.Name} перемещается на {System.Math.Abs(Offset)} клеток {direction}. Новая позиция: {newPos}.";
        }
    }
}