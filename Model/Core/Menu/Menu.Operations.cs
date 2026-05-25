using System;
using Model.Core.Dishes;

namespace Model.Core.Menu
{
    public partial class Menu
    {
        /// <summary>
        /// Добавляет новое блюдо в меню.
        /// </summary>
        public void AddDish(Dish dish)
        {
            if (dish == null)
            {
                throw new ArgumentNullException(
                    nameof(dish),
                    "Нельзя добавить пустое блюдо (null)."
                );
            }

            _dishes.Add(dish);
        }

        /// <summary>
        /// Удаляет блюдо из меню.
        /// </summary>
        /// <returns>True, если удаление прошло успешно.</returns>
        public bool RemoveDish(Dish dish)
        {
            if (dish == null)
                return false;

            return _dishes.Remove(dish);
        }

        /// <summary>
        /// Очищает всё меню.
        /// </summary>
        public void Clear()
        {
            _dishes.Clear();
        }
    }
}
