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
    /// Interakční logika pro Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private DataClasses1DataContext dataContx = new DataClasses1DataContext();
        private static Random random = new Random();
        private ZdravotnickeProstredky zp;
        private bool update = false;

        public Window1(int id)
        {
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
            RevisorCB.ItemsSource = dataContx.OdpovednaOsoba.Select(o => new { Id = o.Id, Name = o.Jmeno }).ToList();
            RevisorCB.DisplayMemberPath = "Name";
            RevisorCB.SelectedValuePath = "Id";

            if (id != -1)
            {
                zp = dataContx.ZdravotnickeProstredky.Single(i => i.Id.Equals(id));
                update = true;
                NameTB.Text = zp.Nazev;
                YearTB.Text = zp.TechnickaKontrola.ToString();
                RevisorCB.SelectedValue = zp.OdpovednaOsobaId;
            }

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            string Name = NameTB.Text;
            int Year;
            try
            {
                Year = Int32.Parse(YearTB.Text);
            }
            catch (Exception)
            {

                MessageBox.Show("Rok technické konroly musí být číslo");
                return;
            }
            Year = Int32.Parse(YearTB.Text);
            int RevisorId = (int) RevisorCB.SelectedValue;
            if (!update)
            {
                zp = new ZdravotnickeProstredky();
                zp.Uid = RandomString();
            }
            zp.Nazev = Name;
            zp.TechnickaKontrola = Year;
            zp.OdpovednaOsobaId = RevisorId;
            if (!update) dataContx.ZdravotnickeProstredky.InsertOnSubmit(zp);
            dataContx.SubmitChanges();
            DialogResult = false;
        }

        public static string RandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
