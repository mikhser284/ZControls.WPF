using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        ObservableCollection<TagableObject> TagableObjectsCollection = new ObservableCollection<TagableObject>();
        ObservableCollection<TagableObject> QueriedItems = new ObservableCollection<TagableObject>();

        public MainWindow()
        {
            InitializeComponent();
            
        }
    }

    public static class Ext_Linq
    {
        public static Boolean ContainsAnyFrom(this HashSet<Int32> hashSetA, HashSet<Int32> hashSetB)
        {
            foreach(Int32 item in hashSetB) if (hashSetA.Contains(item)) return true;
            return false;
        }
    }
}
