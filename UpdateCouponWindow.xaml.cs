using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace MyShop
{
    /// <summary>
    /// Interaction logic for UpdateCouponWindow.xaml
    /// </summary>
    public partial class UpdateCouponWindow : Window
    {
        public Coupon _updateCoupon { get; set; }
        BindingList<Coupon> _coupons = new BindingList<Coupon>();
        public UpdateCouponWindow(Coupon cp, BindingList<Coupon> coupons)
        {
            InitializeComponent();
            _coupons = new BindingList<Coupon>(coupons.ToList());
            _updateCoupon = (Coupon)cp.Clone();
            DataContext = _updateCoupon;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (CouponCode.Text == "" || DiscountPercentage.Text == "" || ExpiryDate.SelectedDate == null)
            {
                MessageBox.Show("Please don't leave any field as blank");
            }
            else
            {
                try
                {
                    string code = CouponCode.Text;
                    decimal discountPercentage = decimal.Parse(DiscountPercentage.Text);
                    bool codeExists = _coupons.Any(c => c.Code == code && c.Id != _updateCoupon.Id);
                    DateTime expiryDate = ExpiryDate.SelectedDate.Value;
                    if (codeExists)
                    {
                        MessageBox.Show("This coupon code already exists.");
                    }
                    else
                    {
                        _updateCoupon.Code = code;
                        _updateCoupon.ExpiryDate = expiryDate;
                        _updateCoupon.DiscountPercentage = discountPercentage;

                        this.DialogResult = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Please enter valid discount percentage");
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
