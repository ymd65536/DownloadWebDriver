# インターネットからブラウザに合わせたWebDriverをダウンロードするプログラム

## 導入方法

1. このリポジトリをクローン
2. カレントディレクトリを変更して `dotnet build`
3. `auth.json` を編集
4. 実行

デフォルトではDドライブに保存されるようになっています。

## アプリケーションの設定

### auth.json

認証情報は`auth.json`で管理

```json
{
  "ProxyAuth": false,
  "UserName": "UserName",
  "PassWord": "password",
  "ProxyName": "proxy_name:port"
}
```

### appConfig.json

アプリケーションの設定はappConfig.jsonで管理

```json
{
  "app": [
    {
      "AuthJsonFile": "auth.json",
      "AuthJsonFileEncode": "utf-8",
      "ExtractFolderPath": "D:\\edgedriver_win32"
    }
  ],
  "MSEdgeConfig":
  {
    "ZipFilePath": "D:\\edgedriver_win32.zip",
    "ZipFileName": "edgedriver_win32.zip",
    "Path":"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\",
    "FileName":"msedge.exe",
    "WebDriverPath":"D:\\edgedriver_win32",
    "WebDriverFileName":"msedgedriver.exe",
    "RequestEndPoint": "https://msedgedriver.azureedge.net/"
  },
  "GCConfig":
  {
    "Path":"C:\\Program Files (x86)\\Google\\Chrome\\Application\\",
    "FileName":"chrome.exe",
    "WebDriverPath":"",
    "WebDriverFileName":"",
    "RequestEndPoint": "https://chromedriver.chromium.org/downloads"
  }
}
```

## 単一のアプリケーションとして利用する

dotNETCoreがインストールされていない場合でもプログラムを実行したい場合は
`dotnet publish` を用いて実行可能形式のexeを生成する。

```bash
dotnet publish -r win-x64 -p:PublishSingleFile=true --self-contained false
```

MimeKit が必要となる場合はパッケージを追加する。

```bash
dotnet add package MimeKit --version 2.15.1
```
