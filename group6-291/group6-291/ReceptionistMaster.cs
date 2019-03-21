﻿using System;
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
    public partial class ReceptionistMaster : Form
    {
        public ReceptionistMaster()
        {
            InitializeComponent();
        }

        private void ReceptionistMaster_Load(object sender, EventArgs e)
        {
            populateCurrentPatientBox();
        }

        private void populateCurrentPatientBox()
        {
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("select firstName, lastName, concat(firstName, ' ',lastName) as fullName, Patient.patientSIN, Register.registerID from Patient, Register where Patient.patientSIN = Register.patientSIN and leaveDate is null", conn);
            //Fill the dataset, sort it, and bind it to the list box
            adapter.Fill(ds);
            ds.Tables[0].DefaultView.Sort = "fullName asc";
            currentPatientsBox.DataSource = ds.Tables[0];
            currentPatientsBox.DisplayMember = "fullName";
            conn.Close();
        }

        private void currentPatientsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPatientDoctorsList();
            DataRowView currPatient = currentPatientsBox.SelectedItem as DataRowView;
            int regID = Int32.Parse(currPatient["registerID"].ToString());

            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();

            SqlCommand getWard = new SqlCommand("select wardName, dateIn from Patient_Ward where registerID = @regID and dateOut is null", conn);
            getWard.Parameters.AddWithValue("@regID", regID);
            SqlDataReader wardReader = getWard.ExecuteReader();
            if (wardReader.HasRows)
            {
                wardReader.Read();
                currentPatWard.Text = wardReader["wardName"].ToString();
                currentPatDateIn.Text = wardReader["dateIn"].ToString();
            }
            else
            {
                currentPatWard.Text = "N/A";
                currentPatDateIn.Text = "N/A";
            }
            conn.Close();
            currentPatientName.Text = currPatient["fullName"].ToString();
            currentPatientNameDocs.Text = currPatient["fullName"].ToString();
            populateAssignDoctor(regID);
            populateUnassignDoctor(regID);
            FillWardBox(regID);
            medicalCaseTextBox.Text = "";
            miscDetailsTextBox.Text = "";
            surgicalDetailsBox.Text = "";
        }

        private void currentWardPatientList()
        {
            if (NewWardBox.SelectedIndex < 0)
            {
                selectedWardGridView.Hide();
            }
            else
            {
                DataRowView selectedNewWard = NewWardBox.SelectedItem as DataRowView;
                string wardName = selectedNewWard["wardName"].ToString();

                DataSet patientsInWard = new DataSet();
                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();

                SqlCommand getWard = new SqlCommand("select concat(firstName, ' ', lastName) as Name from Patient, Register where Patient.patientSIN = Register.patientSIN and Register.leaveDate is null and Register.registerID in (select registerID from Patient_Ward where dateOut is null and wardName = @wardName)", conn);
                getWard.Parameters.AddWithValue("@wardName", wardName);
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = getWard;
                adapter.Fill(patientsInWard);
                selectedWardGridView.AutoGenerateColumns = true;
                selectedWardGridView.DataSource = patientsInWard.Tables[0];

                int rowCount = selectedWardGridView.RowCount;
                if (rowCount > 0)
                {
                    selectedWardGridView.Show();
                    int totalRowHeight = selectedWardGridView.ColumnHeadersHeight;
                    if (rowCount > 8)
                    {
                        totalRowHeight += (selectedWardGridView.Rows[0].Height * 8)-20;
                        selectedWardGridView.Height = totalRowHeight;
                    }
                    else
                    {
                        totalRowHeight += (selectedWardGridView.Rows[0].Height * (rowCount + 1))-20;
                        selectedWardGridView.Height = totalRowHeight;
                    }
                }
                else
                    selectedWardGridView.Hide();
            }
        }

        private void populateAssignDoctor(int regID)
        {
            assignDocBox.SelectedIndex = -1;
            assignDocBox.DataSource = null;

            DataSet unassignedDocs = new DataSet();

            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            //Fill assigned box
            SqlCommand getUnassigned = new SqlCommand("select doctorID, firstName, lastName, concat(firstName, ' ', lastName) as fullName from Doctor where doctorID not in (select doctorID from Doctor_Patient where registerID = @regID and unassignedDate is null)", conn);
            getUnassigned.Parameters.AddWithValue("@regID", regID);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = getUnassigned;
            adapter.Fill(unassignedDocs);
            unassignedDocs.Tables[0].DefaultView.Sort = "fullName asc";
            assignDocBox.DataSource = unassignedDocs.Tables[0];
            assignDocBox.DisplayMember = "fullName";

            conn.Close();
            assignDocBox.SelectedIndex = -1;
        }

        private void populateUnassignDoctor(int regID)
        {
            unassignDocBox.SelectedIndex = -1;
            unassignDocBox.DataSource = null;

            DataSet assignedDocs = new DataSet();

            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            //Fill unassigned box
            SqlCommand getAssigned = new SqlCommand("select doctorID, firstName, lastName, concat(firstName, ' ', lastName) as fullName from Doctor where doctorID in (select doctorID from Doctor_Patient where registerID = @regID and unassignedDate is null)", conn);
            getAssigned.Parameters.AddWithValue("@regID", regID);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = getAssigned;
            adapter.Fill(assignedDocs);
            assignedDocs.Tables[0].DefaultView.Sort = "fullName asc";
            unassignDocBox.DataSource = assignedDocs.Tables[0];
            unassignDocBox.DisplayMember = "fullName";
            conn.Close();
            unassignDocBox.SelectedIndex = -1;
        }

        private void unassignDocButton_Click(object sender, EventArgs e)
        {
            if (unassignDocBox.SelectedIndex < 0)
            {
                unassignErrorLabel.Text = "*Invalid doctor to unassign";
                unassignErrorLabel.ForeColor = Color.Red;
                manageDocSuccess.Text = "";
            }
            else
            {
                DataRowView selectedUnassignDoc = unassignDocBox.SelectedItem as DataRowView;
                int doctorID = Int32.Parse(selectedUnassignDoc["doctorID"].ToString());

                DataRowView currPatient = currentPatientsBox.SelectedItem as DataRowView;
                int regID = Int32.Parse(currPatient["registerID"].ToString());

                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();
                SqlCommand unassignDoc = new SqlCommand("update Doctor_Patient set unassignedDate = @unassignedDate where doctorID = @doctorID and registerID = @registerID and unassignedDate is null", conn);
                unassignDoc.Parameters.AddWithValue("@unassignedDate", DateTime.Now);
                unassignDoc.Parameters.AddWithValue("@doctorID", doctorID);
                unassignDoc.Parameters.AddWithValue("@registerID", regID);
                unassignDoc.ExecuteNonQuery();
                conn.Close();
                manageDocSuccess.Text = "Doctor unassigned successfully";
                manageDocSuccess.ForeColor = Color.Green;
                medicalErrorLabel.Text = "";
                unassignErrorLabel.Text = "";
                currentPatientsBox_SelectedIndexChanged(sender, e);
            }
        }

        private void assignDocButton_Click(object sender, EventArgs e)
        {
            string medicalCase = medicalCaseTextBox.Text;

            if (medicalCase.Length < 0 || !Regex.IsMatch(medicalCase, @"^[a-zA-Z0-9]+$"))
            {
                medicalErrorLabel.Text = "*Invalid medical case";
                medicalErrorLabel.ForeColor = Color.Red;
                manageDocSuccess.Text = "";
            }
            else
            {
                DataRowView selectedAssignDoc = assignDocBox.SelectedItem as DataRowView;
                int doctorID = Int32.Parse(selectedAssignDoc["doctorID"].ToString());

                DataRowView currPatient = currentPatientsBox.SelectedItem as DataRowView;
                int regID = Int32.Parse(currPatient["registerID"].ToString());

                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();
                SqlCommand assignDoc = new SqlCommand("insert into Doctor_Patient (registerID, doctorID, assignedDate, medicalCase, surgicalDetails, miscDetails) values (@registerID, @doctorID, @assignedDate, @medicalCase, @surgicalDetails, @miscDetails)", conn);
                assignDoc.Parameters.AddWithValue("@assignedDate", DateTime.Now);
                assignDoc.Parameters.AddWithValue("@doctorID", doctorID);
                assignDoc.Parameters.AddWithValue("@registerID", regID);
                assignDoc.Parameters.AddWithValue("@medicalCase", medicalCase);
                if (miscDetailsTextBox.Text.Length < 1)
                    assignDoc.Parameters.AddWithValue("@miscDetails", DBNull.Value);
                else
                    assignDoc.Parameters.AddWithValue("@miscDetails", miscDetailsTextBox.Text);

                if (surgicalDetailsBox.Text.Length < 1)
                    assignDoc.Parameters.AddWithValue("@surgicalDetails", DBNull.Value);
                else
                    assignDoc.Parameters.AddWithValue("@surgicalDetails", surgicalDetailsBox.Text);

                assignDoc.ExecuteNonQuery();
                conn.Close();
                manageDocSuccess.Text = "Doctor assigned successfully";
                manageDocSuccess.ForeColor = Color.Green;
                medicalErrorLabel.Text = "";
                unassignErrorLabel.Text = "";
                currentPatientsBox_SelectedIndexChanged(sender, e);
            }
        }

        private void releaseButton_Click(object sender, EventArgs e)
        {
            DataRowView currPatient = currentPatientsBox.SelectedItem as DataRowView;
            int regID = Int32.Parse(currPatient["registerID"].ToString());

            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            SqlCommand releasePatient = new SqlCommand("update Register set leaveDate = @leaveDate where registerID = @registerID", conn);
            releasePatient.Parameters.AddWithValue("@leaveDate", DateTime.Now);
            releasePatient.Parameters.AddWithValue("@registerID", regID);
            releasePatient.ExecuteNonQuery();
            conn.Close();
            populateCurrentPatientBox();
        }

        private void FillWardBox(int regID)
        {
            NewWardBox.SelectedIndex = -1;
            NewWardBox.DataSource = null;

            DataSet ward = new DataSet();

            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            SqlCommand getAvailWards = new SqlCommand("Select * from [Ward] where current_capacity < overall_capacity and wardName not in (select wardName from Patient_Ward where registerID = @regID and dateOut is null)", conn);
            SqlDataAdapter adap = new SqlDataAdapter();
            getAvailWards.Parameters.AddWithValue("@regID", regID);
            adap.SelectCommand = getAvailWards; 
            adap.Fill(ward);
            ward.Tables[0].DefaultView.Sort = "wardName asc";
            NewWardBox.DataSource = ward.Tables[0];
            NewWardBox.DisplayMember = "wardName";
            conn.Close();
            NewWardBox.SelectedIndex = -1;
        }

        private void SubmitNewWard_Click(object sender, EventArgs e)
        {
            if (NewWardBox.SelectedIndex > -1)
            {
                DataRowView currPatient = currentPatientsBox.SelectedItem as DataRowView;
                DataRowView selectedWard = NewWardBox.SelectedItem as DataRowView;
                int regID = Int32.Parse(currPatient["registerID"].ToString());
                string newWardName = selectedWard["wardName"].ToString();

                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();

                SqlCommand getWard = new SqlCommand("select wardName from Patient_Ward where registerID = @regID and dateOut is null", conn);
                getWard.Parameters.AddWithValue("@regID", regID);
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
                changeWard.Parameters.AddWithValue("@regID", regID);
                changeWard.Parameters.AddWithValue("@wardName", newWardName);
                changeWard.Parameters.AddWithValue("@dateIn", DateTime.Now);
                changeWard.ExecuteNonQuery();

                //Update capacity of ward patient is tranferred to
                SqlCommand updateCapacity1 = new SqlCommand("update Ward set current_capacity = current_capacity + 1 where wardName = @wardname", conn);
                updateCapacity1.Parameters.AddWithValue("@wardname", newWardName);
                updateCapacity1.ExecuteNonQuery();

                //Update dateout of ward patient is was in
                SqlCommand updateDateOut = new SqlCommand("update Patient_Ward set dateOut = @dateOut where registerID = @regID and wardName = @oldWardName", conn);
                updateDateOut.Parameters.AddWithValue("@oldWardname", oldWardName);
                updateDateOut.Parameters.AddWithValue("@regID", regID);
                updateDateOut.Parameters.AddWithValue("@dateOut", DateTime.Now);
                updateDateOut.ExecuteNonQuery();
                conn.Close();


                wardSuccess.Text = "Ward assigned successfully";
                wardSuccess.ForeColor = Color.Green;
                wardErrorLabel.Text = "";
                currentPatientsBox_SelectedIndexChanged(sender, e);
            }
            else
            {
                wardErrorLabel.Text = "*Select a ward";
                wardErrorLabel.ForeColor = Color.Red;
                wardSuccess.Text = "";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void NewWardBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentWardPatientList();
            if(NewWardBox.SelectedIndex > -1)
            {
                DataRowView selectedNewWard = NewWardBox.SelectedItem as DataRowView;
                currentSelectedWard.Text = selectedNewWard["wardName"].ToString();
            }
            else
                currentSelectedWard.Text = "N/A";
        }

        private void currentPatientDoctorsList()
        {
            DataRowView currPatient = currentPatientsBox.SelectedItem as DataRowView;
            int regID = Int32.Parse(currPatient["registerID"].ToString());

            DataSet patientDoctors = new DataSet();
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();

            SqlCommand getWard = new SqlCommand("select concat(Doctor.firstName, ' ', Doctor.lastName)as Name, medicalCase as 'Medical Case', surgicalDetails as 'Surgical Details', miscDetails as 'Misc Details' from Doctor, Doctor_Patient where Doctor.doctorID = Doctor_Patient.doctorID and unassignedDate is null and registerID = @regID", conn);
            getWard.Parameters.AddWithValue("@regID", regID);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = getWard;
            adapter.Fill(patientDoctors);
            currentDoctorsGridView.AutoGenerateColumns = true;
            currentDoctorsGridView.DataSource = patientDoctors.Tables[0];

            int rowCount = currentDoctorsGridView.RowCount;
            if (rowCount > 0)
            {
                noCurrentDocs.Hide();
                currentDoctorsGridView.Show();
                int totalRowHeight = selectedWardGridView.ColumnHeadersHeight;
                if (rowCount > 8)
                {
                    totalRowHeight += (currentDoctorsGridView.Rows[1].Height * 8)+8;
                    currentDoctorsGridView.Height = totalRowHeight;
                }
                else
                {
                    totalRowHeight += (currentDoctorsGridView.Rows[0].Height * (rowCount + 1))+8;
                    currentDoctorsGridView.Height = totalRowHeight;
                }
            }
            else
            {
                currentDoctorsGridView.Hide();
                noCurrentDocs.Show();
            }
        }
        }
}