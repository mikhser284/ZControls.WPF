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
        

        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<TagableObject> tagableObjectsCollection = new ObservableCollection<TagableObject>
            {
                new TagableObject("Data item 001"),
                new TagableObject("Data item 002"),
                new TagableObject("Data item 003"),
                new TagableObject("Data item 004"),
                new TagableObject("Data item 005"),
                new TagableObject("Data item 006"),
                new TagableObject("Data item 007"),
                new TagableObject("Data item 008"),
                new TagableObject("Data item 009"),
                new TagableObject("Data item 010"),
                new TagableObject("Data item 011"),
                new TagableObject("Data item 012"),
                new TagableObject("Data item 013"),
                new TagableObject("Data item 014"),
                new TagableObject("Data item 015"),
                new TagableObject("Data item 016"),
                new TagableObject("Data item 017"),
                new TagableObject("Data item 018"),
                new TagableObject("Data item 019"),
                new TagableObject("Data item 020"),
                new TagableObject("Data item 021"),
                new TagableObject("Data item 022"),
            };
            Ctrl_Data.ItemsSource = tagableObjectsCollection;
            Ctrl_Tags.TagableObjectsCollection = tagableObjectsCollection;
        }

        private void Btn_Select_Click(object sender, RoutedEventArgs e)
        {
            HashSet<Int32> redTags;
            HashSet<Int32> greenTags;
            HashSet<Int32> blueTags;
            Ctrl_Tags.GetTagsByState(out redTags, out greenTags, out blueTags);

            foreach(DataGridRow row in GetDataGridRows(Ctrl_Data))
            {
                if(row.Item is TagableObject obj)
                {
                    if(obj.TagsIds.ContainsAnyFrom(redTags))
                        row.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataGridRow row in GetDataGridRows(Ctrl_Data))
            {
                row.Visibility = Visibility.Visible;
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
