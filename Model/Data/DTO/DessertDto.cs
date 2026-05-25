using Model.Core.Dishes;

namespace Model.Data.DTO
{
    public class DessertDto : DishDto
    {
        public int Calories { get; set; }
        public bool ContainsNuts { get; set; }

        public DessertDto()
            : base() { }

        public DessertDto(Dessert dessert)
            : base(dessert)
        {
            Calories = dessert.Calories;
            ContainsNuts = dessert.ContainsNuts;
        }

        public override Dish ToDomainObject()
        {
            return new Dessert(Name, Price, Calories, ContainsNuts, Category);
        }
    }
}
