using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    public class ProfileViewModel
    {
        Entities.DBContext db;

        public ICommand UpdateCustomerCommand { get; }
        public Entities.Account Profile { get; set; }

        public ProfileViewModel()
        {
            db = new Entities.DBContext();
            UpdateCustomerCommand = new RelayCommand(UpdateCustomer);   
        }

        public ProfileViewModel(string customerId) : this() 
        {
            LoadProfile(customerId);
            
        }

        public void LoadProfile(string customerId)
        {
            db = new Entities.DBContext();
            Profile = db.Accounts.Where(p => p.Id == customerId).FirstOrDefault();
        }

        public void UpdateCustomer(object obj)
        {
            db.Accounts.Update(Profile);
            db.SaveChanges();
        }

    }
}
