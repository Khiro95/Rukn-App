using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rukn.Controls
{
    public class TextBlockUtils
    {
        // WPF has a very weird behavior of text alignment when FlowDirection is set to RightToLeft,
        // Left becomes Right while Right becomes Left, this is not intuitive, so need manual fix to improve UX

        public static DependencyProperty SmartAlignmentProperty = DependencyProperty.RegisterAttached(
            "SmartAlignment",
            typeof(TextAlignment),
            typeof(TextBlockUtils),
            new FrameworkPropertyMetadata(OnSmartAlignmentChanged));

        private static void OnSmartAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
            {
                TextAlignment alignment = (TextAlignment)e.NewValue;
                textBlock.TextAlignment = GetAdjustedTextAlignment(textBlock.FlowDirection == FlowDirection.RightToLeft, alignment);
            }
        }

        public static TextAlignment GetSmartAlignment(DependencyObject d) => (TextAlignment)d.GetValue(SmartAlignmentProperty);
        public static void SetSmartAlignment(DependencyObject d, TextAlignment textAlignment) => d.SetValue(SmartAlignmentProperty, textAlignment);

        public static TextAlignment GetAdjustedTextAlignment(bool isRTL, string alignment)
        {
            return GetAdjustedTextAlignment(isRTL, Enum.Parse<TextAlignment>(alignment));
        }

        public static TextAlignment GetAdjustedTextAlignment(bool isRTL, TextAlignment alignment)
        {
            if (isRTL && alignment is TextAlignment.Left or TextAlignment.Right)
            {
                return alignment == TextAlignment.Left ? TextAlignment.Right : TextAlignment.Left;
            }
            else
            {
                return alignment;
            }
        }
    }
}
