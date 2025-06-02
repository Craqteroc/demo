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

        //ВТОРОЙ ВАРИАНТ СОХРАНЕНИЯ ОТЧЕТА

        //private void SaveMaterialMinBatchReport_Click(object sender, RoutedEventArgs e)
        //{
        //    var materials = db.Materials.ToList();

        //    var reportLines = new List<string>
        //        {
        //            "Отчет по стоимости минимальной партии материалов",
        //            "",
        //            "Наименование\tТип\tНа складе\tМин. ост.\tВ уп.\tЦена/ед.\tСтоимость мин. партии (руб.)"
        //        };

        //    foreach (var material in materials)
        //    {
        //        decimal stock = material.stockQuantity;
        //        decimal minRequired = material.minRequiredQuantity;
        //        decimal unitPrice = material.unitPrice;
        //        decimal packageQty = material.packageQuantity;

        //        decimal cost = 0.00m;

        //        if (stock < minRequired)
        //        {
        //            decimal deficit = minRequired - stock;
        //            decimal packagesNeeded = Math.Ceiling(deficit / packageQty);
        //            decimal purchaseQty = packagesNeeded * packageQty;
        //            cost = purchaseQty * unitPrice;
        //        }

        //        // Безопасное округление
        //        string costFormatted = cost.ToString("F2");

        //        reportLines.Add($"{material.name}\t{material.type}\t{stock}\t{minRequired}\t{packageQty}\t{unitPrice:F2}\t{costFormatted}");
        //    }

        //    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
        //    {
        //        FileName = "МинимальнаяПартия_Материалы.txt",
        //        Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
        //        Title = "Сохранить отчет"
        //    };

        //    if (saveFileDialog.ShowDialog() == true)
        //    {
        //        string path = saveFileDialog.FileName;
        //        System.IO.File.WriteAllLines(path, reportLines);
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
