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
            //Event Set up
            A1.Click += ClickedPlayButtons;
            A2.Click += ClickedPlayButtons;
            A3.Click += ClickedPlayButtons;
            A4.Click += ClickedPlayButtons;
            A5.Click += ClickedPlayButtons;

            B1.Click += ClickedPlayButtons;
            B2.Click += ClickedPlayButtons;
            B3.Click += ClickedPlayButtons;
            B4.Click += ClickedPlayButtons;
            B5.Click += ClickedPlayButtons;

            C1.Click += ClickedPlayButtons;
            C2.Click += ClickedPlayButtons;
            C3.Click += ClickedPlayButtons;
            C4.Click += ClickedPlayButtons;
            C5.Click += ClickedPlayButtons;

            D1.Click += ClickedPlayButtons;
            D2.Click += ClickedPlayButtons;
            D3.Click += ClickedPlayButtons;
            D4.Click += ClickedPlayButtons;
            D5.Click += ClickedPlayButtons;

            E1.Click += ClickedPlayButtons;
            E2.Click += ClickedPlayButtons;
            E3.Click += ClickedPlayButtons;
            E4.Click += ClickedPlayButtons;
            E5.Click += ClickedPlayButtons;
            
            //Color Set up
            A1.BackColor = bColors[rnd.Next(0, 4) * 2];
            A2.BackColor = bColors[rnd.Next(0, 4) * 2];
            A3.BackColor = bColors[rnd.Next(0, 4) * 2];
            A4.BackColor = bColors[rnd.Next(0, 4) * 2];
            A5.BackColor = bColors[rnd.Next(0, 4) * 2];

            B1.BackColor = bColors[rnd.Next(0, 4) * 2];
            B2.BackColor = bColors[rnd.Next(0, 4) * 2];
            B3.BackColor = bColors[rnd.Next(0, 4) * 2];
            B4.BackColor = bColors[rnd.Next(0, 4) * 2];
            B5.BackColor = bColors[rnd.Next(0, 4) * 2];

            C1.BackColor = bColors[rnd.Next(0, 4) * 2];
            C2.BackColor = bColors[rnd.Next(0, 4) * 2];
            C3.BackColor = bColors[rnd.Next(0, 4) * 2];
            C4.BackColor = bColors[rnd.Next(0, 4) * 2];
            C5.BackColor = bColors[rnd.Next(0, 4) * 2];

            D1.BackColor = bColors[rnd.Next(0, 4) * 2];
            D2.BackColor = bColors[rnd.Next(0, 4) * 2];
            D3.BackColor = bColors[rnd.Next(0, 4) * 2];
            D4.BackColor = bColors[rnd.Next(0, 4) * 2];
            D5.BackColor = bColors[rnd.Next(0, 4) * 2];

            E1.BackColor = bColors[rnd.Next(0, 4) * 2];
            E2.BackColor = bColors[rnd.Next(0, 4) * 2];
            E3.BackColor = bColors[rnd.Next(0, 4) * 2];
            E4.BackColor = bColors[rnd.Next(0, 4) * 2];
            E5.BackColor = bColors[rnd.Next(0, 4) * 2];
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
                Int32 buttonIndex = allButtonList.BinarySearch(((Button)sender));
                Color buttonColor = allbuttColor[buttonIndex];
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
            foreach (Control c in this.Controls)
            {
                if (c is Button)
                {
                    allButtonList.Add((Button)c);
                    allbuttColor.Add(c.BackColor);
                }
            }

            foreach (Button bp in allButtonList)
            {
                bp.BackColor = Color.AntiqueWhite;
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
                ((Timer) sender).Stop();
                TimerText.Text = "";
            }

            ScoreText.Text = score.ToString();

            if (score == 50)
            {
                GameState = 2;
                CurrentLevel.Text = "Level: 2";
                timeLeft += 40;
            }

            else if (score > 100)
            {
                GameState = 3;
                CurrentLevel.Text = "Level: 3";
                timeLeft += 100;
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
