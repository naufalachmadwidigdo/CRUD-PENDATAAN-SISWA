using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BelajarCrud
{
    public partial class Siswa : Form
    {
        public MySqlCommand cmd;
        public MySqlDataReader dr;
        public String id;

        public Siswa()
        {
            InitializeComponent();
        }
        private void Tampil()
        {
            try
            {
                //MessageBox.Show("Ini Muncul Saat Form dipanggil");

                Koneksi.conn.Open(); //ini membuka koneksi database

                //ini digunakan untuk mengambil data di tabel siswa
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM `data_siswa`.`siswa`", Koneksi.conn);

                //membuat objek Dasa Set Baru
                DataSet ds = new DataSet();

                //ini digunakan untuk mengisi dataset dari Data Adapter
                da.Fill(ds);

                //Mengisi DataGrid Siswa dengan DataSet
                dgsiswa.DataSource = ds.Tables[0];
                Koneksi.conn.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Duh!!, Ada Error Nih");
            }
        }

        private void dgsiswa_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btDelete.Enabled = true;
            btUpdate.Enabled = true;
            btCancel.Enabled = true;
            btInsert.Enabled = false;

            int baris = dgsiswa.CurrentCell.RowIndex;
            id = dgsiswa.Rows[baris].Cells[0].Value.ToString();
            txtnama.Text = dgsiswa.Rows[baris].Cells[1].Value.ToString();
            cbkelamin.Text = dgsiswa.Rows[baris].Cells[2].Value.ToString();
            txtalamat.Text = dgsiswa.Rows[baris].Cells[3].Value.ToString();
            txtnohp.Text = dgsiswa.Rows[baris].Cells[4].Value.ToString();
            txtjurusan.Text = dgsiswa.Rows[baris].Cells[5].Value.ToString();
            txtemail.Text = dgsiswa.Rows[baris].Cells[6].Value.ToString();
        }


        private void Clear()
        {
            txtnama.Text = "";
            txtalamat.Text = "";
            txtemail.Text = "";
            txtjurusan.Text = "";
            txtnohp.Text = "";
            txtcari.Text = "";

            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = false;
            btInsert.Enabled = true;
        }


        private void Siswa_Load(object sender, EventArgs e)
        {
            Tampil();
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = false;
        }

        private void btInsert_Click(object sender, EventArgs e)
        {
            //if (txtnama.Text.Contains("'") == true)
            //{
            //    txtnama.Text.Replace("'", @"\\'");
            //}
            if (string.IsNullOrEmpty(txtnama.Text) || string.IsNullOrEmpty(txtalamat.Text) || string.IsNullOrEmpty(txtemail.Text) || string.IsNullOrEmpty(txtjurusan.Text) || string.IsNullOrEmpty(txtnohp.Text))
            {
                MessageBox.Show("Ada yang belum diisi");
            }
            else
            {
                string email = txtemail.Text;
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                {
                    if (txtnohp.Text.Length < 14 && txtnohp.Text.Length > 10)
                    {

                        try
                        {
                            Koneksi.conn.Open();
                            string query = "INSERT INTO siswa(nama, jenkel, alamat, nohp, jurusan, email) VALUES (@nama, @jenkel, @alamat, @nohp, @jurusan, @email)";
                            using (MySqlCommand cmd = new MySqlCommand(query, Koneksi.conn))
                            {
                                cmd.Parameters.AddWithValue("@nama", txtnama.Text);
                                cmd.Parameters.AddWithValue("@jenkel", cbkelamin.Text);
                                cmd.Parameters.AddWithValue("@alamat", txtalamat.Text);
                                cmd.Parameters.AddWithValue("@nohp", txtnohp.Text);
                                cmd.Parameters.AddWithValue("@jurusan", txtjurusan.Text);
                                cmd.Parameters.AddWithValue("@email", txtemail.Text);

                                cmd.ExecuteNonQuery();
                            }
                            //String Queri = "INSERT INTO siswa (`nama`, `jenkel`, `alamat`, `nohp`, `jurusan`, `email`) VALUES ('" + txtnama.Text + "', '" + cbkelamin.Text + "', '" + txtalamat.Text + "', '" + txtnohp.Text + "', '" + txtjurusan.Text + "', '" + txtemail.Text + "')";
                            //cmd = new MySqlCommand(Queri, Koneksi.conn);
                            //cmd.ExecuteNonQuery();
                            MessageBox.Show("Berhasil Simpan Data Siswa");
                            Koneksi.conn.Close();

                            Tampil(); //memanggil method Tampil
                            Clear(); //memanggil method Clear
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Tambah Data Gagal");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Panjang digit no hp tidak boleh lebih dari 13 dan kurang dari 11");
                    }
                }
                else
                {
                    MessageBox.Show("email salah");
                }
               
                
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtnama.Text) || string.IsNullOrEmpty(txtalamat.Text) || string.IsNullOrEmpty(txtemail.Text) || string.IsNullOrEmpty(txtjurusan.Text) || string.IsNullOrEmpty(txtnohp.Text))
            {
                MessageBox.Show("Ada yang belum terisi");
            }
            else
            {
                string email = txtemail.Text;
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (match.Success)
                {
                    if (txtnohp.Text.Length < 14 && txtnohp.Text.Length > 10)
                    {
                        try
                        {
                            Koneksi.conn.Open();
                            String Queri = "UPDATE siswa SET `nama`='" + txtnama.Text + "', `jenkel`='" + cbkelamin.Text + "', `alamat`='" + txtalamat.Text + "', `nohp`='" + txtnohp.Text + "', `jurusan`='" + txtjurusan.Text + "', `email`='" + txtemail.Text + "' WHERE  `id`='" + id + "'";
                            cmd = new MySqlCommand(Queri, Koneksi.conn);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Berhasil Update Data Siswa");
                            Koneksi.conn.Close();

                            Tampil();
                            Clear();
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("Update Gagal");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Panjang digit no hp tidak boleh lebih dari 13 dan kurang dari 11");
                    }
                }
                else
                {
                    MessageBox.Show("email salah");
                }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Koneksi.conn.Open();

                cmd = new MySqlCommand("DELETE FROM siswa WHERE  `id`= '" + id + "'", Koneksi.conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Berhasil Hapus Data Siswa");
                Koneksi.conn.Close();

                Tampil();
                Clear();
            }
            catch (Exception)
            {

                MessageBox.Show("Hapus Data Gagal");
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtcari_TextChanged(object sender, EventArgs e)
        {
            try
            {

                Koneksi.conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM `siswa` where nama LIKE '%" + txtcari.Text + "%'", Koneksi.conn);
                DataSet ds = new DataSet();

                da.Fill(ds);
                dgsiswa.DataSource = ds.Tables[0];
                Koneksi.conn.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Duh!!, Ada Error Nih");
            }
        }

        private void txtnohp_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtnohp_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back;
        }

        private void txtnama_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
