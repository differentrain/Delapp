using System;
using System.Collections.Concurrent;
using System.IO.Pipes;
using System.Text;
using System.Threading;

namespace DelApp.Internals
{
    internal static class PipeService
    {
        private const int DEFALUT_BYTES_BUFFER_SIZE = 32767 * 2;


        public static event EventHandler<FileNDir> PathRecived;

        public static ConcurrentQueue<FileNDir> PathQueue = new ConcurrentQueue<FileNDir>();


        public static bool CreateService()
        {
            return ThreadPool.QueueUserWorkItem(ServiceCoreAsync);
        }

        public static void SendPath(string targetPath)
        {
            using (var pipeClient = new NamedPipeClientStream(".", Utils.MyGuidString, PipeDirection.Out))
            {
                try
                {
                    var buffer = Encoding.Unicode.GetBytes(targetPath);
                    pipeClient.Connect();
                    pipeClient.Write(buffer, 0, buffer.Length);
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
                var buffer = new byte[DEFALUT_BYTES_BUFFER_SIZE];
                while (true)
                {
                    try
                    {
                        if (!pipeServer.IsConnected)
                        {
                            await pipeServer.WaitForConnectionAsync().ConfigureAwait(false);
                        }

                        int length = pipeServer.Read(buffer, 0, DEFALUT_BYTES_BUFFER_SIZE);
                        string path = Encoding.Unicode.GetString(buffer, 0, length);
                        var file = new FileNDir(path);
                        PathQueue.Enqueue(file);
                        PathRecived?.Invoke(PathQueue, file);
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
