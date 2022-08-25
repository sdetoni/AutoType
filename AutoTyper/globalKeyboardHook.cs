using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;

namespace Utilities {
    /// <summary>
    /// A class that manages a global low level keyboard hook
    /// </summary>
    /// 
    public delegate void KeyEventHandlerExtended(object sender, KeyEventArgs e, bool shift, bool control, bool alt);

    class globalKeyboardHook {
		#region Constant, Structure and Delegate Definitions
		/// <summary>
		/// defines the callback type for the hook
		/// </summary>
		public delegate int keyboardHookProc(int code, int wParam, ref keyboardHookStruct lParam);

		public struct keyboardHookStruct {
			public int vkCode;
			public int scanCode;
			public int flags;
			public int time;
			public int dwExtraInfo;
		}

        const int WH_KEYBOARD_LL = 13;
		const int WM_KEYDOWN     = 0x100;
		const int WM_KEYUP       = 0x101;
		const int WM_SYSKEYDOWN  = 0x104;
		const int WM_SYSKEYUP    = 0x105;
        #endregion

        #region Instance Variables
        public double cntrlAltShftActiveRng = 10.0d; // seconds control + alt+ shift key down must be active.
        public double controlKeyTS = 0;
        public bool   controlKey   = false;

        public double altlKeyTS  = 0;
        public bool   altKey     = false;

        public double shiftKeyTS = 0;
        public bool   shiftKey   = false;

        public keyboardHookProc kbHookProc = null;

        /// <summary>
        /// The collections of keys to watch for
        /// </summary>
        public List<Keys> HookedKeys = new List<Keys>();
		/// <summary>
		/// Handle to the hook, need this to unhook and call the next hook
		/// </summary>
		IntPtr hhook = IntPtr.Zero;
		#endregion

		#region Events
		/// <summary>
		/// Occurs when one of the hooked keys is pressed
		/// </summary>
		public event KeyEventHandlerExtended KeyDown;
		/// <summary>
		/// Occurs when one of the hooked keys is released
		/// </summary>
		public event KeyEventHandlerExtended KeyUp;
		#endregion

		#region Constructors and Destructors
		/// <summary>
		/// Initializes a new instance of the <see cref="globalKeyboardHook"/> class and installs the keyboard hook.
		/// </summary>
		public globalKeyboardHook() {
			hook();
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="globalKeyboardHook"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
		/// </summary>
		~globalKeyboardHook() {
			unhook();
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Installs the global hook
		/// </summary>
		public void hook() {
			IntPtr hInstance = LoadLibrary("User32");
            kbHookProc = new keyboardHookProc(this.hookProc);
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, kbHookProc, hInstance, 0);
		}

		/// <summary>
		/// Uninstalls the global hook
		/// </summary>
		public void unhook() {
			UnhookWindowsHookEx(hhook);
		}

		/// <summary>
		/// The callback for the keyboard hook
		/// </summary>
		/// <param name="code">The hook code, if it isn't >= 0, the function shouldn't do anyting</param>
		/// <param name="wParam">The event type</param>
		/// <param name="lParam">The keyhook event information</param>
		/// <returns></returns>
		public int hookProc(int code, int wParam, ref keyboardHookStruct lParam) {
			if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                switch (key)
                {
                    case Keys.LControlKey:
                    case Keys.RControlKey:
                        controlKey   = (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN);
                        if (controlKey)
                            controlKeyTS = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
                        else
                            controlKeyTS = 0;
                        // Debug.Write("control " + controlKey.ToString() + "\n");
                        break;

                    case Keys.LShiftKey:
                    case Keys.RShiftKey:
                        shiftKey = (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN);
                        if (shiftKey)
                            shiftKeyTS = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
                        else
                            shiftKeyTS = 0;
                        // Debug.Write("shift " + shiftKey.ToString() + "\n");
                        break;

                    case Keys.LMenu:
                    case Keys.RMenu:
                        altKey = (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN);
                        if (altKey)
                            altlKeyTS = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
                        else
                            altlKeyTS = 0;

                        // Debug.Write("alt " + altKey.ToString() + "\n");
                        break;
                }

				if (HookedKeys.Contains(key))
                {
                    KeyEventArgs kea = new KeyEventArgs(key);

                    if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN) && (KeyDown != null))
                    {
                        // Test if shift, alt, or control key modify is stale and may be an inconsistent result.
                        double now = (DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;
                        if (altKey && ((now - altlKeyTS) > cntrlAltShftActiveRng))
                        {
                            //Debug.Write("alt override \n");
                            altKey = false;
                        }
                        if (shiftKey && ((now - shiftKeyTS) > cntrlAltShftActiveRng))
                        {
                            //Debug.Write("shift override \n");
                            shiftKey = false;
                        }
                        if (controlKey && ((now - controlKeyTS) > cntrlAltShftActiveRng))
                        {
                            //Debug.Write("control override \n");
                            controlKey = false;
                        }
                        KeyDown(this, kea, shiftKey, controlKey, altKey);
                    }
                    else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null))
                    {
                        KeyUp(this, kea, shiftKey, controlKey, altKey);
                    }
                    if (kea.Handled)
						return 1;
				}
			}
			return CallNextHookEx(hhook, code, wParam, ref lParam);
		}
		#endregion

		#region DLL imports
		/// <summary>
		/// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null
		/// </summary>
		/// <param name="idHook">The id of the event you want to hook</param>
		/// <param name="callback">The callback.</param>
		/// <param name="hInstance">The handle you want to attach the event to, can be null</param>
		/// <param name="threadId">The thread you want to attach the event to, can be null</param>
		/// <returns>a handle to the desired hook</returns>
		[DllImport("user32.dll")]
		static extern IntPtr SetWindowsHookEx(int idHook, keyboardHookProc callback, IntPtr hInstance, uint threadId);

		/// <summary>
		/// Unhooks the windows hook.
		/// </summary>
		/// <param name="hInstance">The hook handle that was returned from SetWindowsHookEx</param>
		/// <returns>True if successful, false otherwise</returns>
		[DllImport("user32.dll")]
		static extern bool UnhookWindowsHookEx(IntPtr hInstance);

		/// <summary>
		/// Calls the next hook.
		/// </summary>
		/// <param name="idHook">The hook id</param>
		/// <param name="nCode">The hook code</param>
		/// <param name="wParam">The wparam.</param>
		/// <param name="lParam">The lparam.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref keyboardHookStruct lParam);

		/// <summary>
		/// Loads the library.
		/// </summary>
		/// <param name="lpFileName">Name of the library</param>
		/// <returns>A handle to the library</returns>
		[DllImport("kernel32.dll")]
		static extern IntPtr LoadLibrary(string lpFileName);
		#endregion
	}
}
