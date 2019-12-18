using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            //
            TagableObjectsCollection.Add(new TagableObject("Data item 001"));
            TagableObjectsCollection.Add(new TagableObject("Data item 002"));
            TagableObjectsCollection.Add(new TagableObject("Data item 003"));
            TagableObjectsCollection.Add(new TagableObject("Data item 004"));
            TagableObjectsCollection.Add(new TagableObject("Data item 005"));
            TagableObjectsCollection.Add(new TagableObject("Data item 006"));
            TagableObjectsCollection.Add(new TagableObject("Data item 007"));
            TagableObjectsCollection.Add(new TagableObject("Data item 008"));
            TagableObjectsCollection.Add(new TagableObject("Data item 009"));
            TagableObjectsCollection.Add(new TagableObject("Data item 010"));
            TagableObjectsCollection.Add(new TagableObject("Data item 011"));
            TagableObjectsCollection.Add(new TagableObject("Data item 012"));
            TagableObjectsCollection.Add(new TagableObject("Data item 013"));
            TagableObjectsCollection.Add(new TagableObject("Data item 014"));
            TagableObjectsCollection.Add(new TagableObject("Data item 015"));
            TagableObjectsCollection.Add(new TagableObject("Data item 016"));
            TagableObjectsCollection.Add(new TagableObject("Data item 017"));
            TagableObjectsCollection.Add(new TagableObject("Data item 018"));
            TagableObjectsCollection.Add(new TagableObject("Data item 019"));
            TagableObjectsCollection.Add(new TagableObject("Data item 020"));
            TagableObjectsCollection.Add(new TagableObject("Data item 021"));
            TagableObjectsCollection.Add(new TagableObject("Data item 022"));
            //
            QueriedItems = TagableObjectsCollection;
            Ctrl_Data.ItemsSource = QueriedItems;
            Ctrl_Tags.TagableObjectsCollection = TagableObjectsCollection;
            Ctrl_Tags.RelatedControl = Ctrl_Data;
        }

        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            HashSet<Int32> selectedTags = null;
            Ctrl_Data.ItemsSource = Ctrl_Tags.PerformQuery(TagableObjectsCollection, out selectedTags);
        }

        private void Btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            Ctrl_Data.ItemsSource = TagableObjectsCollection;
            foreach (DataGridRow row in GetDataGridRows(Ctrl_Data))
            {
                row.Visibility = Visibility.Visible;
                row.IsSelected = false;
                if (row.Item is TagableObject obj) obj.IsSelected = false;
            }
        }


        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void Btn_AsChecked_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataGridRow row in GetDataGridRows(Ctrl_Data))
            {
                if (!row.IsSelected) continue;
                if (row.Item is TagableObject obj) obj.IsSelected = true;
            }
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
