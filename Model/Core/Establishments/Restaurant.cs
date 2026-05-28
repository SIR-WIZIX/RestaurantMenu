using Model.Core.Interfaces;

namespace Model.Core.Establishments
{
    public class Restaurant : Establishment
    {
        private int _starsCount; // Например, звезды Мишлен или внутренний рейтинг
        public override string ToString() => Name;

        public int StarsCount => _starsCount;

        public Restaurant(string name, string address, IMenu mainMenu, int starsCount)
            : base(name, address, mainMenu)
        {
            _starsCount = starsCount < 0 ? 0 : starsCount;
        }
    }
}
