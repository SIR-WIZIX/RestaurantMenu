namespace Model;

[XmlInclude(typeof(HotDish))]
[XmlInclude(typeof(Drink))]
[XmlInclude(typeof(Dessert))]
[JsonDerivedType(typeof(HotDish), typeDiscriminator: "hot_dish")]
[JsonDerivedType(typeof(Drink), typeDiscriminator: "drink")]
[JsonDerivedType(typeof(Dessert), typeDiscriminator: "dessert")]
public abstract class Dish { }
