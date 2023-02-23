using System.Windows;

namespace projektarbeit_Pizzashop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenPizzaView(object sender, RoutedEventArgs e)
        {
            PizzaView pizzaView = new PizzaView();
            pizzaView.ShowDialog();
        }

        private void OpenKundeView(object sender, RoutedEventArgs e)
        {
            KundeView kundeView = new KundeView();
            kundeView.ShowDialog();
        }

        private void OpenBestellungView(object sender, RoutedEventArgs e)
        {
            BestellungView bestellungView = new BestellungView();
            bestellungView.ShowDialog();
        }
    }
}
