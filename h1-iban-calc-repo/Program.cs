using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace h1_iban_calc_repo
{
    class Program
    {
        static string bbannumber;
        static char[] computerBBAN = { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' }; //new char[14];
        static string ibannumber;
        static string ibannumbertemp;

        static void Main(string[] args)
        {
            // About program desing: BBAN -> IBAN converter
            //  Chapter  I  - Asks BBAN number       - input : None   - returns: bbannumber
            //  Chapter  II - Convert to computerBBAN format : bbannumber : computerBBAN
            //  Chapter III - Check by calculating checksum  : computerBBAN : result
            //  Chapter IV  - Convert BBAN to IBAN format    : computerBBAN : ibannumber
            //  Chapter  V  - IBAN number checksum           : ibannumber : ibannumber
            //  Chapter VI  - Writes ibannumber on screen

            // Main starts :

            bbannumber = InputBBANNumber();     // I
            Console.WriteLine(bbannumber);
            ConvertToComputerBBAN(bbannumber);  // II
            CalcBBANChecksum(computerBBAN);     // III // Maybe return result here and print it in Main and call loop again goto until pass
            ConvertBBANToIBAN(computerBBAN);    // IV
            ibannumber = CalcIBANChecksum(ibannumber, ibannumbertemp);       // V
                                                                             // VI : Write in main.
            Console.WriteLine(ibannumber);

            Console.ReadKey();
        }

        static string InputBBANNumber()   // I
        {
            // asks BBAN account number in format xxxxxx-xx(xxxx)

            Console.WriteLine("Please enter your BBAN account number in format xxxxxx-xx(xxxx): ");
            bbannumber = Console.ReadLine();
            //  remove space and line if needed
            bbannumber = bbannumber.Replace("-", "").Replace(" ", "");

            // TODO! tarkista ettei käyttäjä syötä liiba-laabaa TODO! 6+ (2...6).

            Console.WriteLine(bbannumber);
            return bbannumber;
        }

        static char[] ConvertToComputerBBAN(string bbannumber)    // II
        {
            int templength = 0;
            Console.WriteLine("bug:");
            Console.WriteLine(bbannumber);
            char[] arraytoedit = bbannumber.ToCharArray();

            for (int i = 0; i < arraytoedit.Length; i++) // start converting into computer liguistic mode
            {
                if (arraytoedit[0] == '4' | arraytoedit[0] == '5')
                {
                    for (int i2 = 0; i2 < 7; i2++) // OP and Aktia, starting with 4 and 5
                    {
                        computerBBAN[i2] = arraytoedit[i2];
                    }
                    // 7-14                    
                    templength = arraytoedit.Length - 7;
                }
                else
                {
                    for (int i3 = 0; i3 < 6; i3++) // All the other banks
                    {
                        computerBBAN[i3] = arraytoedit[i3];
                    }
                    templength = arraytoedit.Length - 6; // 6-14
                }
            }
            Console.WriteLine(templength);
            Console.WriteLine(arraytoedit);
            Console.WriteLine(computerBBAN);
            int i5 = 0;
            for (int i4 = arraytoedit.Length; i4 > arraytoedit.Length - templength; i4--)
            {

                computerBBAN[13 - i5] = arraytoedit[i4 - 1];
                i5++;
            }
            Console.WriteLine(computerBBAN);
            return computerBBAN;
        }

        static void CalcBBANChecksum(char[] computerBBANr) // III
        {
            // calc the checksum with Luhn modulo 10 !!            
            int[] multiplermatrix = { 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            char[] computerBBANchecksum = computerBBAN.Take(computerBBAN.Count() - 1).ToArray(); // One last number must be removed for calculations
            int[] BBANint = Array.ConvertAll(computerBBANchecksum, c => (int)Char.GetNumericValue(c));
            //for (int a = 0; a < BBANint.Length; a++)
            //{

            //}
            int[] BBANintChecksum = new int[13];
            for (int ii = 0; ii < BBANintChecksum.Length; ii++)
            {
                BBANintChecksum[ii] = BBANint[ii] * multiplermatrix[ii];
            }
            // Console.WriteLine(BBANintChecksum);
            for (int ai = 0; ai < BBANintChecksum.Length; ai++)
            {
                Console.Write(BBANintChecksum[ai] + " ");

            }
            string results = string.Join("", BBANintChecksum.Select(i => i.ToString()).ToArray());
            Console.WriteLine(results);

            int[] y = results.Select(o => o - 48).ToArray();

            int sum = 0;
            for (int ei = 0; ei < y.Length; ei++)
            {
                sum += y[ei];
            }
            Console.WriteLine(sum);
            int sumCeil = (int)(Math.Ceiling(sum / 10.0d) * 10);
            Console.WriteLine(sumCeil - sum);
            int checksum10 = sumCeil - sum;

            // computerBBAN to int
            int[] BBANint14 = Array.ConvertAll(computerBBAN, c => (int)Char.GetNumericValue(c));

            if (BBANint14[13] == checksum10)
            {
                Console.WriteLine("BBAN Checksum check pass.");
            }
            else
            {
                Console.WriteLine("BBAN Checksum check fail.");
            }
            // Is here need for reorganize
            // Maybe return result here and print it in Main and call loop again goto until pass
        }

        static string ConvertBBANToIBAN(char[] computerBBAN)  // IV
        {

            // convert BBAN to IBAN format : XXyy YYYY YYYY YYYY YY
            // add end FI and numbers
            ibannumber = new string(computerBBAN);
            ibannumbertemp = ibannumber.Insert(14, "151800"); //add FI00 = 151800
            Console.WriteLine(ibannumbertemp);
            return ibannumbertemp;
        }

        static string CalcIBANChecksum(string ibannumber, string ibannumbertemp) // V
        {

            // calc IBAN checksum

            decimal ibannumberinteger = Convert.ToDecimal(ibannumbertemp);
            decimal counting = 0;
            decimal disc = 0;

            counting = (ibannumberinteger % 97);
            disc = (98 - counting); // add to ibannumber (is string)
            Console.WriteLine(disc);
            Console.WriteLine(ibannumberinteger);
            // ibannumberinteger = ibannumberinteger + disc; // helppo muttei käyttökelpoinen

            // palauta string muotoon
            // ibannumber = ibannumberinteger.ToString(); // ei toimi, näyttää doublena

            string end2 = disc.ToString();
            // jos alle 10
            if (disc < 10)
            {
                ibannumber = "FI" + '0' + end2 + ibannumber;
            }
            else
            {
                // if over 10
                ibannumber = "FI" + end2 + ibannumber;
            }

            Console.WriteLine(ibannumber);
            return ibannumber;
        }
    }
}
