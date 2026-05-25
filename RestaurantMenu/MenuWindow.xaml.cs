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

            var newDish = new HotDish("Новое тестовое блюдо", 200, 15, "Горячее");
            _currentMenu.AddDish(newDish);

            RefreshGrid();
            LoadCategories();
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
    }
}
