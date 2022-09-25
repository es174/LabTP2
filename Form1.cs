﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace LabTP2
{
    public partial class Form1 : Form
    {
        static public int id = 0;
        public int getId() { return id; }
        static DataGridView table;
        public DataGridView Table { get { return table; } }
        public void UpdateTable()
        {
            this.Text = "Пользователи";
            table = this.dataGridView1;
            DB db = new DB();
            db.openConnection();
            DataTable dataTable = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT id AS 'Id', firstname AS 'Имя', lastname AS 'Фамилия', createdAt AS 'Создан'  FROM `users`", db.getConnection());
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

                Form2 form2 = new Form2(this, "Добавить пользователя");
                form2.ShowDialog();
            }
            else if (this.Text == "Статьи")
            {
                Form2 form2 = new Form2(this, "Добавить статью");
                form2.ShowDialog();
            }

        }

        private void изменитьToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (this.Text == "Пользователи")
            {

                Form2 form2 = new Form2(this, "Изменить пользователя");
                form2.ShowDialog();
            }
            else if (this.Text == "Статьи")
            {
                Form2 form2 = new Form2(this, "Изменить статью");
                form2.ShowDialog();
            }

        }

        private void назадToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Text = "Пользователи";
            ToolStripMenuItem tool = sender as ToolStripMenuItem;
            tool.Visible = false;
            id = 0;
            UpdateTable();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.Text == "Пользователи")
            {
                id = (int)dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                UpdateArticleTable(id);
                назадToolStripMenuItem.Visible = true;
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
            else if (this.Text == "Статьи")
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM `article` WHERE id = @id", db.getConnection());
                command.Parameters.AddWithValue("id", id);
                command.ExecuteNonQuery();
                UpdateArticleTable(id);
            }
        }

        private void dgrdResults_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }
        static int selectedBiodataId = 0;
        private void dgrdResults_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                    dataGridView1.Focus();
                    selectedBiodataId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception)
                { }
            }
        }

        private void изменитьToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.Text == "Пользователи")
            {
                Form2 form2 = new Form2(this, "Изменить пользователя", selectedBiodataId);
                form2.ShowDialog();
            }
            else if (this.Text == "Статьи")
            {
                Form2 form2 = new Form2(this, "Изменить статью", selectedBiodataId);
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
                UpdateArticleTable(id);
            }
            selectedBiodataId = 0;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter =
                String.Format("Фамилия like '{0}%'", textBox1.Text);
        }
    }
}