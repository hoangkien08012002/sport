﻿using Microsoft.Extensions.Configuration;
using System.Security.Principal;
using HightSportShopBusiness;
using System.Security.Claims;
using HightSportShopBusiness.Dtos;
using HightSportShopBusiness.Models;
using HightSportShopBusiness.Interfaces;
namespace HightSportShopBusiness.Services
{

    public interface IUserOrderService
    {
        public List<UserOrderDto> GetUserOrders(int userId);
    }

    public class UserOrderService : IUserOrderService
    {
        private readonly HighSportShopDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public UserOrderService(HighSportShopDBContext context, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _context = context;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public List<UserOrderDto> GetUserOrders(int userId)
        {

            var orders = _context.Orders
               .Where(o => o.UserId == userId)
               .Select(o => new UserOrderDto
               {
                   OrderId = o.Id,
                   TotalPrice = o.TotalPrice,
                   OrderDate = o.ReceivedDate ?? DateTime.Now,
                   Status = o.Status == 1 ? "Success" : "Cancel",
                   Products = o.OrderDetails.Select(od => new ProductInOrderDto
                   {
                       ProductId = od.ProductId ?? 0,
                       ProductName = od.Product.ProductName,
                       Quantity = od.Quantity ?? 0,
                       Price = od.Price ?? 0M,
                       Size = od.Product.Size,
                       Description = od.Product.Description,
                       Color = od.Product.Color,
                       Offers = od.Product.Offers,
                       MainImageName = od.Product.MainImageName,
                       MainImagePath = od.Product.MainImagePath
                   }
                   ).ToList()
               })
               .ToList();
            return orders;
        }
    }
}
