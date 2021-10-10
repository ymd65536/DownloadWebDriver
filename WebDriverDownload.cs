using System;
using System.IO;

using System.Text;
using System.Text.Json;

// HttpClientを利用
using System.Net;
using System.Net.Http;

// FileSearchクラスで利用
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;

// プロキシ情報を保存しておくクラス
public class UserInfo
{
    public string UserName { get; set; }
    public string PassWord { get; set; }
    public string ProxyName { get; set; }
    public bool ProxyAuth { get; set; }
}
namespace ProxyInfo
{
    class ProxyConfig
    {
        // jsonテキストをjsonオブジェクトに格納
        public UserInfo ReadJson(string path, Encoding encType)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            else
            {
                string jsonStr = File.ReadAllText(path, encType);
                UserInfo jsonData = new UserInfo();
                jsonData = JsonSerializer.Deserialize<UserInfo>(jsonStr);
                return jsonData;
            }
        }
    }
}
// プロキシサーバに接続するクラス
// プロキシ認証情報はjson ファイルから取得
namespace ConnectProxy
{
    class ProxyConnection
    {

        private HttpClientHandler ch;
        private HttpClient client;

        public void Connect(UserInfo data)
        {
            // HttpClientHandlerにProxy情報を設定する
            ch = new HttpClientHandler();
            ch.UseProxy = data.ProxyAuth;
            // 認証が必要な場合とそうでない場合に分ける
            if (data.ProxyAuth)
            {
                ch.Proxy = new WebProxy(data.ProxyName);
                ch.Proxy.Credentials = new NetworkCredential(data.UserName, data.PassWord);
            }
            // HttpClientHandlerを用いてHttpClientを生成
            client = new HttpClient(ch);
        }

        public void Connect(string ProxyName, string UserName, string PassWord)
        {
            // HttpClientHandlerにProxy情報を設定する
            ch = new HttpClientHandler();
            ch.Proxy = new WebProxy(ProxyName);
            ch.Proxy.Credentials = new NetworkCredential(UserName, PassWord);
            ch.UseProxy = true;

            // HttpClientHandlerを用いてHttpClientを生成
            client = new HttpClient(ch);

        }

        public void RequestFile(string RequestUrl, string SaveFilePath)
        {
            try
            {
                // GETでレスポンスを取得
                // var task = client.GetStringAsync(RequestUrl);
                var task = client.GetAsync(RequestUrl);
                task.Wait();

                var stream = task.Result.Content.ReadAsStreamAsync();
                var fileStream = new FileStream(SaveFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
                stream.Result.CopyTo(fileStream);
                stream.Wait();
                fileStream.Dispose();
                stream.Dispose();

            }
            catch (Exception e)
            {
                Exception e2 = e.GetBaseException();
                System.Console.WriteLine(e2.Message);
            }
        }
    }
}

// ファイルを検索するクラス
namespace FileSearch
{
    public class FileSearch
    {
        public static string SearchFile(string FolderPath, string FileName)
        {
            string ErrorMessage = "ファイルの検索に失敗";
            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(FolderPath, FileName, SearchOption.TopDirectoryOnly);
                return files.First();
            }
            catch (Exception e)
            {
                if (e.Message == "Sequence contains no elements")
                {
                    ErrorMessage = "検索フォルダパス「" + FolderPath + "」から「" + FileName + "」が見つかりませんでした。";
                }
                else if (e.Message.Contains("Could not find a part of the path"))
                {
                    ErrorMessage = "検索フォルダパス「" + FolderPath + "」に誤りがあるかフォルダがありません。";
                }
            }
            return ErrorMessage;
        }
    }
}

// ファイルのバージョンチェックをするクラス
namespace AppVersionCheck
{
    public class AppVersionCheck
    {
        public static string GetVersion(string FolderPath, string FileName)
        {
            // FileSearch.csからFileSearch メソッドを参照
            try
            {
                string ProgramFile = FileSearch.FileSearch.SearchFile(FolderPath, FileName);
                FileVersionInfo ProgramFileVersion = FileVersionInfo.GetVersionInfo(ProgramFile);

                return ProgramFileVersion.FileVersion;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}