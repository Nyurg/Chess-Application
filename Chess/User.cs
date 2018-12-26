using System;
using System.Collections.Generic;
using System.IO.Pipes;

namespace Chess
{
    public partial class User
    {
        public int Id { get; private set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public User() { }

        public User(int id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
