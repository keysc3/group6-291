using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace group6_291
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void usernameLabel_Click(object sender, EventArgs e)
        {

        }
        //testing commit
        private void passwordLabel_Click(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //Connect to Databse
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();

            //Create query string and parameters
            string query = "select isAdmin from Users where username = @username and password = @password";
            SqlCommand getUser = new SqlCommand(query, conn);
            getUser.Parameters.AddWithValue("@username", usernameBox.Text);
            getUser.Parameters.AddWithValue("@password", passwordBox.Text);

            //Execute Query
            SqlDataReader userReader = getUser.ExecuteReader();

            //If a user was found, check for admin or recepionist
            while (userReader.Read())
            {
                if (userReader["isAdmin"].Equals(true))
                    loginResponseLabel.Text = "Admin logged in";
                else
                    loginResponseLabel.Text = "Receptionist logged in";
                conn.Close();
                return;
            }
            //Invalid login
            loginResponseLabel.Text = "Invalid username or password";
            conn.Close();
            return;
        }

        private void titleLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
