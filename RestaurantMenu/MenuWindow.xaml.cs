using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Model.Core.Dishes;
using Model.Core.Establishments;
using DomainMenu = Model.Core.Menu.Menu;

namespace RestaurantMenu
{
    public partial class MenuWindow : Window
    {
        private Establishment _establishment;
        private string _menuType;
        private DomainMenu? _currentMenu; // Сделали поле nullable

        public MenuWindow(Establishment establishment, string menuType)
        {
            InitializeComponent();
            _establishment = establishment;
            _menuType = menuType;

            this.Title = $"Меню заведения: {_establishment.Name} ({_menuType})";

            _currentMenu =
                (menuType == "Сезонное" ? _establishment.SeasonalMenu : _establishment.MainMenu)
                as DomainMenu;

            LoadCategories();
            RefreshGrid();
        }

        private void LoadCategories()
        {
            if (_currentMenu == null)
                return;

            var categories = new List<string> { "Все" };
            var dishCategories = _currentMenu.GetDishes().Select(d => d.Category).Distinct();
            categories.AddRange(dishCategories);

            CategoryFilterComboBox.ItemsSource = categories;
            CategoryFilterComboBox.SelectedIndex = 0;
        }

        private void RefreshGrid()
        {
            if (_currentMenu == null)
            {
                MessageBox.Show(
                    "Данный вид меню отсутствует в заведении.",
                    "Информация",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
                return;
            }
            MenuDataGrid.ItemsSource = _currentMenu.GetDishes();
        }

        private void CategoryFilterComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e
        )
        {
            string? selectedCategory = CategoryFilterComboBox.SelectedItem as string;

            if (selectedCategory == "Все" || string.IsNullOrEmpty(selectedCategory))
            {
                RefreshGrid();
            }
            else
            {
                MenuDataGrid.ItemsSource = _establishment.Select(selectedCategory).ToList();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentMenu == null)
                return;

            string name = DishNameTextBox.Text.Trim();
            string category = DishCategoryTextBox.Text.Trim();
            string priceText = DishPriceTextBox.Text.Trim();

            if (
                string.IsNullOrWhiteSpace(name)
                || string.IsNullOrWhiteSpace(category)
                || string.IsNullOrWhiteSpace(priceText)
            )
            {
                MessageBox.Show(
                    "Заполните все поля!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show(
                    "Цена должна быть числом!",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            // Создаём блюдо — используй нужный тип (HotDish, ColdDish, Drink и т.д.)
            var newDish = new HotDish(name, price, 0, category);

            _currentMenu.AddDish(newDish);

            RefreshGrid();
            LoadCategories();

            // Очищаем поля
            DishNameTextBox.Text = "";
            DishPriceTextBox.Text = "";
            DishCategoryTextBox.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedDish = MenuDataGrid.SelectedItem as Dish;
            if (selectedDish != null && _currentMenu != null)
            {
                _currentMenu.RemoveDish(selectedDish);
                RefreshGrid();
                LoadCategories();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Изменения успешно зафиксированы в оперативной памяти!",
                "Сохранение",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }

        private void ClearMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentMenu == null || !_currentMenu.GetDishes().Any())
            {
                MessageBox.Show(
                    "В данном меню еще нет ни одного блюда. Очистка не требуется.",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            var confirmResult = MessageBox.Show(
                $"Вы действительно хотите полностью и безвозвратно удалить все блюда ({_currentMenu.GetDishes().Count()} шт.) из этого меню?",
                "Подтверждение полной очистки меню",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (confirmResult == MessageBoxResult.Yes)
            {
                _currentMenu.Clear();

                RefreshGrid();
                LoadCategories();

                MessageBox.Show(
                    "Все позиции меню были успешно удалены!",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }
    }
}
