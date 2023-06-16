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
    public class DragResizeBox : ListBox
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new DragResizeItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is DragResizeItem;
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            UnselectAll();
            base.OnMouseDown(e);
        }
    }
}
