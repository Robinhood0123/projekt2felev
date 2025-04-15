using System;
using System.IO;
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

namespace VBfoci
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<Resztvevo> resztvevok = new List<Resztvevo>();
        public MainWindow()
        {
            InitializeComponent();
            Beolvasas();

        }
        static void Beolvasas()
        {
            var sorok = File.ReadAllLines("VBfoci.csv", Encoding.UTF8).Skip(1);
            foreach (var sor in sorok)
            {
                resztvevok.Add(new Resztvevo(sor));
            }

        }

        private void Beolvasas_Click(object sender, RoutedEventArgs e)
        {
            string fajlNev = txtFajlNev.Text;

            resztvevok.Clear();
            listEredmenyek.Items.Clear();

            var sorok = File.ReadAllLines(fajlNev).Skip(1);

            foreach (string sor in sorok)
            {
                Resztvevo uj = new Resztvevo(sor);
                resztvevok.Add(uj);

                string szoveg = uj.Ev + " - " + uj.Orszag + " (" + uj.Helyezes + ".) - " + uj.Helyszin;
                listEredmenyek.Items.Add(szoveg);
            }
        }

        private void Kiiras_Click(object sender, RoutedEventArgs e)
        {
            string fajlNev = txtKiirasFajlba.Text;

            StreamWriter iro = new StreamWriter(fajlNev);

            foreach (Resztvevo adat in resztvevok)
            {
                string sor = adat.Orszag + "; " + adat.Helyezes + "; " + adat.Ev + "; " + adat.Helyszin;
                iro.WriteLine(sor);
            }

            txtKiirasFajlba.Background = Brushes.LightGreen;


            iro.Close();
            MessageBox.Show("A kiírás sikeresen befejeződött!");
        }

        private void Kereses_Click(object sender, RoutedEventArgs e)
        {

            string keresettOrszag = txtOrszagKeres.Text;
            var szurtEredmenyek = new List<string>();

            foreach (var item in listEredmenyek.Items)
            {
                string sor = item.ToString();
                if (sor.Contains(keresettOrszag))
                {
                    szurtEredmenyek.Add(sor);
                }
            }

            if (szurtEredmenyek.Any())
            {
                var eredmenyekSzoveg = string.Join("\n", szurtEredmenyek);
                MessageBox.Show($"Talált eredmények:\n{eredmenyekSzoveg}");

                File.WriteAllLines("eredmenyek.txt", szurtEredmenyek);

                if (chkKijeloles.IsChecked == true)
                {
                    listEredmenyek.SelectedItems.Clear();
                    foreach (var eredmeny in szurtEredmenyek)
                    {
                        listEredmenyek.SelectedItems.Add(eredmeny);
                    }
                }
            }
            else
            {
                MessageBox.Show("Nincs találat a keresett országra.");
            }

        }

        private void Megszamolas_Click(object sender, RoutedEventArgs e)
        {
            if (rdbLeggyakoribbHelyszin.IsChecked == true)
            {
                Dictionary<string, int> helyszinSzam = new Dictionary<string, int>();

                foreach (Resztvevo adat in resztvevok)
                {
                    if (helyszinSzam.ContainsKey(adat.Helyszin))
                    {
                        helyszinSzam[adat.Helyszin]++;
                    }
                    else
                    {
                        helyszinSzam.Add(adat.Helyszin, 1);
                    }
                }

                string leggyakoribb = "";
                int maxdb = 0;
                foreach (var par in helyszinSzam)
                {
                    if (par.Value > maxdb)
                    {
                        maxdb = par.Value;
                        leggyakoribb = par.Key;
                    }
                }

                MessageBox.Show("Leggyakoribb helyszín: " + leggyakoribb);
            }
            else if (rdbAtlagosHelyezes.IsChecked == true)
            {
                int osszeg = 0;
                foreach (Resztvevo adat in resztvevok)
                {
                    osszeg += adat.Helyezes;
                }

                double atlag = (double)osszeg / resztvevok.Count;
                MessageBox.Show("Átlagos helyezés: " + atlag.ToString());
            }
            else
            {
                MessageBox.Show("Kérlek válassz egy opciót!");
            }
        }


        // A MinMax_Click eseménykezelo megkeresi a legjobb és legrosszabb helyezést a résztvevők közül.
        // Készítette: Tóth Róbert
        private void RoszJobb_Click(object sender, RoutedEventArgs e)
        {
            if (resztvevok.Count == 0)
            {
                MessageBox.Show("Nincsenek adatok betöltve.");
                return;
            }

            var legjobb = resztvevok.OrderBy(r => r.Helyezes).ThenBy(r => r.Ev).First();
            
            var legrosszabb = resztvevok.OrderByDescending(r => r.Helyezes).ThenBy(r => r.Ev).First();

            string uzenet;
            if (comboRosszJobb.SelectedIndex == 0) 
            {
                uzenet = $"Legjobb helyezés:\n{legjobb.Ev} - {legjobb.Orszag} ({legjobb.Helyezes}. hely) - {legjobb.Helyszin}";
                MessageBox.Show(uzenet, "Minimum");
            }
            else if (comboRosszJobb.SelectedIndex == 1)
            {
                uzenet = $"Legrosszabb helyezés:\n{legrosszabb.Ev} - {legrosszabb.Orszag} ({legrosszabb.Helyezes}. hely) - {legrosszabb.Helyszin}";
                MessageBox.Show(uzenet, "Maximum");
            }
            else
            {
                MessageBox.Show("Kérlek válassz Minimum vagy Maximum értéket!");
            }
        }
    }
}
