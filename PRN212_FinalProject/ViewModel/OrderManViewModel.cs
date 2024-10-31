using PRN212_FinalProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_FinalProject.ViewModel
{
    public class OrderManViewModel : BaseViewModel
    {
        private DBContext db;
        public ObservableCollection<Order> Orders { get; set; }
        public ObservableCollection<OrderState> OrderStates { get; set; }

        public OrderManViewModel()
        {
            db = new DBContext();
            LoadOrder();
        }

        private Order _selectedItem;
        public Order SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    IdInfo = _selectedItem.Id.ToString();
                    UserIdInfo = _selectedItem.User.Id.ToString();
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


        private string _idInfo;
        public string IdInfo
        {
            get => _idInfo;
            set { _idInfo = value; OnPropertyChanged(nameof(IdInfo)); }
        }

        private string _userIdInfo;
        public string UserIdInfo
        {
            get => _userIdInfo;
            set { _userIdInfo = value; OnPropertyChanged(nameof(UserIdInfo)); }
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
        public void LoadOrder()
        {
            var orders = from o in db.Orders
                         join os in db.OrderStates on o.StateId equals os.Id
                         join pi in db.ProductItems on o.ProductItemId equals pi.Id
                         join p in db.Products on pi.ProductId equals p.Id
                         join a in db.Accounts on o.UserId equals a.Id
                         join pc in db.ProductConfigurations on pi.Id equals pc.ProductItemId
                         join vo in db.VariationOptions on pc.VariationOptionId equals vo.Id
                         join va in db.Variations on vo.VariationId equals va.Id
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
            OrderStates = new ObservableCollection<OrderState>(db.OrderStates.ToList());
        }


    }
}
