using System;
using System.Windows.Forms;

namespace LabTP2
{
    public partial class Form2 : Form
    {
        Form1 form1;
        static int dataId = 0;
        static int userId = 0;
        public void ChangeVisible(bool t)
        {
            if (t)
            {
                label1.Text = "Имя";
                label2.Text = "Фамилия";
            }
            else
            {
                label1.Text = "Название";
                label2.Text = "Содержание";
            }
            label4.Visible = t;
            label5.Visible = t;
            checkBox1.Visible = t;
            checkBox2.Visible = t;
            textBox4.Visible = t;
        }
        public Form2(Form1 form, string text, int id, int uId)
        {
            userId = uId;
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
            if (this.Text == "Изменить пользователя" || this.Text == "Изменить статью")
            {
                label3.Visible = true;
                textBox3.Visible = true;
                textBox3.Enabled = true;
                if (dataId != 0)
                {
                    textBox3.Text = dataId.ToString();
                    textBox3.Enabled = false;
                }
                button1.Text = "Изменить";
                if (dataId != 0)
                    for (int i = 0; i < form1.Table.RowCount; i++)
                    {
                        if (form1.Table.Rows[i].Cells[0].Value.ToString() == dataId.ToString())
                        {
                            textBox1.Text = form1.Table.Rows[i].Cells[1].Value.ToString();
                            textBox2.Text = form1.Table.Rows[i].Cells[2].Value.ToString();
                            if (this.Text == "Изменить пользователя")
                            {
                                if (Convert.ToInt32(form1.Table.Rows[i].Cells[4].Value) == 0)
                                    checkBox1.Checked = true;
                                else
                                    checkBox2.Checked = true;
                                textBox4.Text = form1.Table.Rows[i].Cells[5].Value.ToString();
                            }
                        }
                    }

            }
            if (this.Text == "Изменить пользователя" || this.Text == "Добавить пользователя")
            {

                ChangeVisible(true);
            }
            if (this.Text == "Изменить статью" || this.Text == "Добавить статью")
            {

                ChangeVisible(false);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Text == "Добавить пользователя")
                {
                    User user = new User(0, textBox1.Text, textBox2.Text, checkBox1.Checked, Convert.ToDouble(textBox4.Text));
                    user.CreateUser();
                }
                if (this.Text == "Изменить пользователя")
                {
                    User user = new User(Convert.ToInt32(textBox3.Text), textBox1.Text, textBox2.Text, checkBox1.Checked, Convert.ToDouble(textBox4.Text));
                    user.UpdateUser();
                }
                if (this.Text == "Изменить статью")
                {
                    Article article = new Article(Convert.ToInt32(textBox3.Text), textBox2.Text, textBox1.Text, userId);
                    article.UpdateArticle();
                }
                if (this.Text == "Добавить статью")
                {
                    Article article = new Article(0, textBox2.Text, textBox1.Text, userId);
                    article.CreateArticle();
                }
            }
            catch
            {
                MessageBox.Show("Введите правильные данные");
                return;
            }
            this.Clo();
        }

        public void Clo()
        {
            if (this.Text == "Изменить пользователя" || this.Text == "Добавить пользователя")
            {
                form1.UpdateTable();
            }
            else if (this.Text == "Изменить статью" || this.Text == "Добавить статью")
            {
                form1.UpdateArticleTable(form1.UserId);
            }

            this.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                checkBox1.CheckState = CheckState.Unchecked;

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState == CheckState.Checked)
            {
                checkBox2.CheckState = CheckState.Unchecked;

            }

        }
    }
}
