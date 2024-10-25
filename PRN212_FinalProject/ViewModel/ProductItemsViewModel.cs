using PRN212_FinalProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_FinalProject.ViewModel
{
    public class ProductItemsViewModel : BaseViewModel
    {
        private DBContext db;
        public ObservableCollection<Entities.ProductItem> ProductItems { get; set; }

        public ProductItemsViewModel()
        {
            db = new DBContext();
            LoadProductItem("P0000001");
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

        public static int CalculatePriceAfterDiscount(int? SellingPrice, decimal? discount)
        {
            var d = (discount == null) ? 0 : discount;
            var s = (SellingPrice == null) ? 0 : SellingPrice;
            return (int)(s - (s * d));
        }

        public int? CalculateProfit(int? PriceAfterDiscount, int? ImportPrice)
        {
            return PriceAfterDiscount - ImportPrice;
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
                Ram = GetProductVariationOption(p.Id, "Ram"),
                Storage = GetProductVariationOption(p.Id, "Storage"),
                PriceAfterDiscount = CalculatePriceAfterDiscount(p.SellingPrice, p.Discount / 100),
                Profit = CalculateProfit(CalculatePriceAfterDiscount(p.SellingPrice, p.Discount / 100), p.ImportPrice)
            }).ToList();

            return result;
        }



        public void LoadProductItem(string ProductId)
        {
            var querry = GetProductItem(ProductId);
            ProductItems = new ObservableCollection<Entities.ProductItem>(querry);
        }
    }
}
