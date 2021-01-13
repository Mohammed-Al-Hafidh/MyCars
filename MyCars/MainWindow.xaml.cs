using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace MyCars
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string fileName = @"..\..\mydata.txt";
        List<Car> cars = new List<Car>();        
        public MainWindow()
        {
            InitializeComponent();
            loadFile();
        }
       

        private void MenuItem_Delete(object sender, RoutedEventArgs e)
        {
            DeleteItem();
            updateStatusBar();
        }

        private void ResetValue()
        {
            lvList.Items.Refresh();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveFile();
        }

        private void MenuItem_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Add(object sender, RoutedEventArgs e)
        {
            AddItem();
            updateStatusBar();
        }

        private void lvList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            UpdateItem();
        }
        public void loadFile()
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                while (line != null)
                {
                    string[] myData = line.Split(';');
                    double engineSize;
                    if (!double.TryParse(myData[1], out engineSize))
                    {
                        MessageBox.Show("Input string error. Go to next line...", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        continue;
                    }
                    Car car = new Car(myData[0], engineSize, myData[2]);
                    cars.Add(car);
                    line = sr.ReadLine();
                }
                lvList.ItemsSource = cars;
                updateStatusBar();
            }
        }
        public void saveFile()
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (Car myCar in cars)
                {
                    writer.WriteLine(myCar.ToDataString());
                }
            }
        }
        public void updateStatusBar()
        {
            int carsNo = lvList.Items.Count;
            txtStatusBarText.Text = string.Format("You currently have {0} car(s)", carsNo);
        }
        private void AddItem()
        {
            string makeModel = "";
            double engineSize = 0.0;
            string fuelType = "";

            CarDialog cardialog = new CarDialog("Save", "", 0.0, "");
            cardialog.Owner = this;

            cardialog.BackValues += (a, b, c) => { makeModel = a; engineSize = b; fuelType = c; };
            bool? result = cardialog.ShowDialog();
            if (result == true)
            {
                cars.Add(new Car(makeModel, engineSize, fuelType));
                ResetValue();
            }
            else if (result == false)
            {
                MessageBox.Show("Request canceled", "Cancel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateItem()
        {
            if (lvList.SelectedIndex == -1)
            {
                return;
            }
            string makeModel = "";
            double engineSize = 0.0;
            string fuelType = "";

            Car carToBeUpdated = (Car)lvList.SelectedItem;

            CarDialog cardialog = new CarDialog("Update", carToBeUpdated.MakeModel, carToBeUpdated.EngineSize, carToBeUpdated.FuelType);
            cardialog.Owner = this;

            cardialog.BackValues += (a, b, c) => { makeModel = a; engineSize = b; fuelType = c; };
            bool? result = cardialog.ShowDialog();
            if (result == true)
            {
                carToBeUpdated.MakeModel = makeModel;
                carToBeUpdated.EngineSize = engineSize;
                carToBeUpdated.FuelType = fuelType;
                ResetValue();
            }
            else if (result == false)
            {
                MessageBox.Show("Request canceled", "Cancel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void DeleteItem()
        {
            if (lvList.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Car carToBeDeleted = (Car)lvList.SelectedItem;
            MessageBoxResult result = MessageBox.Show("Are you sure?", "CONFIRMATION", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                cars.Remove(carToBeDeleted);
                ResetValue();
            }
        }

        private void MenuItem_UpdateItem(object sender, RoutedEventArgs e)
        {
            if(lvList.SelectedIndex==-1)
            {
                return;
            }
            UpdateItem();
            updateStatusBar();
        }

        private void MenuItem_DeleteItem(object sender, RoutedEventArgs e)
        {
            if (lvList.SelectedIndex == -1)
            {
                return;
            }
            DeleteItem();
            updateStatusBar();
        }

        private void lvList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvList.UnselectAll();            
        }

        private void MenuItem_ExportToCSV(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "CSV|*.csv";
            if (dialog.ShowDialog() == true)
            {
                string allData = "";
                foreach (Car mycar in cars)
                {
                    allData += mycar.ToCSVString() + "\n";
                }
                File.WriteAllText(dialog.FileName, allData);
            }
            else
            {
                MessageBox.Show("File error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
