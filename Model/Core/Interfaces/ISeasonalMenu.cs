using Model.Core.Interfaces; // Для ссылки на интерфейс меню IMenu

namespace Model.Core.Interfaces
{
    public interface ISeasonalMenu
    {
        IMenu SeasonalMenu { get; }
        void AddSeasonalMenu(IMenu seasonalMenu);
        void RemoveSeasonalMenu();
    }
}
