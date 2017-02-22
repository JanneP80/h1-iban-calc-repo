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
            // asks BBAN account number in format xxxxxx-xx(xxxx)

            Console.WriteLine("Please enter your BBAN account number: ");
            string bbannumber = Console.ReadLine();
            char[] arraytoedit = bbannumber.ToCharArray();

            // poista vielä väliviiva tässä TODO!
            for (int i=0; i<arraytoedit.Length; i++)
            {
                if (arraytoedit[0] == '4' | arraytoedit[0] == '5')
                {
                    for (int i2 = 0; i2 < 7; i2++) // OP and Aktia, starting with 4 and 5
                    {

                    }
                }
                else
                for (int i2 = 0; i2 < 6; i2++) // All the other banks
                {


                }
            }
            // convert BBAN to IBAN format : XXyy YYYY YYYY YYYY YY


            Console.ReadKey();
        }
    }
}
