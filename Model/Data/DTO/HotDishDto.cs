using Model.Core.Dishes;

namespace Model.Data.DTO
{
    public class HotDishDto : DishDto
    {
        public int CookingTimeMinutes { get; set; }

        public HotDishDto()
            : base() { }

        public HotDishDto(HotDish hotDish)
            : base(hotDish)
        {
            CookingTimeMinutes = hotDish.CookingTimeMinutes;
        }

        public override Dish ToDomainObject()
        {
            return new HotDish(Name, Price, CookingTimeMinutes, Category);
        }
    }
}
