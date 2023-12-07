using Args;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.IO.Pipes;
using System.Security.Principal;

namespace CommandMonitoring
{
    public class Server
    {
        private readonly AsyncPackage Package;
        private readonly EnvDTE80.DTE2 DTE;

        public Server()
        {

        }

        public Server(AsyncPackage package, EnvDTE80.DTE2 dte)
        {
            Package = package;
            DTE = dte;
        }

        public void Start()
        {
            _ = System.Threading.Tasks.Task.Run(() => MonitorLoopAsync());
        }

        public void Stop()
        {
            StopMonitorLoop();
        }

        /// <summary>
        /// コマンド監視サーバ
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task MonitorLoopAsync()
        {
            using (var pipeServer = new NamedPipeServerStream("CommandMonitoring.Server", PipeDirection.InOut, 1))
            {
                bool isEndServer = false;
                while (!isEndServer)
                {
                    try
                    {
                        // クライアントの接続待ち
                        await pipeServer.WaitForConnectionAsync();

                        using (var ss = new Utility.StreamString(pipeServer))
                        {
                            while (!isEndServer)
                            {
                                // 受信待ち
                                var read = ss.ReadString();
                                if (string.IsNullOrEmpty(read))
                                {
                                    break;
                                }

                                string[] cmds = read.Split('\t');

                                CommandType cmd = CommandType.Undefined;
                                try
                                {
                                    cmd = (CommandType)Enum.Parse(typeof(CommandType), cmds[0]);
                                }
                                catch
                                {
                                    ;
                                }

                                switch (cmd)
                                {
                                    case CommandType.StopMonitor:
                                        isEndServer = true;
                                        break;
                                    case CommandType.OpenDocument:
                                        {
                                            OpenDocumentArgs odArgs = new OpenDocumentArgs();
                                            if (!odArgs.FromStrings(cmds))
                                            {
                                                ss.WriteString(string.Format("Error\tInvalidate args \"{0}\".", cmds[0]));
                                                continue;
                                            }

                                            bool result = await OpenDocumentAsync(odArgs);

                                            ss.WriteString(string.Format(result ? "Success\t{0}" : "Error\t{0}", cmds[0]));
                                        }
                                        break;
                                    case CommandType.CenterLines:
                                    case CommandType.SetCaretPos:
                                    case CommandType.SetSelection:
                                        {
                                            ChangePosArgs cpArgs = new ChangePosArgs();
                                            if (!cpArgs.FromStrings(cmds))
                                            {
                                                ss.WriteString(string.Format("Error\tInvalidate args \"{0}\".", cmds[0]));
                                                continue;
                                            }

                                            bool result = await ChangePosAsync(cmd, cpArgs);

                                            ss.WriteString(string.Format(result ? "Success\t{0}" : "Error\t{0}", cmds[0]));
                                        }
                                        break;
                                    default:
                                        ss.WriteString(string.Format("Error\tUndefined Command \"{0}\".", cmds[0]));
                                        break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        ;
                    }
                    finally
                    {
                        pipeServer.Disconnect();
                    }
                }

            }
        }

        private async System.Threading.Tasks.Task<bool> OpenDocumentAsync(OpenDocumentArgs odArgs)
        {
            try
            {
                if (!VsShellUtilities.IsDocumentOpen(Package, odArgs.Path, Guid.Empty, out IVsUIHierarchy hierarchy, out uint itemID, out IVsWindowFrame windowFrame))
                {
                    VsShellUtilities.OpenDocument(Package, odArgs.Path, Guid.Empty, out hierarchy, out itemID, out windowFrame);
                }

                // メインスレッドに切替える.
                await Package.JoinableTaskFactory.SwitchToMainThreadAsync();
                windowFrame.Show();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private enum CommandType
        {
            Undefined,
            OpenDocument,
            CenterLines,
            SetCaretPos,
            SetSelection,
            StopMonitor,
        }

        private async System.Threading.Tasks.Task<bool> ChangePosAsync(CommandType cmd, ChangePosArgs odArgs)
        {
            int cmdResult = Microsoft.VisualStudio.VSConstants.S_FALSE;

            try
            {
                if (odArgs.iLine < 0)
                {
                    return false;
                }

                // メインスレッドに切替える.
                await Package.JoinableTaskFactory.SwitchToMainThreadAsync();

                var path = odArgs.Path;
                if (string.IsNullOrEmpty(path))
                {
                    path = DTE.ActiveDocument.FullName;
                }

                if (!VsShellUtilities.IsDocumentOpen(Package, path, Guid.Empty, out IVsUIHierarchy hierarchy, out uint itemID, out IVsWindowFrame windowFrame))
                {
                    VsShellUtilities.OpenDocument(Package, path, Guid.Empty, out hierarchy, out itemID, out windowFrame);
                }

                IVsTextView tview = VsShellUtilities.GetTextView(windowFrame);

                switch (cmd)
                {
                    case CommandType.CenterLines:
                        cmdResult = tview.CenterLines(odArgs.iLine, 1);
                        break;
                    case CommandType.SetCaretPos:
                        cmdResult = tview.SetCaretPos(odArgs.iLine, odArgs.iColumn);
                        break;
                    case CommandType.SetSelection:
                        cmdResult = tview.SetSelection(odArgs.iLine, odArgs.iColumn, odArgs.iLine, odArgs.iColumn + odArgs.iSelectLength);
                        break;
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }

            return (cmdResult == Microsoft.VisualStudio.VSConstants.S_OK);
        }


        private void StopMonitorLoop()
        {
            try
            {
                using (var pipeClient = new NamedPipeClientStream(".", "CommandMonitoring.Server", PipeDirection.InOut, PipeOptions.None, TokenImpersonationLevel.Impersonation))
                {
                    pipeClient.Connect();
                    var ss = new Utility.StreamString(pipeClient);

                    var write = ss.WriteString("StopMonitor");
                }
            }
            catch
            {
                ;
            }
        }
    }
}
