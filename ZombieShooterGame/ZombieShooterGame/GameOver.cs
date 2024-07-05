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
using System.Xml.Linq;

namespace ZombieShooterGame
{
    public partial class GameOver : Form
    {
        private int score; // Store the score value

        public GameOver(int score)
        {
            InitializeComponent();
            this.score = score;
            txtKills.Text = "SCORE: " + score.ToString(); // Display the score in txtKills label
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void SaveScore(object sender, EventArgs e)
        {
            bool isAnyEmpty = false;

            // Check if any TextBox is empty
            foreach (Control control in this.Controls)
            {
                if (control is TextBox && control.Text.Length == 0)
                {
                    isAnyEmpty = true;
                    break;
                }
            }

            if (isAnyEmpty)
            {
                MessageBox.Show("Please fill the required form", "info", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show warning message
            }
            else
            {
                try
                {
                    // Create a connection to the SQL Server database
                    using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-LMEVCLVR\\SQLEXPRESS01;Initial Catalog=GLSBoard;Integrated Security=True"))
                    {
                        con.Open(); // Open the connection
                        string selectQuery = "SELECT SCORE FROM Board WHERE NAME = @NAME";
                        using (SqlCommand selectCmd = new SqlCommand(selectQuery, con))
                        {
                            selectCmd.Parameters.AddWithValue("@NAME", txtName.Text);
                            object result = selectCmd.ExecuteScalar();

                            if (result != null)
                            {
                                int existingScore = Convert.ToInt32(result);

                                if (score < existingScore)
                                {
                                    DialogResult dialogResult = MessageBox.Show("Your new score is lower than your existing high score. Are you sure you want to replace it?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                                    if (dialogResult == DialogResult.Cancel)
                                    {
                                        return;
                                    }
                                }

                                string updateQuery = "UPDATE Board SET SCORE = @SCORE WHERE NAME = @NAME";
                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, con))
                                {
                                    updateCmd.Parameters.AddWithValue("@NAME", txtName.Text);
                                    updateCmd.Parameters.AddWithValue("@SCORE", score);
                                    int count = updateCmd.ExecuteNonQuery(); // Execute the update command

                                    if (count > 0)
                                    {
                                        MessageBox.Show("Score updated successfully", "info", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show success message
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error in updating score", "info", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show error message
                                    }
                                }
                            }
                            else
                            {
                                string insertQuery = "INSERT INTO Board (NAME, SCORE) VALUES (@NAME, @SCORE)";
                                using (SqlCommand insertCmd = new SqlCommand(insertQuery, con))
                                {
                                    insertCmd.Parameters.AddWithValue("@NAME", txtName.Text);
                                    insertCmd.Parameters.AddWithValue("@SCORE", score); // Use the score passed to the constructor
                                    int count = insertCmd.ExecuteNonQuery(); // Execute the insert command

                                    if (count > 0)
                                    {
                                        MessageBox.Show("Created successfully", "info", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show success message
                                    }
                                    else
                                    {
                                        MessageBox.Show("Error in creation", "info", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show error message
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BacktoMain(object sender, EventArgs e)
        {
            this.Close(); // Close the GameOver form

            // Find and close the main form (Form1)
            foreach (Form form in Application.OpenForms)
            {
                if (form is Form1)
                {
                    form.Close();
                    break;
                }
            }
        }

        private void LoadRestart(object sender, EventArgs e)
        {
            this.Close(); // Close the GameOver form

            // Find and close the main form (Form1)
            foreach (Form form in Application.OpenForms)
            {
                if (form is Form1)
                {
                    form.Close();
                    break;
                }
            }

            // Create and show a new instance of Form1 to restart the game
            Form1 newGame = new Form1();
            newGame.Show();
        }
    }
}
