namespace Model.Core.Dishes
{
    public class HotDish : Dish
    {
        private int _cookingTimeMinutes;

        public int CookingTimeMinutes => _cookingTimeMinutes;

        public HotDish(
            string name,
            decimal price,
            int cookingTimeMinutes,
            string category = "Горячее"
        )
            : base(name, price, category)
        {
            _cookingTimeMinutes = cookingTimeMinutes < 0 ? 0 : cookingTimeMinutes;
        }
    }
}
