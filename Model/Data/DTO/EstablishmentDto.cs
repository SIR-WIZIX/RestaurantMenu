using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Model.Data.DTO
{
    // Разрешаем полиморфизм для разных типов заведений при сохранении списков
    [XmlInclude(typeof(RestaurantDto))]
    [XmlInclude(typeof(CafeDto))]
    [XmlInclude(typeof(CoffeeShopDto))]
    [JsonDerivedType(typeof(RestaurantDto), typeDiscriminator: "restaurant")]
    [JsonDerivedType(typeof(CafeDto), typeDiscriminator: "cafe")]
    [JsonDerivedType(typeof(CoffeeShopDto), typeDiscriminator: "coffee_shop")]
    public class EstablishmentDto
    {
        public string Type { get; set; } = string.Empty; // Удобно для фильтрации типов в UI
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public List<DishDto> MainMenuDishes { get; set; } = new List<DishDto>();
        public List<DishDto> SeasonalMenuDishes { get; set; } = null; // null, если сезонного меню нет

        public EstablishmentDto() { }
    }

    public class RestaurantDto : EstablishmentDto
    {
        public int StarsCount { get; set; }
    }

    public class CafeDto : EstablishmentDto
    {
        public bool HasBusinessLunch { get; set; }
    }

    public class CoffeeShopDto : EstablishmentDto
    {
        public bool HasOwnRoastery { get; set; }
    }
}
