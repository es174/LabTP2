using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace LabTP2
{
    public partial class Form2 : Form
    {
        Form1 form1;
        static int dataId = 0;
        public void ChangeVisible(bool t)
        {
            label4.Visible = t;
            label5.Visible = t;
            checkBox1.Visible = t;
            checkBox2.Visible = t;
            textBox4.Visible = t;
        }
        public Form2(Form1 form, string text, int id)
        {
            dataId = id;
            InitializeComponent();
            form1 = form;
            this.Text = text;
            if (this.Text == "Добавить пользователя" || this.Text == "Добавить статью")
            {
                label3.Visible = false;
                textBox3.Visible = false;
                button1.Text = "Добавить";
            }
            else
                if (this.Text == "Изменить пользователя" || this.Text == "Изменить статью")
            {
                label3.Visible = true;
                textBox3.Visible = true;
                textBox3.Text = id.ToString();
                textBox3.Enabled = false;
                button1.Text = "Изменить";
                for (int i = 0; i < form1.Table.RowCount; i++)
                {
                    if (form1.Table.Rows[i].Cells[0].Value.ToString() == id.ToString())
                    {
                        textBox1.Text = form1.Table.Rows[i].Cells[1].Value.ToString();
                        textBox2.Text = form1.Table.Rows[i].Cells[2].Value.ToString();
                        if ((bool)form1.Table.Rows[i].Cells[4].Value == true)
                            checkBox1.Checked = true;
                        else
                            checkBox2.Checked = true;
                        textBox4.Text = form1.Table.Rows[i].Cells[5].Value.ToString();
                    }
                }

            }
            if (this.Text == "Изменить пользователя" || this.Text == "Добавить пользователя")
            {
                label1.Text = "Имя";
                label2.Text = "Фамилия";
            }
            else if (this.Text == "Изменить статью" || this.Text == "Добавить статью")
            {
                label1.Text = "Название";
                label2.Text = "Текст";
            }

        }
        public Form2(Form1 form, string text)
        {
            InitializeComponent();
            this.Text = text;
            if (this.Text == "Добавить пользователя" || this.Text == "Добавить статью")
            {
                label3.Visible = false;
                textBox3.Visible = false;
            }
            else
                if (this.Text == "Изменить пользователя" || this.Text == "Изменить статью")
            {
                label3.Visible = true;
                textBox3.Visible = true;
            }
            if (this.Text == "Изменить пользователя" || this.Text == "Добавить пользователя")
            {
                label1.Text = "Имя";
                label2.Text = "Фамилия";
                ChangeVisible(true);
            }
            else if (this.Text == "Изменить статью" || this.Text == "Добавить статью")
            {
                label1.Text = "Название";
                label2.Text = "Текст";
                ChangeVisible(false);
            }
            form1 = form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Text == "Добавить пользователя")
            {
                User user = new User(0, textBox1.Text, textBox2.Text, checkBox1.Checked, Convert.ToDouble(textBox4.Text));
                try
                {
                    user.CreateUser();
                }
                catch
                {
                    MessageBox.Show("Введите правильные данные");
                    return;
                }
                this.Clo();
            }
            else
                if (this.Text == "Изменить пользователя")
            {
                User user = new User(Convert.ToInt32(textBox3.Text), textBox1.Text, textBox2.Text, checkBox1.Checked, Convert.ToDouble(textBox4.Text));
                try
                {
                    user.UpdateUser();
                }
                catch
                {
                    MessageBox.Show("Введите правильные данные");
                    return;
                }
                this.Clo();
            }
            else if (this.Text == "Изменить статью")
            {
                if (String.IsNullOrWhiteSpace(textBox1.Text) && String.IsNullOrWhiteSpace(textBox2.Text) && String.IsNullOrWhiteSpace(textBox3.Text))
                    MessageBox.Show(this, "Введите Id,имя и фамилию");
                else
                {
                    string text = textBox2.Text;
                    string label = textBox1.Text;
                    string id = textBox3.Text;

                    DB db = new DB();
                    db.openConnection();
                    MySqlCommand command = new MySqlCommand("UPDATE `article` SET `label` = @label, `text` = @text WHERE `article`.`id` = @id; ", db.getConnection());
                    command.Parameters.AddWithValue("label", label);
                    command.Parameters.AddWithValue("id", id);
                    command.Parameters.AddWithValue("text", text);
                    command.ExecuteNonQuery();
                    this.Clo();
                }
            }
            else if (this.Text == "Добавить статью")
            {
                if (String.IsNullOrWhiteSpace(textBox1.Text) && String.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show(this, "Введите имя и фамилию");
                }
                else
                {
                    string text = textBox2.Text;
                    string label = textBox1.Text;

                    DB db = new DB();
                    db.openConnection();
                    MySqlCommand command = new MySqlCommand("INSERT INTO `article` (`id`, `userId`, `label`, `text`) VALUES (NULL, @id, @label, @text)", db.getConnection());
                    command.Parameters.AddWithValue("label", label);
                    command.Parameters.AddWithValue("id", form1.getId());
                    command.Parameters.AddWithValue("text", text);
                    command.ExecuteNonQuery();
                    this.Clo();
                }
            }
        }

        public void Clo()
        {
            if (this.Text == "Изменить пользователя" || this.Text == "Добавить пользователя")
            {
                form1.UpdateTable();
            }
            else if (this.Text == "Изменить статью" || this.Text == "Добавить статью")
            {
                form1.UpdateArticleTable(form1.getId());
            }

            this.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                checkBox1.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                checkBox2.Checked = false;
        }
    }
}
