using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AutoTyper
{
    public partial class MainForm : Form
    {
        String  mTitle = "Auto Typer";
        Boolean mStartTextSend       = false;
        String  mTextToSend          = "";
        int     mTextToSendIdx       = 0;
        Boolean exitApp              = false;
        int     mSavedWinHeight      = -1;
        Boolean mIsClipboardAction   = false;
        Boolean mIsDoubleClickAction = false;

        public MainForm()
        {
            InitializeComponent();
            ShowInTaskbar = false;
            Visible       = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Update settings
            Properties.Settings.Default.AutoTypeDelay = DelayStartSecsNum.Value.ToString();
            Properties.Settings.Default.CharTypeDelay = DelayCharsSendNum.Value.ToString();
            Properties.Settings.Default.TurboTypeChk = TurboTypeChk.Checked;

            Properties.Settings.Default.Save();

            base.OnFormClosing(e);
            if ((exitApp) || (e.CloseReason == CloseReason.WindowsShutDown))
                return;

            // hide the window and not actually close it!
            e.Cancel = true;
            Visible  = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load/set settings
            DelayStartSecsNum.Value = Int32.Parse(Properties.Settings.Default.AutoTypeDelay);
            DelayCharsSendNum.Value = Int32.Parse(Properties.Settings.Default.CharTypeDelay);
            TurboTypeChk.Checked    = Properties.Settings.Default.TurboTypeChk;
            Visible                 = false;
            PlaceLowerRight(true);
        }

        private void PlaceLowerRight(Boolean hide=false)
        {
            Screen mainScreen = Screen.PrimaryScreen;

            this.Left = mainScreen.WorkingArea.Right - this.Width;
            if (hide)
                this.Top = mainScreen.WorkingArea.Bottom + this.Height;
            else
                this.Top = mainScreen.WorkingArea.Bottom - this.Height;
        }

        private void AutoTypeTextBut_Click(object sender, EventArgs e)
        {
            mIsDoubleClickAction = false;
            startAutoTypeAction(false);
        }

        private void AutoTypeClipboardBut_Click(object sender, EventArgs e)
        {
            mIsDoubleClickAction = false;
            startAutoTypeAction(true);
        }

        private void startAutoTypeAction (Boolean isClipboardAction)
        {
            this.mIsClipboardAction = isClipboardAction;
            this.Text               = mTitle + ": Delaying " + ((int)DelayStartSecsNum.Value).ToString() + " seconds before typing..";
            mStartTextSend          = false;
            StartTimer.Interval     = ((int)DelayStartSecsNum.Value * 1000);
            ProgBar.Maximum         = StartTimer.Interval / ProgTimer.Interval;
            ProgBar.Minimum         = 0;
            ProgBar.Value           = ProgBar.Maximum;
            ProgBar.Refresh();
            StartTimer.Start();
            ProgTimer.Start();
        }

        private void ProgTimer_Tick(object sender, EventArgs e)
        {
            int v = ProgBar.Value - 1;
            if (v < 0)
                v = 0;
            ProgBar.Value = v;
            ProgBar.Refresh();
        }

        private void StartTimer_Tick(object sender, EventArgs e)
        {
            ProgTimer.Stop();
            StartTimer.Stop();

            this.Text = mTitle + ": Typing now...";

            // Load text into memory
            if  (this.mIsClipboardAction)
                mTextToSend = Clipboard.GetText(TextDataFormat.UnicodeText);  
            else
                mTextToSend = TextBuffer.Text;

            // remove unwanted chars and set start typing pos
            mTextToSend    = mTextToSend.Replace("\r", ""); 
            mTextToSendIdx = 0;

            // Set progress bar output
            ProgBar.Value   = 0;
            ProgBar.Minimum = 0;
            ProgBar.Maximum = mTextToSend.Length;

            // Start send of the text
            mStartTextSend    = true;
            TxtTimer.Interval = (int)DelayCharsSendNum.Value;
            TxtTimer.Start();
        }

        private void TxtTimer_Tick(object sender, EventArgs e)
        {
            mTextToSendIdx++;

            // If at the end of the text send stage ... clean up
            if ((!mStartTextSend) || (mTextToSendIdx > mTextToSend.Length))
            {
                this.Text = mTitle;
                mStartTextSend = false;
                TxtTimer.Stop();

                // if action from double click on system tray, then reset window sizing ... 
                if (mIsDoubleClickAction)
                {
                    Visible              = false;
                    mIsDoubleClickAction = false;
                    Size s               = this.MinimumSize;
                    s.Height             = mSavedWinHeight;
                    this.MinimumSize     = s;
                    this.Height          = s.Height;
                    this.AutoSize        = false;
                }
                return;
            }

            // (+), caret(^), percent sign(%), tilde(~), and parentheses() { }
            Char[] specChar = new Char[8] { '{', '}', '+', '^', '%', '~', '(', ')'};

            if (TurboTypeChk.Checked)
            {
                String newSendString = "";
                for (int sci = 0; sci < mTextToSend.Length; sci++)
                {
                    bool matched = false;
                    for (int idx = 0; idx < specChar.Length; idx++)
                    {
                        if  (mTextToSend[sci] == specChar[idx])
                        {
                            newSendString += "{" + mTextToSend[sci] + "}";
                            matched = true;
                            break;
                        }
                    }
                    if (!matched)
                        newSendString += mTextToSend[sci];
                }

                SendKeys.Send(newSendString);
                mTextToSendIdx = mTextToSend.Length;
            }
            else
            {
                String sendString = "";
                for (int idx = 0; idx < specChar.Length; idx++)
                {
                    if (mTextToSend[mTextToSendIdx - 1] == specChar[idx])
                    {
                        sendString = "{" + mTextToSend[mTextToSendIdx - 1] + "}";
                        break;
                    }
                }
                if (sendString == "")
                    sendString = mTextToSend[mTextToSendIdx - 1].ToString();

                SendKeys.Send(sendString);

                // Update the progress bar
                ProgBar.Value = ProgBar.Value + 1;

                // this hack makes it display quicker! So it show real representation of progress.
                if (ProgBar.Value > 0)
                {
                    ProgBar.Value = ProgBar.Value - 1;
                    ProgBar.Value = ProgBar.Value + 1;
                }
            }
        }

        private void ExitAppMenuItem_Click(object sender, EventArgs e)
        {
            exitApp = true;
            System.Windows.Forms.Application.Exit();
        }

        private void ConfigMenuItem_Click(object sender, EventArgs e)
        {            
            PlaceLowerRight();
            Visible = true;
            this.Focus();
        }

        private void IconTray_DoubleClick(object sender, EventArgs e)
        {
            mIsDoubleClickAction = true;
            if (mSavedWinHeight < 0)
                mSavedWinHeight = this.Height;

            Rectangle screenRectangle = this.RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;

            Size s = this.MinimumSize;
            s.Height = TopPos.Top + titleHeight + (SystemInformation.Border3DSize.Height * 2);
            this.MinimumSize = s;
            this.Height = s.Height;
            this.AutoSize = false;

            PlaceLowerRight();
            Visible = true;
            this.Focus();
            this.Refresh();

            // start the auto typing ... 
            startAutoTypeAction (true);
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            IconTray.ShowBalloonTip(10000);
        }

        private void TurboTyperChkBox_Changed(object sender, EventArgs e)
        {
            if (TurboTypeChk.Checked)
            {
                DelayCharsSendNum.Enabled = false;
            }
            else
            {
                DelayCharsSendNum.Enabled = true;
            }
        }
    }
}
