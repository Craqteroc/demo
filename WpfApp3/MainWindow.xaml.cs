using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RepairDbEntities db;
        public MainWindow()
        {
            InitializeComponent();
            db = new RepairDbEntities();
        }

        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
           string tblog = TbLogin.Text;
            string tbpas = PasswordBx.Password;

            var auth = db.User.FirstOrDefault(u => u.login == tblog && u.password == tbpas);

            if (auth.type == "Менеджер")
            {
                MainDtgWindow mainDtgWindow = new MainDtgWindow();
                mainDtgWindow.Show();
                this.Close();
            }
            else if(auth.type == "Заказчик")
            {
                
                ClientViewRequestWindow clientViewRequestWindow = new ClientViewRequestWindow(auth.userID);
                clientViewRequestWindow.Show();
                this.Close();
            }
        }
    }
}
