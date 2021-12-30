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
using System.Configuration;

namespace ZdravotnickeProstredkyLinq
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataClasses1DataContext dataContx;
        public MainWindow()
        {
            InitializeComponent();
            string connStr = ConfigurationManager.ConnectionStrings["ZdravotnickeProstredkyLinq.Properties.Settings.ZdravotnickeProstredkyConnectionString"].ConnectionString;
            dataContx = new DataClasses1DataContext(connStr);

            showItems();

        }

        private void showItems()
        {
            var items = dataContx.ZdravotnickeProstredky.Select(t => new {
                Id = t.Id,
                UId = t.Uid, 
                Name = t.Nazev,
                Check = t.TechnickaKontrola,
                Revisor = t.OdpovednaOsoba.Jmeno
            });

            ItemDG.ItemsSource = items;
            ItemDG.SelectedValuePath = "Id";
        }

        private void Add(object sender, RoutedEventArgs e)
        {
            int id = -1;
            if(ItemDG.SelectedValue != null){
                id = (int)ItemDG.SelectedValue;
            }
            Window1 w = new Window1(id);
            w.ShowDialog();
            showItems();
        }

        private void Expired(object sender, RoutedEventArgs e)
        {
            Expired exp = new Expired();
            exp.ShowDialog();
            showItems();
        }

        private void Revisors(object sender, RoutedEventArgs e)
        {
            Revisors rev = new Revisors();
            rev.ShowDialog();
            showItems();
        }

        private void Deselect(object sender, MouseButtonEventArgs e)
        {
            ItemDG.SelectedItem = null;
        }
    }
}
