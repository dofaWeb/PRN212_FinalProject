using Microsoft.EntityFrameworkCore;
using PRN212_FinalProject.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_FinalProject.ViewModel
{
    class ProductViewModel: BaseViewModel
    {
        public ObservableCollection<Entities.Product> Products { get; set; }
        Entities.DBContext db;

        public ProductViewModel() {
            db = new DBContext();
            LoadProducts();
        }

        private void LoadProducts() {
            
            var querry = from p in db.Products
                         join c in db.Categories on p.CategoryId equals c.Id
                         join s in db.Suppliers on p.SupplierId equals s.Id
                         select new Entities.Product
                         {
                             Id = p.Id,
                             Name = p.Name,
                             Picture = p.Picture,
                             Description = p.Description,
                             Category= p.Category,
                            Supplier= p.Supplier,
                         };
            Products = new ObservableCollection<Entities.Product>(querry.ToList());
        }
    }
}
