using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SMKToolbox
{
    public partial class AssetManager : Form
    {
        public struct XmlEntry
        {
            public string Filename;
            public string Description;
            public int Offset;
            public bool Compression;
            public int MaxSize;
        }
        public AssetManager()
        {
            InitializeComponent();
            List<XmlEntry> compression = Parse("compress.xml");
            foreach (XmlEntry entry in compression)
            {
                Console.WriteLine("Filename: {0}", entry.Filename);
                Console.WriteLine("Description: {0}", entry.Description);
                Console.WriteLine("Offset: {0}", entry.Offset);
                Console.WriteLine("Compression: {0}", entry.Compression ? "yes" : "no");
                Console.WriteLine("MaxSize: {0}", entry.MaxSize);
                Console.WriteLine();
            }
        }

        public static List<XmlEntry> Parse(string filename)
        {
            List<XmlEntry> entries = new List<XmlEntry>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(filename);

                XmlNodeList nodes = doc.DocumentElement.SelectNodes("/entries/entry");

                foreach (XmlNode node in nodes)
                {
                    XmlEntry entry = new XmlEntry();
                    entry.Filename = node.SelectSingleNode("filename").InnerText;
                    entry.Description = node.SelectSingleNode("description").InnerText;
                    entry.Offset = int.Parse(node.SelectSingleNode("offset").InnerText);
                    entry.Compression = node.SelectSingleNode("compression").InnerText == "yes" ? true : false;
                    entry.MaxSize = int.Parse(node.SelectSingleNode("maxsize").InnerText);
                    entries.Add(entry);
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine("Error loading XML file: " + e.Message);
                // or: log the error, show a message box, etc.
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Error parsing XML data: " + e.Message);
                // or: log the error, show a message box, etc.
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected error: " + e.Message);
                // or: log the error, show a message box, etc.
            }

            return entries;
        }

    }
}
