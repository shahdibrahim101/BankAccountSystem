namespace BankAccountSystem
{
    internal class Program
    {
       

public class Account
    {
        public string Name { get; set; }
        public double Balance { get; set; }

        public Account(string name = "Unnamed Account", double balance = 0.0)
        {
            Name = name;
            Balance = balance;
        }

        public virtual bool Deposit(double amount)
        {
            if (amount <= 0)
                return false;

            Balance += amount;
            return true;
        }

        public virtual bool Withdraw(double amount)
        {
            if (amount <= 0 || Balance < amount)
                return false;

            Balance -= amount;
            return true;
        }

        public override string ToString()
        {
            return $"{Name}: Balance = ${Balance:F2}";
        }
    }

    // Savings Account
    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(
            string name = "Unnamed Savings Account",
            double balance = 0.0,
            double interestRate = 0.0)
            : base(name, balance)
        {
            InterestRate = interestRate;
        }
    }

    // Checking Account
    public class CheckingAccount : Account
    {
        private const double Fee = 1.50;

        public CheckingAccount(
            string name = "Unnamed Checking Account",
            double balance = 0.0)
            : base(name, balance)
        {
        }

        public override bool Withdraw(double amount)
        {
            if (amount <= 0 || Balance < amount + Fee)
                return false;

            Balance -= amount + Fee;
            return true;
        }
    }

    // Trust Account
    public class TrustAccount : SavingsAccount
    {
        private int withdrawalsThisYear = 0;
        private int withdrawalYear = DateTime.Now.Year;

        public TrustAccount(
            string name = "Unnamed Trust Account",
            double balance = 0.0,
            double interestRate = 0.0)
            : base(name, balance, interestRate)
        {
        }

        public override bool Deposit(double amount)
        {
            if (amount <= 0)
                return false;

            Balance += amount;

            // Bank bonus
            if (amount >= 5000)
                Balance += 50;

            return true;
        }

        public override bool Withdraw(double amount)
        {
            int currentYear = DateTime.Now.Year;

            if (currentYear != withdrawalYear)
            {
                withdrawalsThisYear = 0;
                withdrawalYear = currentYear;
            }

            // Maximum 3 withdrawals per year
            if (withdrawalsThisYear >= 3)
                return false;

            // Withdrawal must be less than 20% of balance
            if (amount <= 0 || amount >= Balance * 0.20)
                return false;

            if (Balance < amount)
                return false;

            Balance -= amount;
            withdrawalsThisYear++;

            return true;
        }
    }

    // Account Utility
    public static class AccountUtil
    {
        public static void Deposit(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Depositing to Accounts ===");

            foreach (Account acc in accounts)
            {
                if (acc.Deposit(amount))
                    Console.WriteLine($"Deposited ${amount:F2} to {acc}");
                else
                    Console.WriteLine($"Deposit failed for {acc}");
            }
        }

        public static void Withdraw(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Withdrawing from Accounts ===");

            foreach (Account acc in accounts)
            {
                if (acc.Withdraw(amount))
                    Console.WriteLine($"Withdrew ${amount:F2} from {acc}");
                else
                    Console.WriteLine($"Withdrawal failed for {acc}");
            }
        }
    }

    // Main Program
    
        static void Main(string[] args)
        {
            List<Account> accounts = new List<Account>();

            // Normal Accounts
            accounts.Add(new Account());
            accounts.Add(new Account("Larry", 2000));

            // Savings Accounts
            accounts.Add(new SavingsAccount("Superman", 3000, 5.0));
            accounts.Add(new SavingsAccount("Batman", 5000, 4.0));

            // Checking Accounts
            accounts.Add(new CheckingAccount("Moe", 2000));
            accounts.Add(new CheckingAccount("Curly", 5000));

            // Trust Accounts
            accounts.Add(new TrustAccount("Trust 1", 10000, 5.0));
            accounts.Add(new TrustAccount("Trust 2", 6000, 4.0));

            // Display Initial Accounts
            Console.WriteLine("=== Initial Accounts ===");

            foreach (Account acc in accounts)
            {
                Console.WriteLine(acc);
            }

            // User enters deposit amount
            Console.Write("\nEnter deposit amount: ");
            double depositAmount = Convert.ToDouble(Console.ReadLine());

            AccountUtil.Deposit(accounts, depositAmount);

            // User enters withdrawal amount
            Console.Write("\nEnter withdrawal amount: ");
            double withdrawAmount = Convert.ToDouble(Console.ReadLine());

            AccountUtil.Withdraw(accounts, withdrawAmount);

            // Display Final Accounts
            Console.WriteLine("\n=== Final Accounts ===");

            foreach (Account acc in accounts)
            {
                Console.WriteLine(acc);
            }
        }
}
    }

