using System;
using System.IO;

using System.Text;
using System.Text.Json;

// HttpClient������
using System.Net;
using System.Net.Http;

// FileSearch���饹������
using System.Collections.Generic;
using System.Linq;

using System.Diagnostics;

// �ץ����������¸���Ƥ������饹
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
        // json�ƥ����Ȥ�json���֥������Ȥ˳�Ǽ
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
// �ץ��������Ф���³���륯�饹
// �ץ���ǧ�ھ����json �ե����뤫�����
namespace ConnectProxy
{
    class ProxyConnection
    {

        private HttpClientHandler ch;
        private HttpClient client;

        public void Connect(UserInfo data)
        {
            // HttpClientHandler��Proxy��������ꤹ��
            ch = new HttpClientHandler();
            ch.UseProxy = data.ProxyAuth;
            // ǧ�ڤ�ɬ�פʾ��Ȥ����Ǥʤ�����ʬ����
            if (data.ProxyAuth)
            {
                ch.Proxy = new WebProxy(data.ProxyName);
                ch.Proxy.Credentials = new NetworkCredential(data.UserName, data.PassWord);
            }
            // HttpClientHandler���Ѥ���HttpClient������
            client = new HttpClient(ch);
        }

        public void Connect(string ProxyName, string UserName, string PassWord)
        {
            // HttpClientHandler��Proxy��������ꤹ��
            ch = new HttpClientHandler();
            ch.Proxy = new WebProxy(ProxyName);
            ch.Proxy.Credentials = new NetworkCredential(UserName, PassWord);
            ch.UseProxy = true;

            // HttpClientHandler���Ѥ���HttpClient������
            client = new HttpClient(ch);

        }

        public void RequestFile(string RequestUrl, string SaveFilePath)
        {
            try
            {
                // GET�ǥ쥹�ݥ󥹤����
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

// �ե�����򸡺����륯�饹
namespace FileSearch
{
    public class FileSearch
    {
        public static string SearchFile(string FolderPath, string FileName)
        {
            string ErrorMessage = "�ե�����θ����˼���";
            try
            {
                IEnumerable<string> files = Directory.EnumerateFiles(FolderPath, FileName, SearchOption.TopDirectoryOnly);
                return files.First();
            }
            catch (Exception e)
            {
                if (e.Message == "Sequence contains no elements")
                {
                    ErrorMessage = "�����ե�����ѥ���" + FolderPath + "�פ����" + FileName + "�פ����Ĥ���ޤ���Ǥ�����";
                }
                else if (e.Message.Contains("Could not find a part of the path"))
                {
                    ErrorMessage = "�����ե�����ѥ���" + FolderPath + "�פ˸�꤬���뤫�ե����������ޤ���";
                }
            }
            return ErrorMessage;
        }
    }
}

// �ե�����ΥС����������å��򤹤륯�饹
namespace AppVersionCheck
{
    public class AppVersionCheck
    {
        public static string GetVersion(string FolderPath, string FileName)
        {
            // FileSearch.cs����FileSearch �᥽�åɤ򻲾�
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