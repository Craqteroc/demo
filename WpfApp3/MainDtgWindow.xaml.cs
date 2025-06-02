using Microsoft.Win32;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Drawing; 
using System.Drawing.Imaging;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainDtgWindow.xaml
    /// </summary>
    public partial class MainDtgWindow : Window
    {
        private RepairDbEntities db;
        public MainDtgWindow()
        {
            InitializeComponent();
            db = new RepairDbEntities();
            mainDtg.ItemsSource = db.Request.ToList();
        }

        private void AddRequest_Click(object sender, RoutedEventArgs e)
        {
            AddRequestWindow addRequestWindow = new AddRequestWindow();
            addRequestWindow.ShowDialog();

            mainDtg.ItemsSource = db.Request.ToList();

        }

        private void EditRequest_Click(object sender, RoutedEventArgs e)
        {
            if (mainDtg.SelectedItem is Request selectedRequest)
            {
                RequestEditWindow requestEditWindow = new RequestEditWindow(selectedRequest);
                requestEditWindow.ShowDialog();

                mainDtg.ItemsSource = db.Request.ToList();
            }
            else
            {
                MessageBox.Show("Выберите завку");
            }
        }

        private void SerchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SerchBox.Text.ToLower();

            var filteredRequests = db.Request
                .Where(r => (r.carModel ?? "").ToLower().Contains(searchText))
                .ToList();

            mainDtg.ItemsSource = filteredRequests;
        }

        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {

            var doneRequests = db.Request
        .Where(r => r.requestStatus == "Готова к выдаче")
        .ToList();

            int doneRequestsCount = doneRequests.Count;

            var completedWithDates = doneRequests
                .Where(r => r.startDate.HasValue && r.completionDate.HasValue)
                .ToList();

            double avgTime = 0;

            if (completedWithDates.Any())
            {
                avgTime = completedWithDates
                    .Average(r => (r.completionDate.Value - r.startDate.Value).TotalMinutes);
            }

            var problemStats = db.Request
                .GroupBy(r => r.problemDescryption)
                .Select(g => new { ProblemDescription = g.Key, Count = g.Count() })
                .ToList();

            var reportLines = new List<string>
            {
                "Отчет по заявкам",
                "",
                $"Количество выполненных заявок: {doneRequestsCount}",
                completedWithDates.Any()
                    ? $"Среднее время выполнения заявки: {Math.Round(avgTime)} минут"
                    : "Среднее время выполнения заявки: Н/Д (не заполнены даты)",
                "",
                "Статистика по типам неисправностей:"
            };

            foreach (var stat in problemStats)
            {
                reportLines.Add($"- {stat.ProblemDescription}: {stat.Count} заявок");
            }

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Отчет.txt",
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                Title = "Сохранить отчет"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string path = saveFileDialog.FileName;
                System.IO.File.WriteAllLines(path, reportLines);
                MessageBox.Show("Отчет успешно сохранен!");
            }
        }
        
        //ВТОРОЙ ВАРИАНТ СОХРАНЕНИЯ ВСЕГО DATAGRID

        //private void SaveReportByStatus(string status)
        //{
        //    var filteredRequests = db.Request.Where(r => r.requestStatus == status).ToList();
        //    int totalCount = filteredRequests.Count;

        //    var completed = filteredRequests.Where(r => r.startDate.HasValue && r.completionDate.HasValue).ToList();

        //    double? avgDuration = completed.Any()
        //        ? (double?)completed.Average(r => (r.completionDate.Value - r.startDate.Value).TotalMinutes)
        //        : null;

        //    var problemStats = db.Request
        //        .GroupBy(r => r.problemDescryption)
        //        .Select(g => new { ProblemDescription = g.Key, Count = g.Count() })
        //        .ToList();

        //    var lines = new List<string>
        //        {
        //            $"Отчет по заявкам со статусом \"{status}\"",
        //            $"Всего заявок: {totalCount}",
        //            avgDuration.HasValue
        //                ? $"Среднее время выполнения: {Math.Round(avgDuration.Value)} минут"
        //                : "Среднее время выполнения: Н/Д (не заполнены даты)",
        //            "",
        //            "Статистика по неисправностям:"
        //        };

        //    lines.AddRange(problemStats.Select(s => $"- {s.ProblemDescription}: {s.Count} заявок"));

        //    var saveDialog = new Microsoft.Win32.SaveFileDialog
        //    {
        //        FileName = $"Отчет_{status}.txt",
        //        Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
        //        Title = "Сохранить отчет"
        //    };

        //    if (saveDialog.ShowDialog() == true)
        //    {
        //        System.IO.File.WriteAllLines(saveDialog.FileName, lines);
        //        MessageBox.Show("Отчет успешно сохранен!");
        //    }
        //}

        private void SaveqQR_Click(object sender, RoutedEventArgs e)
        {
            string staticText = "https://qr-online.ru/?ysclid=mbdtd8goep72518470";

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PNG файлы (*.png)|*.png",
                FileName = "QRCode.png"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrData = qrGenerator.CreateQrCode(staticText, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrData);
                    Bitmap qrImage = qrCode.GetGraphic(20);

                    qrImage.Save(saveDialog.FileName, ImageFormat.Png);
                    MessageBox.Show("QR-код успешно сохранён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
            }
        }
    }
    
}
