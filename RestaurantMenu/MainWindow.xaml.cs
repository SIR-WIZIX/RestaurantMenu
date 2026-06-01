using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Model.Core.Dishes;
using Model.Core.Establishments;
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
                "CoffeeShop",
            };
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
                        est.GetType().Name.Equals(selectedType, StringComparison.OrdinalIgnoreCase)
                    );

            foreach (var est in filtered)
                EstablishmentComboBox.Items.Add(est);

            EstablishmentComboBox.Items.Add(new NewEstablishment());
            EstablishmentComboBox.SelectedIndex = -1;
        }

        private void EstablishmentComboBox_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e
        )
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
            else
            {
                ShowMenuButton.IsEnabled = false;
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
            string? type = NewEstablishmentTypeComboBox.SelectedItem as string;

            if (
                string.IsNullOrWhiteSpace(name)
                || string.IsNullOrWhiteSpace(address)
                || string.IsNullOrWhiteSpace(type)
            )
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            Establishment? newEst = type switch
            {
                "Restaurant" => new Restaurant(name, address, new DomainMenu(), 0),
                "Cafe" => new Cafe(name, address, new DomainMenu(), true),
                "CoffeeShop" => new CoffeeShop(name, address, new DomainMenu(), false),
                _ => null,
            };

            if (newEst == null)
            {
                MessageBox.Show("Неизвестный тип заведения.");
                return;
            }

            _allEstablishments.Add(newEst);

            // Сбрасываем поля ввода
            NewEstablishmentNameTextBox.Text = "";
            NewEstablishmentAddressTextBox.Text = "";

            // Перезагружаем список заведений
            TypeFilterComboBox_SelectionChanged(null!, null!);
            MessageBox.Show("Заведение добавлено!");
        }

        private void FormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void ShowMenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (
                EstablishmentComboBox.SelectedItem is Establishment est
                && MenuTypeComboBox.SelectedItem is string menuType
            )
            {
                MenuWindow menuWindow = new MenuWindow(est, menuType);
                menuWindow.ShowDialog();
            }
        }

        // ОБРАБОТЧИКИ ДЛЯ РАБОТЫ С СЕРИАЛИЗАЦИЕЙ СЛОЯ DTO
        private void SaveDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            string? selectedFormat = FormatComboBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedFormat))
            {
                MessageBox.Show(
                    "Выберите формат сохранения (JSON/XML)!",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            var sfd = new Microsoft.Win32.SaveFileDialog
            {
                Filter =
                    selectedFormat == "JSON"
                        ? "JSON файлы (*.json)|*.json"
                        : "XML файлы (*.xml)|*.xml",
                FileName = $"menu_database.{selectedFormat.ToLower()}",
            };

            if (sfd.ShowDialog() == true)
            {
                try
                {
                    SerializationService.SaveToFile(
                        sfd.FileName,
                        _allEstablishments,
                        selectedFormat
                    );
                    MessageBox.Show(
                        "База данных заведений успешно экспортирована!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Не удалось сохранить файл: {ex.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void LoadDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            string? selectedFormat = FormatComboBox.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedFormat))
            {
                MessageBox.Show(
                    "Выберите формат импортируемого файла (JSON/XML)!",
                    "Внимание",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
                return;
            }

            var ofd = new Microsoft.Win32.OpenFileDialog
            {
                Filter =
                    selectedFormat == "JSON"
                        ? "JSON файлы (*.json)|*.json"
                        : "XML файлы (*.xml)|*.xml",
            };

            if (ofd.ShowDialog() == true)
            {
                try
                {
                    var loadedData = SerializationService.LoadFromFile(
                        ofd.FileName,
                        selectedFormat
                    );
                    _allEstablishments = loadedData;

                    // Синхронизируем UI с новыми загруженными данными
                    TypeFilterComboBox_SelectionChanged(null!, null!);

                    MessageBox.Show(
                        $"Успешно восстановлено заведений: {_allEstablishments.Count}",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Ошибка десериализации данных: {ex.Message}",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                }
            }
        }

        private void DeleteEstablishmentButton_Click(object sender, RoutedEventArgs e)
        {
            if (EstablishmentComboBox.SelectedItem is Establishment selectedEstablishment)
            {
                var confirmResult = MessageBox.Show(
                    $"Вы уверены, что хотите безвозвратно удалить заведение \"{selectedEstablishment.Name}\" и все его меню?",
                    "Подтверждение удаления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (confirmResult == MessageBoxResult.Yes)
                {
                    _allEstablishments.Remove(selectedEstablishment);

                    TypeFilterComboBox_SelectionChanged(null!, null!);

                    MessageBox.Show(
                        "Заведение успешно удалено из списка!",
                        "Успех",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information
                    );
                }
            }
            else
            {
                MessageBox.Show(
                    "Невозможно выполнить удаление. Пожалуйста, выберите конкретное существующее заведение из списка.",
                    "Предупреждение",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }
}
