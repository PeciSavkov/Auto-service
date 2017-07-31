using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace AutoService
{
    public partial class frmMain : Form
    {
        SqlConnection connection;
        string connectionString;

        public frmMain()
        {
            InitializeComponent();

            connectionString = ConfigurationManager.ConnectionStrings["AutoService.Properties.Settings.dbAutoServiceConnectionString"].ConnectionString;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            UpdateClients();
            UpdateAllAutomobiles();
        }
        #region Client manipulation
        //Function to update the DataGridVew for the clients
        private void UpdateClients()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("Select * FROM Client", connection))
            {
                DataTable clientsTable = new DataTable();
                adapter.Fill(clientsTable);

                dgvClients.DataSource = clientsTable;
            }
        }
        //Occurs when you click on a row header
        private void dgvClients_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            UpdateAutomobiles();

            int rowindex = dgvClients.CurrentCell.RowIndex;
            int columnindex = dgvClients.CurrentCell.ColumnIndex;
            int index = Convert.ToInt32(dgvClients.Rows[rowindex].Cells[columnindex].Value);
            MessageBox.Show(index.ToString());

            tbName.Text = dgvClients.Rows[rowindex].Cells[columnindex + 1].Value.ToString();
            tbSurname.Text = dgvClients.Rows[rowindex].Cells[columnindex + 2].Value.ToString();
            tbNumber.Text = dgvClients.Rows[rowindex].Cells[columnindex + 3].Value.ToString();
        }
        //Save new client
        private void btnSave_Click(object sender, EventArgs e)
        {
            string querry = "INSERT INTO Client VALUES (@clientName, @clientSurname, @clientNumber)";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(querry, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@clientName", tbName.Text);
                command.Parameters.AddWithValue("@clientSurname", tbSurname.Text);
                command.Parameters.AddWithValue("@clientNumber", tbNumber.Text);

                command.ExecuteNonQuery();
            }
            tbName.Text = "";
            tbNumber.Text = "";
            tbSurname.Text = "";
            UpdateClients();
        }
        //Update existing client
        private void btnUpdateClient_Click(object sender, EventArgs e)
        {
            int rowindex = dgvClients.CurrentCell.RowIndex;
            int columnindex = dgvClients.CurrentCell.ColumnIndex;
            int index = Convert.ToInt32(dgvClients.Rows[rowindex].Cells[columnindex].Value);

            string querry = "UPDATE Client" +
                " SET Name='" + tbName.Text + "'" + ", Surname='" + tbSurname.Text + "'" + ", Number='" + tbNumber.Text + "'" +
                "WHERE Id=" + index.ToString();
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(querry, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            tbName.Text = "";
            tbNumber.Text = "";
            tbSurname.Text = "";
            UpdateClients();
        }
        //Clear the textboxes for the clients
        private void btnCancel_Click(object sender, EventArgs e)
        {
            tbName.Text = "";
            tbNumber.Text = "";
            tbSurname.Text = "";
        }
        //Searching clients
        private void btnSearchClients_Click(object sender, EventArgs e)
        {
            if (tbSeachCName.Text == "" && tbSearchCSurname.Text == "")
            {
                MessageBox.Show("Atleast one of the fields must have text!");
                return;
            }
            else if (tbSeachCName.Text == "")
            {
                string querry = "Select * FROM Client Where Client.Surname ='" + tbSearchCSurname.Text + "'";
                using (connection = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
                {
                    DataTable clientsTable = new DataTable();
                    adapter.Fill(clientsTable);

                    dgvClients.DataSource = clientsTable;
                }
            }
            else if (tbSearchCSurname.Text == "")
            {
                string querry = "Select * FROM Client Where Client.Name ='" + tbSeachCName.Text + "'";
                using (connection = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
                {
                    DataTable clientsTable = new DataTable();
                    adapter.Fill(clientsTable);

                    dgvClients.DataSource = clientsTable;
                }
            }
            else
            {
                string querry = "Select * FROM Client Where Client.Name ='" + tbSeachCName.Text + "'" + "And Client.Surname = '" + tbSearchCSurname.Text + "'";
                using (connection = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
                {
                    DataTable clientsTable = new DataTable();
                    adapter.Fill(clientsTable);

                    dgvClients.DataSource = clientsTable;
                }
            }
            if (dgvClients.RowCount == 0)
            {
                UpdateClients();
                MessageBox.Show("No maches where found!");
            }
        }
        //Deleting clients 
        private void button1_Click(object sender, EventArgs e)
        {
            int rowindex = dgvClients.CurrentCell.RowIndex;
            int columnindex = dgvClients.CurrentCell.ColumnIndex;
            int index = Convert.ToInt32(dgvClients.Rows[rowindex].Cells[columnindex].Value);


            string querry = "DELETE FROM CustomerAutomobile WHERE ClientId ='" + index + "'";
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
            {
                DataTable clientsTable = new DataTable();
                adapter.Fill(clientsTable);

                dgvClients.DataSource = clientsTable;
            }
            querry = "DELETE FROM Client WHERE Id ='" + index + "'";
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
            {
                DataTable clientsTable = new DataTable();
                adapter.Fill(clientsTable);

                dgvClients.DataSource = clientsTable;
            }
        }

        #endregion

        #region Automobile manipulation
        //Saving the new automobile
        private void btnSaveCar_Click(object sender, EventArgs e)
        {
            string querry = "INSERT INTO Automobile VALUES (@automobilePlateNumber, @automobileModel, @automobileProblem, @automobilePrice)";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(querry, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@automobilePlateNumber", tbPlateNumber.Text);
                command.Parameters.AddWithValue("@automobileModel", tbModel.Text);
                command.Parameters.AddWithValue("@automobileProblem", rtbProblem.Text);
                command.Parameters.AddWithValue("@automobilePrice", tbPrice.Text);

                command.ExecuteNonQuery();
            }
            tbModel.Text = "";
            tbPlateNumber.Text = "";
            tbPrice.Text = "";
            rtbProblem.Text = "";
            UpdateAutomobiles();
        }
        //Update the whole DataGridView for automobiles
        private void UpdateAllAutomobiles()
        {
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("Select * FROM Automobile", connection))
            {
                DataTable automobileTable = new DataTable();
                adapter.Fill(automobileTable);

                dgvAutomobile.DataSource = automobileTable;
            }
        }
        //Occurs when the user selects the header of a row on clients DataGridView to show the automobile of the selected client
        private void UpdateAutomobiles()
        {
            int rowindex = dgvClients.CurrentCell.RowIndex;
            int columnindex = dgvClients.CurrentCell.ColumnIndex;
            int index = Convert.ToInt32(dgvClients.Rows[rowindex].Cells[columnindex].Value);

            string querry = "SELECT a.Id, a.PlateNumber, a.Model, a.Problem, a.Price" +
                " FROM Automobile a, Client c, CustomerAutomobile ca " +
                " WHERE a.Id = ca.AutomobileID AND ca.ClientID = c.Id AND c.Id =" + index;
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
            {
                DataTable automobileTable = new DataTable();
                adapter.Fill(automobileTable);

                dgvAutomobile.DataSource = automobileTable;
            }
        }
        //Clears the textboxes
        private void btnCancelCar_Click(object sender, EventArgs e)
        {
            tbModel.Text = "";
            tbPlateNumber.Text = "";
            tbPrice.Text = "";
            rtbProblem.Text = "";
        }
        //Deletes an automobile
        private void btnDeleteAutomobile_Click(object sender, EventArgs e)
        {
            int rowindex = dgvAutomobile.CurrentCell.RowIndex;
            int columnindex = dgvAutomobile.CurrentCell.ColumnIndex;
            int index = Convert.ToInt32(dgvAutomobile.Rows[rowindex].Cells[columnindex].Value);

            string querry = "DELETE FROM CustomerAutomobile WHERE AutomobileId ='" + index + "'";
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
            {
                DataTable automobileTable = new DataTable();
                adapter.Fill(automobileTable);

                dgvAutomobile.DataSource = automobileTable;
            }

            querry = "DELETE FROM Automobile WHERE Id ='" + index + "'";
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
            {
                DataTable automobileTable = new DataTable();
                adapter.Fill(automobileTable);

                dgvAutomobile.DataSource = automobileTable;
            }
        }
        //Updates an existing automobile
        private void btnUpdateAutomobile_Click(object sender, EventArgs e)
        {
            int rowindex = dgvAutomobile.CurrentCell.RowIndex;
            int columnindex = dgvAutomobile.CurrentCell.ColumnIndex;
            int index = Convert.ToInt32(dgvAutomobile.Rows[rowindex].Cells[columnindex].Value);

            string querry = "UPDATE Automobile" +
                " SET PlateNumber='" + tbPlateNumber.Text + "'" + ", Model='" + tbModel.Text + "'" + ", Problem='" + rtbProblem.Text + "'" + ", Price='" + Convert.ToDouble(tbPrice.Text) + "'" +
                "WHERE Id=" + index.ToString();
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(querry, connection))
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            tbModel.Text = "";
            tbPlateNumber.Text = "";
            tbPrice.Text = "";
            rtbProblem.Text = "";
            UpdateAutomobiles();
        }
        //Occurs when you select the header row
        private void dgvAutomobile_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowindex = dgvAutomobile.CurrentCell.RowIndex;
            int columnindex = dgvAutomobile.CurrentCell.ColumnIndex;
            int index = Convert.ToInt32(dgvAutomobile.Rows[rowindex].Cells[columnindex].Value);

            tbPlateNumber.Text = dgvAutomobile.Rows[rowindex].Cells[columnindex + 1].Value.ToString();
            tbModel.Text = dgvAutomobile.Rows[rowindex].Cells[columnindex + 2].Value.ToString();
            rtbProblem.Text = dgvAutomobile.Rows[rowindex].Cells[columnindex + 3].Value.ToString();
            tbPrice.Text = dgvAutomobile.Rows[rowindex].Cells[columnindex + 4].Value.ToString();

        }
        //Search an automobile by plate number or model
        private void btnSearchAutomobile_Click(object sender, EventArgs e)
        {
            if (tbSAModel.Text == "" && tbSANumber.Text == "")
            {
                MessageBox.Show("Atleast one of the fields must have text!");
                return;
            }
            else if (tbSAModel.Text == "")
            {
                string querry = "Select * FROM Automobile Where Automobile.PlateNumber ='" + tbSANumber.Text + "'";
                using (connection = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
                {
                    DataTable automobileTable = new DataTable();
                    adapter.Fill(automobileTable);

                    dgvAutomobile.DataSource = automobileTable;
                }
            }
            else if (tbSANumber.Text == "")
            {
                string querry = "Select * FROM Automobile Where Automobile.Model ='" + tbSAModel.Text + "'";
                using (connection = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
                {
                    DataTable automobileTable = new DataTable();
                    adapter.Fill(automobileTable);

                    dgvAutomobile.DataSource = automobileTable;
                }
            }
            else
            {
                string querry = "Select * FROM Automobile Where Automobile.PlateNumber ='" + tbSANumber.Text + "'" + "And Automobile.Model = '" + tbSAModel.Text + "'";
                using (connection = new SqlConnection(connectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(querry, connection))
                {
                    DataTable automobileTable = new DataTable();
                    adapter.Fill(automobileTable);

                    dgvAutomobile.DataSource = automobileTable;
                }
            }
            if (dgvAutomobile.RowCount == 0)
            {
                UpdateAllAutomobiles();
                MessageBox.Show("No maches where found!");
            }
        }
        #endregion
        
        private void butUpdateAllTables_Click(object sender, EventArgs e)
        {
            UpdateClients();
            UpdateAllAutomobiles();
        }
        //Save the owner of an automobile by their ID's
        private void btnSaveOwner_Click(object sender, EventArgs e)
        {
            string querry = "INSERT INTO CustomerAutomobile VALUES (@caClientId, @caAutomobileId)";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(querry, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@caClientId", tbClientID.Text);
                command.Parameters.AddWithValue("@caAutomobileId", tbCarID.Text);

                command.ExecuteNonQuery();
            }
            tbCarID.Text = "";
            tbClientID.Text = "";
        }
        //Clears the textboxes
        private void btnCancelOwner_Click_1(object sender, EventArgs e)
        {
            tbCarID.Text = "";
            tbClientID.Text = "";
        }
    }
}