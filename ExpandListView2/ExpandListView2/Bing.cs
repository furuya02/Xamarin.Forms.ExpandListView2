using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExpandListView2 {
    class Bing {
        public async Task<string> Search(string searchWord) {
            string ApiKey = "{Your App Key}";

            string market = "ja-JP";
            string adult = "Off";
            int top = 50;
            string format = "json";
            string filters = "Size:Small";


            //文字列エンコード
        searchWord = Uri.EscapeDataString(searchWord);

            var handler = new HttpClientHandler();
            //認証情報を追加
            handler.Credentials = new NetworkCredential(ApiKey, ApiKey);

            var client = new HttpClient(handler);
            //Getリクエスト
            var resultStr = await client.GetStringAsync("https://api.datamarket.azure.com/Bing/Search/Image"
            +"?Query=" + "'" + searchWord + "'" + "&Market=" + "'" + market + "'" + "&Adult=" + "'" + adult + "'" + "&ImageFilters=" + "'" + filters + "'" + "&$top=" + top + "&$format=" + format);
            //文字列デコード
            return Uri.UnescapeDataString(resultStr);
        }
    }
}
