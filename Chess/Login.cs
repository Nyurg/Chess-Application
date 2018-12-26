using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        // Login
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text;
            string password = textBoxPassword.Text;

            // If username or password is null, fail
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Fields cannot be empty");
            }
            // Appropriate credentials entered
            else
            {
                NamedPipeClientStream newUserPipeClient = new NamedPipeClientStream
                    (".", "userLogin", PipeDirection.Out, PipeOptions.Asynchronous);

                newUserPipeClient.Connect();

                // Creates new user and sends user to server for login
                User newUser = new User(username, password);
                string serialised = JsonConvert.SerializeObject(newUser);
                byte[] messageBytes = Encoding.UTF8.GetBytes(serialised);
                newUserPipeClient.Write(messageBytes, 0, messageBytes.Length);

                newUserPipeClient.Close();

                // Connect to the server. 
                NamedPipeClientStream userClient = new NamedPipeClientStream
                    (".", username, PipeDirection.InOut, PipeOptions.Asynchronous);
                userClient.Connect();

            }
        }

        private void WriteString()
        {

        }

        private static string ReadString(PipeStream readFromStream)
        {
            StringBuilder messageBuilder = new StringBuilder();
            string messageChunk = string.Empty;
            byte[] messageBuffer = new byte[5];

            do
            {
                readFromStream.Read(messageBuffer, 0, messageBuffer.Length);
                messageChunk = Encoding.UTF8.GetString(messageBuffer);
                messageBuilder.Append(messageChunk);
                messageBuffer = new byte[messageBuffer.Length];
            }
            while (!readFromStream.IsMessageComplete);

            return messageBuilder.ToString();
        }
    }
}
