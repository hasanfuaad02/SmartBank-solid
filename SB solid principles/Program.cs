using System;


public interface ITransferable
{
    void SendTransfer(double amount, ITransferable recipient);
    void ReceiveTransfer(double amount, ITransferable sender);
    void DisplayBalance();
}

public interface IRewardable
{
    void EarnRewards(double amount);
}

public interface IDisplayable
{
    void Display(string message);
}

public class TransferHandler : ITransferable
{
    private double balance;

    public double Balance
    {
        get => balance;
        protected set => balance = Math.Max(value, 0); // Ensure balance is non-negative
    }

    public TransferHandler()
    {
    }

    public virtual void SendTransfer(double amount, ITransferable recipient)
    {
        if (Balance >= amount)
        {
            Balance -= amount;
            recipient.ReceiveTransfer(amount, this);
            Display($"Transfer of ${amount} sent.");
        }
        else
        {
            Display("Insufficient funds for the transfer.");
        }
    }

    public virtual void ReceiveTransfer(double amount, ITransferable sender)
    {
        Balance += amount;
        Display($"Received ${amount} from {sender}.");
    }

    public void DisplayBalance()
    {
        Display($"Current Balance: ${Balance}");
    }

    public virtual void Display(string message)
    {
        Console.WriteLine(message);
    }
}

public class SmartAccount : TransferHandler, IRewardable
{
    private int rewardsPoints;

    public SmartAccount(double initialBalance, int initialRewardsPoints) : base()
    {
        Balance = initialBalance;
        rewardsPoints = initialRewardsPoints;
    }

    public override void ReceiveTransfer(double amount, ITransferable sender)
    {
        base.ReceiveTransfer(amount, sender);
        EarnRewards(amount);
    }

    public void EarnRewards(double amount)
    {
        rewardsPoints += (int)(amount * 0.1);
        Display($"Earned {amount * 0.1} rewards points. Total points: {rewardsPoints}");
    }

    public override void Display(string message)
    {
        Console.WriteLine($"SmartAccount: {message}");
    }
}

class Program
{
    static void Main()
    {
        ITransferable account1 = new TransferHandler();
        ITransferable account2 = new SmartAccount(500, 50);

        Console.WriteLine("Initial Balances:");
        account1.DisplayBalance();
        account2.DisplayBalance();

        Console.Write("\nEnter the transfer amount from Account 1 to Account 2: ");
        double transferAmount = Convert.ToDouble(Console.ReadLine());

        account1.SendTransfer(transferAmount, account2);

        Console.WriteLine("\nUpdated Balances after Transfer:");
        account1.DisplayBalance();
        account2.DisplayBalance();

        Console.Write("\nEnter the transfer amount from Account 2 to Account 1: ");
        transferAmount = Convert.ToDouble(Console.ReadLine());

        account2.SendTransfer(transferAmount, account1);

        Console.WriteLine("\nFinal Balances after Transfer:");
        account1.DisplayBalance();
        account2.DisplayBalance();
    }
}

