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

namespace Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Create a object of CustomerEntities
            CustomerEntities db = new CustomerEntities();
            var custs = from c in db.Customers
                        select c;
            foreach (var item in custs)
            {
                Console.WriteLine(item.FirstName);
                Console.WriteLine(item.LastName);
            }
        }
    }
}
