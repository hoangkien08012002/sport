﻿using HightSportShopBusiness.Dtos;
using HightSportShopBusiness.Interfaces;
using HightSportShopBusiness.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HightSportShopBusiness.Services
{
    public interface IDashboardService
    {
        public DashboardDto GetTotalRevenue(DateTime startDate, DateTime endDate);
        public DashboardDto GetTotalOrder(DateTime startDate, DateTime endDate);
        public DashboardDto GetTotalUser();
        public DashboardDto GetListOrder(DateTime startDate, DateTime endDate);
    }
    public class DashboardService : IDashboardService
    {

        private readonly HighSportShopDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        public DashboardService(HighSportShopDBContext context, IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _context = context;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }


        public DashboardDto GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            var revenue = new DashboardDto
            {
                TotalRevenue = _context.Orders.Where(o => o.ReceivedDate >= startDate && o.ReceivedDate <= endDate)
                 .Sum(o => o.TotalPrice),
            };
            return revenue;
        }

        public DashboardDto GetTotalOrder(DateTime startDate, DateTime endDate)
        {
            var order = new DashboardDto
            {
                TotalOrder = _context.Orders
                .Count(o => o.ReceivedDate >= startDate && o.ReceivedDate <= endDate)
            };
            return order;
        }

        public DashboardDto GetTotalUser()
        {
            var user = new DashboardDto
            {
                TotalUser = _context.Users.Count()
            };
            return user;
        }

        public DashboardDto GetListOrder(DateTime startDate, DateTime endDate)
        {
            List<OrderDto> orders = _context.Orders
                .Where(o => o.ReceivedDate >= startDate && o.ReceivedDate <= endDate)
                .Select(o => new OrderDto
                {
                    OrderId = o.Id,
                    Date = o.ReceivedDate,
                    CustomerName = o.User.FullName,
                    Status = o.Status,
                    TotalPrice = o.TotalPrice
                }).ToList();

            return new DashboardDto
            {  
                Orders = orders
            };
        }
    }
}


