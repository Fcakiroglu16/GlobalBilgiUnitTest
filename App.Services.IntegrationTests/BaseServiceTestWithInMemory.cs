using App.Repository;
using App.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace App.Services.IntegrationTests;

public abstract class BaseServiceTestWithInMemory
{
    private readonly DbContextOptionsBuilder<AppDbContext> optionsBuilder;

    protected BaseServiceTestWithInMemory()
    {
        optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase("TestDb");

        SeedData();


    }

    public void SeedData()
    {
        AppDbContext appDbContext = new AppDbContext(optionsBuilder.Options);

        Order order = new Order()
        {
            OrderCode = "abc",
            CreatedAt = DateTime.Now,
            UserId = 1,

        };

        order.OrderItems.Add(new OrderItem()
        {
            ProductId = 1,
            UnitPrice = 100,
            Count = 1,

        });


        order.TotalPrice = order.OrderItems.Sum(oi => oi.UnitPrice * oi.Count);

        appDbContext.Orders.Add(order);

        appDbContext.SaveChanges();


    }

    public ILogger<T> GetDummyLogger<T>()
    {
        return new Mock<ILogger<T>>().Object;
    }

    public Mock<EmailService> GetEmailService()
    {

        return new Mock<EmailService>();

    }

    public AppDbContext GetContext()
    {
        return new AppDbContext(optionsBuilder.Options);
    }

    public List<OrderItemDto> GetOrderItems(int count)
    {

        List<OrderItemDto> orderItems = new List<OrderItemDto>();
        for (int i = 1; i <= count; i++)
        {
            orderItems.Add(new OrderItemDto(i, i * 10, 1, i * 100));
        }
        return orderItems;
    }



}