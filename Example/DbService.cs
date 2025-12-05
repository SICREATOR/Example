using Example.Models;
using MySql.Data.MySqlClient;
using System.Windows;

namespace Example
{
    internal class DbService
    {
        public static string connectionString = "server=tompsons.beget.tech;user=tompsons_example;database=tompsons_example;password=78919913Zero;CharSet=utf8mb4;";

        public static MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();

            return connection;
        }

        public static int ExecuteNonQueryWithParameters(string query, Dictionary<string, object> parameters)
        {
            int rowsAffected = 0;
            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                rowsAffected = command.ExecuteNonQuery();
            }
            return rowsAffected;
        }

        public static List<T> GetData<T>(string query, Func<MySqlDataReader, T> mapFunction)
        {
            List<T> data = new List<T>();
            using (MySqlConnection connection = GetConnection())
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        data.Add(mapFunction(reader));
                    }
                }
            }
            return data;
        }

        public static object ExecuteScalar(string query, Dictionary<string, object> parameters)
        {
            object result = null;
            using (MySqlConnection connection = GetConnection())
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }
                result = command.ExecuteScalar();
            }
            return result;
        }

        public static User AuthenticateUser(string login, string password)
        {
            try
            {
                string query = "SELECT * FROM users WHERE login = @login AND password = @password";

                using (MySqlConnection connection = GetConnection())
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Id = reader.GetInt32("id"),
                                Surname = reader.GetString("surname"),
                                Name = reader.GetString("name"),
                                Patronymic = reader.GetString("patronymic"),
                                Role = reader.GetString("role"),
                                Login = reader.GetString("login"),
                                Password = reader.GetString("password")
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка аутентификации: {ex.Message}");
            }

            return null;
        }

        public static bool RegisterUser(User newUser)
        {
            try
            {
                // Проверка существующего логина
                string checkQuery = "SELECT COUNT(*) FROM users WHERE login = @login";
                using (MySqlConnection connection = GetConnection())
                using (MySqlCommand checkCommand = new MySqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@login", newUser.Login);
                    long count = (long)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Пользователь с таким логином уже существует!");
                        return false;
                    }
                }

                // Регистрация нового пользователя
                string insertQuery = @"INSERT INTO users (surname, name, patronymic, role, login, password) 
                                   VALUES (@surname, @name, @patronymic, @role, @login, @password)";

                var parameters = new Dictionary<string, object>
                {
                    ["@surname"] = newUser.Surname,
                    ["@name"] = newUser.Name,
                    ["@patronymic"] = newUser.Patronymic,
                    ["@role"] = "user", // Все новые пользователи получают роль 'user'
                    ["@login"] = newUser.Login,
                    ["@password"] = newUser.Password
                };

                ExecuteNonQueryWithParameters(insertQuery, parameters);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
                return false;
            }
        }

        public static List<User> GetAllUsers()
        {
            string query = "SELECT * FROM users ORDER BY id";
            return GetData(query, reader => new User
            {
                Id = reader.GetInt32("id"),
                Surname = reader.GetString("surname"),
                Name = reader.GetString("name"),
                Patronymic = reader.GetString("patronymic"),
                Role = reader.GetString("role"),
                Login = reader.GetString("login"),
                Password = reader.GetString("password")
            });
        }

        public static bool UpdateUser(User user)
        {
            try
            {
                string query = @"UPDATE users 
                            SET surname = @surname, 
                                name = @name, 
                                patronymic = @patronymic, 
                                role = @role, 
                                login = @login, 
                                password = @password 
                            WHERE id = @id";

                var parameters = new Dictionary<string, object>
                {
                    ["@id"] = user.Id,
                    ["@surname"] = user.Surname,
                    ["@name"] = user.Name,
                    ["@patronymic"] = user.Patronymic,
                    ["@role"] = user.Role,
                    ["@login"] = user.Login,
                    ["@password"] = user.Password
                };

                ExecuteNonQueryWithParameters(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления пользователя: {ex.Message}");
                return false;
            }
        }

        public static bool DeleteUser(int userId)
        {
            try
            {
                string query = "DELETE FROM users WHERE id = @id";
                var parameters = new Dictionary<string, object>
                {
                    ["@id"] = userId
                };

                ExecuteNonQueryWithParameters(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления пользователя: {ex.Message}");
                return false;
            }
        }

        public static List<News> GetAllNews()
        {
            string query = "SELECT * FROM news ORDER BY id DESC";
            return GetData(query, reader => new News
            {
                Id = reader.GetInt32("id"),
                Title = reader.GetString("title"),
                Description = reader.GetString("description"),
                ImagePath = reader.GetString("image_path")
            });
        }

        public static bool AddNews(News news)
        {
            try
            {
                string query = @"INSERT INTO news (title, description, image_path) 
                            VALUES (@title, @description, @image_path)";

                var parameters = new Dictionary<string, object>
                {
                    ["@title"] = news.Title,
                    ["@description"] = news.Description,
                    ["@image_path"] = news.ImagePath
                };

                ExecuteNonQueryWithParameters(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления новости: {ex.Message}");
                return false;
            }
        }

        public static bool UpdateNews(News news)
        {
            try
            {
                string query = @"UPDATE news 
                            SET title = @title, 
                                description = @description, 
                                image_path = @image_path 
                            WHERE id = @id";

                var parameters = new Dictionary<string, object>
                {
                    ["@id"] = news.Id,
                    ["@title"] = news.Title,
                    ["@description"] = news.Description,
                    ["@image_path"] = news.ImagePath
                };

                ExecuteNonQueryWithParameters(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления новости: {ex.Message}");
                return false;
            }
        }

        public static bool DeleteNews(int newsId)
        {
            try
            {
                string query = "DELETE FROM news WHERE id = @id";
                var parameters = new Dictionary<string, object>
                {
                    ["@id"] = newsId
                };

                ExecuteNonQueryWithParameters(query, parameters);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления новости: {ex.Message}");
                return false;
            }
        }
    }
}
