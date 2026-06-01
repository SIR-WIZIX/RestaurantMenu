using Model.Core.Interfaces;
using DomainMenu = Model.Core.Menu.Menu;

namespace Model.Core.Establishments
{
    public abstract partial class Establishment : ISeasonalMenu
    {
        private IMenu _seasonalMenu = new DomainMenu();

        public IMenu SeasonalMenu => _seasonalMenu;

        public void AddSeasonalMenu(IMenu seasonalMenu)
        {
            _seasonalMenu = seasonalMenu ?? new DomainMenu();
        }

        public void RemoveSeasonalMenu()
        {
            _seasonalMenu = new DomainMenu();
        }
    }
}
