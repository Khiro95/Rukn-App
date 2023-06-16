using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Rukn.ViewModels;

namespace Rukn.Controls
{
    public class TextInfoView : ToggleButton
    {
        private static DependencyPropertyKey IsSelectedPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsSelected",
            typeof(bool),
            typeof(TextInfoView),
            new PropertyMetadata());

        public static DependencyProperty IsSelectedProperty => IsSelectedPropertyKey.DependencyProperty;

        public static DependencyProperty IsReferenceProperty = DependencyProperty.Register(
            "IsReference",
            typeof(bool),
            typeof(TextInfoView));

        public static DependencyProperty InfoSourceProperty = DependencyProperty.Register(
            "InfoSource",
            typeof(object),
            typeof(TextInfoView));

        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(TextInfoView),
            new FrameworkPropertyMetadata(OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextInfoView textInfo = (TextInfoView)d;
            if (e.OldValue is not null)
            {
                CollectionViewSource.GetDefaultView(e.OldValue).CurrentChanged -= textInfo.OnSelectionChanged;
            }
            if (e.NewValue is not null)
            {
                CollectionViewSource.GetDefaultView(e.NewValue).CurrentChanged += textInfo.OnSelectionChanged;
            }
        }

        public bool IsSelected { get => (bool)GetValue(IsSelectedProperty); private set => SetValue(IsSelectedPropertyKey, value); }
        public bool IsReference { get => (bool)GetValue(IsReferenceProperty); set => SetValue(IsReferenceProperty, value); }
        public object InfoSource { get => GetValue(InfoSourceProperty); set => SetValue(InfoSourceProperty, value); }
        public IEnumerable ItemsSource { get => (IEnumerable)GetValue(ItemsSourceProperty); set => SetValue(ItemsSourceProperty, value); }

        static TextInfoView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextInfoView), new FrameworkPropertyMetadata(typeof(TextInfoView)));
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            IsSelected = InfoSource is not null && CollectionViewSource.GetDefaultView(ItemsSource).CurrentItem == InfoSource;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (!IsReference)
            {
                base.OnMouseLeftButtonDown(e);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (GetTemplateChild("PART_ResetPositionButton") is Button resetPosition)
            {
                resetPosition.ToolTip = Rukn.Resources.ResetPosition;
            }
            if (GetTemplateChild("PART_xLabel") is TextBlock xLabel)
            {
                xLabel.Text = Rukn.Resources.x;
            }
            if (GetTemplateChild("PART_yLabel") is TextBlock yLabel)
            {
                yLabel.Text = Rukn.Resources.y;
            }
        }
    }
}
