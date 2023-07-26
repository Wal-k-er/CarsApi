using Cars.Models;

namespace Cars.Dto;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    
    //Список "Id" машин в позициях заказа
    public List<OrderItemDto> OrderItems { get; set; }
}