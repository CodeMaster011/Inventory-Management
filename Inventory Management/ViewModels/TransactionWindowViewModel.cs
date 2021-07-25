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
        public TranscationLine TranscationLine { get; set; } = new TranscationLine();
        public bool IsReadOnly { get; set; }
        public bool IsCreateAllow { get; set; }
        public StandardCommand CreateNewCommand { get; set; }
        public StandardCommand AddTransactionLineCommand { get; set; }

        public TransactionWindowViewModel()
        {
            CreateNewCommand = new StandardCommand(_ => createNew());
            AddTransactionLineCommand = new StandardCommand(_ => addTransactionLine());
        }

        private void addTransactionLine()
        {
            if(string.IsNullOrEmpty(TranscationLine.InventoryId))
            {
                MessageBox.Show("Please choose an inventory. Press F9 to do search.");
                return;
            }
            Transaction.Lines.Add(TranscationLine);
            TranscationLine = new TranscationLine();
            
            NotifyPropertyChanged(nameof(Transaction));
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
