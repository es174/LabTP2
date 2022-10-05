using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace LabTP2
{
    public partial class Form1 : Form
    {
        private static int userId = 0;

        static DataGridView table;
        public DataGridView Table { get { return table; } }

        public int UserId { get { return userId; } }

        public void UpdateTable()
        {
            this.Text = "Пользователи";
            table = this.dataGridView1;
            DB db = new DB();
            db.openConnection();
            DataTable dataTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("CALL `selectUsers`();", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            table.DataSource = dataTable;
            table.Update();
        }
        public void UpdateArticleTable(int id)
        {
            this.Text = "Статьи";
            table = dataGridView1;
            DB db = new DB();
            db.openConnection();
            DataTable dataTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT id AS 'Id',label AS 'Название', text AS 'Содержание' FROM `article` WHERE userId =@id", db.getConnection());
            command.Parameters.AddWithValue("id", id);
            adapter.SelectCommand = command;
            adapter.Fill(dataTable);
            table.DataSource = dataTable;
            table.Update();
            db.closeConnection();
        }

        public Form1()
        {

            InitializeComponent();
            UpdateTable();
        }

        private void добавитьToolStripMenuItem1_Click(object sender, System.EventArgs e)
        {
            if (this.Text == "Пользователи")
            {

                Form2 form2 = new Form2(this, "Добавить пользователя", 0, 0);
                form2.ShowDialog();
            }
            else if (this.Text == "Статьи")
            {
                Form2 form2 = new Form2(this, "Добавить статью", 0, UserId);
                form2.ShowDialog();
            }

        }

        private void изменитьToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.Text == "Пользователи")
            {

                Form2 form2 = new Form2(this, "Изменить пользователя", 0, 0);
                form2.ShowDialog();
            }
            else if (this.Text == "Статьи")
            {
                Form2 form2 = new Form2(this, "Изменить статью", 0, UserId);
                form2.ShowDialog();
            }

        }

        private void назадToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Text = "Пользователи";
            ToolStripMenuItem tool = sender as ToolStripMenuItem;
            tool.Visible = false;
            userId = 0;
            UpdateTable();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.Text == "Пользователи")
            {
                userId = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                UpdateArticleTable(UserId);
                назадToolStripMenuItem.Visible = true;
                базаДанныхToolStripMenuItem.Visible = true;
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowCancelEventArgs e)
        {
            int id = (int)e.Row.Cells[0].Value;
            DB db = new DB();
            db.openConnection();
            if (this.Text == "Пользователи")
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM `users` WHERE id = @id", db.getConnection());
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                UpdateTable();
            }
            if (this.Text == "Статьи")
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM `article` WHERE id = @id", db.getConnection());
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                UpdateArticleTable(id);
            }
        }
        static int selectedBiodataId = 0;

        private void изменитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.Text == "Пользователи")
            {
                Form2 form2 = new Form2(this, "Изменить пользователя", selectedBiodataId, 0);
                form2.ShowDialog();
            }
            else if (this.Text == "Статьи")
            {
                Form2 form2 = new Form2(this, "Изменить статью", selectedBiodataId, UserId);
                form2.ShowDialog();
            }
            selectedBiodataId = 0;
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            db.openConnection();
            if (this.Text == "Пользователи")
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM `users` WHERE id = @id", db.getConnection());
                command.Parameters.AddWithValue("id", selectedBiodataId);
                command.ExecuteNonQuery();
                UpdateTable();
            }
            else if (this.Text == "Статьи")
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM `article` WHERE id = @id", db.getConnection());
                command.Parameters.AddWithValue("id", selectedBiodataId);
                command.ExecuteNonQuery();
                UpdateArticleTable(UserId);
            }
            selectedBiodataId = 0;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter =
                String.Format("Фамилия like '{0}%'", textBox1.Text);
        }


        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    contextMenuStrip1.Enabled = true;
                    contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.Focus();
                    selectedBiodataId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception)
                { }
            }
        }
    }
}
