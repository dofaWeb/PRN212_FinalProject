﻿using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN212_FinalProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_FinalProject.ViewModel
{
    public class UserOrderHistoryViewModel : BaseViewModel
    {
        public Entities.Account Account { get; set; }
        DBContext dbContext;
        public ObservableCollection<Order> Orders { get; set; }
        public UserOrderHistoryViewModel(Entities.Account acc)
        {
            Account = acc;
            LoadData();
        }

        void LoadData()
        {
            using (dbContext = new DBContext())
            {
                var orders = from o in dbContext.Orders
                             join os in dbContext.OrderStates on o.StateId equals os.Id
                             join pi in dbContext.ProductItems on o.ProductItemId equals pi.Id
                             join p in dbContext.Products on pi.ProductId equals p.Id
                             join a in dbContext.Accounts on o.UserId equals a.Id
                             join pc in dbContext.ProductConfigurations on pi.Id equals pc.ProductItemId
                             join vo in dbContext.VariationOptions on pc.VariationOptionId equals vo.Id
                             join va in dbContext.Variations on vo.VariationId equals va.Id
                             where o.UserId == Account.Id
                             group new { va.Name, vo.Value } by new
                             {
                                 OrderId = o.Id,
                                 o.Date,
                                 o.Price,
                                 OrderStateName = os.Name,     // Renamed to avoid conflict
                                 AccountId = a.Id,
                                 AccountName = a.Name,         // Renamed to avoid conflict
                                 ProductName = p.Name          // Renamed to avoid conflict
                             } into g
                             select new Order
                             {
                                 Id = g.Key.OrderId,
                                 Date = g.Key.Date,
                                 Price = g.Key.Price,
                                 State = new OrderState
                                 {
                                     Name = g.Key.OrderStateName
                                 },
                                 User = new Entities.Account
                                 {
                                     Id = g.Key.AccountId,
                                     Name = g.Key.AccountName
                                 },
                                 ProductItem = new ProductItem
                                 {
                                     Product = new Product
                                     {
                                         Name = g.Key.ProductName
                                     },
                                     Option = "Ram: " + g.Where(x => x.Name == "Ram").Select(x => x.Value).FirstOrDefault() +
                                              " Storage: " + g.Where(x => x.Name == "Storage").Select(x => x.Value).FirstOrDefault()
                                 },

                             };

                Orders = new ObservableCollection<Order>(orders.ToList());
                OnPropertyChanged(nameof(Orders));
            }
        }

        private string _userNameInfo;
        public string UserNameInfo
        {
            get => _userNameInfo;
            set { _userNameInfo = value; OnPropertyChanged(nameof(UserNameInfo)); }
        }

        private string _productNameInfo;
        public string ProductNameInfo
        {
            get => _productNameInfo;
            set { _productNameInfo = value; OnPropertyChanged(nameof(ProductNameInfo)); }
        }

        public DateTime _dateInfor;
        public DateTime DateInfor
        {
            get => _dateInfor;
            set { _dateInfor = value; OnPropertyChanged(nameof(DateInfor)); }
        }

        public string _variationInfor;
        public string VariationInfor
        {
            get => _variationInfor;
            set { _variationInfor = value; OnPropertyChanged(nameof(VariationInfor)); }
        }

        public int _priceInfor;
        public int PriceInfor
        {
            get => _priceInfor;
            set { _priceInfor = value; OnPropertyChanged(nameof(PriceInfor)); }
        }

        public OrderState _stateInfor;
        public OrderState StateInfor
        {
            get => _stateInfor;
            set { _stateInfor = value; OnPropertyChanged(nameof(StateInfor)); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged(nameof(Date)); }
        }

        private Entities.Order _selectedItem;

        public Entities.Order SelectedItem
        {
            get { return _selectedItem; }
            set {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    UserNameInfo = _selectedItem.User.Name;
                    ProductNameInfo = _selectedItem.ProductItem.Product.Name;
                    DateInfor = _selectedItem.Date;
                    VariationInfor = _selectedItem.ProductItem.Option;
                    PriceInfor = _selectedItem.Price;
                    StateInfor = _selectedItem.State;
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
    }
}