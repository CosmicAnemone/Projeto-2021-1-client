using System;
using System.Net.Http;
using System.Threading.Tasks;
using SimpleJSON;

namespace network {
    public static class NetworkingClient {
        private static readonly HttpClient client = new HttpClient();

        public static JSONNode tryGet(string URL) {
            return tryTask(client.GetAsync(URL));
        }

        public static JSONNode tryPost(string URL, JSONObject content) {
            return tryTask(client.PostAsync(URL, new StringContent(content.ToString())));
        }

        private static JSONNode tryTask(Task<HttpResponseMessage> task) {
            task.Wait();
            if (task.IsCanceled || task.IsFaulted) return null;
            Task<string> contentTask = task.Result.Content.ReadAsStringAsync();
            contentTask.Wait();
            if (contentTask.IsCanceled || contentTask.IsFaulted) return null;
            try {
                return JSON.Parse(contentTask.Result);
            } catch (Exception) {
                return null;
            }
        }
    }
}