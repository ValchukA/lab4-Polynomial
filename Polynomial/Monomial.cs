using System;
using System.Text.RegularExpressions;

namespace Polynomial
{
    public class Monomial
    {
        public int Coefficient { get; set; }

        public int Degree { get; set; }

        public Monomial(int coefficient, int degree)
        {
            Coefficient = coefficient;
            Degree = degree;
        }

        public Monomial(Monomial monomial)
        {
            Coefficient = monomial.Coefficient;
            Degree = monomial.Degree;
        }

        public static Monomial Parse(string monomial)
        {
            if (Int32.TryParse(monomial, out int coefficient))
            {
                return new Monomial(coefficient, 0);
            }
            else
            {
                coefficient = 1;

                var coeffMatch = Regex.Match(monomial, @"^[+-]\d+");

                if (coeffMatch.Success)
                {
                    coefficient = Convert.ToInt32(coeffMatch.Value);

                    monomial = monomial.Remove(coeffMatch.Index, coeffMatch.Length);
                }

                var varMatch = Regex.Match(monomial, @"^[+-]?(x\^|x)");

                if (!varMatch.Success)
                {
                    throw new FormatException();
                }
                else
                {
                    monomial = monomial.Remove(varMatch.Index, varMatch.Length);

                    if (varMatch.Value[0] == '-')
                    {
                        coefficient = -1;
                    }
                }

                if (String.IsNullOrEmpty(monomial))
                {
                    return new Monomial(coefficient, 1);
                }
                else if (Int32.TryParse(monomial, out int degree))
                {
                    return new Monomial(coefficient, degree);
                }
                else
                {
                    throw new FormatException();
                }
            }
        }

        public override string ToString()
        {
            return $"{Coefficient}x^{Degree}";
        }
    }
}
