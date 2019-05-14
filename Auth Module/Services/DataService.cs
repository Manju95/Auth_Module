using AuthModule.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AuthModule.Services
{
    public class DataService : IDataService
    {
        private IDbConnection _Idbconnection;

        public DataService(IDbConnection dbConnection)
        {
            _Idbconnection = dbConnection;
        }



        public User isAuthenticated(User user)
        {
            User userResult = null;
            string sqlcmd = "select username from users where username=@username and password=@pwd";
            using (IDbConnection con = _Idbconnection)
            {
                con.Open();
                userResult = con.QueryFirstOrDefault<User>(sqlcmd, new { username = user.userName, pwd = user.password });
                con.Close();
            }
            return userResult;

        }

    }
}
