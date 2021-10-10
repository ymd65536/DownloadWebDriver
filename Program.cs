using System;
using System.Text;
using System.Text.Json;
using System.IO;
using System.IO.Compression;

namespace WebDriverDownload
{
    class Program
    {
        static void Main()
        {
            // アプリケーションの設定を読み込み
            string CurDir = Directory.GetCurrentDirectory();

            if (!File.Exists(CurDir + "\\" + "appConfig.json"))
            {
                Console.WriteLine("アプリケーションの設定に失敗しました。");
                return;
            }

            StreamReader AppSetting = new StreamReader(CurDir + "\\" + "appConfig.json");
            JsonDocument appConfig = JsonDocument.Parse(AppSetting.ReadToEnd());
            JsonElement appElement = appConfig.RootElement.GetProperty("app");
            //JsonElement GCConfig = appConfig.RootElement.GetProperty("GCConfig");

            string jsonPath = appElement[0].GetProperty("AuthJsonFile").ToString();
            string encode = appElement[0].GetProperty("AuthJsonFileEncode").ToString();
            string extractPath = appElement[0].GetProperty("ExtractFolderPath").ToString();

            // ブラウザのインストール先を変数にロード
            JsonElement MSEdgeConfig = appConfig.RootElement.GetProperty("MSEdgeConfig");
            string MSEdgeZipFilePath = MSEdgeConfig.GetProperty("ZipFilePath").ToString();
            string MSEdgeZipFileName = MSEdgeConfig.GetProperty("ZipFileName").ToString();
            string MSEdgePath = MSEdgeConfig.GetProperty("Path").ToString();
            string MSEdgeFileName = MSEdgeConfig.GetProperty("FileName").ToString();
            string MSEdgeWebDriverPath = MSEdgeConfig.GetProperty("WebDriverPath").ToString();
            string MSEdgeWebDriverFileName = MSEdgeConfig.GetProperty("WebDriverFileName").ToString();
            string MSEdgeRequestUrl = MSEdgeConfig.GetProperty("RequestEndPoint").ToString();
            string MSEdgeVersion = AppVersionCheck.AppVersionCheck.GetVersion(MSEdgePath, MSEdgeFileName);
            string MSEdgeWebDriverVersion = AppVersionCheck.AppVersionCheck.GetVersion(MSEdgeWebDriverPath, MSEdgeWebDriverFileName);

            string ZipFileRequestUrl = MSEdgeRequestUrl + MSEdgeVersion + "/" + MSEdgeZipFileName;

            // バージョンチェック
            // ダウンロードするかどうかの判定
            if (MSEdgeVersion == MSEdgeWebDriverVersion)
            {
                Console.WriteLine("WebDriver アップデート済");
                return;
            }

            // プロキシ接続
            // WebDriverをダウンロード
            // Zipファイルを出力
            UserInfo ConfigData = new UserInfo();
            ProxyInfo.ProxyConfig SetConfig = new ProxyInfo.ProxyConfig();

            ConfigData = SetConfig.ReadJson(jsonPath, Encoding.GetEncoding(encode));
            Console.WriteLine("ファイルリクエスト：" + ZipFileRequestUrl);

            if (ConfigData == null)
            {
                ConnectProxy.ProxyConnection ProCon = new ConnectProxy.ProxyConnection();
                ProCon.Connect();
                ProCon.RequestFile(ZipFileRequestUrl, MSEdgeZipFilePath);
            }
            else
            {
                ConnectProxy.ProxyConnection ProCon = new ConnectProxy.ProxyConnection();
                ProCon.Connect(ConfigData);
                ProCon.RequestFile(ZipFileRequestUrl, MSEdgeZipFilePath);
            }
            // Zipファイルを解凍
            Console.WriteLine("zipファイルの解凍開始！！：" + MSEdgeZipFilePath);
            Console.WriteLine(MSEdgeZipFilePath + "=>" + extractPath);

            try
            {
                if (Directory.Exists(extractPath))
                {
                    Directory.Delete(extractPath, true);
                }
                ZipFile.ExtractToDirectory(MSEdgeZipFilePath, extractPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("解凍に失敗しました。");
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("インストール終了！！");
            return;
        }
    }
}
