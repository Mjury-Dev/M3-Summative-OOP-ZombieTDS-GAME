using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZombieShooterGame
{
    public partial class MenuScreen : Form
    {
        public MenuScreen()
        {
            InitializeComponent();
        }

        //START GAME BUTTON
        private void LoadGame(object sender, EventArgs e)
        {
            Form1 gameWindow = new Form1();

            gameWindow.Show();
        }

        //LEADERBOARD BUTTON
        private void LoadBoard(object sender, EventArgs e)
        {
            Form2 boardWindow = new Form2();

            boardWindow.Show();

        }

        //HELP BUTTON
        private void LoadHelp(object sender, EventArgs e)
        {
            HelpScreen helpWindow = new HelpScreen();

            helpWindow.Show();

        }

        //EXIT BUTTON
        private void LoadExit(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
