using System;
using System.IO;
using System.Threading.Tasks;

namespace Renci.SshNet.Async
{
    public static class SshNetExtensions
    {
        public static Task UploadAsync(this SftpClient client,
           Stream input, string path, Action<ulong> uploadCallback = null,
           TaskFactory factory = null,
           TaskCreationOptions creationOptions = default(TaskCreationOptions),
           TaskScheduler scheduler = null)
        {
            return (factory = factory ?? Task.Factory).FromAsync(
                client.BeginUploadFile(input, path, null, null, uploadCallback),
                client.EndUploadFile,
                creationOptions, scheduler ?? factory.Scheduler ?? TaskScheduler.Current);
        }
    }
}