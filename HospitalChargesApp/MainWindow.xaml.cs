using System.Windows;

namespace HospitalChargesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ViewModel _vm = new ViewModel();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = _vm;
        }

        private void CalculateButton_OnClick(object sender, RoutedEventArgs e) => _vm.CalcTotalCharges();

        private void ClearButton_OnClick(object sender, RoutedEventArgs e) => _vm.Clear();
    }
}
