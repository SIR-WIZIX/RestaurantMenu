namespace Model.Core.Dishes
{
    public abstract class Dish
    {
        private string _name;
        private decimal _price;
        private string _category;

        // Только геттеры. Изменение состояния — через методы, если необходимо
        public string Name => _name;
        public decimal Price => _price;
        public string Category => _category;

        protected Dish(string name, decimal price, string category)
        {
            _name = string.IsNullOrEmpty(name) ? "Без названия" : name;
            _price = price < 0 ? 0 : price;
            _category = string.IsNullOrEmpty(category) ? "Общая" : category;
        }

        public override string ToString() => $"[{Category}] {Name} — {Price:C}";
    }
}
