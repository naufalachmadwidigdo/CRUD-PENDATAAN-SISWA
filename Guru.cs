using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BelajarCrud
{
    public partial class Guru : Form
    {
        string tampung;
        public MySqlCommand cmd;
        public MySqlDataReader dr;
        public String id;
        public Guru()
        {
            InitializeComponent();
        }

        private void Tampil()
        {
            try
            {

                Koneksi.conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM `guru`", Koneksi.conn);
                DataSet ds = new DataSet();
                da.Fill(ds);

                dgguru.DataSource = ds.Tables[0];
                Koneksi.conn.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Duh!!, Ada Error Nih");
            }
        }

        private void Guru_Load(object sender, EventArgs e)
        {
            Tampil();
            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = false;
        }

        private void Clear()
        {
            txtnama.Text = "";
            txtnip.Text = "";

            btDelete.Enabled = false;
            btUpdate.Enabled = false;
            btCancel.Enabled = false;
            btInsert.Enabled = true;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (txtnama.Text.Contains("'") == true)
            {
                txtnama.Text.Replace("'", @"\\");
            } 

            if (string.IsNullOrEmpty(txtnama.Text) || string.IsNullOrEmpty(txtnip.Text))
            {
                MessageBox.Show("Ada yang belum diisi");
            }
            else
            {
                if (txtnip.Text.Length == 18)
                {
                    try
                    {
                        Koneksi.conn.Open();
                        //String Queri = "INSERT INTO guru (`nama`, `nip`) VALUES ('" + txtnama.Text + "', '" + txtnip.Text + "')";
                        cmd = new MySqlCommand("INSERT INTO guru (`nama`, `nip`) VALUES (@nama,@nip)", Koneksi.conn);
                        cmd.Parameters.AddWithValue("@nama", txtnama.Text);
                        cmd.Parameters.AddWithValue("@nip", txtnip.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Berhasil Simpan Data Guru");
                        Koneksi.conn.Close();

                        Tampil();
                        Clear();

                        //using (MySqlConnection connection = new MySqlConnection(connectionString))
                        //{
                        //    connection.Open();
                        //    using (MySqlCommand cmd = new MySqlCommand("INSERT INTO TabelNama (Kolom1) VALUES (@Value1)", connection))
                        //    {
                        //        cmd.Parameters.AddWithValue("@Value1", textBox1.Text); // textBox1 adalah input dari pengguna
                        //        cmd.ExecuteNonQuery();
                        //    }
                        //}

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Tambah Data Gagal");
                    }
                }
                else
                {
                    MessageBox.Show("NIP tidak valid");
                }
            } 
        }

        private void dgguru_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            btDelete.Enabled = true;
            btUpdate.Enabled = true;
            btCancel.Enabled = true;
            btInsert.Enabled = false;

            int baris = dgguru.CurrentCell.RowIndex;
            id = dgguru.Rows[baris].Cells[0].Value.ToString();
            txtnama.Text = dgguru.Rows[baris].Cells[1].Value.ToString();
            txtnip.Text = dgguru.Rows[baris].Cells[2].Value.ToString();
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            
            if (txtnama.Text.Contains("'") == true)
            {
                tampung = txtnama.Text.Replace("`", @"\\");
            }
            if (string.IsNullOrEmpty(txtnama.Text) || string.IsNullOrEmpty(txtnip.Text))
            {
                MessageBox.Show("Ada yang belum diisi");
            }
            else
            {
                if (txtnip.Text.Length == 18)
                {
                    try
                    {
                        Koneksi.conn.Open();
                        String Queri = "UPDATE guru SET `nama`='" + tampung + "', `nip`='" + txtnip.Text + "' WHERE  `id`='" + id + "'";
                        cmd = new MySqlCommand(Queri, Koneksi.conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Berhasil Update Data Guru");
                        Koneksi.conn.Close();

                        Tampil();
                        Clear();
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Update Gagal");
                    }
                } else
                {
                    MessageBox.Show("NIP tidak valid");
                }
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Koneksi.conn.Open();
                cmd = new MySqlCommand("DELETE FROM guru WHERE  `id`= '" + id + "'", Koneksi.conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Berhasil Hapus Data Guru");
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

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            try
            {

                Koneksi.conn.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM `guru` where nama LIKE '%" + txtcari.Text + "%'", Koneksi.conn);
                DataSet ds = new DataSet();

                da.Fill(ds);
                dgguru.DataSource = ds.Tables[0];
                Koneksi.conn.Close();
            }
            catch (Exception)
            {

                MessageBox.Show("Duh!!, Ada Error Nih");
            }
        }

        private void txtnip_KeyPress(object sender, KeyPressEventArgs e)
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
