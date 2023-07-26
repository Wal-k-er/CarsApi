using AutoMapper;
using Cars.Dto;
using Cars.Models;

namespace Cars.Helper;

public class MappingProfiles: Profile
{
    public MappingProfiles()
    {
        CreateMap<Car, CarDto>();
        CreateMap<CarDto, Car>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Country, CountryDto>();
        CreateMap<CountryDto, Country >();
        CreateMap<Customer, CustomerDto>();
        CreateMap<CustomerDto, Customer>();
        CreateMap<ManufactureDto, Manufacture >();
        
        CreateMap<Manufacture, ManufactureDto>()
            .ForMember(m=>
                    m.ManufacturedCars, 
                opt=>
                    opt.MapFrom(m=>m.CarManufactures));
        
        CreateMap<CarManufacture, CarManufactureDto>()
                    .ForMember(oi => oi.CarId, 
                        opt => 
                            opt.MapFrom(p => p.Car.Id));
        
        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(oi => oi.CarId, 
                opt => 
                    opt.MapFrom(p => p.Car.Id));
        
        CreateMap<Order, OrderDto>()
            .ForMember(o => 
                o.CreatedDate, opt => 
                opt.MapFrom(d => d.CreatedDate))
            .ForMember(oi => 
                oi.OrderItems, opt => 
                opt.MapFrom(o => o.OrderItems));
        
    }
}