using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    public class ProductItemViewModel : BaseViewModel
    {
        private DBContext db;

        public ICommand AddProductItemCommand { get; }
        public ICommand EditProductItemCommand { get; }
        public ICommand DeleteProductItemCommand { get; }

        public ObservableCollection<Entities.ProductItem> ProductItems { get; set; }

        private string ProductId = "";

        public ProductItemViewModel(string productId)
        {
            db = new DBContext();
            AddProductItemCommand = new RelayCommand(AddProductItem);
            EditProductItemCommand = new RelayCommand(EditProductItem);
            DeleteProductItemCommand = new RelayCommand(DeleteProductItem);
            ProductId = productId;
            LoadProductItem(productId);
        }

        // New SelectedItem property to handle item selection in the UI
        private ProductItem _selectedItem;
        public ProductItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem != null)
                {
                    // Update input fields with the selected item's properties
                    IdInfo = _selectedItem.Id;
                    QuantityInfo = _selectedItem.Quantity.ToString();
                    ImportPriceInfo = _selectedItem.ImportPrice.ToString();
                    SellingPriceInfo = _selectedItem.SellingPrice.ToString();
                    DiscountInfo = _selectedItem.Discount.ToString();
                    RamInfo = GetProductVariationOption(_selectedItem.Id, "Ram");
                    StorageInfo = GetProductVariationOption(_selectedItem.Id, "Storage");
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        // Properties for binding input fields
        private string _idInfo;
        public string IdInfo
        {
            get => _idInfo;
            set { _idInfo = value; OnPropertyChanged(nameof(IdInfo)); }
        }

        private string _quantityInfo;
        public string QuantityInfo
        {
            get => _quantityInfo;
            set { _quantityInfo = value; OnPropertyChanged(nameof(QuantityInfo)); }
        }

        private string _importPriceInfo;
        public string ImportPriceInfo
        {
            get => _importPriceInfo;
            set { _importPriceInfo = value; OnPropertyChanged(nameof(ImportPriceInfo)); }
        }

        private string _sellingPriceInfo;
        public string SellingPriceInfo
        {
            get => _sellingPriceInfo;
            set { _sellingPriceInfo = value; OnPropertyChanged(nameof(SellingPriceInfo)); }
        }

        private string _discountInfo;
        public string DiscountInfo
        {
            get => _discountInfo;
            set { _discountInfo = value; OnPropertyChanged(nameof(DiscountInfo)); }
        }

        private string _ramInfo;
        public string RamInfo
        {
            get => _ramInfo;
            set { _ramInfo = value; OnPropertyChanged(nameof(RamInfo)); }
        }

        private string _storageInfo;
        public string StorageInfo
        {
            get => _storageInfo;
            set { _storageInfo = value; OnPropertyChanged(nameof(StorageInfo)); }
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

        public void LoadProductItem(string productId)
        {
            var query = GetProductItem(productId);
            ProductItems = new ObservableCollection<Entities.ProductItem>(query);
            OnPropertyChanged(nameof(ProductItems));
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

        public string getNewProducConfigurationID()
        {
            string lastId = db.ProductConfigurations
                .OrderByDescending(a => a.Id)
                .Select(a => a.Id)
                .FirstOrDefault();

            if (lastId == null)
            {
                return "Pc000001";
            }

            string prefix = lastId.Substring(0, 2);
            int number = int.Parse(lastId.Substring(2));

            int newNumber = number + 1;
            string newId = $"{prefix}{newNumber:D6}";

            return newId;
        }

        public Boolean checkExistVariation(string productId, string variationOpId1, string variationOpId2)
        {
            var v1 = (from p in db.Products
                      join pi in db.ProductItems on p.Id equals pi.ProductId
                      join pc in db.ProductConfigurations on pi.Id equals pc.ProductItemId
                      where p.Id == productId && pc.VariationOptionId == variationOpId1
                      select p).Any();
            var v2 = (from p in db.Products
                      join pi in db.ProductItems on p.Id equals pi.ProductId
                      join pc in db.ProductConfigurations on pi.Id equals pc.ProductItemId
                      where p.Id == productId && pc.VariationOptionId == variationOpId2
                      select p).Any();
            if (v1 && v2)
            {
                return true;
            }
            return false;
        }


        public string GetVariationId(string name)
        {
            var id = db.Variations.Where(p => p.Name == name).Select(p => p.Id).FirstOrDefault();
            return id;
        }

        public string GetVariationOptionId(string value, string varationId)
        {
            var id = db.VariationOptions.Where(p => p.Value == value && p.VariationId == varationId).Select(p => p.Id).FirstOrDefault();
            return id;
        }
        public void AddProductConfiguration(ProductItem model)
        {
            var variation_option_id1 = GetVariationOptionId(model.Ram, GetVariationId("Ram"));
            var variation_option_id2 = GetVariationOptionId(model.Storage, GetVariationId("Storage"));

            var id = getNewProducConfigurationID();

            db.ProductConfigurations.Add(new Entities.ProductConfiguration
            {
                Id = id,
                ProductItemId = model.Id,
                VariationOptionId = variation_option_id1,
            });
            db.SaveChanges();

            id = getNewProducConfigurationID();
            db.ProductConfigurations.Add(new Entities.ProductConfiguration
            {
                Id = id,
                ProductItemId = model.Id,
                VariationOptionId = variation_option_id2,
            });
            db.SaveChanges();

        }

        public void AddProductItem(object parameter)
        {
            var newProductItem = new Entities.ProductItem
            {
                Id = getNewProductId(),
                Quantity = int.Parse(QuantityInfo),
                ImportPrice = int.Parse(ImportPriceInfo),
                SellingPrice = int.Parse(SellingPriceInfo),
                Discount = decimal.Parse(DiscountInfo),
                ProductId = ProductId
            };
            string Ram = RamInfo;
            string storage = StorageInfo;
            var variation_option_id1 = GetVariationOptionId(Ram, GetVariationId("Ram"));
            var variation_option_id2 = GetVariationOptionId(storage, GetVariationId("Storage"));
            if (!checkExistVariation(ProductId, variation_option_id1, variation_option_id2))
            {
                db.ProductItems.Add(newProductItem);
                newProductItem.Storage = storage;
                newProductItem.Ram = Ram;
                db.SaveChanges();

                AddProductConfiguration(newProductItem);

                newProductItem.PriceAfterDiscount = CalculatePriceAfterDiscount(newProductItem.SellingPrice, newProductItem.Discount / 100);
                newProductItem.Profit = CalculateProfit(newProductItem.PriceAfterDiscount, newProductItem.ImportPrice);
                ProductItems.Add(newProductItem);
            }
            ClearFields();
        }

        public void EditProductItem(object parameter)
        {
            if (SelectedItem != null)
            {
                var existingItem = db.ProductItems.FirstOrDefault(p => p.Id == SelectedItem.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity = int.Parse(QuantityInfo);
                    existingItem.ImportPrice = int.Parse(ImportPriceInfo);
                    existingItem.SellingPrice = int.Parse(SellingPriceInfo);
                    existingItem.Discount = decimal.Parse(DiscountInfo); // For decimal valuesexistingItem.Discount = int.Parse(DiscountInfo);

                    db.SaveChanges();
                    LoadProductItem(existingItem.ProductId);
                }
            }
        }

        public void DeleteProductItem(object parameter)
        {
            if (SelectedItem != null)
            {
                var pc = db.ProductConfigurations.Where(p => p.ProductItemId == SelectedItem.Id);
                var existingItem = db.ProductItems.FirstOrDefault(p => p.Id == SelectedItem.Id);
                if (pc != null && existingItem != null)
                {
                    foreach (var p in pc)
                    {
                        db.ProductConfigurations.Remove(p);
                    }
                    db.ProductItems.Remove(existingItem);
                    db.SaveChanges();
                    ProductItems.Remove(SelectedItem);
                }
            }
        }

        public string getNewProductId()
        {
            string lastId = db.ProductItems
                .OrderByDescending(a => a.Id)
                .Select(a => a.Id)
                .FirstOrDefault();

            if (lastId == null)
            {
                return "Pi000001";
            }

            string prefix = lastId.Substring(0, 2);
            int number = int.Parse(lastId.Substring(2));

            int newNumber = number + 1;
            string newId = $"{prefix}{newNumber:D6}";

            return newId;
        }

        private void ClearFields()
        {
            IdInfo = string.Empty;
            QuantityInfo = string.Empty;
            ImportPriceInfo = string.Empty;
            SellingPriceInfo = string.Empty;
            DiscountInfo = string.Empty;
            RamInfo = string.Empty;
            StorageInfo = string.Empty;
        }
    }

}
