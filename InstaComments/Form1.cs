using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Threading;
using ResmusXR;
using System.Drawing;
using System.Runtime.InteropServices;

namespace InstaComments
{
    public partial class Form1 : Form
    {
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        public Form1()
        {
            InitializeComponent();
        }
        Thread thread;
        int cursor_x, cursor_y;
        public static void SaveLogs(string text)
        {
            File.AppendAllText("logs.txt", DateTime.Now.ToString("HH-mm-ss") + " | " + text + Environment.NewLine);
        }
        public static void SetUpToken(string source)
        {
            string name_pattern_pattern = "{\"access_token\":\"(.*)\",\"user";
            Match name;
            name = Regex.Match(source, name_pattern_pattern);
            Settings1.Default.token = name.Groups[1].ToString();
        }
        public static string GetUserName(string source)
        {
            string name_pattern_pattern = "<strong>(.*)</strong>";
            Match name;
            name = Regex.Match(source, name_pattern_pattern);
            return name.Groups[1].ToString();
        }
        public static string GetUserID(string source)
        {
            string name_pattern_pattern = "\"id\":\"(.*)\"},\"csrf_token\"";
            Match name;
            name = Regex.Match(source, name_pattern_pattern);
            return name.Groups[1].ToString();
        }
        bool Visible;
        public string GetPhoto(string source)
        {
            string lastname_pattern_pattern = "profile_picture\":\"https:(.*)\",\"bio\"";
            Match lastname;
            lastname = Regex.Match(source, lastname_pattern_pattern);
            return lastname.Groups[1].ToString();
        }
        public string GetUN(string source)
        {
            string lastname_pattern_pattern = "instagram.com/(.*)/";
            Match lastname;
            lastname = Regex.Match(source, lastname_pattern_pattern);
            return lastname.Groups[1].ToString();
        }
        public string GetID(string source)
        {
            string lastname_pattern_pattern = "id\":\"(.*)\"}}";
            Match lastname;
            lastname = Regex.Match(source, lastname_pattern_pattern);
            return lastname.Groups[1].ToString();
        }
        [DllImport("user32.dll",
CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags,
                                      int dx,
                          int dy,
                      int dwData,
                      int dwExtraInfo);

        //Нормированные абсолютные координаты
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        //Нажатие на левую кнопку мыши
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;

        //Поднятие левой кнопки мыши
        private const int MOUSEEVENTF_LEFTUP = 0x0004;

        //перемещение указателя мыши
        private const int MOUSEEVENTF_MOVE = 0x0001;
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);
        int cout_comments = 0;
        int current_comment_number = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            try
            {
                System.IO.Directory.Delete(Path, true);
            }
            catch (Exception)
            {
            }
            timer1.Start();
            //webBrowser1.Visible = Visible;
            webControl1.Source = new Uri("https://api.instagram.com/oauth/authorize/?client_id=3ed71c0ced8f4ac2a7629f781d35cee6&redirect_uri=https://www.instagram.com/accounts/edit&response_type=code");
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread.IsAlive)
            {
                thread.Abort();
            }
            Application.Exit();
        }
        Object lock1 = new Object();
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //int count_usernames1 = 0;
            //int count_usernames2 = 0;


        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label3.Text = Settings1.Default.username;
            label4.Text = Settings1.Default.id;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                label9.Text = "0";
                listBox1.Items.Clear();
                OpenFileDialog src = new OpenFileDialog();
                src.ShowDialog();
                StreamReader f = File.OpenText(src.FileName);
                string str;
                while (null != (str = f.ReadLine()))
                {
                    listBox1.Items.Add(GetUN(str));
                }
                f.Close();
                label9.Text = listBox1.Items.Count.ToString();
                SaveLogs("Было загружено:" + listBox1.Items.Count.ToString() + " ссылок");
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки файла!");
                SaveLogs("Ошибка при загрузке файла.");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Заполните поля прокси");
            }
            else
            {
                string proxy = textBox1.Text + ":" + textBox2.Text;
                Proxy.Set(new WebProxy("http://" + proxy));
                label16.Text = proxy;
                label15.Text = "On";
                label15.ForeColor = Color.Green;
                SaveLogs("Статус прокси: ON . Используемый прокси:" + proxy);
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Proxy.Set(null);
            label16.Text = "-";
            label15.Text = "Off";
            label15.ForeColor = Color.Red;
            SaveLogs("Статус прокси: OFF");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            
            if (textBox4.Text != "")
            {
                if (button3.Text == "Пуск")
                {
                    thread.Start();
                    button3.Text = "Стоп";
                }
                else
                {
                    thread.Abort();
                    button3.Text = "Пуск";
                }
            }
            else
            {
                MessageBox.Show("Заполни поле комментария.");
            }
        }
        public delegate void AddListItem(String str);
        public AddListItem myDelegate;
        public void AddListItemMethod(String str)
        {
            try
            {
                listBox2.Items.Add(str);
            }
            catch(Exception ex) { listBox2.Items.Add("null"); }

        }
        public delegate void AddListItem2(String str);
        public AddListItem2 myDelegate2;
        public void AddListItemMethod2(String str)
        {
            try
            {
                webControl1.Source = new Uri(str);
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

        }
        public delegate void AddListItem1(String str);
        public AddListItem1 myDelegate1;
        public void AddListItemMethod1(String str)
        {
            listBox3.Items.Add(str);
        }
        string current_url = "sdfjskldf";
        string current_comment = "";
        bool success = false;
        public void Feeding()
        {
            myDelegate = new AddListItem(AddListItemMethod);
            myDelegate1 = new AddListItem1(AddListItemMethod1);
            myDelegate2 = new AddListItem2(AddListItemMethod2);
            SaveLogs("Запуск процесса сканирования аккаунтов.Всего ссылок:" + listBox1.Items.Count.ToString());
            while (true)
            {
                int i = 0;
                foreach (string name in listBox1.Items)
                {
                    label10.BeginInvoke(new Action<string>((s) => label10.Text = s), name);
                    if (listBox1.Items.Count == listBox2.Items.Count)
                    {
                        string html = null;
                        string url = "https://instagram.com/" + name + "/";
                        HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                        HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding(65001));
                        html = sr.ReadToEnd().Replace('"', '~');
                        Regulars.GetFeed(html, i);
                        foreach(string post in listBox2.Items)
                        {
                            listBox3.Invoke(myDelegate1, new object[] { post.Split('|')[0] });
                            current_url = post.Split('|')[0];
                            lock (lock1)
                            {
                                webControl1.Invoke(myDelegate2, new object[] { "https://www.instagram.com/p/" + current_url + "/" });
                                while(success == false)
                                {
                                  Thread.Sleep(10000);
                                }
                                success = false;
                            }
                        }
                        int time = Convert.ToInt32(textBox4.Text) * 60000;
                        Thread.Sleep(time);

                    }
                    else
                    {
                        string html = null;
                        string url = "https://instagram.com/" + name + "/";
                        HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                        HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                        StreamReader sr = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding(65001));
                        html = sr.ReadToEnd().Replace('"', '~');
                        Regulars.GetFeed(html, i);
                        listBox2.Invoke(myDelegate, new object[] { Regulars.likes });
                        Regulars.likes = null;
                        i++;
                    }
                }
            }
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            thread = new Thread(Feeding);
            thread.SetApartmentState(ApartmentState.STA);
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Awesomium_Windows_Forms_WebControl_LoadingFrameComplete(object sender, Awesomium.Core.FrameEventArgs e)
        {
            try
            {
                if (webControl1.Source.ToString().Contains("https://www.instagram.com/accounts/edit/"))
                {
                    Settings1.Default.auth = true;
                    string html = webControl1.HTML;
                    Settings1.Default.username = GetUserName(html);
                    Settings1.Default.id = GetUserID(html);
                    pictureBox1.ImageLocation = "http:" + GetPhoto(html).Replace("\\", "");
                    webControl1.Source = new Uri("http://instagram.com/" + Settings1.Default.username);
                }
                if (webControl1.Source.ToString().Contains(current_url) && current_url!="null")
                {
                    //string html = webBrowser1.DocumentText;
                    //String pattern1 = Settings1.Default.username;
                    //Regex REX = new Regex(pattern1, RegexOptions.IgnoreCase);
                    //MatchCollection RTQ = REX.Matches(html);
                    //count_usernames1 = RTQ.Count;
                    while (true)
                    {
                        int asd = 0;
                        foreach (char qwe in textBox3.Text)
                        {
                            string s = string.Join("", qwe);
                            if (s == "|")
                            {
                                asd++;
                            }
                        }
                        asd++;
                        if (cout_comments == numericUpDown1.Value)
                        {
                            current_comment_number++;
                            cout_comments = 0;
                            if (current_comment_number > asd - 1)
                            {
                                current_comment_number = 0;
                            }
                        }
                        label21.Text = textBox3.Text.Split('|')[current_comment_number];
                        SetCursorPos(cursor_x, cursor_y);
                        SendKeys.SendWait("{PGDN}");
                        SendKeys.SendWait("{PGDN}");
                        SendKeys.SendWait("{PGDN}");
                        SendKeys.SendWait("{PGDN}");
                        SendKeys.SendWait("{PGDN}");
                        mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                        Thread.Sleep(1500);
                        //current_comment = textBox3.Text.Split('|')[current_comment_number][0].ToString();
                        //string fl = current_comment;
                        //SendKeys.SendWait(fl);
                        foreach (char qwe in textBox3.Text.Split('|')[current_comment_number])
                        {
                            string s = string.Join("", qwe);
                            SendKeys.SendWait(s);
                        }
                        SendKeys.SendWait("{ENTER}");
                        //html = webBrowser1.DocumentText;
                        //String pattern2 = Settings1.Default.username;
                        //Regex REX1 = new Regex(pattern1, RegexOptions.IgnoreCase);
                        //MatchCollection RTQ1 = REX.Matches(html);
                        //count_usernames2 = RTQ1.Count;
                        //if (count_usernames1!=count_usernames2)
                        //{
                        SaveLogs("Комментарий " + textBox3.Text.Split('|')[current_comment_number] + " был успешно уставлен на новости https://www.instagram.com/p/" + current_url + "/");
                        //}
                        //else
                        //{
                        //    SaveLogs("При попытке оставить комментарий " + textBox3.Text.Split('|')[current_comment_number] + " на новости https://www.instagram.com/p/" + current_url + "/ произошла ошибка");
                        //}
                        cout_comments++;
                        current_url = "12321312dfgdg3";
                        success = true;
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {
            label19.Visible = false;
            StreamWriter sw = new StreamWriter("settings.ini");
            sw.Write(Cursor.Position.X + "|" + Cursor.Position.Y);
            cursor_y = Cursor.Position.Y;
            cursor_x = Cursor.Position.X;
            MessageBox.Show("Настройки были успешно сохранены в файл Settings.ini");
            sw.Close();
        }
       
    }
}
