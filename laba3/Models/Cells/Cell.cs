using System.Text.Json.Serialization;

namespace BoardGame.Models.Cells
{
    // Указываем, какие наследники бывают у базового класса Cell
    [JsonDerivedType(typeof(NormalCell), typeDiscriminator: "normal")]
    [JsonDerivedType(typeof(MoveCell), typeDiscriminator: "move")]
    [JsonDerivedType(typeof(SkipTurnCell), typeDiscriminator: "skip")]
    [JsonDerivedType(typeof(FinishCell), typeDiscriminator: "finish")]
    public abstract class Cell
    {
        public int Index { get; set; }
        public string Description { get; set; }

        protected Cell() { }

        protected Cell(int index, string description)
        {
            Index = index;
            Description = description;
        }

        // Виртуальный метод, который будет переопределяться
        // Возвращает строку-лог о том, что произошло
        public virtual string ApplyEffect(Player player, GameEngine engine)
        {
            return "";
        }
    }
}