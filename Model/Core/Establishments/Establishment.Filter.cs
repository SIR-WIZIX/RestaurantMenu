using System;
using System.Collections.Generic;
using System.Linq;
using Model.Core.Dishes;

namespace Model.Core.Establishments
{
    public delegate bool DishCriteria(Dish dish);

    public abstract partial class Establishment
    {
        /// <summary>
        /// Возвращает из меню только позиции подходящего вида (категории).
        /// </summary>
        public IEnumerable<Dish> Select(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return _mainMenu.GetDishes();
            }

            return _mainMenu
                .GetDishes()
                .Where(dish => dish.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Универсальный метод фильтрации меню на основе кастомного делегата-критерия.
        /// </summary>
        public IEnumerable<Dish> FilterMenu(DishCriteria criteria)
        {
            if (criteria == null)
                return _mainMenu.GetDishes();

            var filteredList = new List<Dish>();

            foreach (var dish in _mainMenu.GetDishes())
            {
                if (criteria(dish))
                {
                    filteredList.Add(dish);
                }
            }

            return filteredList;
        }
    }
}
