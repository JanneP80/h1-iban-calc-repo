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
            d1: // Checking return result here and print fail in Main and call loop again goto until pass
            { 
                bbannumber = InputBBANNumber();                              // I
                ConvertToComputerBBAN(bbannumber);                           // II
                if (CalcBBANChecksum(computerBBAN) == false)                 // III 
                {
                    Console.WriteLine("Try Again.");
                    computerBBAN = new char[] { '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' }; // reset to avoid false on next correct time
                    goto d1;
                }
                else goto o1;
            }
            o1:
            ConvertBBANToIBAN(computerBBAN);                                 // IV
            ibannumber = CalcIBANChecksum(ibannumber, ibannumbertemp);       // V
                                                                             // VI : Write in main.
            Console.Write("Your IBAN number is: ");
            Console.WriteLine(ibannumber); // Final result of conversion
            Console.ReadKey();
        }

        static string InputBBANNumber()   // I
        {
            // asks BBAN account number in format xxxxxx-xx(xxxx)
            b2:
            Console.WriteLine("Please enter your BBAN account number in format xxxxxx-xx(xxxx): ");
            bbannumber = Console.ReadLine();
            //  remove space and line if needed
            bbannumber = bbannumber.Replace("-", "").Replace(" ", "");            
            // Checking for correct input
            if (bbannumber.Length < 8 | bbannumber.Length > 14 | !bbannumber.All(Char.IsNumber))
            {                
                    string errorMessage = "The number you gave is not in BBAN format.\nYour input was either too short or long or it contained non numeric characters.";
                    Console.WriteLine(errorMessage);
                    goto b2;                    
            }
            else
            {
                return bbannumber;
            } 
        }

        static char[] ConvertToComputerBBAN(string bbannumber)    // II
        {
            int templength = 0;
            char[] arraytoedit = bbannumber.ToCharArray();

            for (int i = 0; i < arraytoedit.Length; i++) // Start converting into computer liguistic mode
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
            
            int i5 = 0;
            for (int i4 = arraytoedit.Length; i4 > arraytoedit.Length - templength; i4--)
            {

                computerBBAN[13 - i5] = arraytoedit[i4 - 1];
                i5++;
            }
            // Console.WriteLine(computerBBAN);
            return computerBBAN;
        }

        static bool CalcBBANChecksum(char[] computerBBANr) // III
        {
            // calc the checksum with Luhn modulo 10 !!            
            int[] multiplermatrix = { 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2, 1, 2 };
            int[] BBANintChecksum = new int[13];
            int sum = 0;

            char[] computerBBANchecksum = computerBBAN.Take(computerBBAN.Count() - 1).ToArray(); // One last number must be removed for calculations
            int[] BBANint = Array.ConvertAll(computerBBANchecksum, c => (int)Char.GetNumericValue(c));
            
            for (int ii = 0; ii < BBANintChecksum.Length; ii++)
            {
                BBANintChecksum[ii] = BBANint[ii] * multiplermatrix[ii];
            }
            /*
            for (int ai = 0; ai < BBANintChecksum.Length; ai++)
            {
                Console.Write(BBANintChecksum[ai] + " "); // Error checking option
            }
            */
            string results = string.Join("", BBANintChecksum.Select(i => i.ToString()).ToArray());
            // Console.WriteLine(results);

            int[] y = results.Select(o => o - 48).ToArray();

            for (int ei = 0; ei < y.Length; ei++)
            {
                sum += y[ei];
            }
            // Console.WriteLine(sum); // for error checking
            int sumCeil = (int)(Math.Ceiling(sum / 10.0d) * 10);            
            int checksum10 = sumCeil - sum;
            Console.Write("BBAN checksum: {0} - {1} = {2}. ", sumCeil, sum, checksum10);

            // computerBBAN to int
            int[] BBANint14 = Array.ConvertAll(computerBBAN, c => (int)Char.GetNumericValue(c));
            Console.WriteLine("Compared to: {0}", BBANint14[13]);
            if (BBANint14[13] == checksum10)
            {
                Console.WriteLine("BBAN Checksum check pass.");
                return true;
            }
            else
            {
                Console.WriteLine("BBAN Checksum check fail.");
                return false;
            }            
        }

        static string ConvertBBANToIBAN(char[] computerBBAN)  // IV
        {
            // Goal: convert BBAN to IBAN format : XXyy YYYY YYYY YYYY YY
            // First add end FI and numbers
            ibannumber = new string(computerBBAN);
            ibannumbertemp = ibannumber.Insert(14, "151800"); //add FI00 = 151800
            // Console.WriteLine(ibannumbertemp);
            return ibannumbertemp; // Also could be calling CalcIBANChecksum here for better logic.
        }

        static string CalcIBANChecksum(string ibannumber, string ibannumbertemp) // V
        {
            // calc IBAN checksum

            decimal ibannumberinteger = Convert.ToDecimal(ibannumbertemp);
            decimal counting = 0;
            decimal disc = 0;

            counting = (ibannumberinteger % 97);
            disc = (98 - counting); // Next add this to ibannumber (is string)
            // Console.WriteLine(disc);            
            // ibannumberinteger = ibannumberinteger + disc; // helppo muttei käyttökelpoinen
            // Console.WriteLine(ibannumberinteger);

            // Revert back to string
            // ibannumber = ibannumberinteger.ToString(); // ei toimi, näyttää doublena

            string end2 = disc.ToString();
            // If under 10, add extra zero
            if (disc < 10)
            {
                ibannumber = "FI" + '0' + end2 + ibannumber;
            }
            else
            {
                // if over 10
                ibannumber = "FI" + end2 + ibannumber;
            }            
            return ibannumber;
        }
    }
}
