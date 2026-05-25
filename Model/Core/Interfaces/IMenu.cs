using System.Collections.Generic;
using Model.Core.Dishes;

namespace Model.Core.Interfaces
{
    public interface IMenu
    {
        /// <summary>
        /// Возвращает массив (или перечисление) всех блюд в меню.
        /// </summary>
        IEnumerable<Dish> GetDishes();

        /// <summary>
        /// Возвращает количество позиций в меню.
        /// </summary>
        int Count { get; }
    }
}
