using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService
{
    public class MyWindowsService
    {
        private HttpListener _listener = new HttpListener();
        static List<Task> OngoingTasks = new List<Task>();

        public MyWindowsService()
        {
            _listener.Prefixes.Add("http://+:9000/");
        }

        public void Start()
        {
            _listener.Start();
            ProcessAsync(_listener);
        }

        private async Task ProcessAsync(HttpListener listener)
        {
            while (_listener.IsListening)
            {
                var context = await listener.GetContextAsync();
                OngoingTasks.Add(ProcessAsync(_listener));
                await HandleRequestAsync(context);
            }
        }

        static async Task HandleRequestAsync(HttpListenerContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            var bytes = Encoding.UTF8.GetBytes("MyString");
            context.Response.OutputStream.Write(bytes, 0, bytes.Length);
            context.Response.Close();
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}
