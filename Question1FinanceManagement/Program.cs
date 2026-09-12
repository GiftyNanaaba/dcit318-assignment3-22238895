using System;
using System.Collections.Generic;

namespace Question1FinanceManagement
{
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Bank Transfer: Processing GHC{transaction.Amount} for {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Mobile Money: Processing GHC{transaction.Amount} for {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"Crypto Wallet: Processing GHC{transaction.Amount} for {transaction.Category}");
        }
    }

    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Applied transaction of GHC{transaction.Amount}. New balance: GHC{Balance}");
        }
    }

    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance) : base(accountNumber, initialBalance)
        {
        }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
                return;
            }

            Balance -= transaction.Amount;
            Console.WriteLine($"Transaction applied. Updated balance: GHC{Balance}");
        }
    }

    public class FinanceApp
    {
        private readonly List<Transaction> _transactions = new();

        public void Run()
        {
            Console.WriteLine("--- Finance Management System ---");

            var account = new SavingsAccount("SA-1001", 1000m);
            Console.WriteLine($"Created SavingsAccount {account.AccountNumber} with balance GHC{account.Balance}");

            var t1 = new Transaction(1, DateTime.Now, 100m, "Groceries");
            var t2 = new Transaction(2, DateTime.Now, 200m, "Utilities");
            var t3 = new Transaction(3, DateTime.Now, 300m, "Entertainment");

            ITransactionProcessor p1 = new MobileMoneyProcessor();
            ITransactionProcessor p2 = new BankTransferProcessor();
            ITransactionProcessor p3 = new CryptoWalletProcessor();

            Console.WriteLine();
            p1.Process(t1);
            account.ApplyTransaction(t1);
            _transactions.Add(t1);

            Console.WriteLine();
            p2.Process(t2);
            account.ApplyTransaction(t2);
            _transactions.Add(t2);

            Console.WriteLine();
            p3.Process(t3);
            account.ApplyTransaction(t3);
            _transactions.Add(t3);

            Console.WriteLine();
            Console.WriteLine("Transactions processed:");
            foreach (var tx in _transactions)
            {
                Console.WriteLine($"ID:{tx.Id} Date:{tx.Date} Amount:GHC{tx.Amount} Category:{tx.Category}");
            }

            Console.WriteLine($"Final account balance: GHC{account.Balance}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var app = new FinanceApp();
            app.Run();
        }
    }
}
