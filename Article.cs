using MySql.Data.MySqlClient;
using System;

namespace LabTP2
{
    class Article
    {
        int id, userId;
        string label, text;

        public int Id { get => id; set => id = value; }
    
        public string Label
        {
            get => label; set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new Exception();
                label = value;
            }
        }
        public string Text
        {
            get => text; set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new Exception();
                text = value;
            }
        }

        public int UserId { get => userId; set => userId = value; }

        public Article(int id, string text, string label, int userId)
        {
            Id = id;
            Label = label;
            Text = text;
            UserId = userId;
        }

        public void UpdateArticle()
        {
            DB db = new DB();
            Article article = this;
            db.openConnection();
            MySqlCommand command = new MySqlCommand("UPDATE `article` SET `label` = @label, `text` = @text WHERE `article`.`id` = @id; ", db.getConnection());
            command.Parameters.AddWithValue("label", article.Label);
            command.Parameters.AddWithValue("id", article.Id);
            command.Parameters.AddWithValue("text", article.Text);
            command.ExecuteNonQuery();
        }

        public void CreateArticle()
        {
            Article article = this;
            DB db = new DB();
            db.openConnection();
            MySqlCommand command = new MySqlCommand("INSERT INTO `article` (`id`, `userId`, `label`, `text`) VALUES (NULL, @id, @label, @text)", db.getConnection());
            command.Parameters.AddWithValue("label", article.Label);
            command.Parameters.AddWithValue("id", article.UserId);
            command.Parameters.AddWithValue("text", article.Text);
            command.ExecuteNonQuery();
        }
    }
}
