﻿using System;
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
    /// Логика взаимодействия для ClientViewRequestWindow.xaml
    /// </summary>
    public partial class ClientViewRequestWindow : Window
    {
        private RepairDbEntities db;
        private int _clientID;
        public ClientViewRequestWindow(int ClientID)
        {
            InitializeComponent();
            db = new RepairDbEntities();
            _clientID = ClientID;
            mainDtg.ItemsSource = db.Request.Where(u => u.userID2 == _clientID).ToList();
            
            
        }
    }
}
