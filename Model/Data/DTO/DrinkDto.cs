using Model.Core.Dishes;

namespace Model.Data.DTO
{
    public class DrinkDto : DishDto
    {
        public int VolumeMl { get; set; }
        public bool IsIceRequired { get; set; }

        public DrinkDto()
            : base() { }

        public DrinkDto(Drink drink)
            : base(drink)
        {
            VolumeMl = drink.VolumeMl;
            IsIceRequired = drink.IsIceRequired;
        }

        public override Dish ToDomainObject()
        {
            return new Drink(Name, Price, VolumeMl, IsIceRequired, Category);
        }
    }
}
