using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Management.Instrumentation;

using System.Security;



namespace WMI2
{
    public partial class Form1 : Form
    {

        string ram_usage,kalann,toplam,total_disk,disk_space;
        ConnectionOptions baglanti;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            servis_sorgula();
            grafik_ciz_ram();
            disk_grafik();
            label40.Visible = true;
            label44.Visible = true;
            label39.Visible = true;

        }

        public void servis_sorgula()


        {
            try
            {
                /*Bağlantı yapılıyor*/
                ConnectionOptions baglanti = new ConnectionOptions();
                baglanti.Username = textBox3.Text;
                baglanti.Password = textBox4.Text;
                baglanti.Authority = "ntlmdomain:" + textBox2.Text;
                if (textBox2.Text == "")
                    baglanti.Authority = "";

                string path = "\\\\" + textBox1.Text + "\\root" + "\\CIMV2";
                ManagementScope scope = new ManagementScope(
                    path, baglanti);
              //  MessageBox.Show("Bağlantı Kuruluyor.. Servis Listesi Alınıyor .....");
                scope.Connect();

                /*
                 Servis listesi için sorgu oluşturulup managementonjseacher nesnesi tanımlanıyor. 
                 */
                MessageBox.Show("bağlandı","Bağlantı",MessageBoxButtons.OK,MessageBoxIcon.Information);
                ObjectQuery sorgu = new ObjectQuery(
                    "SELECT * FROM Win32_Service");

                ManagementObjectSearcher man_ob_arayici =
                    new ManagementObjectSearcher(scope, sorgu);

                foreach (ManagementObject obj in man_ob_arayici.Get())
                {
                    listBox1.Items.Add(obj["Name"].ToString());
                }

                label41.Text = listBox1.Items.Count.ToString();
                /*
              proccess bilgileri alınıyor. 
               */
                //MessageBox.Show("Bağlantı Kuruluyor.. Process Listesi Alınıyor .....");
                ObjectQuery sorgu2 = new ObjectQuery("SELECT * FROM Win32_process");
                ManagementObjectSearcher man_ob_arayici2 =
                  new ManagementObjectSearcher(scope, sorgu2);
                foreach (ManagementObject obj in man_ob_arayici2.Get())
                {
                    listBox2.Items.Add(obj["Name"].ToString());
                }
                label42.Text = listBox2.Items.Count.ToString();


                //sistem bilgisi alınıyor 
                //MessageBox.Show("Bağlantı Kuruluyor.. Sistem Bilgisi  Alınıyor .....");
                ObjectQuery sorgu4 = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher man_ob_arayici4 = new ManagementObjectSearcher(scope, sorgu4);
                foreach (ManagementObject obj in man_ob_arayici4.Get())
                {
                    label6.Text = obj["Caption"].ToString();
                    label8.Text = obj["OSArchitecture"].ToString();
                    label10.Text = obj["SerialNumber"].ToString();
                    label12.Text = obj["Version"].ToString();

                    label21.Text = obj["SystemDirectory"].ToString();
                    string total = (Convert.ToInt64(obj["TotalVisibleMemorySize"]) / (1024)).ToString();
                    string kalan = (Convert.ToInt64(obj["FreePhysicalMemory"]) / (1024)).ToString();
                    string kullanılan = (Convert.ToInt64(total) - Convert.ToInt64(kalan)).ToString();

                    toplam = total;
                    kalann = kalan;
                    ram_usage = kullanılan;
                    label19.Text = kullanılan + " MB";
                    double yuzde = (Convert.ToDouble(Convert.ToDouble(kullanılan) / Convert.ToDouble(total)) * 100);
                    double a = Math.Round(yuzde, 2);
                    label15.Text = a.ToString() + " %";


                }

                //Donanım  bilgileri alınıyor.
                //MessageBox.Show("Bağlantı Kuruluyor.. Donanım Bilgileri Alınıyor .....");
                ObjectQuery sorgu3 = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
                ManagementObjectSearcher man_ob_arayici3 =
                  new ManagementObjectSearcher(scope, sorgu3);

                foreach (ManagementObject obj in man_ob_arayici3.Get())
                {
                    label18.Text = (obj["NumberOfLogicalProcessors"].ToString());
                    label14.Text = (Convert.ToInt64(obj["TotalPhysicalMemory"]) / (1024 * 1024)).ToString() + " MB";
                    label29.Text = obj["Manufacturer"].ToString();
                    label27.Text = obj["Model"].ToString();
                    label25.Text = obj["Name"].ToString();
                }


                //Bios bilgileri alınıyor.
                //MessageBox.Show("Bağlantı Kuruluyor.. BİOS Bilgileri  Alınıyor .....");
                ObjectQuery sorgu5 = new ObjectQuery("SELECT * FROM Win32_BIOS");
                ManagementObjectSearcher man_ob_arayici5 =
                  new ManagementObjectSearcher(scope, sorgu5);

                foreach (ManagementObject obj in man_ob_arayici5.Get())
                {

                    label23.Text = obj["Caption"].ToString();
                    label31.Text = obj["Manufacturer"].ToString();
                    label33.Text = obj["SerialNumber"].ToString();

                }
                //MessageBox.Show("cpu usage");
                ObjectQuery sorgu6 = new ObjectQuery("SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor ");
                ManagementObjectSearcher man_ob_arayici6 = new ManagementObjectSearcher(scope, sorgu6);
                foreach (ManagementObject obj in man_ob_arayici6.Get())
                {
                    listBox3.Items.Add(obj["PercentProcessorTime"].ToString());
                    listBox4.Items.Add(obj["Name"].ToString());
                }
             /*   ObjectQuery sorgu7 = new ObjectQuery("SELECT * FROM Win32_Volume Where DriveLetter='C'");
                ManagementObjectSearcher man_ob_arayici7 = new ManagementObjectSearcher(scope, sorgu7);
                foreach (ManagementObject obj in man_ob_arayici7.Get())
                {
                    int a = 1024 * 1024 * 1024;
                     long total_disk2 = Convert.ToInt64(obj["Size"].ToString())/a;
                    total_disk = total_disk2.ToString();
                    long disk_space2 = Convert.ToInt64(obj["SizeRemaining"].ToString())/a;
                    disk_space = disk_space2.ToString();
                }
                MessageBox.Show(total_disk.ToString());
                MessageBox.Show(disk_space.ToString());
               */ 
                /*     ObjectQuery sorgu8 = new ObjectQuery("SELECT * FROM Win32_DiskDrive WHERE Model='" + label48.Text + "'");
                     ManagementObjectSearcher man_ob_arayici8 = new ManagementObjectSearcher(scope, sorgu8);
                     foreach (ManagementObject obj in man_ob_arayici8.Get())
                     {
                         //label48.Text = (obj["Model"].ToString());
                         total_disk = (Convert.ToInt64(obj["Size"]) / 1024).ToString() + " GB";
                         // disk_space = obj["FreeSpace"].ToString();
                     }

                     MessageBox.Show(disk_space);*/

            }
            catch (ManagementException hata)
            {
                MessageBox.Show("Sorgu alınırken hata oluştu: "
                    + hata.Message);
            }
            catch (System.UnauthorizedAccessException login_hata)
            {
                MessageBox.Show("Bağlantı hatası " +
                    "(kullanıcı adı or parola hatalı): " +
                    login_hata.Message);
            }
         
         
        }
      

        

