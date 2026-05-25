using Model.Core.Interfaces;

namespace Model.Core.Establishments
{
    public abstract partial class Establishment
    {
        private string _name;
        private string _address;
        private IMenu _mainMenu;

        public string Name => _name;
        public string Address => _address;
        public IMenu MainMenu => _mainMenu;

        protected Establishment(string name, string address, IMenu mainMenu)
        {
            _name = string.IsNullOrEmpty(name) ? "Без названия" : name;
            _address = string.IsNullOrEmpty(address) ? "Адрес не указан" : address;
            _mainMenu = mainMenu;
        }
    }
}
