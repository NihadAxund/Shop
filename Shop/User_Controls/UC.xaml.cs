using Shop.Concrete;
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

namespace Shop.User_Controls
{
    /// <summary>
    /// Interaction logic for UC.xaml
    /// </summary>
    public partial class UC : UserControl
    {
        private Product Product { get; set; }
        public UC()
        {
            InitializeComponent();
            ID_LBL.Content = "ID";
            ProductName_LBL.Content = "Product Name";
            CategoryId_LBL.Content = "Category Id";
            UnitPrice_LBL.Content = "Unit Price";
            UnitsInStock_LBL.Content = "Unit In Stock";
        }
        public UC(Product pr)
        {
            InitializeComponent();
            Product = pr;
            ID_LBL.Content = pr.ProductId.ToString();
            ProductName_LBL.Content = pr.ProductName;
            CategoryId_LBL.Content = pr.CategoryId;
            UnitPrice_LBL.Content = pr.UnitPrice;
            UnitsInStock_LBL.Content = pr.UnitsInStock;
        }
    }
}
