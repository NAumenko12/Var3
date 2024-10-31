using System.Windows.Controls;
using Var3.Model;
using Var3.ViewModel;

namespace Var3.View
{
    public partial class Shop : UserControl
    {
        public Shop()
        {
            InitializeComponent();
            this.DataContext = new ShopViewModel(this);
        }
    }
}