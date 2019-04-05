using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
        Form1 loginForm;
        DataSet patientList = new DataSet();
        DataSet patientRecordList = new DataSet();
        DataSet currPatientList = new DataSet();
        DataSet wardList = new DataSet();
        DataSet doctorList = new DataSet();
        public ReceptionistMaster(Form1 login)
        {
            InitializeComponent();
            loginForm = login;
        }

        private void ReceptionistMaster_Load(object sender, EventArgs e)
        {
            populateCurrentPatientBox();
            populateWardList();
            populateDoctorList();
            registerListBox.DoubleClick += new EventHandler(registerListBox_DoubleClick);
            currentPatientsBox.DoubleClick += new EventHandler(currentPatientsBox_DoubleClick);
            PatientDoctorGrid.SelectionChanged += new EventHandler(PatientDoctorGrid_SelectionChanged);
            populatePatientList();
        }

        private void populateCurrentPatientBox()
        {
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            DataSet ds = new DataSet();
            currPatientList = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("select Patient.*, lastName, concat(firstName, ' ',lastName) as fullName, Patient.patientSIN, Register.registerID from Patient, Register where Patient.patientSIN = Register.patientSIN and leaveDate is null", conn);
            //Fill the dataset, sort it, and bind it to the list box
            adapter.Fill(currPatientList);
            adapter.Fill(ds);
            ds.Tables[0].DefaultView.Sort = "fullName asc";
            currentPatientsBox.DataSource = ds.Tables[0];
            currentPatientsBox.DisplayMember = "fullName";
            conn.Close();
        }
        private void populateWardList()
        {
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("select * from Ward", conn);
            //Fill the dataset, sort it, and bind it to the list box
            wardList = new DataSet();
            adapter.Fill(wardList);
            adapter.Fill(ds);
            ds.Tables[0].DefaultView.Sort = "wardName asc";
            WardListBox.DataSource = ds.Tables[0];
            WardListBox.DisplayMember = "wardName";
            conn.Close();
        }
        private void populateDoctorList()
        {
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            DataSet ds = new DataSet();
            doctorList = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter("select concat(firstName, ' ', lastName) as Name, Doctor.* from [Doctor]", conn);
            //Fill the dataset, sort it, and bind it to the list box
            adapter.Fill(doctorList);
            adapter.Fill(ds);
            ds.Tables[0].DefaultView.Sort = "Name asc";
            DoctorListBox.DataSource = ds.Tables[0];
            DoctorListBox.DisplayMember = "Name";
            conn.Close();
        }
        private void currentPatientsBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentPatientsInfo();
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
            else if (assignDocBox.SelectedIndex == -1)
            {
                docErrorLabel.Text = "*Please select a doctor";
                docErrorLabel.ForeColor = Color.Red;
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
                docErrorLabel.Text = "";
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
                    totalRowHeight += (currentDoctorsGridView.Rows[1].Height * 8)+10;
                    currentDoctorsGridView.Height = totalRowHeight;
                }
                else
                {
                    totalRowHeight += (currentDoctorsGridView.Rows[0].Height * (rowCount + 1))+10;
                    currentDoctorsGridView.Height = totalRowHeight;
                }
            }
            else
            {
                currentDoctorsGridView.Hide();
                noCurrentDocs.Show();
            }
        }
        private void currentPatientsBox_DoubleClick(object sender, EventArgs e)
        {

        }
        private void currentPatientsInfo()
        {
            if (tabControl2.SelectedTab == tabControl2.TabPages["updatePatientInfo"])
            {
                DataRowView currPatient = currentPatientsBox.SelectedItem as DataRowView;
                int regID = Int32.Parse(currPatient["registerID"].ToString());
                if (currentPatientsBox.SelectedItem != null)
                {
                    DataRowView currentPatient = currentPatientsBox.SelectedItem as DataRowView;
                    int patientType = Int32.Parse(currentPatient["patientType"].ToString());
                    updateSINBox.Text = currentPatient["patientSIN"].ToString();
                    if (patientType == 0)
                        updatePatientTypeBox.SelectedIndex = 0;
                    else if (patientType == 1)
                        updatePatientTypeBox.SelectedIndex = 1;
                    else
                        updatePatientTypeBox.SelectedIndex = 2;

                    updateFirstNameBox.Text = currentPatient["firstName"].ToString();
                    updateLastNameBox.Text = currentPatient["lastName"].ToString();
                    updateStreetBox.Text = currentPatient["street"].ToString();
                    updateCityBox.Text = currentPatient["city"].ToString();
                    updateProvinceBox.Text = currentPatient["province"].ToString();
                    updateCountryBox.Text = currentPatient["country"].ToString();
                    if (currentPatient["sex"].ToString().Equals("Male"))
                        updateGenderBox.SelectedIndex = 0;
                    else
                        updateGenderBox.SelectedIndex = 1;
                    updateDOBBox.Text = Convert.ToDateTime(currentPatient["dateOfBirth"]).ToString("MM/dd/yyyy");
                    updateHomePhoneBox.Text = "";
                    updateCellphoneBox.Text = "";
                }
            }
        }

        private void updatePatientButton_Click(object sender, EventArgs e)
        {
            //Clear error/success info everytime add button is clicked
            updatePatientErrorInfo.Text = "";
            updatePatientRequestInfo.Text = "";
            DataRowView currentPatient = currentPatientsBox.SelectedItem as DataRowView;

            //If all criteria for every field is met, add user to databse
            if (fieldsAreValid("update"))
            {
                //Insert into database
                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();
                SqlCommand updatePatient = new SqlCommand("update Patient set patientSIN = @patientSIN, patientType = @patientType, firstName = @firstName, lastName = @lastName, street = @street, city = @city, province = @province, country = @country, sex = @sex, dateOfBirth = @dateOfBirth where patientSIN = @oldSIN", conn);
                updatePatient.Parameters.AddWithValue("@patientSIN", updateSINBox.Text);
                updatePatient.Parameters.AddWithValue("@patientType", Int32.Parse(updatePatientTypeBox.Text));
                updatePatient.Parameters.AddWithValue("@firstName", updateFirstNameBox.Text);
                updatePatient.Parameters.AddWithValue("@lastName", updateLastNameBox.Text);
                updatePatient.Parameters.AddWithValue("@street", updateStreetBox.Text);
                updatePatient.Parameters.AddWithValue("@city", updateCityBox.Text);
                updatePatient.Parameters.AddWithValue("@province", updateProvinceBox.Text);
                updatePatient.Parameters.AddWithValue("@country", updateCountryBox.Text);
                updatePatient.Parameters.AddWithValue("@sex", updateGenderBox.Text);
                updatePatient.Parameters.AddWithValue("@dateOfBirth", updateDOBBox.Text);
                updatePatient.Parameters.AddWithValue("@oldSIN", Int32.Parse(currentPatient["patientSIN"].ToString()));
                updatePatient.ExecuteNonQuery();

                conn.Close();
                //Update status and reset fields
                updatePatientRequestInfo.Text = "Patient updated successfully";
                updatePatientRequestInfo.ForeColor = Color.Green;
                resetAddUpdatePatientFields("update");
            }
        }

        private void resetUpdatePatientButton_Click(object sender, EventArgs e)
        {
            resetAddUpdatePatientFields("update");
            updatePatientErrorInfo.Text = "";
            updatePatientRequestInfo.Text = "";
        }

        private void resetAddUpdatePatientFields(string type)
        {
            if (type.Equals("update"))
            {
                updateSINBox.Text = "";
                updatePatientTypeBox.SelectedIndex = -1;
                updateFirstNameBox.Text = "";
                updateLastNameBox.Text = "";
                updateStreetBox.Text = "";
                updateCityBox.Text = "";
                updateProvinceBox.Text = "";
                updateCountryBox.Text = "";
                updateGenderBox.SelectedIndex = -1;
                updateDOBBox.Text = "";
                updateHomePhoneBox.Text = "";
                updateCellphoneBox.Text = "";
                populateCurrentPatientBox();
            }
            else
            {
                addSINBox.Text = "";
                addPatientTypeBox.SelectedIndex = -1;
                addFirstNameBox.Text = "";
                addLastNameBox.Text = "";
                addStreetBox.Text = "";
                addCityBox.Text = "";
                addProvinceBox.Text = "";
                addCountryBox.Text = "";
                addGenderBox.SelectedIndex = -1;
                addDOBBox.Text = "";
                addHomePhoneBox.Text = "";
                addCellphoneBox.Text = "";
            }
        }

        //Check all fields after add button is pressed. If there are any invalid fields, points them out.
        private bool fieldsAreValid(string inputeType)
        {
            string inputedSIN;
            string inputedFName;
            string inputedLName;
            string inputedStreet;
            string inputedCity;
            string inputedProvince;
            string inputedCountry;
            string inputedPType;
            string inputedGender;

            if (inputeType.Equals("update"))
            {
                inputedSIN = updateSINBox.Text;
                inputedFName = updateFirstNameBox.Text;
                inputedLName = updateLastNameBox.Text;
                inputedStreet = updateStreetBox.Text;
                inputedCity = updateCityBox.Text;
                inputedProvince = updateProvinceBox.Text;
                inputedCountry = updateCountryBox.Text;
                inputedPType = updatePatientTypeBox.SelectedText;
                inputedGender = updateGenderBox.SelectedText;
                updatePatientErrorInfo.ForeColor = Color.Red;
            }
            else
            {
                inputedSIN = addSINBox.Text;
                inputedFName = addFirstNameBox.Text;
                inputedLName = addLastNameBox.Text;
                inputedStreet = addStreetBox.Text;
                inputedCity = addCityBox.Text;
                inputedProvince = addProvinceBox.Text;
                inputedCountry = addCountryBox.Text;
                inputedPType = addPatientTypeBox.SelectedText;
                inputedGender = addGenderBox.SelectedText;
                addRegisterInfo.ForeColor = Color.Red;
            }

            string errorInfo = "";

            //Check SIN against SIN constraints (all num, length = 9)
            if (!Regex.IsMatch(inputedSIN, @"^[0-9]+$") | inputedSIN.Length != 9)
            {
                errorInfo = errorInfo + "*SIN must be 9 numbers\n";
            }
            //Check First Name
            if (!Regex.IsMatch(inputedFName, @"^[a-zA-Z]+$") | inputedFName.Length > 32)
            {
                errorInfo = errorInfo + "*First Name must be between 0 and 32 letters long.\n";
            }
            //Check Last Name
            if (!Regex.IsMatch(inputedLName, @"^[a-zA-Z]+$") | inputedLName.Length > 32)
            {
                errorInfo = errorInfo + "*Last Name must be between 0 and 32 letters long.\n";
            }
            //Check Street (Allow for numbers and white space)
            if (!Regex.IsMatch(inputedStreet, @"^[a-zA-Z0-9\s]+$") | inputedStreet.Length > 50)
            {
                errorInfo = errorInfo + "*Street must be between 0 and 50 characters long.\n";
            }
            //Check City (Allow whitespace)
            if (!Regex.IsMatch(inputedCity, @"^[a-zA-Z\s]+$") | inputedCity.Length > 50)
            {
                errorInfo = errorInfo + "*City must be between 0 and 50 characters long.\n";
            }
            //Check Province (Allow whitespace)
            if (!Regex.IsMatch(inputedProvince, @"^[a-zA-Z\s]+$") | inputedProvince.Length > 50)
            {
                errorInfo = errorInfo + "*Province must be between 0 and 50 characters long.\n";
            }
            //Check Country (Allow whitespace)
            if (!Regex.IsMatch(inputedCountry, @"^[a-zA-Z\s]+$") | inputedCountry.Length > 50)
            {
                errorInfo = errorInfo + "*Country must be between 0 and 50 characters long.\n";
            }
            //Check DOB, Admit Date, and Depart Date: 1. Valid date, 1.5 Depart is completely empty or full, 2. DOB < Admit < Depart
            errorInfo = dateIsValid(inputeType, errorInfo);
            //Check Patient Type, error if nothing chosen
            if (inputeType.Equals("update"))
            {
                if (updatePatientTypeBox.SelectedItem == null)
                {
                    errorInfo = errorInfo + "*Please choose a Patient Type.\n";
                }
            }
            else
            {
                if (addPatientTypeBox.SelectedItem == null)
                {
                    errorInfo = errorInfo + "*Please choose a Patient Type.\n";
                }
            }
            //Check Gender, error if nothing chosen
            if (inputeType.Equals("update"))
            {
                if (updateGenderBox.SelectedItem == null)
                {
                    errorInfo = errorInfo + "*Please choose a Gender.\n";
                }
            }
            else
            {
                if (addGenderBox.SelectedItem == null)
                {
                    errorInfo = errorInfo + "*Please choose a Gender.\n";
                }
            }
            if (inputeType.Equals("update"))
            {
                updatePatientErrorInfo.Text = errorInfo;
            }
            else
            {
                addRegisterInfo.Text = errorInfo;
            }
            //If there are any warnings, return false.
            if (errorInfo.Length > 0) { return false; }
            else { return true; }
        }

        private string dateIsValid(string inputType, string errorInfo)
        {
            DateTime inputedDOB;
            if (inputType.Equals("update")){
                //Check if entry is empty or incorrect
                if (!updateDOBBox.MaskCompleted | !DateTime.TryParse(updateDOBBox.Text, out inputedDOB))
                {
                    errorInfo = errorInfo + "*Invalid Date of Birth.\n";
                }
            }
            else
            {
                if (!addDOBBox.MaskCompleted | !DateTime.TryParse(addDOBBox.Text, out inputedDOB))
                {
                    errorInfo = errorInfo + "*Invalid Date of Birth.\n";
                }
            }
            return errorInfo;

        }

        private void logoutRecep_Click(object sender, EventArgs e)
        {
            loginForm.Show();
            this.Close();
        }

        private void currentDoctorsGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void WardListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView WardList = WardListBox.SelectedItem as DataRowView;
            string wardName = WardList["wardName"].ToString();

            DataSet patientsInWard = new DataSet();
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();

            SqlCommand getWard = new SqlCommand("select concat(firstName, ' ', lastName) as Name, sex as Sex, dateOfBirth as Date_of_Birth," +
                " patientType as Patient_Type, city as City from Patient, Register" +
                " where Patient.patientSIN = Register.patientSIN and Register.leaveDate is null and Register.registerID in" +
                " (select registerID from Patient_Ward where dateOut is null and wardName = @wardName)", conn);

            getWard.Parameters.AddWithValue("@wardName", wardName);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = getWard;
            adapter.Fill(patientsInWard);
            WardListGrid.AutoGenerateColumns = true;
            WardListGrid.DataSource = patientsInWard.Tables[0];

            int rowCount = WardListGrid.RowCount;
            if (rowCount > 0)
            {
                int totalRowHeight = WardListGrid.ColumnHeadersHeight;
                if (rowCount > 8)
                {
                    totalRowHeight += (WardListGrid.Rows[0].Height * 8) - 20;
                    WardListGrid.Height = totalRowHeight;
                }
                else
                {
                    totalRowHeight += (WardListGrid.Rows[0].Height * (rowCount + 1)) - 20;
                    WardListGrid.Height = totalRowHeight;
                }
            }

            int max = Int32.Parse(WardList["overall_capacity"].ToString());
            int cur = Int32.Parse(WardList["current_capacity"].ToString());
            //Send these to your WinForms textboxes
            MaxCapacityLabel.Text = WardList["overall_capacity"].ToString();
            CurrentPatientLabel.Text = WardList["current_capacity"].ToString();

            if (cur == max)
            {
                StatusLabel.Text = "Full";
                StatusLabel.ForeColor = Color.Red;
            }
            else
            {
                StatusLabel.Text = "Not Full";
                StatusLabel.ForeColor = Color.Green;
            }
        }

        private void DoctorListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DocClearReset();
            PatientGrid.Hide();
            DataRowView selectedDoctor = DoctorListBox.SelectedItem as DataRowView;
            doctorName.Text = selectedDoctor["Name"].ToString();
            deptName.Text = selectedDoctor["departmentName"].ToString();
            specName.Text = selectedDoctor["specialization"].ToString();
            if(selectedDoctor["Name"].ToString().Length > 0)
                docDuties.Text = selectedDoctor["duties"].ToString();


            getDoctorPatient();
        }


        private void getDoctorPatient()
        {
            DataRowView DoctorList = DoctorListBox.SelectedItem as DataRowView;
            string Name = DoctorList["Name"].ToString();
            string ID = DoctorList["doctorID"].ToString();

            DataSet Doctor_Patient = new DataSet();
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();

            //query needs work for current patients, currenty only shows all patients for that doctor
            SqlCommand getPatient = new SqlCommand("select patientSIN, concat(firstName, ' ', lastName) as Name, sex as Sex, dateOfBirth as Date_of_Birth," +
                " patientType as Patient_Type from Patient where patientSIN in" +
                " (SELECT patientSIN FROM Register WHERE registerID IN" +
                " (SELECT registerID FROM Doctor_Patient where doctorID = @doctorID))", conn);

            getPatient.Parameters.AddWithValue("@doctorID", ID);
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = getPatient;
            adapter.Fill(Doctor_Patient);
            PatientGrid.AutoGenerateColumns = true;
            PatientGrid.DataSource = Doctor_Patient.Tables[0];
            PatientGrid.Columns[0].Visible = false;
            int rowCount = PatientGrid.RowCount;
            if (rowCount > 0)
            {
                PatientGrid.Show();
                int totalRowHeight = PatientGrid.ColumnHeadersHeight;
                if (rowCount > 8)
                {
                    totalRowHeight += (PatientGrid.Rows[0].Height * 8) - 20;
                    PatientGrid.Height = totalRowHeight;
                }
                else
                {
                    totalRowHeight += (PatientGrid.Rows[0].Height * (rowCount + 1)) - 20;
                    PatientGrid.Height = totalRowHeight;
                }
            }
            conn.Close();

        }

        private void addRegisterButton_Click(object sender, EventArgs e)
        {
            //Clear error/success info everytime add button is clicked
            addRegisterInfo.Text = "";
            addRegisterRequestInfo.Text = "";

            //If all criteria for every field is met, add user to databse
            if (fieldsAreValid("add"))
            {
                //Insert into database
                SqlConnection conn = new SqlConnection(Globals.conn);
                //conn.Open();

                //Check if patient already exists in Patient Table
                if (patientIsValid())
                {
                    conn.Open();
                    SqlCommand addPatient = new SqlCommand(@"INSERT into [Patient] (patientSIN, patientType, firstName, lastName, street, city, province, country, sex, dateOfBirth) 
                                                        VALUES (@SIN, @pType, @fName, @lName, @street, @city, @province, @country, @sex, @dateOfBirth)", conn);
                    addPatient.Parameters.AddWithValue("@SIN", addSINBox.Text);
                    addPatient.Parameters.AddWithValue("@pType", Int32.Parse(addPatientTypeBox.Text));
                    addPatient.Parameters.AddWithValue("@fName", addFirstNameBox.Text);
                    addPatient.Parameters.AddWithValue("@lName", addLastNameBox.Text);
                    addPatient.Parameters.AddWithValue("@street", addStreetBox.Text);
                    addPatient.Parameters.AddWithValue("@city", addCityBox.Text);
                    addPatient.Parameters.AddWithValue("@province", addProvinceBox.Text);
                    addPatient.Parameters.AddWithValue("@country", addCountryBox.Text);
                    addPatient.Parameters.AddWithValue("@sex", addGenderBox.Text);
                    addPatient.Parameters.AddWithValue("@dateOfBirth", addDOBBox.Text);
                    addPatient.ExecuteNonQuery();
                    conn.Close();
                }

                if (checkRegister())
                {
                    conn.Open();
                    SqlCommand addRegister = new SqlCommand(@"INSERT into [Register] (patientSIN, admitDate, notes) VALUES (@SIN, @admitDate, @notes)", conn);
                    addRegister.Parameters.AddWithValue("@SIN", addSINBox.Text);
                    addRegister.Parameters.AddWithValue("@admitDate", DateTime.Now);

                    //Check if notes are null
                    if (addNotesBox.TextLength == 0) { addRegister.Parameters.AddWithValue("@notes", DBNull.Value); }
                    else { addRegister.Parameters.AddWithValue("@notes", addNotesBox.Text); }

                    addRegister.ExecuteNonQuery();
                    conn.Close();
                    //Update status and reset fields
                    addRegisterRequestInfo.Text = "Patient registered successfully";
                    addRegisterRequestInfo.ForeColor = Color.Green;
                    resetAddUpdatePatientFields("Add");
                    populatePatientList();
                }
            }
        }

        private bool checkRegister()
        {
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            string SIN = addSINBox.Text;
            //checks if the patient is still registered
            SqlCommand checkSIN = new SqlCommand("select count(*) from [Register] where patientSIN = @patientSIN and leaveDate is null", conn);
            checkSIN.Parameters.AddWithValue("@patientSin", SIN);
            int SINExist = (int)checkSIN.ExecuteScalar();
            //Record (SIN) already exists
            if (SINExist > 0)
            {
                addRegisterInfo.Text = "*Patient is already registered,\n no new record was created.";
                addRegisterInfo.ForeColor = Color.Red;
                conn.Close();
                return false;
            }
            conn.Close();
            return true;
        }

        //For Patient Table (no dupes): If SIN already exists, do not add to Patient Table.
        private bool patientIsValid()
        {
            string inputedSIN = addSINBox.Text;
            //Check to see if the patient record already exists in Patient Table
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            SqlCommand checkSIN = new SqlCommand("select count(*) from [Patient] where patientSin = @patientSin", conn);
            checkSIN.Parameters.AddWithValue("@patientSin", inputedSIN);
            int SINExist = (int)checkSIN.ExecuteScalar();
            //Record (SIN) already exists
            if (SINExist > 0)
            {
                addRegisterInfo.Text = "*SIN already exists, no new record was created.";
                addRegisterInfo.ForeColor = Color.Red;
                return false;
            }
            else { return true; }
        }

        private void resetRegisterButton_Click(object sender, EventArgs e)
        {
            resetAddUpdatePatientFields("add");
            registerListBox.DataSource = patientList.Tables[0];
            populatePatientList();
            addRegisterInfo.Text = "";
            addRegisterRequestInfo.Text = "";
        }

        private void populatePatientList()
        {
            patientList = new DataSet();
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT *, CONCAT(lastName, ', ', firstName) as fullName FROM [Patient]", conn);
            //Fill the dataset, sort it, and bind it to the list box
            patientRecordList = new DataSet();
            adapter.Fill(patientRecordList);
            adapter.Fill(patientList);
            patientList.Tables[0].DefaultView.Sort = "fullName asc";
            registerListBox.DataSource = patientList.Tables[0];
            registerListBox.DisplayMember = "fullName";
            patientRecordBox.DataSource = patientList.Tables[0];
            patientRecordBox.DisplayMember = "fullName";
            conn.Close();
        }

        private void registerListBox_DoubleClick(object sender, EventArgs e)
        {
            if (registerListBox.SelectedItem != null && addSINBox.Text.Length == 9)
            {
                DataRowView registrantList = registerListBox.SelectedItem as DataRowView;
                int patientType = Int32.Parse(registrantList["patientType"].ToString());
                addSINBox.Text = registrantList["patientSIN"].ToString();
                if (patientType == 0)
                    addPatientTypeBox.SelectedIndex = 0;
                else if (patientType == 1)
                    addPatientTypeBox.SelectedIndex = 1;
                else
                    addPatientTypeBox.SelectedIndex = 2;

                addFirstNameBox.Text = registrantList["firstName"].ToString();
                addLastNameBox.Text = registrantList["lastName"].ToString();
                addStreetBox.Text = registrantList["street"].ToString();
                addCityBox.Text = registrantList["city"].ToString();
                addProvinceBox.Text = registrantList["province"].ToString();
                addCountryBox.Text = registrantList["country"].ToString();
                if (registrantList["sex"].ToString().Equals("Male"))
                    addGenderBox.SelectedIndex = 0;
                else
                    addGenderBox.SelectedIndex = 1;
                addDOBBox.Text = Convert.ToDateTime(registrantList["dateOfBirth"]).ToString("MM/dd/yyyy");
                //addAdmitDateBox.Text = "";
                //addDepartDateBox.Text = "";
                addInsuranceBox.Text = "";
                addHomePhoneBox.Text = "";
                addCellphoneBox.Text = "";
                addNotesBox.Text = "";
            }
        }

        private void addSINBox_TextChanged(object sender, EventArgs e)
        {
            if (addSINBox.Text.Length == 9)
            {
                DataSet patientSINs = new DataSet();
                DataTable matchingSINs = patientList.Tables[0].Clone();
                foreach (DataRow row in patientList.Tables[0].Rows)
                {
                    if (row["patientSIN"].ToString().StartsWith(addSINBox.Text))
                        matchingSINs.ImportRow(row);
                }
                patientSINs.Tables.Add(matchingSINs);
                patientSINs.Tables[0].DefaultView.Sort = "fullName asc";
                registerListBox.DataSource = patientSINs.Tables[0];
                registerListBox.DisplayMember = "fullName";
            }
            if (addSINBox.Text.Length == 0)
                registerListBox.DataSource = patientList.Tables[0];
        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void patientRecordBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView selectedPatient = registerListBox.SelectedItem as DataRowView;
            string name = selectedPatient["fullName"].ToString();
            string address = selectedPatient["street"].ToString() + ", " + selectedPatient["city"].ToString() 
                + ", " + selectedPatient["province"].ToString() + ", " + selectedPatient["country"].ToString();
            string gender = selectedPatient["sex"].ToString();
            string DOB = selectedPatient["dateOfBirth"].ToString();
            patientName.Text = name;
            patientAddress.Text = address;
            patientGender.Text = gender;
            patientDOB.Text = DOB;
            populatePatientRegsList();

        }

        private void populatePatientRegsList()
        {
            
            DataSet regNums = new DataSet();
            DataRowView selectedPatient = registerListBox.SelectedItem as DataRowView;
            //Open connection and create a dataset from the query
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            
            SqlCommand getRegs = new SqlCommand("SELECT * FROM [Register] where patientSIN = @patientSIN", conn);
            getRegs.Parameters.AddWithValue("@patientSIN", selectedPatient["patientSIN"].ToString());
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = getRegs;
            //Fill the dataset, sort it, and bind it to the list box
            adapter.Fill(regNums);
            regNums.Tables[0].DefaultView.Sort = "admitDate asc";
            selectedPatientRegs.DataSource = regNums.Tables[0];
            selectedPatientRegs.DisplayMember = "admitDate";
            conn.Close();
        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void selectedPatientRegs_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView WardList = selectedPatientRegs.SelectedItem as DataRowView;
            string registerID = WardList["registerID"].ToString();

            DataSet patientsInWard = new DataSet();
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();

            SqlCommand getWard = new SqlCommand("Select wardName, dateIn, dateOut from Patient_Ward" +
                " where registerID = @registerID", conn);
            getWard.Parameters.AddWithValue("@registerID", Int32.Parse(registerID));
            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = getWard;
            adapter.Fill(patientsInWard);
            PatientWardGrid.AutoGenerateColumns = true;
            PatientWardGrid.DataSource = patientsInWard.Tables[0];

            int rowCount = PatientWardGrid.RowCount;
            if (rowCount > 0)
            {
                int totalRowHeight = PatientWardGrid.ColumnHeadersHeight;
                if (rowCount > 4)
                {
                    totalRowHeight += (PatientWardGrid.Rows[0].Height * 4);
                    PatientWardGrid.Height = totalRowHeight;
                }
                else
                {
                    totalRowHeight += (PatientWardGrid.Rows[0].Height * (rowCount + 1));
                    PatientWardGrid.Height = totalRowHeight;
                }
            }

            patientDoctorList();
            conn.Close();

        }

        private void patientDoctorList()
        {
            DataRowView WardList = selectedPatientRegs.SelectedItem as DataRowView;
            string registerID = WardList["registerID"].ToString();
            DataSet patientsDoctor = new DataSet();
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            SqlCommand getDoctor = new SqlCommand("Select doctorID, concat(firstName, ' ',lastName) as fullName, departmentName, specialization, duties" +
                " from Doctor where doctorID in" +
                " (Select doctorID from Doctor_Patient where registerID= @registerID)", conn);
            getDoctor.Parameters.AddWithValue("@registerID", Int32.Parse(registerID));
            SqlDataAdapter adapterDoc = new SqlDataAdapter();
            adapterDoc.SelectCommand = getDoctor;
            adapterDoc.Fill(patientsDoctor);
            PatientDoctorGrid.AutoGenerateColumns = true;
            PatientDoctorGrid.DataSource = patientsDoctor.Tables[0];
            PatientDoctorGrid.Columns[0].Visible = false;
            int rowCount = PatientDoctorGrid.RowCount;
            if (rowCount > 0)
            {
                int totalRowHeight = PatientDoctorGrid.ColumnHeadersHeight;
                if (rowCount > 4)
                {
                    totalRowHeight += (PatientDoctorGrid.Rows[0].Height * 4);
                    PatientDoctorGrid.Height = totalRowHeight;
                }
                else
                {
                    totalRowHeight += (PatientDoctorGrid.Rows[0].Height * (rowCount + 1));
                    PatientDoctorGrid.Height = totalRowHeight;
                }
            }
            conn.Close();
        }

        private void PatientDoctorGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (PatientDoctorGrid.SelectedCells.Count > 0 && PatientDoctorGrid.RowCount > 0)
            {

                int selectedrowindex = PatientDoctorGrid.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = PatientDoctorGrid.Rows[selectedrowindex];
                DataRowView selectedPatientReg = selectedPatientRegs.SelectedItem as DataRowView;

                SqlConnection conn = new SqlConnection(Globals.conn);
                conn.Open();
                SqlCommand getDetails = new SqlCommand("Select * from Doctor_Patient where registerID = @registerID and doctorID = @doctorID", conn);
                getDetails.Parameters.AddWithValue("@registerID", selectedPatientReg["registerID"].ToString());
                getDetails.Parameters.AddWithValue("@doctorID", selectedRow.Cells["doctorID"].Value.ToString());
                SqlDataReader docPatientDetails = getDetails.ExecuteReader();
                while (docPatientDetails.Read())
                {
                    DoctorAssignLabel.Text = docPatientDetails["assignedDate"].ToString();
                    DoctorUnassignLabel.Text = docPatientDetails["unassignedDate"].ToString();
                    DoctorMedLabel.Text = docPatientDetails["medicalCase"].ToString();
                    DoctorMiscLabel.Text = docPatientDetails["miscDetails"].ToString();
                    DoctorSurgLabel.Text = docPatientDetails["surgicalDetails"].ToString();
                }

            }
        }

        private void registerListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            addPatientTypeBox.SelectedIndex = -1;
            addFirstNameBox.Text = "";
            addLastNameBox.Text = "";
            addStreetBox.Text = "";
            addCityBox.Text = "";
            addProvinceBox.Text = "";
            addCountryBox.Text = "";
            addGenderBox.SelectedIndex = -1;
            addDOBBox.Text = "";
            addInsuranceBox.Text = "";
            addHomePhoneBox.Text = "";
            addCellphoneBox.Text = "";
            addNotesBox.Text = "";
            addRegisterInfo.Text = "";
        }

        private void refreshWard_Click(object sender, EventArgs e)
        {
            populateWardList();
            vacancyFilter.SelectedIndex = -1;
            filterErrorWard.Text = "";
            wardNameFilter.Text = "";
        }

        private void filterWardButton_Click(object sender, EventArgs e)
        {
            string exp = "";
            if (wardNameFilter.Text.Length > 0)
                exp += "wardName like '" + wardNameFilter.Text + "%' ";
            if (vacancyFilter.SelectedIndex == 0)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "current_capacity < overall_capacity";
            }
            if (vacancyFilter.SelectedIndex == 1)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "current_capacity = overall_capacity";
            }
            if (exp.Length > 0)
            {
                //Debug.WriteLine("first check: " + wardList.Tables[0].Rows.Count.ToString());
                DataRow[] foundRows = wardList.Tables[0].Select(exp);
                //Debug.WriteLine("second check: " + wardList.Tables[0].Rows.Count.ToString());
                if (foundRows.Length > 0)
                {
                    wardList.Tables[0].DefaultView.RowFilter = exp;
                    WardListBox.DataSource = wardList.Tables[0];
                    filterErrorWard.Text = "";
                }
                else
                {
                    filterErrorWard.Text = "No results found.";
                }
            }
        }

        private void refreshCurrPatients_Click(object sender, EventArgs e)
        {
            populateCurrentPatientBox();
            currPatFilterError.Text = "";
            currPatFNameFilter.Text = "";
            currPatLNameFilter.Text = "";
            filterPType.SelectedIndex = -1;
            filterGender.SelectedIndex = -1;
            filterSIN.Text = "";
        }

        private void filterCurrPatients_Click(object sender, EventArgs e)
        {
            string exp = "";
            if (currPatFNameFilter.Text.Length > 0)
                exp += "firstName like '" + currPatFNameFilter.Text + "%' ";
            if (currPatLNameFilter.Text.Length > 0)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "lastName like '" + currPatLNameFilter.Text + "%' ";
            }
            if (filterPType.SelectedIndex > -1)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "patientType = '" + filterPType.SelectedItem.ToString() + "' ";
            }
            if (filterGender.SelectedIndex > -1)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "sex = '" + filterGender.SelectedItem.ToString() + "' ";
            }
            if (filterSIN.Text.Length > 0)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "patientSIN like '" + filterSIN.Text + "%'";
            }
            if (exp.Length > 0)
            {
                DataRow[] foundRows = currPatientList.Tables[0].Select(exp);
                if (foundRows.Length > 0)
                {
                    currPatientList.Tables[0].DefaultView.RowFilter = exp;
                    currPatientList.Tables[0].DefaultView.Sort = "lastName asc";
                    currentPatientsBox.DataSource = currPatientList.Tables[0];
                    currPatFilterError.Text = "";
                }
                else
                {
                    currPatFilterError.Text = "No results found";
                }
                filterGender.SelectedIndex = -1;
                filterPType.SelectedIndex = -1;
            }
            else
            {
                currPatFilterError.Text = "No filters selected";
            }
        }

        private void refreshDocList_Click(object sender, EventArgs e)
        {
            populateDoctorList();
            recepDocFilterError.Text = "";
            docFirstNameFilter.Text = "";
            docLastNameFilter.Text = "";
            docSpecFilter.Text = "";
            docDeptFilter.SelectedIndex = -1;
        }

        private void applyDocFilter_Click(object sender, EventArgs e)
        {
            string exp = "";
            if (docFirstNameFilter.Text.Length > 0)
                exp += "firstName like '" + docFirstNameFilter.Text + "%' ";
            if (docLastNameFilter.Text.Length > 0)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "lastName like '" + docLastNameFilter.Text + "%' ";
            }
            if (docDeptFilter.SelectedIndex > -1)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "departmentName = '" + docDeptFilter.SelectedItem.ToString() + "' ";
            }
            if (docSpecFilter.Text.Length > 0)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "specialization like '" + docSpecFilter.Text + "%'";
            }
            if (exp.Length > 0)
            {
                Debug.WriteLine(exp);
                DataRow[] foundRows = doctorList.Tables[0].Select(exp);
                if (foundRows.Length > 0)
                {
                    doctorList.Tables[0].DefaultView.RowFilter = exp;
                    doctorList.Tables[0].DefaultView.Sort = "Name asc";
                    DoctorListBox.DataSource = doctorList.Tables[0];
                    recepDocFilterError.Text = "";
                }
                else
                {
                    recepDocFilterError.Text = "No results found.";
                }
                docDeptFilter.SelectedIndex = -1;
            }
            else
            {
                recepDocFilterError.Text = "No filters selected.";
            }
        }

        private void refreshRecords_Click(object sender, EventArgs e)
        {
            populatePatientList();
            recordFilterError.Text = "";
            recordFNameFilter.Text = "";
            recordLNameFilter.Text = "";
            recordGenderFilter.SelectedIndex = -1;
        }

        private void filterRecords_Click(object sender, EventArgs e)
        {
            string exp = "";
            if (recordFNameFilter.Text.Length > 0)
                exp += "firstName like '" + recordFNameFilter.Text + "%' ";
            if (recordLNameFilter.Text.Length > 0)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "lastName like '" + recordLNameFilter.Text + "%' ";
            }
            if (recordGenderFilter.SelectedIndex > -1)
            {
                if (exp.Length > 0)
                    exp += "and ";
                exp += "sex = '" + recordGenderFilter.SelectedItem.ToString() + "' ";
            }
            if (exp.Length > 0)
            {
                Debug.WriteLine(exp);
                DataRow[] foundRows = patientRecordList.Tables[0].Select(exp);
                if (foundRows.Length > 0)
                {
                    patientRecordList.Tables[0].DefaultView.RowFilter = exp;
                    patientRecordList.Tables[0].DefaultView.Sort = "lastname asc";
                    patientRecordBox.DataSource = patientRecordList.Tables[0];
                    recordFilterError.Text = "";
                }
                else
                {
                    recordFilterError.Text = "No results found.";
                }
                recordGenderFilter.SelectedIndex = -1;
            }
            else
            {
                recordFilterError.Text = "No filters selected.";
            }
        }
        
        private void PatientGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DocClearReset();
            string patientSIN = PatientGrid.CurrentRow.Cells["patientSIN"].Value.ToString();
            DataRowView selectedDoctor = DoctorListBox.SelectedItem as DataRowView;
            string doctorID = selectedDoctor["doctorID"].ToString();
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            SqlCommand getDetails = new SqlCommand("Select miscDetails, surgicalDetails, medicalCase from Doctor_Patient WHERE doctorID = @doctorID" +
                " and registerID in (Select registerID from Register Where patientSIN = @patientSIN)", conn);
            getDetails.Parameters.AddWithValue("@patientSIN", patientSIN);
            getDetails.Parameters.AddWithValue("@doctorID", Int32.Parse(doctorID));
            SqlDataReader docPatientDetails = getDetails.ExecuteReader();
            while (docPatientDetails.Read())
            {
                SurgNoteText.Text = docPatientDetails["surgicalDetails"].ToString();
                MiscNoteText.Text = docPatientDetails["miscDetails"].ToString();
                MedNoteText.Text = docPatientDetails["medicalCase"].ToString();
            }
            conn.Close();
        }

        private void DocUpdateNoteButton_Click(object sender, EventArgs e)
        {
            DataRowView selectedDoctor = DoctorListBox.SelectedItem as DataRowView;
            int doctorID = Int32.Parse(selectedDoctor["doctorID"].ToString());
            if (PatientGrid.CurrentRow == null)
                return;
            string patientSIN = PatientGrid.CurrentRow.Cells["patientSIN"].Value.ToString();
            SqlConnection conn = new SqlConnection(Globals.conn);
            conn.Open();
            SqlCommand updateNotes = new SqlCommand("update Doctor_Patient set miscDetails = @miscDetails, surgicalDetails = @surgDetails," +
                " medicalCase = @medCase where doctorID = @doctorID and registerID in" +
                " (Select registerID from Register Where patientSIN = @patientSIN)", conn);

            updateNotes.Parameters.AddWithValue("@miscDetails", MiscNoteText.Text);
            updateNotes.Parameters.AddWithValue("@surgDetails", SurgNoteText.Text);
            updateNotes.Parameters.AddWithValue("@medCase", MedNoteText.Text);
            updateNotes.Parameters.AddWithValue("@doctorID", doctorID);
            updateNotes.Parameters.AddWithValue("@patientSIN", patientSIN);
            updateNotes.ExecuteNonQuery();
            conn.Close();
            UpdateNotesLabel.Text = "Notes updated for patient: " + PatientGrid.CurrentRow.Cells["Name"].Value.ToString();
            UpdateNotesLabel.ForeColor = Color.Green;
        }

        private void DocClearButton_Click(object sender, EventArgs e)
        {
            DocClearReset();
        }

        private void DocClearReset()
        {
            SurgNoteText.Clear();
            MiscNoteText.Clear();
            MedNoteText.Clear();
            UpdateNotesLabel.Text = "";
        }
       
    }

}
