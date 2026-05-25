using System.Collections.Generic;
using Model.Core.Dishes;
using Model.Core.Interfaces;

namespace Model.Core.Menu
{
    public partial class Menu : IMenu
    {
        // Инкапсулированный список блюд. Доступен внутри partial-класса
        private readonly List<Dish> _dishes;

        public int Count => _dishes.Count;

        // Конструктор для создания пустого меню
        public Menu()
        {
            _dishes = new List<Dish>();
        }

        // Конструктор для создания меню с начальным набором блюд
        public Menu(IEnumerable<Dish> initialDishes)
        {
            _dishes = initialDishes != null ? new List<Dish>(initialDishes) : new List<Dish>();
        }

        /// <summary>
        /// Возвращает элементы меню в виде массива для соблюдения требований ТЗ
        /// </summary>
        public IEnumerable<Dish> GetDishes()
        {
            return _dishes.ToArray();
        }
    }
}
