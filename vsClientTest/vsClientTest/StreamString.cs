using System;
using System.IO;
using System.Text;

namespace Utility
{
    public class StreamString : IDisposable
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;
        private bool disposedValue;

        public StreamString(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadString()
        {
            int len = 0;
            byte[] inSizeLE = BitConverter.GetBytes(len);

            ioStream.Read(inSizeLE, 0, inSizeLE.Length);
            Array.Reverse(inSizeLE);
            len = BitConverter.ToInt32(inSizeLE, 0);

            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteString(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;

            byte[] outSizeBE = BitConverter.GetBytes(len);
            Array.Reverse(outSizeBE);

            ioStream.Write(outSizeBE, 0, outSizeBE.Length);
            ioStream.Write(outBuffer, 0, len);
            ioStream.Flush();

            return outBuffer.Length + 2;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    ioStream = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを 'Dispose(bool disposing)' メソッドに記述します
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
