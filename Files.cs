using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMKToolbox
{
    /*
     * This class is to be used to handle all file access.  
     *  -Opening & reading
     *  -Writing
     *  -Appending (not sure if required)
     *  -Inserting
     *  
     * It is to be interfaced by passing and returning data in byte arrays.
     * 
     */
    public static class Files
    {
        //Read: Takes in a filename and returns the contents as a byte[] and the status of it's success
        public static (byte[], bool) Read(string fileName)
        {
            try
            {
                byte[] fileBytes = File.ReadAllBytes(fileName);
                return (fileBytes, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while reading the file: " + ex.Message);
                return (null, false);
            }
        }

        //Write: Takes in a filename, byte[] and overwrite flag and writes the byte[] to the file, then returns the the status of it's success
        //Write: Takes in a filename, byte[] and overwrite flag and writes the byte[] to the file, then returns the the status of it's success
        public static bool Write(string fileName, byte[] fileBytes, bool overwrite)
        {
            try
            {
                if (File.Exists(fileName) && !overwrite)
                {
                    DialogResult result = MessageBox.Show("File already exists. Do you want to overwrite?", "File Exists", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        return false;
                    }
                }
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fileStream.Write(fileBytes, 0, fileBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while writing the file: " + ex.Message);
                return false;
            }
        }

        //Insert: Takes in a filename, byte[] and offset flag and writes the byte[] to the file at the offset provided, then returns the the status of it's success
        public static bool Insert(string fileName, byte[] fileBytes, int offset)
        {
            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    fileStream.Seek(offset, SeekOrigin.Begin);
                    fileStream.Write(fileBytes, 0, fileBytes.Length);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while inserting the byte array: " + ex.Message);
                return false;
            }
        }

        //Append: Takes in a filename, byte[] writes the byte[] to end of the file, then returns the the status of it's success
        public static bool Append(string fileName, byte[] fileBytes)
        {
            try
            {

                if (File.Exists(fileName))
                {
                    using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                    {
                        fs.Write(fileBytes, 0, fileBytes.Length);
                    }
                    return true;
                }
                else
                { 
                    MessageBox.Show("File does not exist.");
                    return false;
                }  
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while appending the byte array: " + ex.Message);
                return false;
            }
        }
    }

}


