using System;
using System.ComponentModel;
using System.Windows.Forms;
using Hooks;
using System.Threading;
using ResmusXR;
using System.Net;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Text;

namespace TestPosts
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            MouseHook.MouseMove += new MouseEventHandler(MouseHook_MouseMove);
            MouseHook.LocalHook = false;
            MouseHook.InstallHook();
        }
        int urlxy = 0;
        int commentxy = 0;
        int loginxy = 0;
        int passxy = 0;
        int logxy = 0;
        int cout_comments = 0;
        int current_comment_number = 0;
        string current_url = null;
        Thread thread;
        public delegate void AddListItem1(String str);
        public AddListItem1 myDelegate1;
        public void AddListItemMethod1(String str)
        {
            listBox2.Items.Add(str);
        }
        Object lock1 = new Object();
        public static string GetX(string source)
        {
            string name_pattern_pattern = "{X=(.*),";
            Match name;
            name = Regex.Match(source, name_pattern_pattern);
            return name.Groups[1].ToString();
        }
        public static string GetY(string source)
        {
            string name_pattern_pattern = "Y=(.*)}";
            Match name;
            name = Regex.Match(source, name_pattern_pattern);
            return name.Groups[1].ToString();
        }
        public delegate void AddListItem(String str, String page);
        public AddListItem myDelegate;
        public void AddListItemMethod(String str, String page)
        {
            if (str != "null")
            {
                listBox3.Items.Add(str);
            }
            else
            {
                SaveLogs("Не удалось получить комментарий со страницы:" + page);
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_MOVE = 0x0001;
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);
        string url_coo, comment_coo, login_coo, pass_coo, log_coo;
        public string GetUN(string source)
        {
            string lastname_pattern_pattern = "instagram.com/(.*)/";
            Match lastname;
            lastname = Regex.Match(source, lastname_pattern_pattern);
            return lastname.Groups[1].ToString();
        }
        void MouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            if (urlxy != 0)
            {
                url_coo = e.Location.ToString();
                urlxy = 0;
                label20.Text = url_coo;
                backgroundWorker1.CancelAsync();
            }
            if (commentxy != 0)
            {
                comment_coo = e.Location.ToString();
                commentxy = 0;
                label21.Text = comment_coo;
                backgroundWorker2.CancelAsync();
            }
            if (loginxy != 0)
            {
                login_coo = e.Location.ToString();
                loginxy = 0;
                label22.Text = login_coo;
                backgroundWorker3.CancelAsync();
            }
            if (passxy != 0)
            {
                pass_coo = e.Location.ToString();
                passxy = 0;
                label23.Text = pass_coo;
                backgroundWorker4.CancelAsync();
            }
            if (logxy != 0)
            {
                log_coo = e.Location.ToString();
                logxy = 0;
                label24.Text = log_coo;
                backgroundWorker5.CancelAsync();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            urlxy = 1;
            backgroundWorker1.CancelAsync();
        }
        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            commentxy = 1;
            backgroundWorker1.CancelAsync();
        }
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            loginxy = 1;
            backgroundWorker1.CancelAsync();
        }
        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            passxy = 1;
            backgroundWorker1.CancelAsync();
        }
        private void backgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(3000);
            logxy = 1;
            backgroundWorker1.CancelAsync();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker2.RunWorkerAsync();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            backgroundWorker3.RunWorkerAsync();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            backgroundWorker4.RunWorkerAsync();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            backgroundWorker5.RunWorkerAsync();
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Proxy.Set(null);
            label6.Text = "-";
            label4.Text = "Off";
            label4.ForeColor = Color.Red;
            SaveLogs("Статус прокси: OFF");
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            thread = new Thread(Feeding);
            thread.SetApartmentState(ApartmentState.STA);
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            MouseHook.UnInstallHook();
        }
        public static void SaveLogs(string text)
        {
            File.AppendAllText("logs.txt", DateTime.Now.ToString("HH-mm-ss") + " | " + text + Environment.NewLine);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;
            notifyIcon1.BalloonTipTitle = "Программа была спрятана";
            notifyIcon1.BalloonTipText = "Обратите внимание что программа была спрятана в трей и продолжит свою работу.";
            notifyIcon1.ShowBalloonTip(5000);
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label25.Text = "Аккаунты:" + textBox4.Text.Split('|').Length;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == String.Empty)
            {
                MessageBox.Show("Введите прокси!");
            }
            else
            {
                Proxy.Set(new WebProxy("http://" + textBox1.Text));
                label6.Text = textBox1.Text;
                label4.Text = "On";
                label4.ForeColor = Color.Green;
                SaveLogs("Статус прокси: ON . Используемый прокси:" + textBox1.Text);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                label14.Text = "0";
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
                label14.Text = listBox1.Items.Count.ToString();
                SaveLogs("Было загружено:" + listBox1.Items.Count.ToString() + " ссылок");
            }
            catch
            {
                MessageBox.Show("Ошибка загрузки файла!");
                SaveLogs("Ошибка при загрузке файла.");
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                if (button9.Text == "Старт")
                {
                    thread.Start();
                    button9.Text = "Стоп";
                }
                else
                {
                    thread.Abort();
                    button9.Text = "Старт";
                }
            }
            else
            {
                MessageBox.Show("Заполни поле комментария.");
            }
        }
        public void Feeding()
        {
            myDelegate1 = new AddListItem1(AddListItemMethod1);
            int current_account_index = 0;
            string accs = "";
            int all_comments = 0;
            myDelegate = new AddListItem(AddListItemMethod);
            SaveLogs("Запуск процесса сканирования аккаунтов.Всего ссылок:" + listBox1.Items.Count.ToString());
                while (true)
                {
                    lock (lock1)
                    {
                        int i = 0;
                        foreach (string name in listBox1.Items)
                        {
                            label13.BeginInvoke(new Action<string>((s) => label13.Text = s), name);
                            if (listBox1.Items.Count == listBox3.Items.Count)
                            {
                                if (url_coo != "" && comment_coo != "" && login_coo != "" && pass_coo != "" && log_coo != "")
                                {
                                    foreach (string post in listBox3.Items)
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
                                        current_url = post.Split('|')[0];
                                        label10.BeginInvoke(new Action<string>((s) => label10.Text = s), textBox3.Text.Split('|')[current_comment_number]);
                                        SetCursorPos(Convert.ToInt32(GetX(url_coo)), Convert.ToInt32(GetY(url_coo)));
                                        mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                                        Thread.Sleep(1500);
                                        Clipboard.Clear();
                                        Clipboard.SetText("www.instagram.com/p/" + current_url + "/");
                                        Thread.Sleep(300);
                                        SendKeys.SendWait("+{INS}");
                                        SendKeys.SendWait("{ENTER}");
                                        Thread.Sleep(4500);
                                        SendKeys.SendWait("{PGDN}");
                                        SendKeys.SendWait("{PGDN}");
                                        SendKeys.SendWait("{PGDN}");
                                        SendKeys.SendWait("{PGDN}");
                                        SendKeys.SendWait("{PGDN}");
                                        SetCursorPos(Convert.ToInt32(GetX(comment_coo)), Convert.ToInt32(GetY(comment_coo)));
                                        mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                                        Thread.Sleep(1000);
                                        string current_comment = textBox3.Text.Split('|')[current_comment_number];
                                        Clipboard.SetText(current_comment);
                                        SendKeys.SendWait("+{INS}");
                                        SendKeys.SendWait("{ENTER}");
                                        cout_comments++;
                                        all_comments++;
                                    listBox2.Invoke(myDelegate1, new object[] { post.Split('|')[0] });
                                    SaveLogs("Комментарий " + textBox3.Text.Split('|')[current_comment_number] + " был успешно уставлен на новости https://www.instagram.com/p/" + current_url + "/");
                                    if (all_comments == numericUpDown2.Value)
                                        {
                                        accs = textBox4.Text.Split('|')[current_account_index];
                                            all_comments = 0;
                                            SetCursorPos(Convert.ToInt32(GetX(url_coo)), Convert.ToInt32(GetY(url_coo)));
                                            Thread.Sleep(500);
                                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                                            Thread.Sleep(1000);
                                            Clipboard.SetText("www.instagram.com/accounts/logout/");
                                            SendKeys.SendWait("+{INS}");
                                            Thread.Sleep(1000);
                                            SendKeys.SendWait("{ENTER}");
                                            Thread.Sleep(4000);
                                            SetCursorPos(Convert.ToInt32(GetX(url_coo)), Convert.ToInt32(GetY(url_coo)));
                                            Thread.Sleep(1000);
                                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                                            Thread.Sleep(1000);
                                            Clipboard.SetText("www.instagram.com/accounts/login/");
                                            SendKeys.SendWait("+{INS}");
                                            Thread.Sleep(1000);
                                            SendKeys.SendWait("{ENTER}");
                                            Thread.Sleep(3000);
                                            SetCursorPos(Convert.ToInt32(GetX(login_coo)), Convert.ToInt32(GetY(login_coo)));
                                            Thread.Sleep(1000);
                                            Clipboard.Clear();
                                            Clipboard.SetText(accs.Split(':')[0]);
                                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                                            Thread.Sleep(1000);
                                            SendKeys.SendWait("+{INS}");
                                            Thread.Sleep(500);
                                            int xq = Convert.ToInt32(GetX(login_coo)) + 190;
                                            SetCursorPos(xq, Convert.ToInt32(GetY(login_coo)));
                                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                                            Thread.Sleep(200);
                                            Clipboard.Clear();
                                            SetCursorPos(Convert.ToInt32(GetX(pass_coo)), Convert.ToInt32(GetY(pass_coo)));
                                            Clipboard.SetText(accs.Split(':')[1]);
                                            Thread.Sleep(1000);
                                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                                            Thread.Sleep(1000);
                                            SendKeys.SendWait("+{INS}");
                                            Thread.Sleep(1000);
                                            SetCursorPos(Convert.ToInt32(GetX(log_coo)), Convert.ToInt32(GetY(log_coo)));
                                            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
                                        current_account_index++;
                                        if (current_account_index > textBox4.Text.Split('|').Length-1)
                                        {
                                            current_account_index = 0;
                                        }
                                    }
                                        Thread.Sleep(4000);
                                    }
                                }
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
                            if (Regulars.likes != null)
                            {
                                listBox3.Invoke(myDelegate, new object[] { Regulars.likes, name });
                            }
                                Regulars.likes = null;
                                i++;
                            }
                        }
                    }
                int time = Convert.ToInt32(textBox2.Text) * 60000;
                Thread.Sleep(time);
            }
            }
        }
    }

