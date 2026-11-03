namespace BoardGame.Models.Cells
{
    public class FinishCell : Cell
    {
        public FinishCell() : base() { }
        public FinishCell(int index, string description) : base(index, description) { }

        public override string ApplyEffect(Player player, GameEngine engine)
        {
            player.IsFinished = true;
            return $"Клетка '{Description}': игрок {player.Name} финиширует!";
        }
    }
}