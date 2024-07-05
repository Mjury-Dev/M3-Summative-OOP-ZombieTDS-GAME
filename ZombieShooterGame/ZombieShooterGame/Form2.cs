using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ZombieShooterGame
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        //BUTTON FOR READ
        private void button2_Click(object sender, EventArgs e)
        {
            // Create a connection to the SQL Server database
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-LMEVCLVR\\SQLEXPRESS01;Initial Catalog=GLSBoard;Integrated Security=True");
            string readQuery = @"
        USE GLSBoard;
        SELECT 
            ROW_NUMBER() OVER (ORDER BY SCORE DESC, NAME DESC) AS RANKING,
            NAME,
            SCORE
        FROM dbo.Board
        ORDER BY SCORE DESC, NAME DESC;";

            SqlDataAdapter sda = new SqlDataAdapter(readQuery, con); // Data adapter to fill the DataTable
            SqlCommandBuilder cmd = new SqlCommandBuilder(sda);
            DataTable dt = new DataTable();
            sda.Fill(dt); // Fill the DataTable with data from the database
            dataGridView1.DataSource = dt; // Bind the DataTable to the DataGridView
        }

        // BUTTON FOR UPDATE
        private void button4_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtRank.Text) || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter both Rank and New Name.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rank;
            if (!int.TryParse(txtRank.Text, out rank))
            {
                MessageBox.Show("Please enter a valid integer for Rank.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a connection to the SQL Server database
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-LMEVCLVR\\SQLEXPRESS01;Initial Catalog=GLSBoard;Integrated Security=True");
            con.Open(); // Open the connection

            // Construct the update query
            string updateQuery = @"
        USE GLSBoard;
        UPDATE dbo.Board
        SET NAME = @NewName
        WHERE NAME IN (
            SELECT NAME
            FROM (
                SELECT 
                    NAME,
                    ROW_NUMBER() OVER (ORDER BY SCORE DESC, NAME DESC) AS RANKING
                FROM dbo.Board
            ) AS Ranked
            WHERE RANKING = @Rank
        );";

            // Create and execute the SQL command
            SqlCommand cmd = new SqlCommand(updateQuery, con);
            cmd.Parameters.AddWithValue("@NewName", txtName.Text);
            cmd.Parameters.AddWithValue("@Rank", rank);

            int rowsAffected = cmd.ExecuteNonQuery(); // Execute the command

            con.Close(); // Close the connection

            // Check if the update was successful
            if (rowsAffected > 0)
            {
                MessageBox.Show("Name updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the DataGridView after update
                button2_Click(sender, e); // Call the button2_Click event to refresh data
            }
            else
            {
                MessageBox.Show("Failed to update name. Please check the Rank provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //BUTTON FOR DELETE
        private void button1_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(txtRank.Text))
            {
                MessageBox.Show("Please enter a Rank.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int rank;
            if (!int.TryParse(txtRank.Text, out rank))
            {
                MessageBox.Show("Please enter a valid integer for Rank.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a connection to the SQL Server database
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-LMEVCLVR\\SQLEXPRESS01;Initial Catalog=GLSBoard;Integrated Security=True");
            con.Open(); // Open the connection

            // Construct the delete query
            string deleteQuery = @"
        USE GLSBoard;
        DELETE FROM dbo.Board
        WHERE NAME IN (
            SELECT NAME
            FROM (
                SELECT 
                    NAME,
                    ROW_NUMBER() OVER (ORDER BY SCORE DESC, NAME DESC) AS RANKING
                FROM dbo.Board
            ) AS Ranked
            WHERE RANKING = @Rank
        );";

            // Create and execute the SQL command
            SqlCommand cmd = new SqlCommand(deleteQuery, con);
            cmd.Parameters.AddWithValue("@Rank", rank);

            int rowsAffected = cmd.ExecuteNonQuery(); // Execute the command

            con.Close(); // Close the connection

            // Check if the delete was successful
            if (rowsAffected > 0)
            {
                MessageBox.Show("Record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the DataGridView after delete
                button2_Click(sender, e); // Call the button2_Click event to refresh data
            }
            else
            {
                MessageBox.Show("Failed to delete record. Please check the Rank provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void LoadHome(object sender, EventArgs e)
        {
            // Close the current form (Form2)
            this.Close();

            // Find and show the existing instance of MenuScreen if it's already open
            foreach (Form form in Application.OpenForms)
            {
                if (form is MenuScreen)
                {
                    form.Show();
                    return;
                }
            }

            // If MenuScreen is not already open, create a new instance and show it
            MenuScreen menuScreen = new MenuScreen();
            menuScreen.Show();
        }
    }
}
