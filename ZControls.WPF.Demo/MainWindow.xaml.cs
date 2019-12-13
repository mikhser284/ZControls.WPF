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

using ZControls.WPF.Demo.DataModel;

namespace ZControls.WPF.Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TagsTree tTree = new TagsTree();
            tTree
                .AddDir("Dir A")
                .AddDirAndDown("Dir B")
                    .AddDirAndDown("Dir B-B")
                    .AddTag("Tag B1")
                    .AddTag("Tag B2")
                    .AddTagAndUp("Tag B3")
                .AddDir("Dir C")
                .AddDirAndDown("Dir D")
                    .AddDirAndDown("Dir D-A")
                        .AddTag("Tag D1")
                        .AddTagAndUp("Tag D2")
                    .AddDir("Dir D-A")
                    ;
            tTree
                .AddDir("Dir C")
                .AddTag("Tag B1")
                .AddTag("Tag B2")
                .AddTag("Tag B3");
            //
            Ctrl_TagsTree.ItemsSource = tTree.Items;
        }
    }
}
