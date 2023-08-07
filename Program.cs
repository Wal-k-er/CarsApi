using System.Reflection;
using Cars.Consumers;
using Cars.Data;
using Cars.Dto;
using Cars.Helper;
using Cars.Interfaces;
using Cars.PipelineBehaviours;
using Cars.Repository;
using Cars.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Подгрузка всех сопоставлений, созданных при помощи AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddMediatR(c => 
    c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), 
    typeof(ValidationBehaviour<,>));

builder.Services.AddTransient<HangfireConfiguration>();

builder.Services.AddScoped<IValidator<CarDto>, CarValidator>();
builder.Services.AddScoped<IValidator<CategoryDto>, CategoryValidator>();
builder.Services.AddScoped<IValidator<CountryDto>, CountryValidator>();
builder.Services.AddScoped<IValidator<CustomerDto>, CustomerValidator>();
builder.Services.AddScoped<IValidator<ManufactureDto>, ManufactureValidator>();
builder.Services.AddScoped<IValidator<OrderDto>, OrderValidator>();

builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IManufactureRepository, ManufactureRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IRandomPriceRepository, RandomPriceRepository>();

builder.Services.AddDbContext<DataContext>(option =>
    option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfire(configuration =>
    configuration.UsePostgreSqlStorage(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer();

var rabbit = new Uri(builder.Configuration.GetConnectionString("RabbitMQConnection"));

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<CreateOrderConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.Host(rabbit, h =>
        {
            h.Username("test_user");
            h.Password("user");
        });
        config.ReceiveEndpoint("create-order", cfg =>
        {
            cfg.ConfigureConsumer<CreateOrderConsumer>(provider);
        });
    }));
    
});


var app = builder.Build();

var hangfireConfiguration = app.Services.GetService<HangfireConfiguration>();

hangfireConfiguration.ScheduleUpdateTask();

var bus = app.Services.GetRequiredService<IBusControl>();

bus.Start();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();