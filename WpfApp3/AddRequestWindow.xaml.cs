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
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для AddRequestWindow.xaml
    /// </summary>
    public partial class AddRequestWindow : Window
    {
        private RepairDbEntities db;
        public AddRequestWindow()
        {
            InitializeComponent();
            db = new RepairDbEntities();

            MasterCb.ItemsSource = db.User.Where(u => u.type == "Автомеханик").ToList();
            MasterCb.DisplayMemberPath = "fio";
            MasterCb.SelectedValuePath = "userID";

            ClientCb.ItemsSource = db.User.Where(u => u.type == "Заказчик").ToList();
            ClientCb.DisplayMemberPath = "fio";
            ClientCb.SelectedValuePath = "userID";
        }

        private void AddRequestBt_Click(object sender, RoutedEventArgs e)
        {
            var newRequest = new Request
            {
                startDate = DatePic.SelectedDate.Value,
                carType = TypeBt.Text,
                carModel = ModelBt.Text,
                problemDescryption = ProblemBt.Text,
                userID = (int)MasterCb.SelectedValue,
                userID2 = (int)ClientCb.SelectedValue
            };

            db.Request.Add(newRequest);
            db.SaveChanges();

            this.Close();
        }
    }
}
