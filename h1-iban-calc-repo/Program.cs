﻿using System;
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
            //  remove space and line if needed
            bbannumber = bbannumber.Replace("-", "").Replace(" ", "");

            // TODO! tarkista ettei käyttäjä syötä liiba-laabaa TODO! 6+ (2...6).
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
            // calc the checksum with Luhn modulo 10 TODO!!
            // string checsum = new string(computerBBAN);
            // checsum = checsum.Remove(checsum.Length - 1, 1);
            // for (int i6 = computerBBAN.Length; i6 > computerBBAN.Length

            // convert BBAN to IBAN format : XXyy YYYY YYYY YYYY YY
            // add end FI and numbers TODO LUNCH!!!
            ibannumber = new string(computerBBAN);
            string ibannumbertemp = ibannumber.Insert(14, "151800"); //add FI00 = 151800
            Console.WriteLine(ibannumbertemp);
            // calc IBAN checksum
            double ibannumberinteger = Convert.ToDouble(ibannumbertemp);
            double counting = 0;
            double disc = 0;
            counting = ibannumberinteger % 97;
            disc = 98 - counting; // add to ibannumber (is string)
            Console.WriteLine(disc);
            Console.WriteLine(ibannumberinteger);
            // ibannumberinteger = ibannumberinteger + disc; // helppo muttei käyttökelpoinen
            Console.WriteLine(ibannumberinteger);

            // palauta string muotoon
            //ibannumber = ibannumberinteger.ToString(); // ei toimi, näyttää doublena

            string end2 = disc.ToString();
            // jos alle 10
            if (disc < 10)
            {
                ibannumber =  "FI" +'0' + end2 + ibannumber;
            }
            else
            {
                // if over 10
                ibannumber = "FI" + end2 + ibannumber;
            }

            Console.WriteLine(ibannumber);

            // 

            Console.ReadKey();
        }
    }
}
