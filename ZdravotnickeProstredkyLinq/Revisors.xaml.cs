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
    /// Interakční logika pro Revisors.xaml
    /// </summary>
    public partial class Revisors : Window
    {
        DataClasses1DataContext dataContx = new DataClasses1DataContext();
        OdpovednaOsoba oo;
        public Revisors()
        {
            InitializeComponent();
            showItems();
            revisorsDG.SelectedValuePath = "Id";
            oo = new OdpovednaOsoba();

        }

        private void showItems()
        {
            var items = dataContx.OdpovednaOsoba.Select(o => new { Id = o.Id, Name = o.Jmeno, Year = o.RokNarozeni });
            revisorsDG.ItemsSource = items;

        }

        private void AddUpdate(object sender, RoutedEventArgs e)
        {
            if(!dataContx.OdpovednaOsoba.Any(o => o.Id == oo.Id))
            {               
                dataContx.OdpovednaOsoba.InsertOnSubmit(oo);
            }
            oo.Jmeno = nameTB.Text;
            try
            {
                oo.RokNarozeni = Int32.Parse(yearTB.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("Rok narozenímusí být číslo");
                return;
            }
            dataContx.SubmitChanges();
            showItems();
        }

        private void revisorSelect(object sender, SelectionChangedEventArgs e)
        {
            if(revisorsDG.SelectedValue is null)
            {
                oo = new OdpovednaOsoba();
                nameTB.Text = "";
                yearTB.Text = "";
            }
            else
            {
                int id = (int)revisorsDG.SelectedValue;
                oo = dataContx.OdpovednaOsoba.Single(i => i.Id == id);
                nameTB.Text = oo.Jmeno;
                yearTB.Text = oo.RokNarozeni.ToString();
            }
            
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if(revisorsDG.SelectedValue is null)
            {
                MessageBox.Show("Není vybrána žádná odpovědně osoba k vymazání");
                return;
            }
            int id = (int)revisorsDG.SelectedValue;
            oo = dataContx.OdpovednaOsoba.Single(i => i.Id == id);
            dataContx.OdpovednaOsoba.DeleteOnSubmit(oo);
            try
            {
                dataContx.SubmitChanges();
            }
            catch (Exception)
            {

                MessageBox.Show("Odpovědnou osobu nemůžete vymazat,protože je přiřazena k některým zdravotnickým prostředkům");
            }
            
            showItems();
        }

        private void Deselect(object sender, MouseButtonEventArgs e)
        {
            revisorsDG.SelectedItem = null;
        }
    }
}
