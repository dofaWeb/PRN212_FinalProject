using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    class ProductViewModel : BaseViewModel
    {
        public ICommand AddProductCommand { get; }
        public ICommand EditProductCommand { get; }
        public ICommand DeleteProductCommand { get; }

        public ICommand SelectPictureCommand { get; }
        public ObservableCollection<Entities.Product> Products { get; set; }
        public ObservableCollection<Category> CategoryList { get; set; }
        public ObservableCollection<Supplier> SupplierList { get; set; }
        private Entities.DBContext db;

        public ProductViewModel()
        {
            db = new DBContext();
            AddProductCommand = new RelayCommand(AddProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            EditProductCommand = new RelayCommand(UpdateProduct);
            SelectPictureCommand = new RelayCommand(param => SelectPicture()); // 
            LoadProducts();
            LoadCategory();
            LoadSupplier();
        }

        public void SelectPicture()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                PictureInfo = System.IO.Path.GetFileName(openFileDialog.FileName); // chỉ lấy tên tệp
                                                                                   // Hoặc lưu toàn bộ đường dẫn ảnh:
                                                                                   // PictureInfo = openFileDialog.FileName;
            }
        }

        public void LoadCategory()
        {
            var query = db.Categories.ToList();
            CategoryList = new ObservableCollection<Category>(query);
        }

        public void LoadSupplier()
        {
            var query = db.Suppliers.ToList();
            SupplierList = new ObservableCollection<Supplier>(query);
        }

        private string _IdInfo;
        public string IdInfo
        {
            get { return _IdInfo; }
            set { _IdInfo = value; OnPropertyChanged(nameof(IdInfo)); }
        }

        private string _NameInfo;
        public string NameInfo
        {
            get { return _NameInfo; }
            set { _NameInfo = value; OnPropertyChanged(nameof(NameInfo)); }
        }

        private string _DescriptionInfo;
        public string DescriptionInfo
        {
            get { return _DescriptionInfo; }
            set { _DescriptionInfo = value; OnPropertyChanged(nameof(DescriptionInfo)); }
        }

        private Category _categoryInfo;
        public Category CategoryInfo
        {
            get { return _categoryInfo; }
            set { _categoryInfo = value; OnPropertyChanged(nameof(CategoryInfo)); }
        }

        private Supplier _supplierInfo;
        public Supplier SupplierInfo
        {
            get { return _supplierInfo; }
            set
            {
                _supplierInfo = value; OnPropertyChanged(nameof(SupplierInfo));
            }
        }

        private string _pictureInfo;
        public string PictureInfo
        {
            get { return _pictureInfo; }
            set
            {
                _pictureInfo = value;
                OnPropertyChanged(nameof(PictureInfo));
                OnPropertyChanged(nameof(PictureFilePath)); // Notify when PictureInfo changes
            }
        }

        public string PictureFilePath
        {
            get
            {
                string basePath = "/img/"; // Adjust path to match your project structure
                return Path.Combine(basePath, PictureInfo); // Combines folder with filename
            }
        }

        private Product _selectItem;
        public Product SelectItem
        {
            get { return _selectItem; }
            set
            {
                _selectItem = value;
                if (_selectItem != null)
                {
                    IdInfo = _selectItem.Id;
                    NameInfo = _selectItem.Name;
                    DescriptionInfo = _selectItem.Description;
                    CategoryInfo = _selectItem.Category;
                    SupplierInfo = _selectItem.Supplier;
                    PictureInfo = _selectItem.Picture;  // Set filename for Picture
                }
                OnPropertyChanged(nameof(SelectItem));
            }
        }


        private void LoadProducts()
        {
            var query = from p in db.Products
                        join c in db.Categories on p.CategoryId equals c.Id
                        join s in db.Suppliers on p.SupplierId equals s.Id
                        select new Product
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Picture = p.Picture??"",
                            Description = p.Description,
                            Category = c,
                            Supplier = s,
                        };
            Products = new ObservableCollection<Product>(query.ToList());
        }

        public string GetNewProductId()
        {
            string lastId = db.Products.OrderByDescending(a => a.Id).Select(a => a.Id).FirstOrDefault();
            if (lastId == null) return "P0000001";

            string prefix = lastId.Substring(0, 1);
            int number = int.Parse(lastId.Substring(1));
            return $"{prefix}{(number + 1):D7}";
        }

        private void AddProduct(object parameter)
        {
            //// Ensure image is copied to the project’s Resources folder in the runtime directory.
            //string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            //Directory.CreateDirectory(resourcesPath);

            //// Get image name and paths
            //string imageName = Path.GetFileName(PictureInfo);
            //string targetPath = Path.Combine(resourcesPath, imageName);

            //// Define newTargetPath pointing to project Images folder (for database storage)
            //string projectBasePath = AppDomain.CurrentDomain.BaseDirectory;
            //string newTargetPath = Path.Combine(projectBasePath.Substring(0, projectBasePath.IndexOf("PRN212_FinalProject") + "PRN212_FinalProject".Length), "Images", imageName);

            //// Copy image if not already present in Resources folder (runtime directory)
            //if (!File.Exists(newTargetPath))
            //{
            //    File.Copy(PictureInfo, newTargetPath);
            //}

            // Set product properties and add to database (using relative path)
            var newProduct = new Product
            {
                Id = GetNewProductId(),
                Name = NameInfo,
                Description = DescriptionInfo,
                CategoryId = CategoryInfo?.Id,
                SupplierId = SupplierInfo?.Id,
                Picture = PictureInfo
            };

            db.Products.Add(newProduct);
            db.SaveChanges();
            Products.Add(newProduct);

            ClearInputFields();
            OnPropertyChanged(nameof(Products));
        }



        private void UpdateProduct(object parameter)
        {
            if (SelectItem != null)
            {
                var existingProduct = db.Products.Include(p => p.Category).Include(p => p.Supplier).FirstOrDefault(p => p.Id == SelectItem.Id);
                if (existingProduct != null)
                {
                    existingProduct.Name = NameInfo;
                    existingProduct.Description = DescriptionInfo;
                    existingProduct.CategoryId = CategoryInfo?.Id;
                    existingProduct.SupplierId = SupplierInfo?.Id;
                    existingProduct.Picture = PictureInfo;

                    db.SaveChanges();

                    Products[Products.IndexOf(SelectItem)] = existingProduct;
                    LoadProducts();
                    OnPropertyChanged(nameof(Products));
                }
            }
        }

        private void DeleteProduct(object parameter)
        {
            var productToDelete = db.Products.FirstOrDefault(p => p.Id == IdInfo);
            if (productToDelete != null)
            {
                db.Products.Remove(productToDelete);
                db.SaveChanges();
                Products.Remove(SelectItem);

                ClearInputFields();
                LoadProducts();
                OnPropertyChanged(nameof(Products));
            }
        }

       

        private void ClearInputFields()
        {
            IdInfo = string.Empty;
            NameInfo = string.Empty;
            DescriptionInfo = string.Empty;
            PictureInfo = string.Empty;
        }
    }
}
