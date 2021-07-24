using Inventory_Management.Services;
using System.Windows.Threading;

namespace Inventory_Management
{
    public class Global
    {
        public static Dispatcher Dispatcher { get; set; } = Dispatcher.CurrentDispatcher;
        public static IServices Services { get; set; }
        public static DataSource DataSource { get; set; }
    }
}
