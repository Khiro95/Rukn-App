using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Rukn.Controls
{
    public enum ResizeMode
    {
        Both,
        Horizontal,
        Vertical
    }

    public class DragResizeItem : ListBoxItem
    {
        private TextBlock textElement;

        public static DependencyProperty ResizeModeProperty = DependencyProperty.Register(
            "ResizeMode",
            typeof(ResizeMode),
            typeof(DragResizeItem));

        public ResizeMode ResizeMode { get => (ResizeMode)GetValue(ResizeModeProperty); set => SetValue(ResizeModeProperty, value); }

        public static DependencyProperty ReadOnlyHeightProperty = DependencyProperty.Register(
            "ReadOnlyHeight",
            typeof(double),
            typeof(DragResizeItem));

        public double ReadOnlyHeight { get => ActualHeight; set { } }

        static DragResizeItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragResizeItem), new FrameworkPropertyMetadata(typeof(DragResizeItem)));
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            SetCurrentValue(ReadOnlyHeightProperty, sizeInfo.NewSize.Height);
        }

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            SetCurrentValue(IsSelectedProperty, true);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnLostKeyboardFocus(e);
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            Focus();
        }

        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            if (this.IsKeyboardFocused)
            {
                Keyboard.ClearFocus();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            double offset = Keyboard.Modifiers == ModifierKeys.Shift ? 10 : 1;
            switch (e.Key)
            {
                case Key.Right:
                    SetCurrentValue(Canvas.LeftProperty, Canvas.GetLeft(this) + offset);
                    break;
                case Key.Left:
                    SetCurrentValue(Canvas.LeftProperty, Canvas.GetLeft(this) - offset);
                    break;
                case Key.Up:
                    SetCurrentValue(Canvas.TopProperty, Canvas.GetTop(this) - offset);
                    break;
                case Key.Down:
                    SetCurrentValue(Canvas.TopProperty, Canvas.GetTop(this) + offset);
                    break;
                default:
                    break;
            }
            //e.Handled = true;
            base.OnKeyDown(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            SetCurrentValue(IsSelectedProperty, true);
            if (!IsFocused)
            {
                Focus();
            }
            if (e.ClickCount == 2)
            {
                textElement ??= FindFirstChild<TextBlock>(this);
                if (textElement is not null)
                {
                    if (this.ActualWidth != (textElement.DesiredSize.Width + 1))
                    {
                        double diff = this.ActualWidth - (textElement.DesiredSize.Width + 1);
                        if (ResizeMode is ResizeMode.Both or ResizeMode.Horizontal)
                        {
                            TextAlignment alignment = TextBlockUtils.GetAdjustedTextAlignment(
                                textElement.FlowDirection == FlowDirection.RightToLeft,
                                textElement.TextAlignment);
                            double offset = alignment switch
                            {
                                TextAlignment.Left => 0,
                                TextAlignment.Right => diff,
                                _ => diff / 2
                            };
                            SetCurrentValue(WidthProperty, textElement.DesiredSize.Width + 1);
                            SetCurrentValue(Canvas.LeftProperty, Canvas.GetLeft(this) + offset);
                        }
                    }
                    if (ResizeMode is ResizeMode.Both or ResizeMode.Vertical)
                    {
                        SetCurrentValue(HeightProperty, textElement.ActualHeight > this.ActualHeight ? textElement.ActualHeight : textElement.DesiredSize.Height);
                    }
                }
            }
        }

        private T FindFirstChild<T>(FrameworkElement element) where T : FrameworkElement
        {
            int childrenCount = System.Windows.Media.VisualTreeHelper.GetChildrenCount(element);
            var children = new FrameworkElement[childrenCount];

            for (int i = 0; i < childrenCount; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(element, i) as FrameworkElement;
                children[i] = child;
                if (child is T)
                    return (T)child;
            }

            for (int i = 0; i < childrenCount; i++)
                if (children[i] != null)
                {
                    var subChild = FindFirstChild<T>(children[i]);
                    if (subChild != null)
                        return subChild;
                }

            return null;
        }
    }
}
