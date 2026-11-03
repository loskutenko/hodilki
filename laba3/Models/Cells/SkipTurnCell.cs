namespace BoardGame.Models.Cells
{
    // Клетка, которая пропускает ход (например, "Красный свет")
    public class SkipTurnCell : Cell
    {
        public SkipTurnCell() : base() { }
        public SkipTurnCell(int index, string description) : base(index, description) { }

        public override string ApplyEffect(Player player, GameEngine engine)
        {
            player.SkipNextTurn = true; // Нам надо будет добавить это поле в Player
            return $"Клетка '{Description}': игрок {player.Name} пропускает следующий ход!";
        }
    }
}