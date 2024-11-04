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
                StateId = a.StateId,
            }).FirstOrDefault();

            if (acc != null)
            {
                if (acc.StateId == "S0000001")
                {
                    // Account is active, proceed based on role
                    switch (acc.RoleId)
                    {
                        case "Role0001":
                            Admin adminWindow = new Admin();
                            adminWindow.Show();
                            break;

                        case "Role0002":
                            UserViewModel viewModel = new UserViewModel(acc.Id);
                            User userWindow = new User(viewModel, acc.Id);
                            userWindow.Show();
                            break;

                        default:
                            MessageBox.Show("UserName or PassWord invalid");
                            break;
                    }

                    // Close the Login window
                    Application.Current.Windows.OfType<Login>().FirstOrDefault()?.Close();
                }
                else if (acc.StateId == "S0000002")
                {
                    // Account is banned
                    MessageBox.Show("Your account has been banned. Please log in with a different account.");
                }
                else
                {
                    // Handle other states if necessary
                    MessageBox.Show("Invalid account state.");
                }
            }
            else
            {
                MessageBox.Show("UserName or PassWord invalid");
            }
        }

    }
}