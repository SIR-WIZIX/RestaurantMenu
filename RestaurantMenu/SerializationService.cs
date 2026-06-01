using System;
using System.Collections.Generic;
using System.Linq;
using Model.Core.Dishes;
using Model.Core.Establishments;
using Model.Data;
using Model.Data.DTO;
using DomainMenu = Model.Core.Menu.Menu;

namespace RestaurantMenu
{
    public static class SerializationService
    {
        public static void SaveToFile(
            string filePath,
            List<Establishment> establishments,
            string format
        )
        {
            var dtos = new List<EstablishmentDto>();

            foreach (var est in establishments)
            {
                EstablishmentDto estDto = est switch
                {
                    Restaurant r => new RestaurantDto { StarsCount = r.StarsCount },
                    Cafe c => new CafeDto { HasBusinessLunch = c.HasBusinessLunch },
                    CoffeeShop cs => new CoffeeShopDto { HasOwnRoastery = cs.HasOwnRoastery },
                    _ => new EstablishmentDto(),
                };

                estDto.Type = est.GetType().Name;
                estDto.Name = est.Name;
                estDto.Address = est.Address;

                if (est.MainMenu is DomainMenu dm)
                {
                    estDto.MainMenuDishes = dm.GetDishes().Select(ConvertDishToDto).ToList();
                }

                if (est.SeasonalMenu is DomainMenu sm)
                {
                    estDto.SeasonalMenuDishes = sm.GetDishes().Select(ConvertDishToDto).ToList();
                }

                dtos.Add(estDto);
            }

            BaseSerializer<List<EstablishmentDto>> serializer = format.Equals(
                "JSON",
                StringComparison.OrdinalIgnoreCase
            )
                ? new Model.Data.JsonSerializer<List<EstablishmentDto>>()
                : new Model.Data.XmlSerializer<List<EstablishmentDto>>();

            serializer.Serialize(dtos, filePath);
        }

        private static DishDto ConvertDishToDto(Dish dish)
        {
            return dish switch
            {
                HotDish hd => new HotDishDto(hd),
                Drink dr => new DrinkDto(dr),
                Dessert ds => new DessertDto(ds),
                _ => new DishDto(dish),
            };
        }

        public static List<Establishment> LoadFromFile(string filePath, string format)
        {
            BaseSerializer<List<EstablishmentDto>> serializer = format.Equals(
                "JSON",
                StringComparison.OrdinalIgnoreCase
            )
                ? new Model.Data.JsonSerializer<List<EstablishmentDto>>()
                : new Model.Data.XmlSerializer<List<EstablishmentDto>>();

            var dtos = serializer.Deserialize(filePath);

            var resultList = new List<Establishment>();
            if (dtos == null)
                return resultList;

            foreach (var dto in dtos)
            {
                var mainDishes =
                    dto.MainMenuDishes?.Select(d => d.ToDomainObject()).ToList()
                    ?? new List<Dish>();
                var mainMenu = new DomainMenu(mainDishes);

                var seasonalDishes =
                    dto.SeasonalMenuDishes?.Select(d => d.ToDomainObject()).ToList()
                    ?? new List<Dish>();

                Establishment? est = dto switch
                {
                    RestaurantDto rDto => new Restaurant(
                        rDto.Name,
                        rDto.Address,
                        mainMenu,
                        rDto.StarsCount
                    ),
                    CafeDto cDto => new Cafe(
                        cDto.Name,
                        cDto.Address,
                        mainMenu,
                        cDto.HasBusinessLunch
                    ),
                    CoffeeShopDto csDto => new CoffeeShop(
                        csDto.Name,
                        csDto.Address,
                        mainMenu,
                        csDto.HasOwnRoastery
                    ),
                    _ => null,
                };

                if (est != null)
                {
                    if (est.SeasonalMenu is DomainMenu estSeasonal)
                    {
                        foreach (var dish in seasonalDishes)
                        {
                            estSeasonal.AddDish(dish);
                        }
                    }

                    resultList.Add(est);
                }
            }

            return resultList;
        }
    }
}
