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
            var sorok = File.ReadAllLines("VBfoci.csv").Skip(1);
            foreach (var sor in sorok)
            {
                resztvevok.Add(new Resztvevo(sor));
            }

        }
    }
}
