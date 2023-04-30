using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMKToolbox
{
    public static partial class ASMReading
    {
        public struct labeledData
        {
            public string label;
            public byte[] data;
        }

        private static string[] asmInstructions =
        {
            "EQU", "ORG", "SECT", "REL",                                   // 0, 1, 2, 3,
            "ADC", "SBC",                                                  // 4, 5,
            "CMP",                                                         // 6,
            "DEC", "INC",                                                  // 7, 8,
            "AND", "EOR", "ORA", "BIT", "TRB", "TSB",                      // 9, 10, 11, 12, 13, 14,
            "ASL", "LSR", "ROL", "ROR",                                    // 15, 16, 17, 18,
            "BCC", "BCS", "BEQ", "BMI", "BNE", "BPL", "BRA", "BVC", "BVS", // 19, 20, 21, 22, 23, 24, 25, 26, 27,
            "BRL", "JMP", "JSL", "JSR",                                    // 28, 29, 30, 31,
            "COP",                                                         // 32,
            "REP", "SEP",                                                  // 33, 34,
            "LDA", "LDX", "LDY", "STA", "STX", "STY", "STZ",               // 35, 36, 37, 38, 39, 40, 41,
            "MVN", "MVP",                                                  // 42, 43,
            "PEA", "PEI", "PER",                                           // 44, 45, 46,
            "DB", "BYTE", "DW", "WORD", "DL", "LWORD"                      // 47, 48, 49, 50, 51, 52
        };

        private static char[] whitespace =
        {
            ' ', '\r', '\n', '\t'
        };

        private static char[] searchTermsComma =
        {
            ' ', '\r', '\n', '\t', ','
        };

        const int dataTerms = 47;
        const int byteShort = dataTerms;
        const int byteLong = dataTerms + 1;
        const int wordShort = dataTerms + 2;
        const int wordLong = dataTerms + 3;
        const int lWordShort = dataTerms + 4;
        const int lWordLong = dataTerms + 5;
        const int SearchNotFound = -1;

        public static int skipInvalidChars(char[] text, char[] invChars, int index, bool back)
        {
            if (Array.IndexOf(invChars, text[index]) == SearchNotFound) return index;

            int a;
            if (back) a = -1; else a = 1;

            while ((index > 0) & (index < text.Length))
            {
                index += a;
                if (Array.IndexOf(invChars, text[index]) == SearchNotFound) return index;
            }

            return index;
        }

        public static string writeBeforeWhitespaceBack(char[] text, int index)
        {
            string textOut = "";

            while (index > 0)
            {
                textOut = text[index] + textOut;
                index--;

                if (Array.IndexOf(whitespace, text[index]) != SearchNotFound) return textOut.ToUpper();
            }

            return textOut.ToUpper();
        }

        public static (string, int) writeBeforeWhistespaceFore(char[] text, int index)
        {
            string textOut = "";
            int b = text.Length - 1;

            while (index < b)
            {
                textOut += text[index];
                index++;

                if (Array.IndexOf(whitespace, text[index]) != SearchNotFound) return (textOut.ToUpper(), index);
            }

            return (textOut.ToUpper(), index);
        }

        private static bool isComment(char[] source, int position)
        {
            char chkChar;

            while (true)
            {
                chkChar = source[position];
                switch (chkChar)
                {
                    case ';':
                        return true;
                    case '\r':
                        return false;
                    case '\n':
                        return false;
                    default:
                        position--;
                        continue;
                }
            }
        }

        public static int findLabel(string source, string label)
        {
            int i = 0;
            int j;

            string useLabel = label.Trim();
            string findKeyword;
            char[] sourceChar = source.ToCharArray();

            while (true)
            {
                i = source.IndexOf(useLabel, i);
                if (i > 0)
                {
                    j = skipInvalidChars(sourceChar, whitespace, i - 1, true);

                    if (sourceChar[j] != ',')
                    {
                        findKeyword = writeBeforeWhitespaceBack(sourceChar, j);
                        if ((Array.IndexOf(asmInstructions, findKeyword) == -1) & !isComment(sourceChar, i)) return i;
                    }

                    i++;
                    continue;
                }
                else return i;
            }
        }

        public static labeledData[] getLabels(char[] source, int start)
        {
            labeledData[] labels = new labeledData[0];

            start = skipInvalidChars(source, whitespace, start, false);
            if (start >= source.Length) return labels;

            (string[], int) funcOut;
            int len;
            int i;

            while (true)
            {
                funcOut = decodeLabelLine(source, start);
                len = funcOut.Item1.Length;
                if (len == 0) break;

                for (i = 0; i < len; i++)
                {
                    labels = labels.Append(new labeledData { label = funcOut.Item1[i], data = new byte[0] }).ToArray();
                }

                start = skipInvalidChars(source, whitespace, funcOut.Item2, false);
                if (start >= source.Length) break;
            }

            return labels;
        }

        public static (string[], int) decodeLabelLine(char[] source, int start)
        {
            (string, int) input = writeBeforeWhistespaceFore(source, start);

            string term = input.Item1;
            (string[], int) labels = (new string[0], start);

            start = skipInvalidChars(source, whitespace, input.Item2, false);
            if (start >= source.Length) return labels;

            if (Array.IndexOf(asmInstructions, term.ToUpper()) < wordShort) return labels;

            while (true)
            {
                term = "";

                while (true)
                {
                    term += source[start];
                    start++;

                    if (start >= source.Length) return labels;
                    if (Array.IndexOf(searchTermsComma, source[start]) != SearchNotFound) break;
                }

                term = term.Trim();
                if (Array.IndexOf(asmInstructions, term.ToUpper()) != SearchNotFound) break;

                labels.Item1 = labels.Item1.Append(term).ToArray();

                while (source[start] != ',')
                {
                    if (Array.IndexOf(searchTermsComma, source[start]) != SearchNotFound)
                    {
                        labels.Item2 = start;
                        return labels;
                    }
                    start++;
                    if (start >= source.Length) return labels;
                }

                start = skipInvalidChars(source, whitespace, start + 1, false);
                if (start >= source.Length) return labels;
            }

            return labels;
        }

        public static byte[] convertData(char[] source, int start)
        {
            byte[] data = new byte[0];

            start = skipInvalidChars(source, whitespace, start, false);
            (byte[], int) funcOut;
            int len;
            int i;

            while (true)
            {
                funcOut = decodeDataLine(source, start);
                len = funcOut.Item1.Length;
                if (len == 0) break;

                for (i = 0; i < len; i++)
                {
                    data = data.Append(funcOut.Item1[i]).ToArray();
                }

                start = skipInvalidChars(source, whitespace, funcOut.Item2, false);
                if (start >= source.Length) break;
            }

            return data;
        }

        public static labeledData[] convertData(string source, labeledData[] datTable)
        {
            int i;
            int j;
            string labelName;

            for (i = 0; i < datTable.Length; i++)
            {
                labelName = datTable[i].label;
                j = findLabel(source, labelName);
                if (j == -1) throw new Exception("Unable to find a label within label table.");
                j += labelName.Length;

                datTable[i].data = convertData(source.ToCharArray(), j);
            }

            return datTable;
        }

        public static byte[] interpretData(string data, int length)
        {
            byte[] datOut = new byte[length];
            char[] datChar = data.ToUpper().ToCharArray();
            int i = 0;
            int l = datChar.Length - 1;
            int val = 0;
            short dig;
            short limit = 0x0032;
            int mul = 2;

            switch (datChar[l])
            {
                case 'B': // 8 binary bits in 1 byte. values are already defined.
                    break;

                case 'O': // 2.67 octal digits in 1 byte.
                    limit = 0x0038;
                    mul = 8;
                    break;

                case 'H': // 2 hexadecimal digits in 1 byte.
                    l--;

                    while (l >= 0)
                    {
                        dig = (short)datChar[l];
                        if ((dig > 0x002F) & (dig < 0x003A)) val += (dig - 0x0030) * (int)Math.Pow(16, i);
                        else if ((dig > 0x0040) & (dig < 0x0047)) val += (dig - 0x0037) * (int)Math.Pow(16, i);
                        else throw new Exception("The data value is invalid.");
                        l--;
                        i++;
                    }

                    goto skipDecoding;

                default:  // listen, nobody f!?king knows what decimal is in the computer science world
                    if (datChar[l] != 'D') l++;
                    limit = 0x003A;
                    mul = 10;
                    break;
            }

            l--;

            while (l >= 0)
            {
                dig = (short)datChar[l];
                if ((dig > 0x002F) & (dig < limit)) val += (dig - 0x0030) * (int)Math.Pow(mul, i); else throw new Exception("The data value is invalid.");
                i++;
                l--;
            }

        skipDecoding:;
            for (i = 0; i < datOut.Length; i++)
            {
                datOut[i] = (byte)(val / (int)(Math.Pow(256, i)));
            }

            return datOut;
        }

        public static (byte[], int) decodeDataLine(char[] source, int start)
        {
            (string, int) input = writeBeforeWhistespaceFore(source, start);

            string term = input.Item1;
            (byte[], int) data = (new byte[0], start);

            start = skipInvalidChars(source, whitespace, input.Item2, false);
            if (start >= source.Length) return data;

            // (string[], int) labels = (new string[0], start);
            byte[] datOut;
            int iof = Array.IndexOf(asmInstructions, term.ToUpper());

            if (iof < byteShort) return data;

            int datLength = 1;
            int i;

            switch (iof)
            {
                case byteShort:
                    datLength = 1;
                    break;
                case byteLong:
                    datLength = 1;
                    break;

                case wordShort:
                    datLength = 2;
                    break;
                case wordLong:
                    datLength = 2;
                    break;

                case lWordShort:
                    datLength = 3;
                    break;
                case lWordLong:
                    datLength = 3;
                    break;
            }

            while (true)
            {
                term = "";

                while (true)
                {
                    term += source[start];
                    start++;

                    if (start >= source.Length) return data;
                    if (Array.IndexOf(searchTermsComma, source[start]) != SearchNotFound) break;
                }

                term = term.Trim().ToUpper();
                if (Array.IndexOf(asmInstructions, term) != SearchNotFound) break;

                datOut = interpretData(term, datLength);

                for (i = 0; i < datOut.Length; i++)
                {
                    data.Item1 = data.Item1.Append(datOut[i]).ToArray();
                }

                while (source[start] != ',')
                {
                    if (Array.IndexOf(searchTermsComma, source[start]) != SearchNotFound)
                    {
                        data.Item2 = start;
                        return data;
                    }
                    start++;
                    if (start >= source.Length) return data;
                }

                start = skipInvalidChars(source, whitespace, start + 1, false);
                if (start >= source.Length) return data;
            }

            return data;
        }
    }
}
