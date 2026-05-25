using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Model.Core.Dishes;
using Model.Core.Establishments;
// Избавляемся от коллизии классов Menu в Главном окне
using DomainMenu = Model.Core.Menu.Menu;

namespace RestaurantMenu
{
    public partial class MainWindow : Window
    {
        private List<Establishment> _allEstablishments = new List<Establishment>();

        public MainWindow()
        {
            InitializeComponent();
            LoadMockData();
            InitializeFilters();
        }

        private void LoadMockData()
        {
            var cafeMenu = new DomainMenu(
                new List<Dish>
                {
                    new HotDish("Омлет", 150, 10, "Завтраки"),
                    new Drink("Капучино", 120, 200, false, "Напитки"),
                }
            );
            var cafe = new Cafe("Уютное Кафе", "ул. Пушкина, 10", cafeMenu, true);

            var restMenu = new DomainMenu(
                new List<Dish>
                {
                    new HotDish("Стейк Рибай", 850, 25, "Горячее"),
                    new Dessert("Тирамису", 250, 300, false, "Десерты"),
                }
            );
            var restaurant = new Restaurant("Премиум Ресторан", "ул. Лемонтова, 5", restMenu, 5);

            _allEstablishments.Add(cafe);
            _allEstablishments.Add(restaurant);
        }

        private void InitializeFilters()
        {
            TypeFilterComboBox.ItemsSource = new List<string>
            {
                "Все",
                "Restaurant",
                "Cafe",
                "CoffeeShop",
            };
            TypeFilterComboBox.SelectedIndex = 0;

            MenuTypeComboBox.ItemsSource = new List<string> { "Обычное", "Сезонное" };
            MenuTypeComboBox.SelectedIndex = 0;

            FormatComboBox.ItemsSource = new List<string> { "JSON", "XML" };
            FormatComboBox.SelectedIndex = 0;
        }

        private void TypeFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedType = TypeFilterComboBox.SelectedItem as string;
            if (selectedType == "Все" || selectedType == null)
            {
                EstablishmentComboBox.ItemsSource = _allEstablishments;
            }
            else
            {
                EstablishmentComboBox.ItemsSource = _allEstablishments
                    .Where(est =>
                        est.GetType().Name.Equals(selectedType, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }
            EstablishmentComboBox.SelectedIndex = -1;
        }

        private void EstablishmentComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e
        )
        {
            ShowMenuButton.IsEnabled = EstablishmentComboBox.SelectedItem != null;
        }

        private void FormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void ShowMenuButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedEst = EstablishmentComboBox.SelectedItem as Establishment;
            string? selectedMenuType = MenuTypeComboBox.SelectedItem as string;

            if (selectedEst != null && selectedMenuType != null)
            {
                MenuWindow menuWindow = new MenuWindow(selectedEst, selectedMenuType);
                menuWindow.ShowDialog();
            }
        }
    }
}

