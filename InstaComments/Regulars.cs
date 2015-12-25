using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace InstaComments
{
    class Regulars
    {
        public static string likes;
        static Form1 mf = new Form1();
        public static void GetFeed(string source, int index)
        {
            Regex a = new Regex(@"{~code~:~-([A-Z,a-z,0-9]\w+)~,~dimensions~");
            Match m = a.Match(source);
            while (m.Success)
            {
                try
                {
                    string like_pattern = "code~:~(.*)~,~";
                    Match name;
                    name = Regex.Match(m.Value, like_pattern);
                    likes += name.Groups[1].ToString() + "|";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                m = m.NextMatch();
            }
        }
        public static void CheckUpdate()
        {

        }

    }
}
