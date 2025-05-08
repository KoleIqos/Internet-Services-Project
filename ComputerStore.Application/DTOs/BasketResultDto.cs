namespace ComputerStore.Application.DTOs
{
    public class BasketResultDto
    {
        public decimal TotalWithoutDiscount { get; set; }
        public decimal TotalWithDiscount { get; set; }
        public decimal DiscountAmount => TotalWithoutDiscount - TotalWithDiscount;
    }
}
