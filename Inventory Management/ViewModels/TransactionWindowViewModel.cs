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
        public bool IsActual { get; set; }
        public bool IsPortal { get; set; }
        public Transaction Transaction { get; set; } = new Transaction();
        public TranscationLine TranscationLine { get; set; } = new TranscationLine();
        public bool IsReadOnly { get; set; }
        public bool IsCreateAllow { get; set; }
        public StandardCommand CreateNewCommand { get; set; }
        public StandardCommand AddTransactionLineCommand { get; set; }
        public StandardCommand ResetTransactionLineCommand { get; set; }

        public TransactionWindowViewModel()
        {
            CreateNewCommand = new StandardCommand(_ => createNew());
            AddTransactionLineCommand = new StandardCommand(_ => addTransactionLine());
            ResetTransactionLineCommand = new StandardCommand(_ => resetTransactionLine());
            if(!Transaction.Date.HasValue) Transaction.Date = DateTime.Now;
        }

        private void resetTransactionLine()
        {
            TranscationLine = new TranscationLine();
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

            Transaction.NotifyPropertyChanged(nameof(Transaction.Lines));
            
        }

        private void createNew()
        {
            Transaction.Id = Guid.NewGuid().ToString("n");
            if(!Transaction.Date.HasValue)
            {
                MessageBox.Show("Transaction does not have date.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (Transaction.Lines?.Count == 0)
            {
                MessageBox.Show("Transaction does not have any items.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(!IsPortal && !IsActual)
            {
                MessageBox.Show("Transaction does not have any mode.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(IsPortal)
                Global.DataSource.PortalTransactions.Add(Transaction.Clone());
            else if (IsActual)
                Global.DataSource.ActualTransactions.Add(Transaction.Clone());
            else
            {
                MessageBox.Show("Transaction does not have any mode.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ThreadPool.QueueUserWorkItem(_ =>
            {
                Global.DataSource.Save().GetAwaiter().GetResult();
                Global.Dispatcher.Invoke(() =>
                {
                    MessageBox.Show("Transaction created");
                    Transaction = new Transaction() { Date = DateTime.Now };
                });
            });
        }
    }
}
