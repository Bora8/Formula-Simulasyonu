using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Formula_Simülasyonu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=KOELSYS\\SQLEXPRESS;Initial Catalog=FormulaSimülasyonu;Integrated Security=True");
        int sayi = 0;
        
        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            
            
            sayi++;
            label2.Text = sayi.ToString();
            switch (sayi)
            {
                case 0:
                    label1.Text = "Melbourne - Avustralya Grand Prix";
                    break;
                case 1:
                    label1.Text = "Imola - Emilia Romagna Grand Prix";
                    break;
                case 2:
                    label1.Text = "Portimao - Portekiz Grand Prix";
                    break;
                case 3:
                    label1.Text = "Circuit de Barcelona-Catalunya - İspanya Grand Prix";
                    break;
                case 4:
                    label1.Text = "Monaco - Monaco Grand Prix";
                    break;
                case 5:
                    label1.Text = "Baku City Circuit - Azerbaycan Grand Prix";
                    break;
                case 6:
                    label1.Text = "Circuit Paul Ricard - Fransa Grand Prix";
                    break;
                case 7:
                    label1.Text = "Red Bull Ring - Avusturya Grand Prix";
                    break;
                case 8:
                    label1.Text = "Silverstone Circuit - İngiltere Grand Prix";
                    break;
                case 9:
                    label1.Text = "Hungaroring - Macaristan Grand Prix";
                    break;
                case 10:
                    label1.Text = "Spa-Francorchamps - Belçika Grand Prix";
                    break;
                case 11:
                    label1.Text = "Zandvoort - Hollanda Grand Prix";
                    break;
                case 12:
                    label1.Text = "Monza - İtalya Grand Prix";
                    break;
                case 13:
                    label1.Text = "Sochi Autodrom - Rusya Grand Prix";
                    break;
                case 14:
                    label1.Text = "Marina Bay Street Circuit - Singapur Grand Prix";
                    break;
                case 15:
                    label1.Text = "Suzuka Circuit - Japonya Grand Prix";
                    break;
                case 16:
                    label1.Text = "Circuit of the Americas - Amerika Birleşik Devletleri Grand Prix";
                    break;
                case 17:
                    label1.Text = "Autódromo Hermanos Rodríguez - Meksika Grand Prix";
                    break;
                case 18:
                    label1.Text = "Interlagos - Brezilya Grand Prix";
                    break;
                case 19:
                    label1.Text = "Albert Park Circuit - Avustralya Grand Prix";
                    break;
                case 20:
                    label1.Text = "Jeddah Street Circuit - Suudi Arabistan Grand Prix";
                    break;
                case 21:
                    label1.Text = "Yas Marina Circuit - Abu Dabi Grand Prix";
                    break;
                case 22:
                    label1.Text = "Yas Marina Circuit - Abu Dabi Grand Prix";
                    break;
                case 23:
                    label1.Text = "Shanghai International Circuit - Çin Grand Prix";
                    break;
                default:
                    label1.Text = "Bilinmeyen Pist";
                    break;
            }       
            grid();
            sirala();
            sezonsilamasi();
            if(sayi == 23)
            {
                MessageBox.Show("Sezon Bitti. Uygulama Kapanacak.");
                Application.Exit();
                
            }
            
        }
        private void grid()
        {
            object[,] dizi = new object[2, 20];
            
            Random random = new Random();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM PilotlarSiralamasi", baglanti);
            SqlDataReader dr = komut.ExecuteReader();

            int i = 0;
            while (dr.Read())
            {
                dizi[0, i] = dr["Pilotlar"].ToString();
                dizi[1, i] = dr["Guc"];
                i++;
            }
            dr.Close();

            for (int j = 0; j < 20; j++)
            {
                string pilotAdi = dizi[0, j].ToString();
                int gucu = Convert.ToInt32(dizi[1, j]);
                int yeniPuan = gucu + random.Next(-10, 11);

                SqlCommand komut1 = new SqlCommand("UPDATE PilotlarSiralamasi SET SonYaris = @yeniPuan WHERE Pilotlar = @pilotAdi", baglanti);
                komut1.Parameters.AddWithValue("@yeniPuan", yeniPuan);
                komut1.Parameters.AddWithValue("@pilotAdi", pilotAdi);
                
                komut1.ExecuteNonQuery();

                
            }

            baglanti.Close();
        }
        private void sirala()
        {
            baglanti.Open();
            object[,] dizi1 = new object[3, 20];

            SqlCommand komut = new SqlCommand("SELECT * FROM PilotlarSiralamasi ORDER BY SonYaris DESC, NEWID()", baglanti);
            SqlDataReader dr = komut.ExecuteReader();

            int a = 0; int p = 0;
            while (dr.Read())
            {
                switch (a)
                {
                    case (0):
                        p = 25;
                        break;
                    case (1):
                        p = 18;
                        break;
                    case (2):
                        p = 15;
                        break;
                    case (3):
                        p = 12;
                        break;
                    case (4):
                        p = 10;
                        break;
                    case (5):
                        p = 8;
                        break;
                    case (6):
                        p = 6;
                        break;
                    case (7):
                        p = 4;
                        break;
                    case (8):
                        p = 2;
                        break;
                    case (9):
                        p = 1;
                        break;
                    default: p = 0; break;
                }
                

                string[] row = { (a + 1).ToString(), dr["Pilotlar"].ToString(), dr["Takımlar"].ToString(), p.ToString() };
                var item = new ListViewItem(row);
                listView1.Items.Add(item);
                
                dizi1[0,a] = dr["Pilotlar"].ToString();
                dizi1[1, a] = p;
                dizi1[2, a] = dr["Takımlar"];
                
                a++;
            }
            dr.Close();
            for (int i = 0; i < 20; i++)
            {
                SqlCommand pkomut = new SqlCommand("UPDATE SezonSiralamasiPilotlar SET Puan = Puan + @puan WHERE Pilotlar = @pilotlar", baglanti);
                pkomut.Parameters.AddWithValue("@puan", dizi1[1, i]);
                pkomut.Parameters.AddWithValue("@pilotlar", dizi1[0, i]);
                pkomut.ExecuteNonQuery();
            }
            for (int i = 0; i < 20; i++)
            {
                SqlCommand pkomut = new SqlCommand("UPDATE SezonSiralamasiTakımlar SET Puan = Puan + @puan WHERE Takımlar = @takimlar", baglanti);
                pkomut.Parameters.AddWithValue("@puan", dizi1[1, i]);
                pkomut.Parameters.AddWithValue("@takimlar", dizi1[2, i]);
                pkomut.ExecuteNonQuery();
            }

            baglanti.Close();
            
        }
        private void sezonsilamasi()
        {
            listView2.Items.Clear();
            listView3.Items.Clear();

            baglanti.Open();
            SqlCommand komut = new SqlCommand("SELECT * FROM SezonSiralamasiPilotlar ORDER BY Puan DESC, NEWID()", baglanti);
            SqlDataReader dr = komut.ExecuteReader();
            int a = 0;
            while (dr.Read())
            {
                a++;
                string[] row = { a.ToString(),dr["Pilotlar"].ToString(), dr["Takımlar"].ToString(), dr["Puan"].ToString() };
                var item = new ListViewItem(row);
                listView2.Items.Add(item);
            }

            dr.Close();

            SqlCommand komut1 = new SqlCommand("SELECT * FROM SezonSiralamasiTakımlar ORDER BY Puan DESC, NEWID()", baglanti);
            SqlDataReader dr1 = komut1.ExecuteReader();
            int b = 0;
            while (dr1.Read())
            {
                b++;
                string[] row1 = { b.ToString(), dr1["Takımlar"].ToString(), dr1["Puan"].ToString() };
                var item1 = new ListViewItem(row1);
                listView3.Items.Add(item1);
            }
            baglanti.Close();
        }

        private void ResetTablolar()
        {
            try
            {
                baglanti.Open();

                
                SqlCommand resetKomut1 = new SqlCommand("UPDATE PilotlarSiralamasi SET SonYaris = 0", baglanti);
                resetKomut1.ExecuteNonQuery();

                
                SqlCommand resetKomut2 = new SqlCommand("UPDATE SezonSiralamasiPilotlar SET Puan = 0", baglanti);
                resetKomut2.ExecuteNonQuery();

                
                SqlCommand resetKomut3 = new SqlCommand("UPDATE SezonSiralamasiTakımlar SET Puan = 0", baglanti);
                resetKomut3.ExecuteNonQuery();

                baglanti.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Tablolar sıfırlanırken bir hata oluştu: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResetTablolar();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
    
}
