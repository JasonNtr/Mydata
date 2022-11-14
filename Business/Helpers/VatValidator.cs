using System.Linq;
using System.Text.RegularExpressions;

namespace Business.Helpers
{
    public static class VatValidator
    {
        public static bool Validate(string vatNumber)
        {
            if (string.IsNullOrEmpty(vatNumber))
            {
                return false;
            }
            vatNumber = vatNumber.Trim();

            if (vatNumber.Length != 9)
            {
                return false;
            }

            var isNaNre = new Regex(@"^\d+$");
            if (!isNaNre.IsMatch(vatNumber))
            {
                return false;
            }

            if (vatNumber == "000000000")
            {
                return false;
            }

            var body = vatNumber.Substring(0, 8);
            var sum = body.Select(t => int.Parse(t.ToString())).Select((digit, i) => digit << (8 - i)).Sum();

            var calc = sum % 11;
            var d9 = int.Parse(vatNumber[8].ToString());
            var valid = calc % 10 == d9;

            return valid;
        }
    }
}