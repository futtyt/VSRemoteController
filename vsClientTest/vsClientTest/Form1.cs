using System;
using System.IO;
using System.IO.Pipes;
using System.Security.Principal;
using System.Windows.Forms;

namespace vsClientTest
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void ConnectVS_Click(object sender, EventArgs e)
        {
            try
            {
                using (var pipeClient = new NamedPipeClientStream(".", "CommandMonitoring.Server", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation))
                {
                    pipeClient.Connect();
                    var ss = new Utility.StreamString(pipeClient);

                    var write = ss.WriteString(string.Format("{0}\t{1}\t{2}\t{3}\t{4}", CommandType.Text, SourcePath.Text, SourceLineNumber.Value, SourceColumnNumber.Value, 10));
                    var result = ss.ReadString();
                    MessageBox.Show(result);
                }
            }
            catch (OverflowException ofex)
            {
                Console.WriteLine(ofex.Message);
            }
            catch (IOException ioe)
            {
                // 送信失敗
                Console.WriteLine(ioe.Message);
            }
        }
    }
}
