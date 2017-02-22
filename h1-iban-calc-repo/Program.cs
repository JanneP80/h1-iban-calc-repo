using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h1_iban_calc_repo
{
    class Program
    {
        static void Main(string[] args)
        {
            //string bbannumber;
            string ibannumber;
            int templength = 0;
            char[] computerBBAN = { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' }; //new char[14];
            // asks BBAN account number in format xxxxxx-xx(xxxx)

            Console.WriteLine("Please enter your BBAN account number in format xxxxxx-xx(xxxx): ");
            string bbannumber = Console.ReadLine();
            // TODO! tarkista ettei käyttäjä syötä liiba-laabaa TODO! 6+ (2...6).
            char[] arraytoedit = bbannumber.ToCharArray();

            // TODO! poista vielä väliviiva tässä TODO!
            for (int i = 0; i < arraytoedit.Length; i++) // start converting into computer liguistic mode
            {
                if (arraytoedit[0] == '4' | arraytoedit[0] == '5')
                {
                    for (int i2 = 0; i2 < 7; i2++) // OP and Aktia, starting with 4 and 5
                    {
                        computerBBAN[i2] = arraytoedit[i2];
                    }
                    // add end numbers 
                    // 7-14
                    // Add the rest zeros
                    templength = arraytoedit.Length - 7;
                    for (int i2 = arraytoedit.Length; i2 > templength; i2--)
                    {
                        computerBBAN[i2] = arraytoedit[i2];
                    }

                }
                else
                {
                    for (int i2 = 6; i2 < templength; i2++) // All the other banks
                    {
                        computerBBAN[i2] = arraytoedit[i2];
                    }
                    templength = arraytoedit.Length - 6;
                    for (int i2 = arraytoedit.Length; i2 > templength; i2--)
                    {
                        computerBBAN[i2] = arraytoedit[i2];
                    }
                    // add end numbers
                    // 6-14
                    // Add the rest zeros

                }

                // add end numbers

                // Add the rest zeros
            }
            // convert BBAN to IBAN format : XXyy YYYY YYYY YYYY YY
            Console.WriteLine(computerBBAN);

            Console.ReadKey();
        }
    }
}
