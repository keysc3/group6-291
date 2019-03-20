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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            populateregList();
            FillWardBox();

        }
    private void FillWardBox()
        {
        SqlConnection conn = new SqlConnection(Globals.conn);
        conn.Open();
        DataTable Ward = new DataTable("Ward");
        SqlDataAdapter adap = new SqlDataAdapter("Select * from [Ward] where current_capacity < overall_capacity", conn);
        adap.Fill(Ward);
        foreach (DataRow items in Ward.Rows)
        {
            NewWardBox.Items.Add(items[0].ToString());
        }
        conn.Close();
        NewWardBox.SelectedIndex = -1;
        }
        private void populateregList()
        {
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("select registerID, patientSIN from [Register]", conn);
            //Fill the dataset, sort it, and bind it to the list box
            adapter.Fill(ds);
            ds.Tables[0].DefaultView.Sort = "registerID asc";
            RegListBox.DataSource = ds.Tables[0];
            RegListBox.DisplayMember = "registerID";
            conn.Close();
        }
    private void label1_Click(object sender, EventArgs e)
        {

        }

        private void RegListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SubmitNewWard_Click(object sender, EventArgs e)
        {
            DataRowView regViewItem = RegListBox.SelectedItem as DataRowView;
            string regnumber = regViewItem["registerID"].ToString();
            string newwardname = NewWardBox.GetItemText(NewWardBox.SelectedItem);

            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();

            SqlCommand getWard = new SqlCommand("select wardName from Patient_ward where registerID = @regID and dateOut is null", conn);
            getWard.Parameters.AddWithValue("@regID", Int32.Parse(regnumber));
            SqlDataReader wardReader = getWard.ExecuteReader();

            string oldWardName = "";
            while (wardReader.Read())
            {
                oldWardName = wardReader["wardName"].ToString();
            }
            wardReader.Close();
   
            SqlCommand decWard = new SqlCommand("update Ward set current_capacity = current_capacity - 1 where wardName = @oldWardName", conn);
            decWard.Parameters.AddWithValue("@oldWardName", oldWardName);
            decWard.ExecuteNonQuery();

            SqlCommand changeWard = new SqlCommand("insert into Patient_Ward (registerID, wardName, dateIn) values (@regID, @wardName, @dateIn)", conn);
            changeWard.Parameters.AddWithValue("@regID", Int32.Parse(regnumber));
            changeWard.Parameters.AddWithValue("@wardName", newwardname);
            changeWard.Parameters.AddWithValue("@dateIn", DateTime.Now);
            changeWard.ExecuteNonQuery();
            
            //Update capacity of ward patient is tranferred to
            SqlCommand updateCapacity1 = new SqlCommand("update Ward set current_capacity = current_capacity + 1 where wardName = @wardname", conn);
            updateCapacity1.Parameters.AddWithValue("@wardname", newwardname);
            updateCapacity1.ExecuteNonQuery();
            conn.Close();
        }

    }
}
