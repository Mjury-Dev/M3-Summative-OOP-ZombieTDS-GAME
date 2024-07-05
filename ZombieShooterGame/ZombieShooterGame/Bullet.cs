using System;
using System.Drawing;
using System.Windows.Forms;

namespace ZombieShooter
{
    class Bullet
    {
        // Variables
        public string Direction { get; set; } // Property for bullet direction
        public int Speed { get; set; } = 20; // Property for bullet speed
        private PictureBox BulletBox = new PictureBox(); // PictureBox for the bullet
        private Timer BulletTimer = new Timer(); // Timer for bullet movement
        public int BulletLeft { get; set; } // Property for bullet's left position
        public int BulletTop { get; set; } // Property for bullet's top position

        // Method to create and add the bullet to the form
        public void MakeBullet(Form form)
        {
            BulletBox.BackColor = Color.White; // Set bullet color to white
            BulletBox.Size = new Size(5, 5); // Set bullet size
            BulletBox.Tag = "bullet"; // Set bullet tag
            BulletBox.Left = BulletLeft; // Set bullet's left position
            BulletBox.Top = BulletTop; // Set bullet's top position
            BulletBox.BringToFront(); // Bring bullet to front
            form.Controls.Add(BulletBox); // Add bullet to the form

            BulletTimer.Interval = Speed; // Set timer interval to bullet speed
            BulletTimer.Tick += new EventHandler(BulletTimer_Tick); // Assign Tick event
            BulletTimer.Start(); // Start the timer
        }

        // Event handler for bullet movement
        private void BulletTimer_Tick(object sender, EventArgs e)
        {
            // Move bullet based on direction
            switch (Direction)
            {
                case "left":
                    BulletBox.Left -= Speed;
                    break;
                case "right":
                    BulletBox.Left += Speed;
                    break;
                case "up":
                    BulletBox.Top -= Speed;
                    break;
                case "down":
                    BulletBox.Top += Speed;
                    break;
            }

            // Check if bullet is out of bounds
            if (BulletBox.Left < 16 || BulletBox.Left > 860 || BulletBox.Top < 10 || BulletBox.Top > 616)
            {
                BulletTimer.Stop(); // Stop the timer
                BulletTimer.Dispose(); // Dispose the timer
                BulletBox.Dispose(); // Dispose the bullet
                BulletTimer = null; // Nullify the timer object
                BulletBox = null; // Nullify the bullet object
            }
        }
    }
}
