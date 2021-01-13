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

namespace MyCars
{
    /// <summary>
    /// Interaction logic for CarDialog.xaml
    /// </summary>
    public partial class CarDialog : Window
    {
        public event Action<string, double, string> BackValues;
        public CarDialog(string buttonName, string makeModel, double engineSize, string fuelType)
        {
            InitializeComponent();
            btnSave.Content = buttonName;
            if (buttonName.Equals("Update"))
            {
                setValues(makeModel, engineSize,  fuelType);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtMakeModel.Text.Trim() == "")
            {
                MessageBox.Show("Input string error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BackValues?.Invoke(txtMakeModel.Text, sldEngineSize.Value, comFuelType.Text);
            DialogResult = true;
        }
        private void setValues(string makeModel, double engineSize, string fuelType)
        {
            txtMakeModel.Text = makeModel;
            sldEngineSize.Value = engineSize;
            comFuelType.Text = fuelType;
        }
    }
}
