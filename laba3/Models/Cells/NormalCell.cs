namespace BoardGame.Models.Cells
{
    public class NormalCell : Cell
    {
        public NormalCell() : base() { }
        public NormalCell(int index, string description) : base(index, description) { }
        // Ничего не делает, просто наследует пустой ApplyEffect
    }
}