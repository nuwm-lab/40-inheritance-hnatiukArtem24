using System;

class Drib
{
    protected double a; // коефіцієнт a

 
    public virtual void SetCoef()
    {
        Console.Write("Введіть коефіцієнт a: ");
        a = Convert.ToDouble(Console.ReadLine());
    }

   
    public virtual void ShowCoef()
    {
        Console.WriteLine($"Коефіцієнт a = {a}");
    }


    public virtual double Value(double x)
    {
        if (a * x == 0)
        {
            Console.WriteLine("Помилка: ділення на нуль!");
            return double.NaN;
        }
        return 1 / (a * x);
    }
}

// Похідний клас - тривимірний підхідний дріб
class TrivymirnyDrib : Drib
{
    protected double a2, a3;

    // Перевизначення методу введення коефіцієнтів
    public override void SetCoef()
    {
        Console.Write("Введіть коефіцієнт a1: ");
        a = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введіть коефіцієнт a2: ");
        a2 = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введіть коефіцієнт a3 (≠0): ");
        a3 = Convert.ToDouble(Console.ReadLine());
        if (a3 == 0)
        {
            Console.WriteLine("a3 не може дорівнювати 0! Встановлено a3 = 1.");
            a3 = 1;
        }
    }

    // Перевизначення методу виведення коефіцієнтів
    public override void ShowCoef()
    {
        Console.WriteLine($"a1 = {a}, a2 = {a2}, a3 = {a3}");
    }

    // Перевизначення методу обчислення значення дробу
    // 1 / (a1*x + 1 / (a2*x + 1 / (a3*x)))
    public override double Value(double x)
    {
        double denominator = a2 * x + 1 / (a3 * x);
        denominator = a * x + 1 / denominator;
        return 1 / denominator;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Клас 'Дріб' ===");
        Drib d = new Drib();
        d.SetCoef();
        d.ShowCoef();

        Console.Write("Введіть значення x: ");
        double x = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine($"Значення дробу: {d.Value(x)}");

        Console.WriteLine("\n=== Клас 'Тривимірний підхідний дріб' ===");
        TrivymirnyDrib t = new TrivymirnyDrib();
        t.SetCoef();
        t.ShowCoef();

        Console.Write("Введіть значення x: ");
        x = Convert.ToDouble(Console.ReadLine());
        Console.WriteLine($"Значення тривимірного дробу: {t.Value(x)}");

        Console.ReadLine();
    }
}
