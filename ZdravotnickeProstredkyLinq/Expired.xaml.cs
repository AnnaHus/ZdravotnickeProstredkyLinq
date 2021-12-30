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

namespace ZdravotnickeProstredkyLinq
{
    /// <summary>
    /// Interakční logika pro Expired.xaml
    /// </summary>
    public partial class Expired : Window
    {
        DataClasses1DataContext dataContx = new DataClasses1DataContext();
        int Year = DateTime.Now.Year;
        public Expired()
        {
            InitializeComponent();
            revisorCB.ItemsSource = dataContx.OdpovednaOsoba.Select(o => new { Id = o.Id, Name = o.Jmeno }).ToList();
            revisorCB.DisplayMemberPath = "Name";
            revisorCB.SelectedValuePath = "Id";
            showExpired();
        }

        private void showExpired()
        {
            var items = dataContx.ZdravotnickeProstredky.Select(t => new {
                Id = t.Id,
                UId = t.Uid,
                Name = t.Nazev,
                Check = t.TechnickaKontrola,
                Revisor = t.OdpovednaOsoba.Jmeno
            }).Where(i => (Year - i.Check) > 4);

            ExpDG.ItemsSource = items;
            ExpDG.SelectedValuePath = "Id";
        }

        private void prolong(object sender, RoutedEventArgs e)
        {
            
            if(ExpDG.SelectedValue is null)
            {
                MessageBox.Show("Není vybrána položka k prodloužení");
                return;
            }
            int itemId = (int)ExpDG.SelectedValue;

            if(revisorCB.SelectedValue is null)
            {
                MessageBox.Show("Není vybrána odpovědně osoba");
                return;
            }
            int revisorId = (int)revisorCB.SelectedValue;

            ZdravotnickeProstredky item = dataContx.ZdravotnickeProstredky.Single(i => i.Id.Equals(itemId));
            item.TechnickaKontrola = Year;
            item.OdpovednaOsobaId = revisorId;

            dataContx.SubmitChanges();
            showExpired();

        }
    }
}
