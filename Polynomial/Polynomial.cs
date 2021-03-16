using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Polynomial
{
    public class Polynomial : IEnumerable<Monomial>
    {
        private List<Monomial> Monomials { get; set; } = new List<Monomial>();

        public Monomial this[int index]
        {
            get { return new Monomial(Monomials[index]); }
            set 
            { 
                Monomials[index] = value;

                Simplify();
            }
        }

        private Polynomial() { }

        private Polynomial(IEnumerable<Monomial> monomials, bool needsSimpl)
        {
            foreach (var monomial in monomials)
            {
                Monomials.Add(new Monomial(monomial));
            }

            if (needsSimpl == true)
            {
                Simplify();
            }
        }

        public Polynomial(IEnumerable<Monomial> monomials) : this(monomials, true) { }

        public Polynomial(string polynomial)
        {
            string polStr = polynomial.TrimStart();

            if (String.IsNullOrEmpty(polStr))
            {
                throw new ArgumentException("Polynomial parameter can't be empty.");
            }

            polStr = FixFormat(polStr);

            var monomialsStr = polStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var monomialStr in monomialsStr)
            {
                if (monomialStr[0] != '-' && monomialStr[0] != '+')
                {
                    throw new FormatException();
                }

                Monomials.Add(Monomial.Parse(monomialStr));
            }

            Simplify();

            static string FixFormat(string polynomialStr)
            {
                if (polynomialStr[0] != '-' && polynomialStr[0] != '+')
                {
                    polynomialStr = polynomialStr.Insert(0, "+");
                }

                int counter = 0;

                foreach (Match m in Regex.Matches(polynomialStr, @"\w[+-]"))
                {
                    polynomialStr = polynomialStr.Insert(m.Index + 1 + counter, " ");

                    counter++;
                }

                polynomialStr = Regex.Replace(polynomialStr, @"-\s+", "-");
                polynomialStr = Regex.Replace(polynomialStr, @"\+\s+", "+");

                return polynomialStr;
            }
        }

        private void Simplify()
        {
            Monomials = new List<Monomial>(Monomials.OrderByDescending(m => m.Degree));

            if (Monomials.Count != 0)
            {
                int degree = Monomials[0].Degree;

                for (int i = 1; i < Monomials.Count; i++)
                {
                    if (Monomials[i].Degree == degree)
                    {
                        Monomials[i - 1].Coefficient += Monomials[i].Coefficient;
               
                        Monomials.RemoveAt(i);

                        i--;
                    }
                    else
                    {
                        degree = Monomials[i].Degree;
                    }
                }
            }

            for (int i = 0; i < Monomials.Count; i++)
            {
                if (Monomials[i].Coefficient == 0)
                {
                    Monomials.RemoveAt(i);
                }
            }
        }

        public IEnumerator<Monomial> GetEnumerator()
        {
            return Monomials.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Monomials.GetEnumerator();
        }

        public static Polynomial operator +(Polynomial a, Polynomial b) 
            => new Polynomial(a.Monomials.Concat(b.Monomials), true);

        public static Polynomial operator -(Polynomial a, Polynomial b)
        {   
            Polynomial polynomial = new Polynomial(a.Monomials, false);

            foreach (var monomial in b.Monomials)
            {
                polynomial.Monomials.Add(new Monomial(-monomial.Coefficient, monomial.Degree));
            }

            polynomial.Simplify();

            return polynomial;
        }

        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            Polynomial polynomial = new Polynomial();

            foreach (var aMon in a.Monomials)
            {
                foreach (var bMon in b.Monomials)
                {
                    polynomial.Monomials.Add(new Monomial(aMon.Coefficient * bMon.Coefficient, 
                        aMon.Degree + bMon.Degree));
                }
            }

            polynomial.Simplify();

            return polynomial;
        }

        public override string ToString()
        {
            StringBuilder polynomialStr = new StringBuilder();

            for (int i = 0; i < Monomials.Count; i++)
            {
                if (i == 0)
                {
                    polynomialStr.Append((Monomials[i].Degree == 0) ? Monomials[i].Coefficient : Monomials[i]);
                }
                else if (Monomials[i].Coefficient < 0 && Monomials[i].Degree == 0)
                {
                    polynomialStr.Append($" - {-Monomials[i].Coefficient}");
                }
                else if (Monomials[i].Coefficient < 0)
                {
                    polynomialStr.Append($" - {-Monomials[i].Coefficient}x^{Monomials[i].Degree}");
                }
                else
                {
                    polynomialStr.Append($" + " +
                        $"{(Monomials[i].Degree == 0 ? Monomials[i].Coefficient : Monomials[i])}");
                }
            }

            return polynomialStr.ToString();
        }
    }
}
