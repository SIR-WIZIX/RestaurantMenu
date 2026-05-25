using Model.Core.Interfaces;

namespace Model.Core.Establishments
{
    public class Cafe : Establishment
    {
        private bool _hasBusinessLunch;

        public bool HasBusinessLunch => _hasBusinessLunch;

        public Cafe(string name, string address, IMenu mainMenu, bool hasBusinessLunch)
            : base(name, address, mainMenu)
        {
            _hasBusinessLunch = hasBusinessLunch;
        }
    }
}
