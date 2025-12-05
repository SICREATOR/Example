using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Models
{
    // Модель данных User, все поля это наши столбцы из БД
    public class User
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string Role { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string FullName => $"{Surname} {Name} {Patronymic}".Trim();
    }
}
