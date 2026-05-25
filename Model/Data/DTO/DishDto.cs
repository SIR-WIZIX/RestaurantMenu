using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Model.Core.Dishes;

namespace Model.Data.DTO
{
    [XmlInclude(typeof(HotDishDto))]
    [XmlInclude(typeof(DrinkDto))]
    [XmlInclude(typeof(DessertDto))]
    [JsonDerivedType(typeof(HotDishDto), typeDiscriminator: "hot_dish")]
    [JsonDerivedType(typeof(DrinkDto), typeDiscriminator: "drink")]
    [JsonDerivedType(typeof(DessertDto), typeDiscriminator: "dessert")]
    public class DishDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;

        // Пустой конструктор для сериализаторов
        public DishDto() { }

        // Конструктор создания DTO из доменной модели
        public DishDto(Dish dish)
        {
            Name = dish.Name;
            Price = dish.Price;
            Category = dish.Category;
        }

        // Метод восстановления доменного объекта из DTO
        public virtual Dish ToDomainObject()
        {
            throw new InvalidOperationException(
                "Невозможно создать базовый доменный объект Dish напрямую."
            );
        }
    }
}
