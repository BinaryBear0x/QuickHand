using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Linq;

namespace QuickHand
{
    public partial class wifi : Form
    {
        public wifi()
        {
            InitializeComponent();
        }
        string name;
          string sif;
        string DosyaYolu;
        private void wifi_Load(object sender, EventArgs e)
        {

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "XML Dosyası|*.xml";
            
            file.RestoreDirectory = true;
            file.CheckFileExists = false;
            
            file.Title = " xml dosyası Seçiniz..";

            if (file.ShowDialog() == DialogResult.OK)
            {
                DosyaYolu = file.FileName;
                string DosyaAdi = file.SafeFileName;
            }

            
            XmlDocument doc = new XmlDocument();
            StreamReader sr = new StreamReader(DosyaYolu);
            string okunan = sr.ReadToEnd();
            doc.LoadXml(okunan);


            XmlNodeList nodes1 = doc.GetElementsByTagName("WLANProfile");
            foreach (XmlNode item in nodes1)
            {
               name = item["name"].InnerText;
               
            }



            XmlNodeList nodes= doc.GetElementsByTagName("sharedKey");
            foreach (XmlNode item in nodes)
            {
                 sif = item["keyMaterial"].InnerText;
                
            }
               listBox1.Items.Add("name: "+name+"  "+"password: "+sif);
         

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
