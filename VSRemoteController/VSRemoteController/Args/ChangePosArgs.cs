using System;

namespace Args
{
    public class ChangePosArgs : EventArgs
    {
        public string Path { get; private set; }
        public int iLine { get; private set; }
        public int iColumn { get; private set; }
        public int iSelectLength { get; private set; }

        public ChangePosArgs()
        {
            Path = string.Empty;
            iLine = int.MinValue;
            iColumn = 0;
            iSelectLength = 0;
        }

        public ChangePosArgs(string path, int line, int column, int sellength)
        {
            Path = path;
            iLine = line;
            iColumn = column;
            iSelectLength = 0;
        }

        public bool FromStrings(string[] src)
        {
            if (src.Length < 2)
            {
                return false;
            }
            Path = src[1];

            if (2 < src.Length)
            {
                if (!int.TryParse(src[2], out int line))
                {
                    return false;
                }
                iLine = line - 1;
            }

            if (3 < src.Length)
            {
                if (!int.TryParse(src[3], out int column))
                {
                    return false;
                }
                iColumn = column - 1;
            }

            if (4 < src.Length)
            {
                if (!int.TryParse(src[4], out int sellength))
                {
                    return false;
                }
                iSelectLength = sellength;
            }

            return true;
        }

    }
}
