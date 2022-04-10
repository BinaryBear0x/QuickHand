using System.Diagnostics;
using System.Deployment.Application;
using System.Collections;
using System.Net;
using System.IO;
using System.Reflection;
using Microsoft.Win32;
using System.Management;
using System.Windows.Forms;
using System;
using System.Text;
using System.Xml;



namespace QuickHand
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            
            InitializeComponent();

        }



        string machiname,benimadımname;
        string ip;
        string winwin = "";
        double islem;



        private void Form1_Load(object sender, EventArgs e)
        {

            SelectQuery Sq = new SelectQuery("Win32_Processor");
            ManagementObjectSearcher objOSDetails = new ManagementObjectSearcher(Sq);
            ManagementObjectCollection osDetailsCollection = objOSDetails.Get();

            foreach (ManagementObject mo in osDetailsCollection)
            {
                label1.Text = (mo["Name"].ToString());
                label2.Text = (mo["Caption"]).ToString();
                label3.Text = ("bit:" + (ushort)mo["AddressWidth"]).ToString();
                label4.Text = ("CurrentClockSpeed: " + mo["CurrentClockSpeed"]).ToString();
                label5.Text = ("MaxClockSpeed:" + mo["MaxClockSpeed"]).ToString();
                label6.Text = ("NumberOfCores:" + mo["NumberOfCores"]).ToString();
                label7.Text = ("SystemCName:" + (string)mo["SystemCreationClassName"].ToString());
                label8.Text = ("SystemName:" + (string)mo["SystemName"].ToString());
                label9.Text = ("NumberOfLogicalProcessors:" + mo["NumberOfLogicalProcessors"]).ToString();
            }

            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            foreach (DriveInfo d in driveInfo)
            {
                comboBox1.Items.Add(d.Name);
            }

            label10.Text = ("Mac adress:" + mac);


            System.Management.SelectQuery query = new System.Management.SelectQuery(@"Select * from Win32_ComputerSystem");

            //initialize the searcher with the query it is supposed to execute
            using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher(query))
            {
                //execute the query
                foreach (System.Management.ManagementObject process in searcher.Get())
                {
                    //print system info
                    process.Get();
                   label17.Text=(process["Manufacturer"].ToString());
                    label18.Text=(process["Model"].ToString());


                }
            }

            string hostName = Dns.GetHostName(); // Retrive the Name of HOST
            Console.WriteLine(hostName);
            // Get the IP
#pragma warning disable CS0618 // Tür veya üye artık kullanılmıyor
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
#pragma warning restore CS0618 // Tür veya üye artık kullanılmıyor
            Console.WriteLine("My IP Address is :" + myIP);
            ip = myIP;
            label20.Text = ip;


            machiname = System.Environment.MachineName.ToString();
            label21.Text = machiname;

            

            benimadımname = System.Environment.UserName.ToString();
            label22.Text = benimadımname;


            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem"))
            {
                ManagementObjectCollection information = searcher.Get();
                if (information != null)
                {
                    foreach (ManagementObject obj in information)
                    {
                        winwin = obj["Caption"].ToString() + " \n " + obj["OSArchitecture"].ToString();
                        label19.Text = winwin;
                    }
                }

            }
            ManagementObjectSearcher Search = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");

            foreach (ManagementObject Mobject in Search.Get())
            {
                double Ram_Bytes = (Convert.ToDouble(Mobject["TotalPhysicalMemory"]));
                double ramgb = Ram_Bytes / 1073741824;
                islem = Math.Ceiling(ramgb);
                label23.Text = islem.ToString()+" GB"+" Ram";

            }
            System.Management.ManagementObjectSearcher searcher1 = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
            System.Management.ManagementObjectCollection collection = searcher1.Get();

            string videoControllerInfo = null;
            string bilgi;
            foreach (ManagementObject obj in collection)
               
            {
                if (((string[])obj["BIOSVersion"]).Length > 1)
                    label25.Text=("BIOS VERSION: " + ((string[])obj["BIOSVersion"])[0] + " - " + ((string[])obj["BIOSVersion"])[1]);
                else
                   label25.Text=("BIOS VERSION: " + ((string[])obj["BIOSVersion"])[0]);
            }
            using (var searcher = new ManagementObjectSearcher("select * from Win32_VideoController"))
            {
                foreach (ManagementObject obj in searcher.Get())
                {
                    
                    string name = null;
                 
                    string ram;
                

                    ram = (Convert.ToDouble(obj["AdapterRam"]) / 1073741824).ToString();
                    name = obj["name"].ToString();

                    videoControllerInfo = name+ "\n";
                    bilgi= (" Ram Miktarı: " + ram + " GB"+"\n");


                    listBox1.Items.Add(videoControllerInfo);
                    listBox1.Items.Add(bilgi);
                    listBox1.Items.Add("--------------------------------------");

                  






                    
                }
               

            }
          

        }
       

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(comboBox1.SelectedItem.ToString()))
            {

                DriveInfo driveDetails = new DriveInfo(comboBox1.SelectedItem.ToString());

                label16.Text = "Drive Name : " + driveDetails.Name + "\n";

                label15.Text = "Drive Type : " + driveDetails.DriveType + "\n";

                label14.Text = "Drive is Ready : " + driveDetails.IsReady + "\n";

                if (driveDetails.IsReady)
                {

                    label13.Text = "Total Space : " + driveDetails.TotalSize / 1024 / 1024 / 1024 +"GB"+ "\n";

                    label12.Text = "Free Space : " + driveDetails.AvailableFreeSpace / 1024 / 1024 / 1024 + "GB" + "\n";

                    label11.Text = "Volume Label : " + driveDetails.VolumeLabel + "\n";

                    label24.Text= ("Format"+ driveDetails.DriveFormat.ToString());
                }
              

            }

          
        }
        string mac = MacAndCheese();
       
       

        private void button1_Click(object sender, EventArgs e)
        {
       

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    FileName = "cmd.exe",
                    Arguments ="/C netsh wlan export profile folder=d: key=clear",
                };

                process.Start();
                process.WaitForExit();


                Form wifi = new wifi();
                wifi.Show();
                 

            }
            
        }


        static string MacAndCheese()
        {
            ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject obj in manager.GetInstances())
            {
                if ((bool)obj["IPEnabled"])
                {
                    return obj["MacAddress"].ToString();
                }
            }
            return String.Empty;

        }
        
      

    }
  
}
