using FluentValidation;


namespace StoreProjectAPI.Dtos.ProductDtos
{
    public class ProductPostDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal SalePrice { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public CategoryInProductPostDto Category { get; set; }
    }
    public class CategoryInProductPostDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ProductPostDtoValidator : AbstractValidator<ProductPostDto>
    {
        public ProductPostDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(25);
        }
    }
}
