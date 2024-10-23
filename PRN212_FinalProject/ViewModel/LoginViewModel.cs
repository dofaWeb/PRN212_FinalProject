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
    public class LoginViewModel
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

            var RoleId = db.Accounts.Where(a => a.Username == UserName && a.Password == Password).Select(a => a.RoleId).FirstOrDefault();

            switch (RoleId)
            {
                case "Role0001":
                    Admin adminWindow = new Admin();
                    adminWindow.Show();
                    break;


                case "Role0002":
                    User userWindow = new User();
                    userWindow.Show();
                    break;


                default:
                    MessageBox.Show("UserName or PassWord invalid");
                    break;



            }



        }
    }
}
