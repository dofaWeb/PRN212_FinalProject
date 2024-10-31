using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    public class ProductDetailViewModel : BaseViewModel
    {
        public ObservableCollection<ProductItem> ProductItems { get; set; }

        public Product Product { get; set; }

        public ProductItem ProductItemSelected { get; set; }

        DBContext db;

        public ICommand AddToCartCommand { get; }

        private readonly string userId;
        public ProductDetailViewModel(string productId, string userId)
        {
            db = new DBContext();
            LoadData(productId);
            AddToCartCommand = new RelayCommand(AddToCart);
            this.userId = userId;
        }
        public string GetProductVariationOption(string productItemId, string option)
        {
            var varianceValue = (from pc in db.ProductConfigurations
                                 join vo in db.VariationOptions on pc.VariationOptionId equals vo.Id
                                 join va in db.Variations on vo.VariationId equals va.Id
                                 where pc.ProductItemId == productItemId && va.Name == option
                                 select vo.Value).FirstOrDefault();

            return varianceValue; // Return empty string if result is null
        }

        public static int CalculatePriceAfterDiscount(int? sellingPrice, decimal? discount)
        {
            var d = discount ?? 0;
            var s = sellingPrice ?? 0;
            return (int)(s - (s * d));
        }

        public int? CalculateProfit(int? priceAfterDiscount, int? importPrice)
        {
            return priceAfterDiscount - importPrice;
        }

        public List<ProductItem> GetProductItem(string productId)
        {
            var productItems = db.ProductItems.Where(p => p.ProductId == productId).ToList();

            var result = productItems.Select(p => new ProductItem
            {
                Id = p.Id,
                Quantity = p.Quantity,
                ImportPrice = p.ImportPrice,
                SellingPrice = p.SellingPrice,
                Discount = p.Discount,
                ProductId = productId,
                Option = "Ram: " + GetProductVariationOption(p.Id, "Ram") + " Storage: " + GetProductVariationOption(p.Id, "Storage"),
                PriceAfterDiscount = CalculatePriceAfterDiscount(p.SellingPrice, p.Discount / 100),
                Profit = CalculateProfit(CalculatePriceAfterDiscount(p.SellingPrice, p.Discount / 100), p.ImportPrice)
            }).ToList();

            return result;
        }

        private ProductItem _productItemInfor;
        public ProductItem ProductItemInfor
        {
            get { return _productItemInfor; }
            set { _productItemInfor = value; OnPropertyChanged(nameof(_productItemInfor)); }
        }
        void LoadData(string productId)
        {
            using (db)
            {
                Product = db.Products.Where(p=> p.Id == productId).FirstOrDefault();
                Product.Picture = $"pack://application:,,,/Images/{Product.Picture}";
                OnPropertyChanged(nameof(Product));
                var query = GetProductItem(productId);

                ProductItems = new ObservableCollection<ProductItem>(query);
            }
        }

        public string GetNewOrderId()
        {
            string lastId = db.Orders.OrderByDescending(a => a.Id).Select(a => a.Id).FirstOrDefault();
            if (lastId == null) return "O0000001";

            string prefix = lastId.Substring(0, 1);
            int number = int.Parse(lastId.Substring(1));
            return $"{prefix}{(number + 1):D7}";
        }

        public int? GetPrice(string productItemId)
        {
            var price = db.ProductItems.Where(p => p.Id == productItemId).Select(p => new
            {
                Price = ProductItemViewModel.CalculatePriceAfterDiscount(p.SellingPrice,p.Discount/100)
            }).FirstOrDefault();
            return price.Price;
        }

        public void AddOrder(string productItemId)
        {
            string orderId = GetNewOrderId();
            string user_id = userId;


            var newOrder = new Order
            {
                Date = DateTime.Now,
                Id = orderId,
                UserId = user_id,
                ProductItemId = productItemId,
                Price = GetPrice(productItemId) ?? 0,
                StateId = "1",
            };

            db.Orders.Add(newOrder);
        }

        public void AddToCart(object parameter)
        {
            var proItemId = ProductItemInfor?.Id;
            if (proItemId != null)
            {
                AddOrder(proItemId);
            }
        }
    }
}
