using MySql.Data.MySqlClient;
using System;

namespace LabTP2
{
    class User
    {
        int id;
        string firstname, lastname;
        bool men;
        double rating;

        public int Id
        {
            get => id; set
            {
                id = value;
            }
        }
        public string Firstname
        {
            get => firstname; set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new Exception();
                firstname = value;
            }
        }
        public string Lastname
        {
            get => lastname; set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new Exception();
                lastname = value;
            }
        }
        public bool Men { get => men; set => men = value; }
        public double Rating
        {
            get => rating; set
            {
                if (value < 0 || value > 5)
                    throw new Exception();
                rating = value;
            }
        }

        public User(int id, string firstname, string lastname, bool men, double rating)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Men = men;
            Rating = rating;
        }

        public void UpdateUser()
        {
            DB db = new DB();
            User user = this;
            db.openConnection();
            MySqlCommand command = new MySqlCommand("UPDATE `users` SET `firstname` = @fn, `lastname` = @ln, `men` = @men, `rating` = @rating WHERE `users`.`id` = @id", db.getConnection());
            command.Parameters.AddWithValue("ln", user.Lastname);
            command.Parameters.AddWithValue("id", user.Id);
            command.Parameters.AddWithValue("fn", user.Firstname);
            if (user.Men)
                command.Parameters.AddWithValue("men", true);
            else
                command.Parameters.AddWithValue("men", false);
            command.Parameters.AddWithValue("rating", user.Rating);
            command.ExecuteNonQuery();
        }

        public void CreateUser()
        {
            User user = this;
            DB db = new DB();
            db.openConnection();

            MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`id`, `createdAt`, `firstname`, `lastname`, `men`, `rating`) VALUES (NULL, CURRENT_TIMESTAMP, @fn, @ln,@men,@rating)", db.getConnection());
            command.Parameters.AddWithValue("ln", user.Lastname);
            command.Parameters.AddWithValue("fn", user.Firstname);
            if (user.Men)
                command.Parameters.AddWithValue("men", true);
            else
                command.Parameters.AddWithValue("men", false);
            command.Parameters.AddWithValue("rating", user.Rating);
            command.ExecuteNonQuery();
        }
    }
}