        private void textBox2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        public void grafik_ciz_ram()
        {
             Pen p = new Pen(Color.Black, 1);
              Graphics g = this.CreateGraphics();
              Rectangle re = new Rectangle(450,  600 , 200, 200);
           
            Color clr;
              clr = new Color();

              float aci1 = float.Parse(ram_usage)/float.Parse(toplam)*360;

              float aci2 = float.Parse(kalann) / float.Parse(toplam) * 360;
              Brush br1 = new SolidBrush(Color.Blue);
              Brush br2 = new SolidBrush(Color.Red);
              g.DrawPie(p, re, 0, aci1);
              g.FillPie(br2, re, 0, aci1);
              g.DrawPie(p, re, aci1, aci2);
              g.FillPie(br1,re, aci1, aci2);
            label40.Text = "Ram Kullanımı";
            label39.Text = "Kullanılan:";
            label44.Text = "Boşta";
            label43.Text = ram_usage + " MB";
            label45.Text = kalann + " MB";

            for (int i = 0; i < listBox3.Items.Count; i++)
            {
                this.chart1.Series["Cpu_%"].Points.AddXY("Core " + listBox4.Items[i], listBox3.Items[i]);
                if (listBox4.Items[i].ToString() == "_Total")
                { label47.Text = "%" + listBox3.Items[i].ToString();
                    label46.Text = "Cpu Kullanım : ";
                }
            }
            chart1.Visible = true;
           
        }

        public void disk_grafik()
        {

            Pen p = new Pen(Color.Black, 1);
            Graphics g = this.CreateGraphics();
            Rectangle re = new Rectangle(150, 600, 200, 200);

            Color clr;
            clr = new Color();

            float aci1 = float.Parse(ram_usage) / float.Parse(toplam) * 360;

            float aci2 = float.Parse(kalann) / float.Parse(toplam) * 360;
            Brush br1 = new SolidBrush(Color.RoyalBlue);
            Brush br2 = new SolidBrush(Color.YellowGreen);
            g.DrawPie(p, re, 0, aci1);
            g.FillPie(br2, re, 0, aci1);
            g.DrawPie(p, re, aci1, aci2);
            g.FillPie(br1, re, aci1, aci2);
            label50.Text = "Disk Kullanım ";
            //label39.Text = "Kullanılan:";
            //label44.Text = "Boşta";
            //label43.Text = ram_usage + " MB";
            //label45.Text = kalann + " MB";

          
            }

        

        private void Form1_Load(object sender, EventArgs e)
        {





        }
    }
}
