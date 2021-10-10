# ���󥿡��ͥåȤ���֥饦���˹�碌��WebDriver���������ɤ���ץ����

## Ƴ����ˡ

1. ���Υ�ݥ��ȥ�򥯥���
2. �����ȥǥ��쥯�ȥ���ѹ����� `dotnet build`
3. `auth.json` ���Խ�
4. �¹�

�ǥե���ȤǤ�D�ɥ饤�֤���¸�����褦�ˤʤäƤ��ޤ���

## ���ץꥱ������������

### auth.json

ǧ�ھ����`auth.json`�Ǵ���

```json
{
  "ProxyAuth": false,
  "UserName": "UserName",
  "PassWord": "password",
  "ProxyName": "proxy_name:port"
}
```

### appConfig.json

���ץꥱ�������������appConfig.json�Ǵ���

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
