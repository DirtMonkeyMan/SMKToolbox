using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;



namespace SMKToolbox
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            /**
            (byte[] fileBytes, bool status) = Files.Read("Super Mario Kart (USA).sfc");
            if (status)
            {
                byte[] positionNumbers = Codec.Decompress(fileBytes, 386928);
                Files.Write("positionNumbers.bin", positionNumbers, true);
                (byte[] fileBytes2, bool status2) = Files.Read("positionNumbersEdited.bin");
                if (status2)
                {
                    byte[] compressedPositionNumbers = Codec.Compress(fileBytes2, false, true);
                    if(compressedPositionNumbers.Length > 1890)
                    {
                       Console.WriteLine("Too big to reinsert to ROM");
                    }
                    else
                    {
                        Files.Insert("Super Mario Kart (USA).sfc", compressedPositionNumbers, 386928);
                    }
                }
                // for example 
                Console.WriteLine(Encoding.UTF8.GetString(fileBytes));


                //Files.Write("Copy.txt", fileBytes, true);
                //Files.Append("Copy.txt", fileBytes);
                //Files.Insert("Copy.txt", fileBytes, 10);
            }
            else
            {
                // Handle the error
            }
            **/
            Application.Run(new Main());



        }

    }
}
