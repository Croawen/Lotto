using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotto.Data
{
    public static class DBInitializer
    {
        public static void InitializeDb(DataContext context)
        {
            context.Database.Migrate();
            context.SaveChanges();

            var testUser = context.Users.FirstOrDefault(u => u.Email == "asdf@asdf.pl");
            if (testUser == null)
            {
                testUser = new Entities.UserEntity
                {
                    Email = "asdf@asdf.pl",
                    PasswordHash = "WMroPAet2JAGGMhvNacj5NURquA=",
                    PasswordSalt = "1r1La+ZMSxwY8RMX0gUTAQ=="
                };

                context.Users.Add(testUser);
                context.SaveChanges();
            }
        }
    }
}
