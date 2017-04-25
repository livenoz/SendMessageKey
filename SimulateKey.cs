﻿using System;
using System.Runtime.InteropServices;

namespace SendMessageKey
{
    public class SimulateKey
    {
        private const uint _lParamKeyDown = 0x001E0001;

        private const uint _lParamChar = 0x001E0001;

        private const uint _lParamKeyUp = 0xC01E0001;

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, uint wMsg, uint wParam, uint lParam);

        public static bool SendText(IntPtr hWnd, string text)
        {
            var isSuccess = false;
            foreach (var item in text)
            {
                isSuccess = SendChar(hWnd, item);
                if (!isSuccess)
                {
                    break;
                }
            }
            return isSuccess;
        }


        public static void ClearText(IntPtr hw, int length = 20)
        {
            for (int i = 0; i < length; i++)
            {
                SendMessage(hw, MessageCode.WM_KEYDOWN, VirtualKeyCode.BACK, _lParamKeyDown);
                SendMessage(hw, MessageCode.WM_CHAR, VirtualKeyCode.BACK, _lParamChar);
                SendMessage(hw, MessageCode.WM_KEYUP, VirtualKeyCode.BACK, _lParamKeyUp);
            }
        }

        private static bool SendChar(IntPtr hWnd, char character)
        {
            var result = true;
            const uint deltaUppercaseAndLowercase = 32;
            var charCode = ConvertCharToInt(character);
            var isNumber = charCode >= VirtualKeyCode.VK_0 && charCode <= VirtualKeyCode.VK_9;
            var isUppercase = charCode >= VirtualKeyCode.VK_A && charCode <= VirtualKeyCode.VK_Z;
            var isLowercase = charCode >= VirtualKeyCode.VK_A + deltaUppercaseAndLowercase && charCode <= VirtualKeyCode.VK_Z + deltaUppercaseAndLowercase;
            if (isNumber || isUppercase || isLowercase)
            {
                uint wParamKey = charCode;
                uint wParamChar = isLowercase ? (charCode - deltaUppercaseAndLowercase) : charCode;
                SendMessage(hWnd, MessageCode.WM_KEYDOWN, wParamKey, _lParamKeyDown);
                SendMessage(hWnd, MessageCode.WM_CHAR, wParamChar, _lParamChar);
                SendMessage(hWnd, MessageCode.WM_KEYUP, wParamKey, _lParamKeyUp);
            }
            else
            {
                result = false;
            }
            return result;
        }

        private static uint ConvertCharToInt(char character)
        {
            return (uint)(character - '0') + VirtualKeyCode.VK_0;
        }

    }
}
