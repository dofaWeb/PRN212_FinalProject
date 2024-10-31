using Microsoft.EntityFrameworkCore;
using PRN212_FinalProject.Entities;
using PRN212_FinalProject.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PRN212_FinalProject.ViewModel
{
    class LoginViewModel
    {
        Entities.DBContext db;

        public Entities.Account LoginAVU { get; set; }


        public ICommand LoginCommand { get; set; }
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);

            LoginAVU = new Entities.Account();
            db = new Entities.DBContext();

        }

        public void Login(object parameter)
        {
            string UserName = LoginAVU.Username;
            string Password = LoginAVU.Password;

            var acc = db.Accounts.Where(a => a.Username == UserName && a.Password == Password).Select(a => new Entities.Account
            {
                Id = a.Id,
                RoleId = a.RoleId,
            }).FirstOrDefault();

            if (acc != null)
            {
                switch (acc.RoleId)
                {
                    case "Role0001":
                        Admin adminWindow = new Admin();
                        adminWindow.Show();
                        break;


                    case "Role0002":
                        UserViewModel viewModel = new UserViewModel(acc.Id);
                        User userWindow = new User(viewModel, acc.Id);

                        //Profile profileWindow = new Profile(acc.Id);
                        userWindow.Show();
                        //profileWindow.Show();

                        break;


                    default:
                        MessageBox.Show("UserName or PassWord invalid");
                        break;

                }
            }
            else
            {
                MessageBox.Show("UserName or PassWord invalid");
            }
        }
    }
}