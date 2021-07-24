using Inventory_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Inventory_Management.ViewModels
{
    public class TransactionWindowViewModel : BindableBaseFody
    {
        public Transaction Transaction { get; set; } = new Transaction();
        public List<string> Categorys { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsCreateAllow { get; set; }
        public StandardCommand CreateNewCommand { get; set; }

        public TransactionWindowViewModel()
        {
            Categorys = Global.DataSource.Inventories.Select(i => i.Category).ToList();
            CreateNewCommand = new StandardCommand(_ => createNew());
        }

        private void createNew()
        {
            Transaction.Id = Guid.NewGuid().ToString("n");
            Global.DataSource.Inventories.Add(Transaction.Clone());
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Global.DataSource.Save().GetAwaiter().GetResult();
                Global.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Transaction created");
                    Transaction = new Transaction();
                });
            });
        }
    }
}
