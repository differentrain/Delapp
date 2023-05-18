using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DelApp.Internals
{
    internal static class PipeService
    {
        private const int DEFALUT_BYTES_BUFFER_SIZE = 32767 * 2;


        public static event EventHandler PathRecived;

        public static ConcurrentQueue<FileNDir> PathQueue = new ConcurrentQueue<FileNDir>();


        public static bool CreateService()
        {
            return ThreadPool.QueueUserWorkItem(ServiceCoreAsync);
        }

        public static void SendPath(IEnumerable<string> pathes)
        {
            using (var pipeClient = new NamedPipeClientStream(".", Utils.MyGuidString, PipeDirection.Out))
            {
                var lenBuf = new byte[4];
                var buffer = new byte[DEFALUT_BYTES_BUFFER_SIZE];
                int len;
                try
                {
                    pipeClient.Connect();
                    unsafe
                    {
                        fixed (byte* p = lenBuf)
                        {
                            foreach (var item in pathes)
                            {
                                len = Encoding.Unicode.GetBytes(item, 0, item.Length, buffer, 0);
                                *(int*)p = len;
                                pipeClient.Write(lenBuf, 0, 4);
                                pipeClient.Write(buffer, 0, len);
                            }
                        }
                    }
                    pipeClient.WaitForPipeDrain();
                }
                catch
                {
                }
            }
        }

        private static async void ServiceCoreAsync(object stateInfo)
        {
            using (var pipeServer = new NamedPipeServerStream(Utils.MyGuidString, PipeDirection.In))
            {
                var lenBuf = new byte[4];
                var buffer = new byte[DEFALUT_BYTES_BUFFER_SIZE];
                int len;
                string path;
                while (true)
                {
                    try
                    {
                        if (!pipeServer.IsConnected)
                            await pipeServer.WaitForConnectionAsync().ConfigureAwait(false);

                        unsafe
                        {
                            fixed (byte* p = lenBuf)
                            {
                                while (pipeServer.Read(lenBuf, 0, 4) == 4)
                                {
                                    len = *(int*)p;
                                    if (pipeServer.Read(buffer, 0, len) == len)
                                    {
                                        path = Encoding.Unicode.GetString(buffer, 0, len);
                                        if (path != Application.ExecutablePath)
                                            PathQueue.Enqueue(new FileNDir(path));
                                      }
                                }
                                PathRecived?.Invoke(PathQueue, EventArgs.Empty);
                            }
                        }
                    }
                    catch (Exception ecx)
                    {
                        Utils.WriteErrorLog(ecx.Message);
                    }
                    finally
                    {
                        try
                        {
                            pipeServer.Disconnect();
                        }
                        catch (Exception ecx2)
                        {
                            Utils.WriteErrorLog(ecx2.Message);
                        }
                    }
                }
            }
        }


    }
}
