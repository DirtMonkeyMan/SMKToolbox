using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#pragma warning disable IDE1006

namespace SMKToolbox
{
    public static partial class Overlay
    {
        public partial class overlayState
        {
            public List<byte[]> pieces;
            public byte[,] sizes;
            public byte[] rawData;
            public string writePath;
            public int writeAt;
        }

        public partial class overlaySetup
        {
            public string sourceFile;
            public string dataFile;
            public string patternRef;
            public string sizeRef;
            public int startsAt;
        }

        public partial class overlayPiece
        {
            // public int dataPos; //unused?
            public Bitmap[] buffers;
            public bool[] valid;
        }

        public partial class overlayBlock
        {
            public bool enabled;

            public int x;
            public int y;
            public int entry;
            public int size;

            public Rectangle rect = new Rectangle();
        }
    }
}