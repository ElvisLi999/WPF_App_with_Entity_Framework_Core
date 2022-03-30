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
                        select new
                        {
                            CustomerId = c.CustomerID,
                            NameStyle = c.NameStyle,
                            Title = c.Title,
                            FirstName = c.FirstName,
                            MiddleName = c.MiddleName,
                            LastName = c.LastName,
                            CompanyName = c.CompanyName,
                            SalesPerson = c.SalesPerson,
                            EmailAddress = c.EmailAddress,
                            Phone = c.Phone,
                            Password = c.Password
                        };
            var addrs = from a in db.Addresses
                        select new
                        {
                            AddressId = a.AddressId,
                            AddressLine1 = a.AddressLine1,
                            AddressLine2 = a.AddressLine2,
                            City = a.City,
                            Province = a.StateProvince,
                            Country = a.CountryRegion,
                            PostalCode = a.PostalCode
                        };
            var custAddrs = from ca in db.CustomerAddresses
                        select new
                        {
                            ca.AddressType,ca.ModifiedDate,ca.CustomerID,ca.AddressID
                        };

            var custInfors = (from c in db.Customers
                              from a in db.Addresses
                              from ca in db.CustomerAddresses
                              where c.CustomerID.Equals(ca.CustomerID) && a.AddressId.Equals(ca.AddressID)
                              select new
                              {
                                c.CustomerID,c.NameStyle,c.Title,c.FirstName,c.MiddleName,c.LastName,c.CompanyName,c.SalesPerson,c.EmailAddress,c.Phone,c.Password,
                                ca.AddressType,ca.ModifiedDate,
                                a.AddressId,a.AddressLine1,a.AddressLine2,a.City,a.StateProvince,a.CountryRegion,a.PostalCode
                              }).Distinct();

            // Assign items source for gridCustomers
            this.custGrid.ItemsSource = custInfors.ToList();
            this.gridCustomers.ItemsSource = custs.ToList();
            this.gridAddresses.ItemsSource = addrs.ToList();
            this.gridCustomersAddresses.ItemsSource = custAddrs.ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Create a object of CustomerEntities
            CustomerEntities db = new CustomerEntities();
            Customer custObject = new Customer()
            {
                NameStyle = txtNameStyle.Text,
                Title = txtTitle.Text,
                FirstName = txtFirstName.Text,
                MiddleName = txtMiddleName.Text,
                LastName = txtLastName.Text,
                CompanyName = txtCompany.Text,
                SalesPerson = txtSalesPerson.Text,
                EmailAddress = txtEmail.Text,
                Phone = txtPhone.Text,
                Password = txtPassword.Text
            };
            db.Customers.Add(custObject); // Save in Memory Level
            db.SaveChanges(); // Save in Database Level;

            var custs = from c in db.Customers
                        select new
                        {
                            CustomerId = c.CustomerID,
                            NameStyle = c.NameStyle,
                            Title = c.Title,
                            FirstName = c.FirstName,
                            MiddleName = c.MiddleName,
                            LastName = c.LastName,
                            CompanyName = c.CompanyName,
                            SalesPerson = c.SalesPerson,
                            EmailAddress = c.EmailAddress,
                            Phone = c.Phone,
                            Password = c.Password
                        };
            this.gridCustomers.ItemsSource = custs.ToList();

        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {

        }
    }
}
