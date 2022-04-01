using System;
using System.Collections.Generic;
using System.Data;
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
        // Create a object of CustomerEntities
        CustomerEntities db = new CustomerEntities();
        public MainWindow()
        {
            InitializeComponent();
            ReloadingCustomers();


        }

        // Saving Customer input
        private void btnSaveCustomers_Click(object sender, RoutedEventArgs e)
        {
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
            ReloadingCustomers(); // Reloading customers from database
        }

        private void btnLoadCustomers_Click(object sender, RoutedEventArgs e)
        {
            ReloadingCustomers();
        }

        // Filling out selected column into textboxes for updating use
        private int updatingCustomerId = 0;
        private void gridCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Cut selectedColumn to list
            string[] delimiterStrings = { ", ", "{ ", " }", " = " };
            if (this.gridCustomers.SelectedItem != null)
            {
                string i = this.gridCustomers.SelectedItem.ToString();
                List<string> list = new List<string>(i.Split(delimiterStrings, System.StringSplitOptions.None));

                // Fill selected column into textbox for updating use
                if (this.gridCustomers.SelectedIndex >= 0 && this.gridCustomers.SelectedItems.Count >= 0)
                {
                    this.updatingCustomerId = int.Parse(list[2].ToString());
                    this.txtNameStyle.Text = list[4].ToString();
                    this.txtTitle.Text = list[6].ToString();
                    this.txtFirstName.Text = list[8].ToString();
                    this.txtMiddleName.Text = list[10].ToString();
                    this.txtLastName.Text = list[12].ToString();
                    this.txtCompany.Text = list[14].ToString();
                    this.txtSalesPerson.Text = list[16].ToString();
                    this.txtEmail.Text = list[18].ToString();
                    this.txtPhone.Text = list[20].ToString();
                    this.txtPassword.Text = list[22].ToString();
                }
            }

        }

        private void BtnUpdateCustomers_Click(object sender, RoutedEventArgs e)
        {
            var custs = from c in db.Customers
                        where c.CustomerID == this.updatingCustomerId
                        select c;
            Customer custObject = custs.SingleOrDefault();
            if (custObject != null)
            {
                custObject.NameStyle = this.txtNameStyle.Text;
                custObject.Title = txtTitle.Text;
                custObject.FirstName = txtFirstName.Text;
                custObject.MiddleName = txtMiddleName.Text;
                custObject.LastName = txtLastName.Text;
                custObject.CompanyName = txtCompany.Text;
                custObject.SalesPerson = txtSalesPerson.Text;
                custObject.EmailAddress = txtEmail.Text;
                custObject.Phone = txtPhone.Text;
                custObject.Password = txtPassword.Text;
                db.SaveChanges(); // Save in Database Level;
                this.gridCustomers.UnselectAll();
                MessageBox.Show($"Id: " + custObject.CustomerID + " has been updated!");
                ReloadingCustomers();
            }

        }


        private void btnDeleteCustomers_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult msgConfirm = MessageBox.Show(
                                                            "Do you want to delete this record?",
                                                            "Deleting Customer Records",
                                                            MessageBoxButton.YesNo,
                                                            MessageBoxImage.Warning,
                                                            MessageBoxResult.No);
            if (msgConfirm == MessageBoxResult.Yes)
            {
                var cust = from c in db.Customers
                           where c.CustomerID == this.updatingCustomerId
                           select c;
                Customer custObject = cust.SingleOrDefault();
                if (custObject != null)
                {
                    db.Customers.Remove(custObject);
                    db.SaveChanges();
                    ReloadingCustomers();
                }
            }

        }

        /* Address Part */

        // Clicking the CustomerId comboBox
        private void ComboBoxCustomerId_Click(object sender, MouseButtonEventArgs e)
        {
            var custs = from c in db.Customers
                        select new
                        {
                            CustomerId = c.CustomerID,
                        };

            cmbxCustomerId.ItemsSource = custs.ToList();

        }
        // Selecting a value for CustomerId comboBox
        private void ComboBoxCustomerId_DropDownClosed(object sender, EventArgs e)
        {
            if (cmbxCustomerId.SelectedItem != null)
            {
                insertCustId = FormatId(this.cmbxCustomerId.SelectedItem.ToString());
                cmbxAddressType.SelectedItem = null;
                ClearAddressTextBox();
            }
            
        }
        // Clicking the AddressType comboBox
        private string selectAddrType = null;
        private string[] addrsTypes = { "Home", "Work" };
        private void ComboBoxAddressType_Click(object sender, MouseButtonEventArgs e)
        {

            if (cmbxCustomerId.SelectedItem == null)
            {
                ClearAddressTextBox();
                MessageBox.Show("Plz select a customer id first!");
            }
            else
            {
                ClearAddressTextBox();
                cmbxAddressType.ItemsSource = addrsTypes;
            }

        }
        // Selecting a value for AddressType comboBox
        private void ComboBoxAddressType_DropDownClosed(object sender, EventArgs e)
        {
            selectAddrType = cmbxAddressType.SelectedItem.ToString();
            CheckingCustomerExistAddresses();
            if (existAddress != null && cmbxAddressType.SelectedItem !=null)
            {
                if (existAddress[0].AddressType.ToString() == cmbxAddressType.SelectedItem.ToString())
                {
                    ClearAddressTextBox();
                    LoadingAddressInformation(selectedCustomerId, cmbxAddressType.SelectedItem.ToString());
                }
                else
                {
                    ClearAddressTextBox();
                }
            }

        }

        // Saving new insert for address block
        private void btnSaveAddress_Click(object sender, RoutedEventArgs e)
        {
            if (cmbxCustomerId.SelectedItem != null)
            {
                if (cmbxAddressType.SelectedItem != null)
                {
                    if (existAddress == null || existAddress[0].AddressType.ToString() != cmbxAddressType.SelectedItem.ToString())
                    {
                        InsertAddress();
                        MessageBox.Show($"Customer Id: " + this.selectedCustomerId + " and Address Type: " + this.selectAddrType + " added into Database!");
                        this.cmbxCustomerId.SelectedItem = null;
                        this.cmbxAddressType.SelectedItem = null;
                        ClearAddressTextBox();
                    }
                    else
                    {
                        UpdateAddress();
                        MessageBox.Show($"Customer Id: " + this.selectedCustomerId + " and Address Type: " + this.selectAddrType + " are updated!");
                        this.cmbxCustomerId.SelectedItem = null;
                        this.cmbxAddressType.SelectedItem = null;
                        ClearAddressTextBox();
                    }
                }


            }
            else
            {

                MessageBox.Show("Plz select a customerId first!");
            }
        }


        // Check the Customers' exist Address information
        List<CustomerAddress> existAddress = null;
        int selectedCustomerId = 0;
        private void CheckingCustomerExistAddresses()
        {
            // Cut selectedColumn to list
            string[] delimiterStrings = { ", ", "{ ", " }", " = " };

            if (this.cmbxCustomerId.SelectedItem != null)
            {
                string i = this.cmbxCustomerId.SelectedItem.ToString();
                List<string> list = new List<string>(i.Split(delimiterStrings, System.StringSplitOptions.None));

                // Fill selected column into variable
                this.selectedCustomerId = int.Parse(list[2].ToString());

                var addrs = from ca in db.CustomerAddresses
                            where ca.CustomerID == this.selectedCustomerId && ca.AddressType == this.selectAddrType
                            select ca;
                if (addrs.FirstOrDefault() == null)
                {
                    existAddress = null;
                }
                else
                {
                    existAddress = addrs.ToList();
                    updateAddrId = int.Parse(addrs.FirstOrDefault().AddressID.ToString());
                    updateCustId = int.Parse(addrs.FirstOrDefault().CustomerID.ToString());
                }
            }

        }

        // Loading the Customers' existing Address information depends on CustomerId and AddressType
        private void LoadingAddressInformation(int custId, string addrType)
        {

            var AddressInfors = (
                              from a in db.Addresses
                              from ca in db.CustomerAddresses
                              where custId == ca.CustomerID && ca.AddressType.Equals(addrType) && a.AddressId == ca.AddressID
                              select new
                              {
                                  ca.AddressType,
                                  ca.ModifiedDate,
                                  a.AddressId,
                                  a.AddressLine1,
                                  a.AddressLine2,
                                  a.City,
                                  a.StateProvince,
                                  a.CountryRegion,
                                  a.PostalCode
                              }).Distinct();

            // Assign items for txtboxes of address block
            this.txtModifiedDate.Text = AddressInfors.FirstOrDefault().ModifiedDate.ToString();
            this.txtAddressLine1.Text = AddressInfors.FirstOrDefault().AddressLine1.ToString();
            this.txtAddressLine2.Text = AddressInfors.FirstOrDefault().AddressLine2.ToString();
            this.txtCity.Text = AddressInfors.FirstOrDefault().City.ToString();
            this.txtState.Text = AddressInfors.FirstOrDefault().StateProvince.ToString();
            this.txtCountry.Text = AddressInfors.FirstOrDefault().CountryRegion.ToString();
            this.txtPostalCode.Text = AddressInfors.FirstOrDefault().PostalCode.ToString();
        }

        private void btnClearAddress_Click(object sender, RoutedEventArgs e)
        {
            this.cmbxCustomerId.SelectedItem = null;
            this.cmbxAddressType.SelectedItem = null;
            ClearAddressTextBox();
        }

        // Clear textBoxes of Address Block
        private void ClearAddressTextBox()
        {
            this.txtModifiedDate.Clear();
            this.txtAddressLine1.Clear();
            this.txtAddressLine2.Clear();
            this.txtCity.Clear();
            this.txtState.Clear();
            this.txtCountry.Clear();
            this.txtPostalCode.Clear();
        }

        // Insert new data to address tables
        private int insertAddrId = 0, insertCustId = 0;
        private void InsertAddress()
        {
            // InsertAddress
            Address addrObject = new Address()
            {
                AddressLine1 = txtAddressLine1.Text,
                AddressLine2 = txtAddressLine2.Text,
                City = txtCity.Text,
                StateProvince = txtState.Text,
                CountryRegion = txtCountry.Text,
                PostalCode = txtPostalCode.Text
            };
            db.Addresses.Add(addrObject); // Save in Memory Level
            db.SaveChanges(); // Save in Database Level;

            insertAddrId = addrObject.AddressId;
            insertCustId = FormatId(this.cmbxCustomerId.SelectedItem.ToString());

            // Insert CustomerAddress
            CustomerAddress caObject = new CustomerAddress()
            {
                AddressType = cmbxAddressType.SelectedItem.ToString(),
                ModifiedDate = DateTime.Now,
                CustomerID = insertCustId,
                AddressID = insertAddrId
            };
            db.CustomerAddresses.Add(caObject); // Save in Memory Level
            db.SaveChanges(); // Save in Database Level;

            ReloadingCustomers();

        }

        // Update data to address tables
        private int updateAddrId = 0, updateCustId = 0;
        private void UpdateAddress()
        {
            // Update Address
            var addr = from a in db.Addresses
                       where a.AddressId == updateAddrId
                       select a;
            Address addrObject = addr.SingleOrDefault();
            if (addrObject != null)
            {
                addrObject.AddressLine1 = this.txtAddressLine1.Text;
                addrObject.AddressLine2 = this.txtAddressLine2.Text;
                addrObject.City = this.txtCity.Text;
                addrObject.StateProvince = this.txtState.Text;
                addrObject.CountryRegion = this.txtCountry.Text;
                addrObject.PostalCode = this.txtPostalCode.Text;
                db.SaveChanges();
            }
            // Update CustomerAddress

            var custaddrs = from ca in db.CustomerAddresses
                            where ca.CustomerID == updateCustId && ca.AddressID == updateAddrId
                            select ca;
            CustomerAddress caObject = custaddrs.SingleOrDefault();
            if (addrObject != null)
            {
                caObject.AddressType = this.cmbxAddressType.SelectedItem.ToString();
                caObject.ModifiedDate = DateTime.Now;
                db.SaveChanges();
            }

            ReloadingCustomers();
        }

        // Format id
        private int FormatId(string stringId)
        {
            // Cut selectedColumn to list
            string[] delimiterStrings = { ", ", "{ ", " }", " = " };

            List<string> list = new List<string>(stringId.Split(delimiterStrings, System.StringSplitOptions.None));

            // Fill selected column into variable
            return int.Parse(list[2].ToString());
        }

        // Reloading Customers Method
        private void ReloadingCustomers()
        {
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
                                ca.AddressType,
                                ca.ModifiedDate,
                                ca.CustomerID,
                                ca.AddressID
                            };

            var custInfors = (from c in db.Customers
                              from a in db.Addresses
                              from ca in db.CustomerAddresses
                              where c.CustomerID.Equals(ca.CustomerID) && a.AddressId.Equals(ca.AddressID)
                              select new
                              {
                                  c.CustomerID,
                                  c.NameStyle,
                                  c.Title,
                                  c.FirstName,
                                  c.MiddleName,
                                  c.LastName,
                                  c.CompanyName,
                                  c.SalesPerson,
                                  c.EmailAddress,
                                  c.Phone,
                                  c.Password,
                                  ca.AddressType,
                                  ca.ModifiedDate,
                                  a.AddressId,
                                  a.AddressLine1,
                                  a.AddressLine2,
                                  a.City,
                                  a.StateProvince,
                                  a.CountryRegion,
                                  a.PostalCode
                              }).Distinct();

            // Assign items source for gridCustomers
            this.custGrid.ItemsSource = custInfors.ToList();
            this.gridCustomers.ItemsSource = custs.ToList();
            this.gridAddresses.ItemsSource = addrs.ToList();
            this.gridCustomersAddresses.ItemsSource = custAddrs.ToList();
        }

    }
}
