namespace Model.Core.Dishes
{
    public class Dessert : Dish
    {
        private int _calories;
        private bool _containsNuts;

        public int Calories => _calories;
        public bool ContainsNuts => _containsNuts;

        public Dessert(
            string name,
            decimal price,
            int calories,
            bool containsNuts,
            string category = "Десерты"
        )
            : base(name, price, category)
        {
            _calories = calories < 0 ? 0 : calories;
            _containsNuts = containsNuts;
        }
    }
}
