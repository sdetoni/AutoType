using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Utilities;
using Microsoft.Win32;

namespace AutoTyper
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)] internal static extern IntPtr GetFocus();
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)] internal static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)] [return: MarshalAs(UnmanagedType.Bool)] static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)] internal static extern IntPtr GetForegroundWindow();

        const String  mVersion              = "1.9";
        const String  mTitle                = "Auto Typer";
        Boolean mStartTextSend              = false;
        String  mTextToSend                 = "";
        int     mTextToSendIdx              = 0;
        Boolean exitApp                     = false;
        int     mSavedWinHeight             = -1;
        Boolean mIsClipboardAction          = false;
        Boolean mIsNonGUIAutoTypeAction     = false;
        Boolean mIsKBShortcutAutoTypeAction = false;
        Boolean mOffScreen                  = false;
 

        event KeyEventHandlerExtended gkhFuncCallback = null;
        globalKeyboardHook gkh = null;

        public MainForm()
        {
            gkh = new globalKeyboardHook();
            gkh.HookedKeys.Add(Keys.V);
            gkhFuncCallback = new KeyEventHandlerExtended(gkh_KeyDown);
            gkh.KeyDown += gkhFuncCallback;

            SystemEvents.DisplaySettingsChanged += new EventHandler(SystemEvents_DisplaySettingsChanged);

            InitializeComponent();
        }

        void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
        {
            this.desktopResizedEvent();
        }

        protected void cleanUp()
        {
            if (gkhFuncCallback != null)
            { 
                gkh.KeyDown -= gkhFuncCallback;
                gkh.unhook();
                gkh             = null;
                gkhFuncCallback = null;
            }
        }

        void gkh_KeyDown(object sender, KeyEventArgs e, bool shift, bool control, bool alt)
        {
            if (alt && control && ShortCutKeyboardChk.Checked && !mIsKBShortcutAutoTypeAction)
            {
                switch (e.KeyCode)
                {
                    case Keys.V:
                        e.Handled = true;
                        mIsKBShortcutAutoTypeAction = true;
                        if (mOffScreen)
                            showAsDelayProgressWindow();

                        startAutoTypeAction(true);
                        break;
                }
            }                        
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // stop any auto type operations.
            this.TimerShutdownAll();

            // Update settings
            Properties.Settings.Default.AutoTypeDelay = DelayStartSecsNum.Value.ToString();
            Properties.Settings.Default.CharTypeDelay = DelayCharsSendNum.Value.ToString();
            Properties.Settings.Default.TurboTypeChk = TurboTypeChk.Checked;
            Properties.Settings.Default.ShortCutKeyboardChk = ShortCutKeyboardChk.Checked;

            Properties.Settings.Default.Save();

            if ((exitApp) || (e.CloseReason == CloseReason.WindowsShutDown))
            {
                base.OnFormClosing(e);
                return;
            }

            // hide the window and not actually close it!
            e.Cancel = true;
            PlaceLowerRight(true);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load/set settings
            DelayStartSecsNum.Value            = Int32.Parse(Properties.Settings.Default.AutoTypeDelay);
            DelayCharsSendNum.Value            = Int32.Parse(Properties.Settings.Default.CharTypeDelay);
            TurboTypeChk.Checked               = Properties.Settings.Default.TurboTypeChk;
            ShortCutKeyboardChk.Checked        = Properties.Settings.Default.ShortCutKeyboardChk;

            // temporarly show then hide the window to prevent focus glitch
            PlaceLowerRight(true);
            showAsDelayProgressWindow();
            BootTimer.Interval = 100;
            BootTimer.Start();
        }

        private void PlaceLowerRight(Boolean hide = false)
        {
            Screen mainScreen = Screen.PrimaryScreen;

            this.Left = mainScreen.WorkingArea.Right - this.Width;
            this.mOffScreen = hide;
            if (hide)
            {
                for (int idx = 0; idx < Screen.AllScreens.Length; idx++)
                {
                    Screen tst         = Screen.AllScreens[idx];
                    int    tstAspectXY = tst.Bounds.Y;
                    try
                    {
                        // Test ascpect ratio of screen, if 0 then treat it as potrait mode, and so test the x-axis
                        if ((tst.Bounds.Width / tst.Bounds.Height) <= 0)
                            tstAspectXY = tst.Bounds.X;
                    }
                    catch  { }

                    if ( (tstAspectXY < (mainScreen.WorkingArea.Bottom + this.Height)) && (tst.Bounds.Bottom > mainScreen.WorkingArea.Bottom) )
                        mainScreen = tst;
                }

                this.Top = mainScreen.WorkingArea.Bottom + this.Height;
            }
            else
                this.Top = mainScreen.WorkingArea.Bottom - this.Height;
        }

        private void AutoTypeTextBut_Click(object sender, EventArgs e)
        {
            mIsNonGUIAutoTypeAction = false;
            startAutoTypeAction(false);
        }

        private void AutoTypeClipboardBut_Click(object sender, EventArgs e)
        {
            mIsNonGUIAutoTypeAction = false;
            startAutoTypeAction(true);
        }

        private void showAsDelayProgressWindow ()
        {
            this.TopLevel = true;
            this.TopMost  = true;
            mIsNonGUIAutoTypeAction = true;

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
            this.Refresh();
        }

        private void startAutoTypeAction (Boolean isClipboardAction, Boolean noDelay=false)
        {
            this.mIsClipboardAction = isClipboardAction;
            if (noDelay)
            {
                this.Text = mTitle + "Prep";
                mStartTextSend = false;
                StartTimer.Interval = 100;
                StartTimer.Start();
            }
            else
            {
                this.Text = mTitle + ": Delaying " + ((int)DelayStartSecsNum.Value).ToString() + " seconds before typing..";
                mStartTextSend = false;
                StartTimer.Interval = ((int)DelayStartSecsNum.Value * 1000);
                ProgBar.Maximum = (StartTimer.Interval / ProgTimer.Interval);
                ProgBar.Minimum = 0;
                ProgBar.Value = ProgBar.Maximum;

                // Magic update code to quickly display progress bar with actual value
                int v = ProgBar.Value - 1;
                if (v < 0)
                    v = 0;
                ProgBar.Value = v;
                ProgBar.Refresh();

                StartTimer.Start();
                ProgTimer.Start();
            }
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
            if (mTextToSend != null)
            {
                mTextToSend = mTextToSend.Replace("\r", "");
                mTextToSendIdx = 0;

                // Set progress bar output
                ProgBar.Value = 0;
                ProgBar.Minimum = 0;
                ProgBar.Maximum = mTextToSend.Length;

                mStartTextSend = true;
                TxtTimer.Interval = (int)DelayCharsSendNum.Value;
                TxtTimer.Start();
            }
        }

        private void TxtTimer_Tick(object sender, EventArgs e)
        {
            mTextToSendIdx++;

            // If at the end of the text send stage ... clean up
            if ((!mStartTextSend) || (mTextToSendIdx > mTextToSend.Length))
            {
                this.TimerShutdownAll();
                return;
            }

            // (+), caret(^), percent sign(%), tilde(~), and parentheses() { }
            Char[]   specChar   = new Char[9]   { '{',     '}',    '+',   '^',   '%',   '~',   '(',   ')',    '\n'};
            string[] transChars = new string[9] { "{{}", "{}}", "+{+}", "{^}", "{%}", "{~}", "{(}", "{)}", "{ENTER}" };


            if (TurboTypeChk.Checked)
            {
                String newSendString = "";
                for (int sci = 0; sci < mTextToSend.Length; sci++)
                {
                    bool matched = false;
                    for (int idx = 0; idx < specChar.Length; idx++)
                    {
                        if (mTextToSend[sci] == specChar[idx])
                        {
                            newSendString += transChars[idx];
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
                        sendString = transChars[idx];
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

        private void TimerShutdownAll()
        {
            ProgTimer.Stop();
            StartTimer.Stop();
            TxtTimer.Stop();
            
            this.Text = mTitle;
            mStartTextSend = false;
            mIsKBShortcutAutoTypeAction = false;
            ProgBar.Value = ProgBar.Minimum;

            // if action from double click on system tray, then reset window sizing ... 
            if (mIsNonGUIAutoTypeAction)
            {
                PlaceLowerRight(true);
                mIsNonGUIAutoTypeAction = false;
                configWinSize();
            }
        }

        private void configWinSize ()
        {
            Size s = this.MinimumSize;
            if (mSavedWinHeight > 0)
            {
                s.Height = mSavedWinHeight;
                mSavedWinHeight = -1;
            }
            this.MinimumSize = s;
            this.Height = s.Height;
            this.AutoSize = false;
        }

        private void ExitAppMenuItem_Click(object sender, EventArgs e)
        {
            exitApp = true;
            System.Windows.Forms.Application.Exit();
        }


        private void ConfigMenuItem_Click(object sender, EventArgs e)
        {            
            configWinSize();
            PlaceLowerRight();
            mIsNonGUIAutoTypeAction = false;
            this.Focus();
        }

        private void IconTray_DoubleClick(object sender, EventArgs e)
        {
            if (mOffScreen)
                showAsDelayProgressWindow();
            startAutoTypeAction (true);
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            IconTray.BalloonTipTitle = "Auto Type " + mVersion;
            IconTray.BalloonTipText = "Written by Steven De Toni 2021\n" +
                                      "Double Click System Tray icon to auto type text from the Clipboard into an input field.";
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

        private void BootTimer_Tick(object sender, EventArgs e)
        {
            PlaceLowerRight(true);
            BootTimer.Stop();
        }

        private void AbortTyping_Click(object sender, EventArgs e)
        {
            this.TimerShutdownAll();
        }
        public void desktopResizedEvent ()
        {
            this.PlaceLowerRight(this.mOffScreen);
        }
    }
}
