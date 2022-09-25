using System.Collections.Generic;
using System;

public class exactchange2
{
    static Result[] cache;
    public static void Main(string[] args)
    {
        var noCases = int.Parse(Console.ReadLine());

        for (int i = 0; i < noCases; i++)
        {
            var amountToPay = int.Parse(Console.ReadLine());
            var coins = ReadData();
            var res = Solve(coins, amountToPay, coins.Length - 1);
            Console.WriteLine(res);
        }
    }

    public static Result Solve(int[] coins, int amountToPay, int index)
    {
        if (amountToPay <= 0) return new Result(0, 0);
        if (index < 0) return Result.invalid;
        if (cache[index] != null)
        {
            return cache[index];
        }

        var currValue = coins[index];
        var include = (new Result(currValue, 1)).Add(Solve(coins, amountToPay - currValue, index - 1));
        var exclude = Solve(coins, amountToPay, index - 1);
        var best = include.GetBestResult(exclude, amountToPay);
        cache[index] = best;
        return best;
    }

    public static int[] ReadData()
    {
        var n = int.Parse(Console.ReadLine());
        var arr = new int[n];
        cache = new Result[n];

        for (int i = 0; i < n; i++)
        {
            var ele = int.Parse(Console.ReadLine());
            arr[i] = ele;
        }
        Array.Sort(arr, (a, b) => a - b);
        return arr;
    }

    public class Result
    {
        public int amountPaid;
        public int coinsUsed;
        public bool isValid;

        public static Result invalid = new Result(-1, -1, false);

        public Result(int amountPaid, int coinsUsed, bool isValid = true)
        {
            this.amountPaid = amountPaid;
            this.coinsUsed = coinsUsed;
            this.isValid = isValid;
        }

        public Result GetBestResult(Result that, int amountToPay)
        {
            if (!this.isValid && !that.isValid)
            {
                return Result.invalid;
            }
            else if (!this.isValid) return that;
            else if (!that.isValid) return this;

            if (this.amountPaid < amountToPay && that.amountPaid < amountToPay)
            {
                return Result.invalid;
            }
            else if (this.amountPaid < amountToPay) return that;
            else if (that.amountPaid < amountToPay) return this;

            if (that.amountPaid == this.amountPaid)
            {
                return that.coinsUsed < this.coinsUsed ? that : this;
            }

            return that.amountPaid < this.amountPaid ? that : this;
        }

        public Result Add(Result that)
        {
            return new Result(this.amountPaid + that.amountPaid, this.coinsUsed + that.coinsUsed, this.isValid && that.isValid);
        }

        public override string ToString()
        {
            return $"{amountPaid} {coinsUsed}";
        }
    }

}
