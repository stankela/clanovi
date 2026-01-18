using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RawInput_dll
{
    public class PreMessageFilter : IMessageFilter
    {
        // Structure of a single record that will be saved in the filterBuffer
        public class FilterRecord
        {
            public int virtualKeyCode;
            public bool filterNextMessage;

            public FilterRecord(int _virtualKeyCode, bool _filterNextMessage)
            {
                virtualKeyCode = _virtualKeyCode;
                filterNextMessage = _filterNextMessage;
            }
        };

        Queue<FilterRecord> filterBuffer = new Queue<FilterRecord>();
        readonly object _lock = new object();

        // true  to filter the message and stop it from being dispatched 
        // false to allow the message to continue to the next filter or control.
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != Win32.WM_KEYDOWN && m.Msg != Win32.WM_KEYUP && m.Msg != Win32.WM_INPUT)
            {
                return false;
            }
            int keyCode = (int)m.WParam;  // za WM_INPUT: 0 foreground, 1 background
                                          // za WM_KEYDOWN i WM_KEYUP: virtual key
            //Console.WriteLine("PreMessageFilter:  " + $"KeyCode:{keyCode}  Message:{m.Msg}");

            int vkey;
            if (m.Msg == Win32.WM_INPUT)
            {
                // Za WM_INPUT, RawInput.GetDeviceNameAndVKey daje validne podatke 
                string deviceName;
                RawInput.GetDeviceNameAndVKey(m.LParam, out deviceName, out vkey);
                if (keyCode == 0) 
                {
                    // Application is in foreground. Bice poslane dodatne WM_KEYDOWN i WM_KEYUP poruke za dati
                    // vkey (nakon WM_INPUT), koje treba filtrirati ako dolaze sa RFID uredjaja. Tj za foreground
                    // se za svaki pritisak i otpustanje tastera generise sledeci niz poruka:
                    //   WM_INPUT 
                    //   WM_KEYDOWN
                    //   WM_INPUT
                    //   WM_KEYUP
                    bool filterNextMessage = deviceName.IndexOf("HID Keyboard Device") != -1;
                    lock (_lock)
                    {
                        filterBuffer.Enqueue(new FilterRecord(vkey, filterNextMessage));
                    }
                }
                else // keyCode == 1
                {
                    // Application is in background. Nece biti poslane dodatne WM_KEYDOWN i WM_KEYUP poruke, pa nema
                    // sta da se filtrira. Tj za background svaki pritisak i otpustanje tastera generise sledeci niz
                    // poruka:
                    //   WM_INPUT 
                    //   WM_INPUT
                }
                // Vracam false, zato sto je potrebno da se izvrsi RawInput.WndProc(), odakle ce biti pozvan
                // _keyboardDriver.ProcessRawInput i generisan KeyPressed event.
                return false;
            }
            else
            {
                // Za WM_KEYDOWN i WM_KEYUP, RawInput.GetDeviceNameAndVKey ne daje validne podatke. Dakle, nije
                // moguce saznati device name, tj da li treba filtrirati poruku. Zato koristim filterBuffer

                vkey = keyCode;
                // Search the buffer for the matching record
                int index = 0;
                FilterRecord found = null;
                lock (_lock)
                {
                    foreach (FilterRecord r in filterBuffer)
                    {
                        if (r.virtualKeyCode == vkey)
                        {
                            found = r;
                            break;
                        }
                        ++index;
                    }
                    if (found != null)
                    {
                        // Remove this and all preceding records from the buffer
                        for (int i = 0; i <= index; ++i)
                        {
                            filterBuffer.Dequeue();
                        }
                    }
                }
                return found != null && found.filterNextMessage;
            }
        }
    }
}
