using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace WpfRSS_Feed
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        String[,] rssData = null;

      
      
        private void CreateLog()
        {
 
            string FilePath = @"Log.txt";
            using (StreamWriter writer = new StreamWriter(FilePath, true))
            {
                writer.WriteLine("TIME :" + DateTime.Now + "<br/>" + Environment.NewLine + "RS Title :" + comboBoxRssList.Text +
                   "" + Environment.NewLine);
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }

        private String[,] GetRssData(string channel)
        {
            System.Net.WebRequest webRequest = System.Net.WebRequest.Create(channel);
            System.Net.WebResponse webResponse = webRequest.GetResponse();

            System.IO.Stream stream = webResponse.GetResponseStream();
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();

            xmlDocument.Load(stream);

            System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes("rss/channel/item");

            String[,] tempRssData = new String[100, 3];

            for (int i = 0; i < xmlNodeList.Count; i++)
            {
                System.Xml.XmlNode rssNode;

                rssNode = xmlNodeList.Item(i).SelectSingleNode("title");

                if (rssNode != null)
                {
                    tempRssData[i, 0] = rssNode.InnerText;
                }
                else
                {
                    tempRssData[i, 0] = "";
                }
                rssNode = xmlNodeList.Item(i).SelectSingleNode("description");

                if (rssNode != null)
                {
                    tempRssData[i, 1] = rssNode.InnerText;
                }
                else
                {
                    tempRssData[i, 1] = "";
                }

                rssNode = xmlNodeList.Item(i).SelectSingleNode("link");

                if (rssNode != null)
                {
                    tempRssData[i, 2] = rssNode.InnerText;
                }
                else
                {
                    tempRssData[i, 2] = "";
                }
            }

            return tempRssData;
        }
        private void ButtonBrowse_Click(object sender, RoutedEventArgs e)
        {
                   
            if (comboBoxList.SelectedIndex > -1)
            {
                comboBoxRssList.Items.Clear();
                rssData = GetRssData(comboBoxList.Text);

                for (int i = 0; i < rssData.GetLength(0); i++)
                {
                    if (rssData[i, 0] != null)
                    {
                        comboBoxRssList.Items.Add(rssData[i, 0]);
                    }
                    comboBoxRssList.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("Please pick a Rss link from ComboBox");
            }
        }

        private void ComboBoxRssList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboBoxRssList.SelectedIndex > 0)
            {
                CreateLog();
            }

            if (comboBoxRssList.SelectedIndex > -1)
            {
                textBoxDes.Text = rssData[comboBoxRssList.SelectedIndex, 1];
                if (rssData[comboBoxRssList.SelectedIndex, 2] != null)
                {
                    textBlock.Text = "Go To: " + rssData[comboBoxRssList.SelectedIndex, 0];
                }
            }
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (rssData[comboBoxRssList.SelectedIndex, 2] != null)
            {
                System.Diagnostics.Process.Start(rssData[comboBoxRssList.SelectedIndex, 2]);
            }
        }
    }
}
