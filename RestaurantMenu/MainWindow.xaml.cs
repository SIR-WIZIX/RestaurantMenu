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


            NewEstablishmentTypeComboBox.ItemsSource = new List<string>
            {
            "Restaurant",
            "Cafe",
            "CoffeeShop"};
            NewEstablishmentTypeComboBox.SelectedIndex = 0;

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

            EstablishmentComboBox.Items.Clear();

            IEnumerable<Establishment> filtered =
                selectedType == "Все" || selectedType == null
                ? _allEstablishments
                : _allEstablishments.Where(est =>
                    est.GetType().Name.Equals(selectedType, StringComparison.OrdinalIgnoreCase));

            // Добавляем реальные объекты
            foreach (var est in filtered)
                EstablishmentComboBox.Items.Add(est);

            // Добавляем пункт "Новое"
            EstablishmentComboBox.Items.Add(new NewEstablishment());

            EstablishmentComboBox.SelectedIndex = -1;
        }




        private void EstablishmentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EstablishmentComboBox.SelectedItem is NewEstablishment)
            {
                NewEstablishmentPanel.Visibility = Visibility.Visible;
                ShowMenuButton.IsEnabled = false;
            }
            else if (EstablishmentComboBox.SelectedItem is Establishment)
            {
                NewEstablishmentPanel.Visibility = Visibility.Collapsed;
                ShowMenuButton.IsEnabled = true;
            }
        }


        public class NewEstablishment 
        {
            public override string ToString() => "Новое";
        }


        private void AddEstablishmentButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NewEstablishmentNameTextBox.Text.Trim();
            string address = NewEstablishmentAddressTextBox.Text.Trim();
            string type = NewEstablishmentTypeComboBox.SelectedItem as string;


            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(address) ||
                string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            Establishment newEst = type switch
            {
                "Restaurant" => new Restaurant(name, address, new DomainMenu(), 0),
                "Cafe" => new Cafe(name, address, new DomainMenu(), true),
                "CoffeeShop" => new CoffeeShop(name, address, new DomainMenu(), 0),
                _ => null
            };

            if (newEst == null)
            {
                MessageBox.Show("Неизвестный тип заведения.");
                return;
            }

            _allEstablishments.Add(newEst);

            // Перезагружаем список
            TypeFilterComboBox_SelectionChanged(null, null);

            MessageBox.Show("Заведение добавлено!");
        }




        private void FormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void ShowMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (EstablishmentComboBox.SelectedItem is Establishment est &&
                MenuTypeComboBox.SelectedItem is string menuType)
            {
                MenuWindow menuWindow = new MenuWindow(est, menuType);
                menuWindow.ShowDialog();
            }
        }


    }
}

