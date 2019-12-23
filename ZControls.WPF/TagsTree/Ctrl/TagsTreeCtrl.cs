using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZControls.WPF.TagsTree.CtrlAux;
using ZControls.WPF.TagsTree.DataModel;

namespace ZControls.WPF.TagsTree.Ctrl
{
    #region ■■■■■ Base ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    [TemplatePart(Name = "Part_TagsTree", Type = typeof(TreeView))]
    public partial class TagsTreeCtrl : Control
    {
        static TagsTreeCtrl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TagsTreeCtrl), new FrameworkPropertyMetadata(typeof(TagsTreeCtrl)));

            #region ————— Dependency property registration ————————————————————————————————————————————————————————————

            StringPropProperty = DependencyProperty.Register(nameof(StringProp), typeof(String), typeof(TagsTreeCtrl),
                new FrameworkPropertyMetadata(default(String), new PropertyChangedCallback(OnDependencyPropChanged_StringProp)));

            #endregion ————— Dependency property registration

            #region ————— Routed events registraiton ——————————————————————————————————————————————————————————————————

            StringPropChangedEvent = EventManager.RegisterRoutedEvent(nameof(StringPropChanged), RoutingStrategy.Bubble,
                typeof(RoutedPropertyChangedEventArgs<String>), typeof(TagsTreeCtrl));

            #endregion ————— Routed events registraiton

            #region ————— Commands registration ———————————————————————————————————————————————————————————————————————

            CommandManager.RegisterClassCommandBinding(typeof(TagsTreeCtrl),
                new CommandBinding(UsersTreeCtrlCommands.AddTag, AddUsersSet_Executed, AddUsersSet_CanExecute));

            #endregion ————— Commands registration
        }

        public TagsTreeCtrl()
        {

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Part_TagsTree = GetTemplateChild(nameof(Part_TagsTree)) as TreeView;
            if(Part_TagsTree != null) Temp_SetupItemsCollection();
        }
    }
    #endregion ■■■■■ Base



    #region ■■■■■ Properties ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        private TreeView Part_TagsTree;

        #region ————— StringPropProperty ———————————————————————————————————————————————————————————————————————————

        public static DependencyProperty StringPropProperty;

        public String StringProp
        {
            get => (String)GetValue(StringPropProperty);
            set => SetValue(StringPropProperty, value);
        }

        private static void OnDependencyPropChanged_StringProp(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TagsTreeCtrl ctrl = sender as TagsTreeCtrl;
            if(ctrl == null) return;
            String oldValue = (String)e.OldValue;
            String newValue = (String)e.NewValue;
            RoutedPropertyChangedEventArgs<String> args = new RoutedPropertyChangedEventArgs<String>(oldValue, newValue);
            args.RoutedEvent = TagsTreeCtrl.StringPropChangedEvent;
            ctrl.RaiseEvent(args);
        }

        #endregion ————— StringPropProperty
    }
    #endregion ■■■■■ Properties



    #region ■■■■■ Events ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        #region ————— StringPropChanged ———————————————————————————————————————————————————————————————————————————————————

        public static readonly RoutedEvent StringPropChangedEvent;

        public event RoutedPropertyChangedEventHandler<String> StringPropChanged
        {
            add => AddHandler(StringPropChangedEvent, value);
            remove => RemoveHandler(StringPropChangedEvent, value);
        }

        #endregion ————— StringPropChanged
    }
    #endregion ■■■■■ Events



    #region ■■■■■ Commands ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        #region ————— AddUsersSet —————————————————————————————————————————————————————————————————————————————————————

        private static void AddUsersSet_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private static void AddUsersSet_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }

        #endregion ————— AddUsersSet
    }
    #endregion ■■■■■ Commands



    #region ■■■■■ Temp ■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■
    public partial class TagsTreeCtrl
    {
        private void Temp_SetupItemsCollection()
        {
            TagsDir dirMain = new TagsDir("_main");
            Tag dirAllActiveUsers = new Tag("Работающие пользователи", dirMain);
            Tag dirAllInactiveUsers = new Tag("Уволенные пользователи", dirMain);
            Tag dirAllUsers = new Tag("Все пользователи", dirMain);

            TagsDir dirUserGrouping = new TagsDir("Пользовательские наборы");
            TagsDir dirUsersByPositions = new TagsDir("Пользьватели по должностям", dirUserGrouping);
            TagsDir economists = new TagsDir("Экономисты", dirUsersByPositions);
            TagsDir lavyers = new TagsDir("Юристы", dirUsersByPositions);
            TagsDir accountants = new TagsDir("Бухгалтеры", dirUsersByPositions);

            ObservableCollection<TagsDir> dirs = new ObservableCollection<TagsDir>
            {
                dirMain,
                dirUserGrouping
            };

            Part_TagsTree.ItemsSource = dirs;

        }
    }
    #endregion ■■■■■ Temp
}
