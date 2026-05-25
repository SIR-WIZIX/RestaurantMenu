using Model.Core.Interfaces;

namespace Model.Core.Establishments
{
    public abstract partial class Establishment : ISeasonalMenu
    {
        private IMenu _seasonalMenu;

        public IMenu SeasonalMenu => _seasonalMenu;

        public void AddSeasonalMenu(IMenu seasonalMenu)
        {
            _seasonalMenu = seasonalMenu;
        }

        public void RemoveSeasonalMenu()
        {
            _seasonalMenu = null;
        }
    }
}
