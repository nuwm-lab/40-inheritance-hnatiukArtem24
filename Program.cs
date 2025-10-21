using System;

class Drib
{
    protected double a; 

    
    protected double ReadDouble(string prompt, bool mustBeNonZero = false)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = Console.ReadLine();
            if (double.TryParse(s, out double val))
            {
                if (mustBeNonZero && val == 0.0)
                {
                    Console.WriteLine("Значення не повинно бути нульовим. Спробуйте ще.");
                    continue;
                }
                return val;
            }
            Console.WriteLine("Невірний формат числа. Спробуйте ще раз.");
        }
    }

    public virtual void SetCoef()
    {
        a = ReadDouble("Введіть коефіцієнт a (необов'язково ≠ 0): ");
    }

    public virtual void ShowCoef()
    {
        Console.WriteLine($"Коефіцієнт a = {a}");
    }

  
    public virtual double Value(double x)
    {
        if (a * x == 0.0)
        {
            Console.WriteLine("Помилка: ділення на нуль (a * x == 0).");
            return double.NaN;
        }
        return 1.0 / (a * x);
    }
}

class TrivymirnyDrib : Drib
{
    protected double a2, a3;

  
    public override void SetCoef()
    {
        a = ReadDouble("Введіть коефіцієнт a1: ");
        a2 = ReadDouble("Введіть коефіцієнт a2: ");
      
        a3 = ReadDouble("Введіть коефіцієнт a3 (повинен бути ≠ 0): ", mustBeNonZero: true);
    }

    public override void ShowCoef()
    {
        Console.WriteLine($"a1 = {a}, a2 = {a2}, a3 = {a3}");
    }

    public override double Value(double x)
    {
        if (a3 * x == 0.0)
        {
            Console.WriteLine("Помилка: (a3 * x) = 0 -> ділення на нуль при обчисленні внутрішнього дробу.");
            return double.NaN;
        }

        double inner = a3 * x;              // a3*x
        double denomLevel2 = a2 * x + 1.0 / inner; // a2*x + 1/(a3*x)

        if (denomLevel2 == 0.0)
        {
            Console.WriteLine("Помилка: проміжний знаменник (a2*x + 1/(a3*x)) = 0 -> ділення на нуль.");
            return double.NaN;
        }

        double denomLevel1 = a * x + 1.0 / denomLevel2; // a1*x + 1/denomLevel2

        if (denomLevel1 == 0.0)
        {
            Console.WriteLine("Помилка: зовнішній знаменник (a1*x + 1/...) = 0 -> ділення на нуль.");
            return double.NaN;
        }

        return 1.0 / denomLevel1;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine(" Клас 'Дріб' ");
        Drib d = new Drib();
        d.SetCoef();
        d.ShowCoef();

        double x = ReadDoubleStatic("Введіть значення x (не повинно робити a*x = 0 для цього класу): ", allowZero: true);
  
        double valD = d.Value(x);
        if (!double.IsNaN(valD))
            Console.WriteLine($"Значення дробу (Drib): {valD}");
        else
            Console.WriteLine("Не вдалося обчислити значення для класу Drib.");

        Console.WriteLine("\n=== Клас 'Тривимірний підхідний дріб' ===");
        TrivymirnyDrib t = new TrivymirnyDrib();
        t.SetCoef();
        t.ShowCoef();

        x = ReadDoubleStatic("Введіть значення x для тривимірного дробу (x ≠ 0 бажано): ", allowZero: true);
        double valT = t.Value(x);
        if (!double.IsNaN(valT))
            Console.WriteLine($"Значення тривимірного дробу: {valT}");
        else
            Console.WriteLine("Не вдалося обчислити значення для тривимірного дробу.");

        Console.WriteLine("Натисніть Enter щоб вийти...");
        Console.ReadLine();
    }


    static double ReadDoubleStatic(string prompt, bool allowZero = true)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = Console.ReadLine();
            if (double.TryParse(s, out double val))
            {
                return val;
            }
            Console.WriteLine("Невірний формат числа. Спробуйте ще раз.");
        }
    }
}

