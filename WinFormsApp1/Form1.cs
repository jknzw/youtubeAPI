using Microsoft.Web.WebView2.Core;
using System.Collections;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = textBox1.Text;
            // URL����ID�𒊏o
            Match matche = Regex.Match(text, "\\?v=([^&]+)");
            if (!string.IsNullOrEmpty(matche.Value))
            {
                // �}�b�`�����f�[�^������ꍇ
                listBox1.Items.Add(matche.Value.Remove(0, 3)); // ?v=���폜���Ēǉ�
            }
        }

        private async void InitializeAsync()
        {
            await webView21.EnsureCoreWebView2Async(null);
            string dir = Directory.GetCurrentDirectory().Replace("\\", "/");
            webView21.CoreWebView2.Navigate("file:///" + dir + "/FileName.html");
            webView21.CoreWebView2.WebMessageReceived += MessageReceived;
            webView21.CoreWebView2.NavigationCompleted += webView2_NavigationCompleted;
        }

        private void MessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string text = args.TryGetWebMessageAsString();
            MessageBox.Show(text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //webView21.Reload();
            SetPlayList();
        }

        //readonly CountdownEvent condition = new CountdownEvent(1);
        private void webView2_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            //�ǂݍ��ݐ����Ȃ珈�����s
            if (e.IsSuccess)
            {
                //condition.Signal();
                //Thread.Sleep(1);
                //condition.Reset();
                //string fileName = dir + "\\savedata";
                //if (File.Exists(fileName))
                //{
                //    StreamReader sr = new StreamReader(fileName, Encoding.GetEncoding("UTF-8"));
                //    string str = sr.ReadToEnd();
                //    sr.Close();
                //    webView21.CoreWebView2.PostWebMessageAsString(str);
                //}

                SetPlayList();

            }
        }

        private void SetPlayList()
        {
            string playlist = GetPlayList();
            // ������,���폜
            if (!string.IsNullOrEmpty(playlist))
            {
                playlist = playlist.Remove(playlist.Length - 1);
            }

            webView21.CoreWebView2.PostWebMessageAsString(playlist);
        }

        private string GetPlayList()
        {
            string playlist = string.Empty;
            foreach (string item in listBox1.Items)
            {
                // Console.WriteLine(item.ToString());
                if (!string.IsNullOrEmpty(item?.Trim()))
                {
                    // ��łȂ��ꍇ�ǉ�
                    playlist += item + ',';
                }
            }

            return playlist;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            InitializeAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("./Resources.resx")){
                using ResXResourceSet resx = new("./Resources.resx");
                string? items = resx.GetString("items");
                if (items != null)
                {
                    foreach (string item in items.Split(","))
                    {
                        listBox1.Items.Add(item);
                    }
                }
            }
            //foreach (string item in Resources.Items.Split(","))
            //{
            //    listBox1.Items.Add(item);
            //}
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            using ResXResourceWriter resx = new("./Resources.resx");
            resx.AddResource("items", GetPlayList());
        }

        private void listBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }
    }
}
