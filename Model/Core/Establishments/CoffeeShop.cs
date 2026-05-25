using Model.Core.Interfaces;

namespace Model.Core.Establishments
{
    public class CoffeeShop : Establishment
    {
        private bool _hasOwnRoastery; // Наличие собственной обжарки зерен

        public bool HasOwnRoastery => _hasOwnRoastery;

        public CoffeeShop(string name, string address, IMenu mainMenu, bool hasOwnRoastery)
            : base(name, address, mainMenu)
        {
            _hasOwnRoastery = hasOwnRoastery;
        }
    }
}
