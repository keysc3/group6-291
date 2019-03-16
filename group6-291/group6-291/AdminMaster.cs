using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace group6_291
{
    public partial class AdminMaster : Form
    {
        public AdminMaster()
        {
            
            InitializeComponent();
            
        }
        private void AdminMaster_Load(object sender, EventArgs e)
        {
            if (Form1.Account == 0)
            {
                TabControl.TabPages.Remove(UserAccTab);
                TabControl.TabPages.Remove(PatientRecTab);
                TabControl.TabPages.Remove(WardTab);
            }
            addUsername.Leave += new EventHandler(addUsername_Leave);
            addPassword.Leave += new EventHandler(addPassword_Leave);
            populateAccountList();
            addWardNameBox.Leave += new EventHandler(addWardNameBox_Leave);
            AddWardCapacityBox.Leave += new EventHandler(addWardCapacityBox_Leave);
            populateWardList();
        }

 
        //Purpose: Populate the account list box with all the registered users
        private void populateAccountList()
        {
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("select username, isAdmin from [User]", conn);
            //Fill the dataset, sort it, and bind it to the list box
            adapter.Fill(ds);
            ds.Tables[0].DefaultView.Sort = "username asc";
            accountListBox.DataSource = ds.Tables[0];
            accountListBox.DisplayMember = "username";
            conn.Close();
        }

        //Purpose: Populate the ward list box with all the registered wards
        private void populateWardList()
        {
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from [Ward]", conn);
            //Fill the dataset, sort it, and bind it to the list box
            adapter.Fill(ds);
            ds.Tables[0].DefaultView.Sort = "wardName asc";
            wardListBox.DataSource = ds.Tables[0];
            wardListBox.DisplayMember = "wardName";
            conn.Close();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void addUsername_TextChanged(object sender, EventArgs e)
        {

        }

        //Purpose: Change the Username textbox's error label if it is invalid after user leaves the texbox
        private void addUsername_Leave(object sender, EventArgs e)
        {
            bool validUsername = usernameIsValid();
        }

        //Purpose: Change the Password textbox's error label if it is invalid after user leaves the texbox
        private void addPassword_Leave(object sender, EventArgs e)
        {
            bool validPassword = passwordIsValid();

        }

        //Purpose: Uncheck the receptionist checkbox if the admin one was checked
        private void adminCheckBox_CheckChanged(object sender, EventArgs e)
        {
            if (adminCheckbox.Checked == true)
            {
                recepCheckbox.Checked = false;
                checkboxInfo.Text = "";
            }
        }

        //Purpose: Uncheck the admin checkbox if the receptionist one was checked
        private void recepCheckBox_CheckChanged(object sender, EventArgs e)
        {
            if (recepCheckbox.Checked == true)
            {
                adminCheckbox.Checked = false;
                checkboxInfo.Text = "";
            }
        }

        //Purpose: Chceck that the username texbox has a valid username as its input
        private bool usernameIsValid()
        {
            string inputedUser = addUsername.Text;
            //Check username against username constraints
            if (!Regex.IsMatch(inputedUser, @"^[a-zA-Z0-9]+$"))
            {
                usernameInfo.Text = "*Invalid username characters";
                usernameInfo.ForeColor = Color.Red;
                return false;
            }
            else if (inputedUser.Length < 4 || inputedUser.Length > 16)
            {
                usernameInfo.Text = "*Invalid username length";
                usernameInfo.ForeColor = Color.Red;
                return false;
            }
            //Check to see if the username already exists
            else
            {
                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();
                SqlCommand checkUsername = new SqlCommand("select count(*) from [User] where username = @user", conn);
                checkUsername.Parameters.AddWithValue("@user", inputedUser);
                int userExist = (int)checkUsername.ExecuteScalar();
                //Username already exists
                if (userExist > 0)
                {
                    usernameInfo.Text = "*Username already exists";
                    usernameInfo.ForeColor = Color.Red;
                    return false;
                }
                //Username does not already exist
                else
                {
                    usernameInfo.Text = "*Username is available";
                    usernameInfo.ForeColor = Color.Green;
                    return true;
                }
            }
        }

        //Purpose: Chceck that the password texbox has a valid password as its input
        private bool passwordIsValid()
        {
            string inputedPassword = addPassword.Text;
            //Check password against password constraints
            if (inputedPassword.Length < 4 || inputedPassword.Length > 16)
            {
                passwordInfo.Text = "*Invalid password length";
                passwordInfo.ForeColor = Color.Red;
                return false;
            }

            else if (!Regex.IsMatch(inputedPassword, @"^[a-zA-Z0-9]+$"))
            {
                passwordInfo.Text = "*Invalid password characters";
                passwordInfo.ForeColor = Color.Red;
                return false;
            }
            //Password is valid
            else
            {
                passwordInfo.Text = "";
                return true;
            }
        }

        //Purpose: Reset all reporting and input fields
        private void resetAddUserFields()
        {
            addUsername.Text = "";
            addPassword.Text = "";
            usernameInfo.Text = "";
            passwordInfo.Text = "";
            checkboxInfo.Text = "";
            adminCheckbox.Checked = false;
            recepCheckbox.Checked = false;
        }

        //Purpose: Add a new user to the User table in the database when the Add Account button is clicked
        private void addAccountButton_Click(object sender, EventArgs e)
        {
            //Make sure a role is checked
            if (adminCheckbox.Checked == false && recepCheckbox.Checked == false)
            {
                checkboxInfo.Text = "*Please select a role";
                checkboxInfo.ForeColor = Color.Red;
            }
            //Make sure username and password are valid
            else if (!usernameIsValid() || !passwordIsValid())
            {
                requestInfo.Text = "*Invalid add user request. Please fix errors!";
                requestInfo.ForeColor = Color.Red;
            }
            //All criteria is met, add user to databse
            else
            {
                //Get role
                bool isAdmin;
                if (adminCheckbox.Checked)
                    isAdmin = true;
                else
                    isAdmin = false;
                //Insert into database
                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();
                SqlCommand addUser = new SqlCommand("insert into [User] (username, password, isAdmin) values (@username, @password, @isAdmin)", conn);
                addUser.Parameters.AddWithValue("@username", addUsername.Text);
                addUser.Parameters.AddWithValue("@password", addPassword.Text);
                addUser.Parameters.AddWithValue("@isAdmin", isAdmin);
                addUser.ExecuteNonQuery();
                //Update status and reset fields
                requestInfo.Text = "User added successfully";
                requestInfo.ForeColor = Color.Green;
                resetAddUserFields();
                conn.Close();
            }

        }

        //Purpose: Reset all reporting fields INCLUDING the add user response when reset button is clicked
        private void resetUserButton_Click(object sender, EventArgs e)
        {
            resetAddUserFields();
            requestInfo.Text = "";
        }

        private void usernameInfo_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        //Purpose: Repopulate the account list box when the refresh button is clicked
        private void refreshAccountList_Click(object sender, EventArgs e)
        {
            populateAccountList();
        }

        private void addWardStatusBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void addWardNameBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void addWardStatusBox_Leave(object sender, EventArgs e)
        {

        }

        private void addWardNameBox_Leave(object sender, EventArgs e)
        {
            wardIsValid();
        }

        private void addWardCapacityBox_Leave(object sender, EventArgs e)
        {
            capacityIsValid();
        }

        private bool wardIsValid()
        {
            string inputedWardName = addWardNameBox.Text;
            //Check username against username constraints
            if (!Regex.IsMatch(inputedWardName, @"^[a-zA-Z]+$"))
            {
                addWardInfo.Text = "*Invalid ward name characters";
                addWardInfo.ForeColor = Color.Red;
                return false;
            }
            else if (inputedWardName.Length < 2 || inputedWardName.Length > 32)
            {
                addWardInfo.Text = "*Invalid ward name length";
                addWardInfo.ForeColor = Color.Red;
                return false;
            }
            //Check to see if the username already exists
            else
            {
                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();
                SqlCommand checkUsername = new SqlCommand("select count(*) from [Ward] where wardName = @wardName", conn);
                checkUsername.Parameters.AddWithValue("@wardName", inputedWardName);
                int wardExist = (int)checkUsername.ExecuteScalar();
                //Username already exists
                if (wardExist > 0)
                {
                    addWardInfo.Text = "*Ward already exists";
                    addWardInfo.ForeColor = Color.Red;
                    return false;
                }
                //Username does not already exist
                else
                {
                    addWardInfo.Text = "*Ward name is available";
                    addWardInfo.ForeColor = Color.Green;
                    return true;
                }
            }
        }

        private bool capacityIsValid()
        {
            int capacity;
            if (Int32.TryParse(AddWardCapacityBox.Text, out capacity))
            {
                if (capacity > 0)
                {
                    addWardCapInfo.Text = "";
                    return true;
                }
                else
                {
                    addWardCapInfo.Text = "*Capacity must be greater than 0";
                    addWardCapInfo.ForeColor = Color.Red;
                    return false;
                }
            }
            else
            {
                addWardCapInfo.Text = "*Invalid capacity";
                addWardCapInfo.ForeColor = Color.Red;
                return false;
            }
        }

        private void addWardTab_Click(object sender, EventArgs e)
        {

        }

        private void addWardButt_Click(object sender, EventArgs e)
        {
            //Make sure ward name and capacity are valid
            if (!wardIsValid() || !capacityIsValid())
            {
                addWardRequestInfo.Text = "*Invalid add ward request. Please fix errors!";
                addWardRequestInfo.ForeColor = Color.Red;
            }
            //All criteria is met, add user to databse
            else
            {
                //Insert into database
                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();
                SqlCommand addWard = new SqlCommand("insert into [Ward] (wardName, overall_capacity, current_capacity) values (@wardName, @overallCap, @currentCap)", conn);
                addWard.Parameters.AddWithValue("@wardName", addWardNameBox.Text);
                addWard.Parameters.AddWithValue("@overallCap", Int32.Parse(AddWardCapacityBox.Text));
                addWard.Parameters.AddWithValue("@currentCap", 0);
                addWard.ExecuteNonQuery();
                conn.Close();
                //Update status and reset fields
                addWardRequestInfo.Text = "Ward added successfully";
                addWardRequestInfo.ForeColor = Color.Green;
                resetAddWardFields();
            }
        }

        //Purpose: Reset all reporting and input fields
        private void resetAddWardFields()
        {
            addWardNameBox.Text = "";
            AddWardCapacityBox.Text = "";
            addWardInfo.Text = "";
            addWardCapInfo.Text = "";
        }

        private void addWardReset_Click(object sender, EventArgs e)
        {
            resetAddWardFields();
            addWardRequestInfo.Text = "";
        }

        private void wardListRefresh_Click(object sender, EventArgs e)
        {
            populateWardList();
        }

        private void deleteWardButton_Click(object sender, EventArgs e)
        {
            DataRowView wardViewItem = wardListBox.SelectedItem as DataRowView;
            string wardName = wardViewItem["wardName"].ToString();

            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            SqlCommand addWard = new SqlCommand("delete from [Ward] where wardName = @wardName", conn);
            addWard.Parameters.AddWithValue("@wardName", wardName);
            addWard.ExecuteNonQuery();
            conn.Close();
            //Update status and reset fields
            populateWardList();
        }
    }
}
