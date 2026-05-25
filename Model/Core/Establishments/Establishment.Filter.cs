using System;
using System.Collections.Generic;
using System.Linq;
using Model.Core.Dishes;

namespace Model.Core.Establishments
{
    public abstract partial class Establishment
    {
        /// <summary>
        /// Возвращает из меню только позиции подходящего вида (категории).
        /// </summary>
        public IEnumerable<Dish> Select(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return _mainMenu.GetDishes(); // Если категория не передана, возвращаем всё
            }

            // Фильтруем массив блюд из основного меню по свойству Category
            return _mainMenu
                .GetDishes()
                .Where(dish => dish.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }
    }
}
