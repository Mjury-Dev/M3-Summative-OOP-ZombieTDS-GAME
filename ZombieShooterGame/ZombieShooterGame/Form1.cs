using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZombieShooter;
using ZombieShooterGame.Properties;

namespace ZombieShooterGame
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, goUp, goDown, gameOver, gamePaused; // Added gamePaused variable
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        double zombieSpeed = 2.0;
        Random randNum = new Random();
        int score;
        List<PictureBox> zombiesList = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (gamePaused || gameOver) return; // Check if game is paused or over

            if (playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            }
            else
            {
                player.Image = Properties.Resources.dead;
                GameTimer.Stop();
                gameOver = true;
                ShowGameOverForm(); // Automatically show the GameOver form
            }

            txtAmmo.Text = "Ammo: " + ammo;
            txtScore.Text = "Kills: " + score;

            if (goLeft && player.Left > 0)
            {
                player.Left -= speed;
            }
            if (goRight && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += speed;
            }
            if (goUp && player.Top > 40)
            {
                player.Top -= speed;
            }
            if (goDown && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += speed;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;
                    }
                }

                if (x is PictureBox && (string)x.Tag == "zombie")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        playerHealth -= 1;
                    }

                    if (x.Left > player.Left)
                    {
                        x.Left -= (int)Math.Round(zombieSpeed); // Cast to int after rounding
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    }
                    if (x.Left < player.Left)
                    {
                        x.Left += (int)Math.Round(zombieSpeed); // Cast to int after rounding
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    }
                    if (x.Top > player.Top)
                    {
                        x.Top -= (int)Math.Round(zombieSpeed); // Cast to int after rounding
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    }
                    if (x.Top < player.Top)
                    {
                        x.Top += (int)Math.Round(zombieSpeed); // Cast to int after rounding
                        ((PictureBox)x).Image = Properties.Resources.zdown;
                    }
                }

                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;
                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zombiesList.Remove(((PictureBox)x));
                            MakeZombies();

                            // Check if the score is a multiple of 25
                            if (score % 25 == 0)
                            {
                                zombieSpeed += 0.2; // Increase zombie speed by 0.1
                            }

                            if (score % 15 == 0)
                            {
                                DropMedkit(); // Drop a medkit when the score is a multiple of 15
                            }
                        }
                    }
                }

                if (x is PictureBox && (string)x.Tag == "medkit")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        playerHealth += 20; // Increase player's health by 20
                        if (playerHealth > 100)
                        {
                            playerHealth = 100; // Ensure health doesn't exceed 100
                        }
                    }
                }

                if (x is PictureBox && (string)x.Tag == "medkit")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        playerHealth += 20; // Increase player's health by 20
                        if (playerHealth > 100)
                        {
                            playerHealth = 100; // Ensure health doesn't exceed 100
                        }
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (gameOver == true)
            {
                return;
            }

            if (e.KeyCode == Keys.A)
            {
                goLeft = true;
                facing = "left";
                player.Image = Properties.Resources.left;
            }

            if (e.KeyCode == Keys.D)
            {
                goRight = true;
                facing = "right";
                player.Image = Properties.Resources.right;
            }

            if (e.KeyCode == Keys.W)
            {
                facing = "up";
                goUp = true;
                player.Image = Properties.Resources.up;
            }

            if (e.KeyCode == Keys.S)
            {
                facing = "down";
                goDown = true;
                player.Image = Properties.Resources.down;
            }

            // Toggle game pause on Tab key press
            if (e.KeyCode == Keys.Tab)
            {
                if (gamePaused)
                {
                    gamePaused = false;
                    GameTimer.Start();
                }
                else
                {
                    gamePaused = true;
                    GameTimer.Stop();
                }
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                ShowGameOverForm(); // Call the method to show GameOver form
                return; // Exit method after showing GameOver form
            }

            if (e.KeyCode == Keys.A)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.D)
            {
                goRight = false;
            }

            if (e.KeyCode == Keys.W)
            {
                goUp = false;
            }

            if (e.KeyCode == Keys.S)
            {
                goDown = false;
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false && !gamePaused)
            {
                ammo--;
                ShootBullet(facing);
                if (ammo < 1)
                {
                    DropAmmo();
                }
            }

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }
        }

        private void ShootBullet(string direction)
        {
            double bulletOffsetX = 0;
            double bulletOffsetY = 0;

            switch (direction)
            {
                case "left":
                    bulletOffsetX = -player.Width / 8.5;
                    bulletOffsetY = player.Height / 3.5;
                    break;
                case "right":
                    bulletOffsetX = player.Width + player.Width / 8.5;
                    bulletOffsetY = player.Height / 1.5;
                    break;
                case "up":
                    bulletOffsetX = player.Width / 1.5;
                    bulletOffsetY = -player.Height / 8.5;
                    break;
                case "down":
                    bulletOffsetX = player.Width / 3.5;
                    bulletOffsetY = player.Height + player.Height / 8.5;
                    break;
            }

            Bullet bullet = new Bullet
            {
                Direction = direction,
                BulletLeft = player.Left + (int)bulletOffsetX,
                BulletTop = player.Top + (int)bulletOffsetY
            };
            bullet.MakeBullet(this);
        }

        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.zdown;
            zombie.Left = randNum.Next(0, 900);
            zombie.Top = randNum.Next(0, 800);
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Controls.Add(zombie);
            zombiesList.Add(zombie);
            player.BringToFront();
        }

        private void DropAmmo()
        {
            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();
        }

        private void DropMedkit()
        {
            PictureBox medkit = new PictureBox();
            medkit.Image = Properties.Resources.medkit;
            medkit.SizeMode = PictureBoxSizeMode.AutoSize;
            medkit.Left = randNum.Next(10, this.ClientSize.Width - medkit.Width);
            medkit.Top = randNum.Next(60, this.ClientSize.Height - medkit.Height);
            medkit.Tag = "medkit";
            this.Controls.Add(medkit);

            medkit.BringToFront();
            player.BringToFront();
        }

        private void RestartGame()
        {
            player.Image = Properties.Resources.up;

            foreach (PictureBox zombie in zombiesList)
            {
                this.Controls.Remove(zombie);
            }
            zombiesList.Clear();

            for (int i = 0; i < 3; i++)
            {
                MakeZombies();
            }

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;
            gamePaused = false; // Reset gamePaused flag

            playerHealth = 100;
            score = 0;
            ammo = 10;
            GameTimer.Start();
        }

        private void ShowGameOverForm()
        {
            GameTimer.Stop(); // Stop the game timer

            // Show the GameOver form
            GameOver gameOverForm = new GameOver(score);
            gameOverForm.Show();
        }

    }
}
