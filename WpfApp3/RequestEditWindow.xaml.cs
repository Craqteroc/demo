using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для RequestEditWindow.xaml
    /// </summary>
    public partial class RequestEditWindow : Window
    {
        private RepairDbEntities db;
        private Request srequest;
        public RequestEditWindow(Request requestToEdit)
        {
            InitializeComponent();

            db = new RepairDbEntities();

            CbMaster.ItemsSource = db.User
        .Where(u => u.type == "Автомеханик")
        .ToList();

            CbMaster.DisplayMemberPath = "fio";
            CbMaster.SelectedValuePath = "userID";

            srequest = db.Request.Find(requestToEdit.requestID);

            if (srequest != null)
            {
                TypeBox.Text = srequest.carType;
                ModelBox.Text = srequest.carModel;
                ProblemBox.Text = srequest.problemDescryption;
                StatusBox.Text = srequest.requestStatus;
                PartsBox.Text = srequest.repairParts;
                CbMaster.SelectedValue = srequest.userID;
            }
        }

        private void SaveBt_Click(object sender, RoutedEventArgs e)
        {
            if (srequest != null)
            {
                srequest.carType = TypeBox.Text;
                srequest.carModel = ModelBox.Text;
                srequest.problemDescryption = ProblemBox.Text;
                srequest.requestStatus = StatusBox.Text;
                srequest.repairParts = PartsBox.Text;
                srequest.userID = (int)CbMaster.SelectedValue;

                
                db.SaveChanges();

                this.Close();
            }
        }
    }
}
