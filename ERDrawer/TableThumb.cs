using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;

namespace ERDrawer
{
    public class TableThumb : Thumb
    {
        private const string BASE_PANEL_NAME = "grid";
        //private Canvas BasePanel;
        private Grid BaseGrid;
        public TableThumb()
        {
            ControlTemplate template = new ControlTemplate();
            template.VisualTree = new System.Windows.FrameworkElementFactory(typeof(Grid), BASE_PANEL_NAME);
            this.Template = template;

            this.ApplyTemplate();
            BaseGrid = (Grid)this.Template.FindName(BASE_PANEL_NAME, this);
        }

        public void SetUserControl(UserControl userControl)
        {
            BaseGrid.Children.Add(userControl);
        }
    }
}
