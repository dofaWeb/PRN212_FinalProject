
using Microsoft.EntityFrameworkCore.Diagnostics;
using PRN212_FinalProject.Entities;
using PRN212_FinalProject.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PRN212_FinalProject.ViewModel
{
    internal class SupplierViewModel : BaseViewModel
    {
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public SupplierViewModel()
        {
            LoadData();
        }
        void LoadData()
        {
            DBContext dBContext = new DBContext();
            Suppliers = new ObservableCollection<Supplier>(dBContext.Suppliers.ToList());
        }


    }
}
