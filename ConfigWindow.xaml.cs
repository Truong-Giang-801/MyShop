using DevExpress.Data.Entity;
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
using Microsoft.Data.SqlClient;
using System.Printing;
using System.Diagnostics;

namespace MyShop
{
    /// <summary>
    /// Interaction logic for ConfigWindow.xaml
    /// </summary>
    public partial class ConfigWindow : Window
    {
        public ConfigWindow()
        {
            InitializeComponent();
            txtServer.Text = Properties.Settings.Default.ServerName;
            txtDatabaseName.Text = Properties.Settings.Default.DatabaseName;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            string server = txtServer.Text;
            string databaseName = txtDatabaseName.Text;
            string connectionString = "Server=" + server + ";Database=" + databaseName + ";Trusted_Connection=yes;TrustServerCertificate=True";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                connection.Close();
                Properties.Settings.Default.ServerName = server;
                Properties.Settings.Default.DatabaseName = databaseName;
                Properties.Settings.Default.ConnectionString = connectionString;
                Properties.Settings.Default.Save();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to database");
            }
        }
    }
}
