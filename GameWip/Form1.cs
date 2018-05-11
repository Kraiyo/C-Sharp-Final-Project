using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameWip
{
    public partial class Form1 : Form
    {
        public Int32 timeLeft;

        public Int32 GameState = 0;

        public List<Color> bColors = new List<Color> { Color.Red, Color.DarkRed, Color.Blue, Color.DarkBlue, Color.Green, Color.DarkGreen, Color.Yellow, Color.GreenYellow }; //Unpressed, 0,2,4,6. Pressed, 1,3,5,7.

        public List<Color> allbuttColor = new List<Color>();

        public List<Button> selectedButtonList = new List<Button>();

        public List<Button> allButtonList = new List<Button>();

        public Random rnd = new Random();

        public Int32 score = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Timer timer1 = new Timer();

            timer1.Enabled = true;
            timer1.Stop();

            Timer CardFlipTimer = new Timer();
            CardFlipTimer.Interval = 2000;
            CardFlipTimer.Tick += OnTimedEvent;
        }

        private void buttonSetUp()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                {
                    if (c != StartButton && c != TileReset)
                    {
                        // Set up OnClickEvent
                        c.Click += ClickedPlayButtons;

                        // Set up Random Color
                        c.BackColor = bColors[rnd.Next(0, 4) * 2];

                        // Set up Gamestate 3 lists
                        allButtonList.Add((Button)c);
                        allbuttColor.Add(c.BackColor);
                    }
                }
            }
        }

        private void ClickedPlayButtons(object sender, EventArgs e)
        {
            Int32 bIndex = bColors.IndexOf(((Button)sender).BackColor);
            if (GameState == 1 || GameState == 2)
            {
                if (bIndex % 2 == 0) //"Selected"
                {
                    ((Button)sender).BackColor = bColors[bIndex + 1];
                    selectedButtonList.Add(((Button)sender));
                    label2.Text = selectedButtonList.Count.ToString();
                }

                else //"Unselected"
                {
                    ((Button)sender).BackColor = bColors[bIndex - 1];
                    selectedButtonList.Remove(((Button)sender));
                    label2.Text = selectedButtonList.Count.ToString();
                }
            }

            else if (GameState == 3)
            {
                CardFlipTimer.Start();

                Int32 buttonIndex = allButtonList.BinarySearch(((Button)sender));
                Color buttonColor = allbuttColor[buttonIndex]; //Out of Range Error (See OnTimedEvent)
                ((Button)sender).BackColor = buttonColor;
            }

            //FoundMatch Method Call. Everytime a button is pressed, check to see if it's a match or not.

            if (GameState == 1)
            {
                if (selectedButtonList.Count() > 1) // Only check if there is enough buttons pressed to check.
                {
                    if (FoundMatch(((Button)sender).BackColor)) // If ture
                    {
                        score += 5;
                    }

                    else // else it's false
                    {
                        score -= 7;
                    }
                }
            } // Game Logic for GameState 1

            else if (GameState == 2)
            {
                if (selectedButtonList.Count() > 2) // Only check if there is enough buttons pressed to check.
                {
                    if (FoundMatch(((Button)sender).BackColor)) // If ture
                    {
                        score += 15;
                    }

                    else // else it's false
                    {
                        score -= 25;
                    }
                }
            } // Game Logic for GameState 2

            else if (GameState == 3)
            {
                if (selectedButtonList.Count() > 1) // Only check if there is enough buttons pressed to check.
                {
                    if (FoundMatch(((Button)sender).BackColor)) // If ture
                    {
                        score += 25;
                    }

                    else // else it's false
                    {
                        score -= 30;
                    }
                }
            } // Game Logic for GameState 3

            if (timeLeft < 4) timeLeft += 2; // I call it desperation time. You're on the edge, how long can you hold on?
            
        }

        private bool FoundMatch(Color selectedColor)
        {
            if (GameState == 1)
            {
                Int32 matchValue = 0;

                foreach (Button bp in selectedButtonList)
                {
                    if (bp.BackColor == selectedColor)
                    {
                        matchValue++;

                        if (matchValue == 2)
                        {
                            ButtonRefresh(); // Clears selectedButtonList
                            return true;
                        }
                    }
                }
                return false;
            } // End Of GameState 1 Code

            else if (GameState == 2)
            {
                Int32 matchValue = 0;

                foreach (Button bp in selectedButtonList)
                {
                    if (bp.BackColor == selectedColor)
                    {
                        matchValue++;

                        if (matchValue == 3)
                        {
                            ButtonRefresh(); // Clears selectedButtonList
                            return true;
                        }
                    }
                }
                return false;
            } // End Of GameState 2 Code

            else if (GameState == 3)
            {
                Int32 matchValue = 0;

                foreach (Button bp in selectedButtonList)
                {

                    if (bp.BackColor == selectedColor)
                    {
                        matchValue++;

                        if (matchValue == 2)
                        {
                            CardFlipTimer.Start();
                            ButtonRefresh(); // Clears selectedButtonList
                            return true;
                        }
                    }
                }
                return false;
            } // End Of GameState 3 Code

            else return false; // Code will never reach this, but added for good practice anyways.
        }

        private void ButtonRefresh() // Changes Button colors and clears selecetedButtonList
        {
            foreach (Button bp in selectedButtonList)
            {
                bp.BackColor = bColors[rnd.Next(0, 4) * 2];
            }

            selectedButtonList.Clear();
        }

        private void OnTimedEvent(object sender, EventArgs e) // AHHHHHHHHHHHHHHHHHHHHHHHHHH 4:51pm 5/7/2018
        {
            foreach (Button bp in allButtonList)
            {
                bp.IsAccessible = false;
            }

            label4.Text = allButtonList.Count.ToString(); // Never runs. Timer Never starts.
            label5.Text = allbuttColor.Count.ToString();

            foreach (Button bp in allButtonList)
            {
                bp.BackColor = Color.AntiqueWhite;
                bp.IsAccessible = true;
            }

            ((Timer)sender).Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0) // Timer count down
            {
                timeLeft--;
                TimerText.Text = timeLeft.ToString();
            }

            else if (timeLeft <= 0) // You ran out of time (like I am, right now. 3:55pm 5/7/2018)
            {
                GameState = 0;
            }

            if (GameState == 0) // End Game code
            {
                StartButton.Visible = true;
                ((Timer)sender).Stop();
                TimerText.Text = "";
            }

            ScoreText.Text = score.ToString();

            while (GameState == 1)
            {
                if (score > 50)
                {
                    timeLeft += 40;
                    CurrentLevel.Text = "Level: 2";
                    GameState = 2;
                }
                break;
            }

            while (GameState == 2)
            {
                if (score > 100)
                {
                    timeLeft += 100;
                    CurrentLevel.Text = "Level: 3";
                    GameState = 3;
                }
                break;
            }

        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            timeLeft = 45;
            timer1.Start();

            Resets.Text = "3 (WIP)"; // Maybe I'll get it done in time.

            CurrentLevel.Text = "Level: 1";

            buttonSetUp();

            GameState = 1;

            StartButton.Visible = false;
        }

    }
}
