using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // SQL Server için gerekli kütüphane

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SqlCommand komut;
        SqlConnection baglan;
        SqlDataAdapter da;
        DataSet ds;



        void listele()
        {
           baglan = new SqlConnection("server=SINAN\\YOSHI; Initial Catalog=word_1k;Integrated Security=SSPI");
            da = new SqlDataAdapter("Select *From word_a", baglan);// veri tabanındaki verileri çekmek için
            ds = new DataSet();// veri tabanındaki verileri tutmak için
            baglan.Open();
            da.Fill(ds, "word_a");
            baglan.Close();
        }
        public int sayi, kod;// sayi değişkeni rastgele sayı üretmek için, kod değişkeni ascii kodunu tutmak için
        public string kelime;// kelime değişkeni kelimeyi tutmak için
        public char harf;// harf değişkeni tablo adını tutmak için
        public Form1()
        {
            InitializeComponent();
            
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            listele();
            label1.Hide();
            label3.Hide();
            label4.Text=0.ToString();
            label5.Text = 0.ToString();
            
            


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();//Random sınıfı
            kod = rnd.Next(97, 119);// 97-119 arası rastgele sayı üretir bunlar ascii kodlarıdır
            harf = Convert.ToChar(kod);// ascii kodunu harfe çevirir


            komut = new SqlCommand("Select Count(id) from word_"+harf+"", baglan);// harf değişkeni ile tablo adını birleştirerek tablodaki kelime sayısını bulur
            SqlDataReader dr;// veri tabanından veri okumak için
            baglan.Open();
            dr = komut.ExecuteReader();// komutu çalıştırır
            while (dr.Read())// veri tabanından veri okur
            {
                label1.Text = dr.GetValue(0).ToString();// tablodaki kelime sayısını label1 e yazdırır

            }

            baglan.Close();
            
            sayi = rnd.Next(1, Convert.ToInt16(label1.Text));// 1 ile tablodaki kelime sayısı arasında rastgele sayı üretir

           
            komut = new SqlCommand("Select word, meaning from word_"+harf+" where id="+sayi+"", baglan);// harf değişkeni ile tablo adını birleştirerek tablodaki kelime sayısını bulur
            SqlDataReader dr1;
            baglan.Open();
            dr1= komut.ExecuteReader();
            while(dr1.Read())
            {
                label2.Text = dr1.GetValue(0).ToString();//tablodaki word sütunundaki veriyi label2 ye yazdırır
                kelime = dr1.GetValue(1).ToString();//tablodaki meaning sütunundaki veriyi kelime değişkenine atar
                
            }
            
            baglan.Close();
            
            
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            button1_Click(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text="";
            button2_Click(sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = kelime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text==kelime)
            {
                label3.Show();
                label3.Text = "Doğru Cevap";
                label3.ForeColor = Color.Green;
                label5.Text = (Convert.ToInt32(label5.Text) + 1).ToString();
                textBox1.Text = "";
                //MessageBox.Show("Doğru Cevap");
            }
            else
            {
               label4.Text = (Convert.ToInt32(label4.Text) + 1).ToString();
                label3.Show();
                label3.Text = kelime;
                label3.ForeColor = Color.Red;
                //MessageBox.Show("Yanlış Cevap");
                textBox1.Text = "";
            }
           button2_Click(sender, e);


        }
    }
}
