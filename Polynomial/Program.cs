using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Polynomial
{
    class Program
    {
        static void Main()
        {
            string polStr1 = "-x^3 + 2x^2";
            string polStr2 = "   + 3x^2-2x^2+ 7x + 1x^1 -   3";

            Polynomial pol1 = new Polynomial(polStr1);
            Polynomial pol2 = new Polynomial(polStr2);

            Console.WriteLine($"Первый многочлен: {pol1}");
            Console.WriteLine($"Второй многочлен: {pol2}");

            Console.WriteLine($"Результат сложения: {pol1 + pol2}");
            Console.WriteLine($"Результат вычитания: {pol1 - pol2}");
            Console.WriteLine($"Результат умножения: {pol1 * pol2}");
        }
    }
}
