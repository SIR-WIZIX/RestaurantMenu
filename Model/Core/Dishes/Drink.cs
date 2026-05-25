namespace Model.Core.Dishes
{
    public class Drink : Dish
    {
        private int _volumeMl;
        private bool _isIceRequired;

        public int VolumeMl => _volumeMl;
        public bool IsIceRequired => _isIceRequired;

        public Drink(
            string name,
            decimal price,
            int volumeMl,
            bool isIceRequired = false,
            string category = "Напитки"
        )
            : base(name, price, category)
        {
            _volumeMl = volumeMl < 0 ? 0 : volumeMl;
            _isIceRequired = isIceRequired;
        }
    }
}
