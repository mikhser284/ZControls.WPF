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
using ZControls.WPF.Demo.Aux;
using ZControls.WPF.Demo.DataModel;

namespace ZControls.WPF.Demo
{
    public partial class MainWindow : Window
    {
        

        public MainWindow()
        {
            InitializeComponent();

            List<DataItem> dataItems = new List<DataItem>
            {
                new DataItem("Data item 001"),
                new DataItem("Data item 002"),
                new DataItem("Data item 003"),
                new DataItem("Data item 004"),
                new DataItem("Data item 005"),
                new DataItem("Data item 006"),
                new DataItem("Data item 007"),
                new DataItem("Data item 008"),
                new DataItem("Data item 009"),
                new DataItem("Data item 010"),
                new DataItem("Data item 011"),
                new DataItem("Data item 012"),
                new DataItem("Data item 013"),
                new DataItem("Data item 014"),
                new DataItem("Data item 015"),
                new DataItem("Data item 016"),
                new DataItem("Data item 017"),
                new DataItem("Data item 018"),
                new DataItem("Data item 019"),
                new DataItem("Data item 020"),
                new DataItem("Data item 021"),
                new DataItem("Data item 022"),
            };
            Ctrl_Data.ItemsSource = dataItems;            
        }


        private void UseTags_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
