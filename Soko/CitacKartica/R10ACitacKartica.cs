using System;
using System.Collections.Concurrent;
using Soko.Domain;
using Soko.Misc;
using Soko.UI;
using RawInput_dll;

namespace Soko
{
    public class R10ACitacKartica : CitacKartica
    {
        public R10ACitacKartica()
        {
            Form1.Instance.CitacKarticaForm.GetRawInput().KeyPressed += OnKeyPressed;
        }

        public override void Cleanup()
        {
            Form1.Instance.CitacKarticaForm.GetRawInput().KeyPressed -= OnKeyPressed;
        }

        class KeyEvent
        {
            public string VKeyName;
            public string KeyPressState;

            public KeyEvent(string name, string state)
            {
                VKeyName = name;
                KeyPressState = state;
            }
        }

        private int index = 0;
        private KeyEvent[] buffer = null;  // circular buffer of last 28 digits
        private static ConcurrentQueue<string> concurrentQueue = new ConcurrentQueue<string>();

        private void OnKeyPressed(object sender, RawInputEventArg e)
        {
            if (buffer == null)
            {
                buffer = new KeyEvent[28];  // circular buffer of last 28 digits
                for (int i = 0; i < 28; ++i)
                {
                    buffer[i] = new KeyEvent("", "");
                }
            }
            buffer[index] = new KeyEvent(e.KeyPressEvent.VKeyName, e.KeyPressEvent.KeyPressState);
            index = (index + 1) % 28;
            if (e.KeyPressEvent.VKeyName != "ENTER" || e.KeyPressEvent.KeyPressState != "BREAK")
            {
                return;
            }
            // Detektovali smo zadnji karakter u nizu. Proveri svih 28 karaktera
            string input = checkInput(index);
            if (input != "")
            {
                concurrentQueue.Enqueue(input);
            }
        }

        private string checkInput(int index)
        {
            // Primer korektnog inputa je ";0722287837?", plus ENTER na kraju. Za svaki karakter imamo MAKE i BREAK.
            // MAKE predstavlja pritisak tastera, BREAK predstavlja otpustanje. Znaku '?' prethodi desni SHIFT
            // (tj. RSHIFT)
            string result = "";
            string[] vKeyNames = new string[28] {
                "OEMSEMICOLON",
                "OEMSEMICOLON",
                /* proizvoljnih deset cifara. svaka cifra se ponavlja dva puta (pritisak tastera i otpustanje */
                "D0","D0","D1","D1","D2","D2","D3","D3","D4","D4","D5","D5","D6","D6","D7","D7","D8","D8","D9","D9",
                "RSHIFT", "OEMQUESTION", "RSHIFT", "OEMQUESTION", "ENTER", "ENTER"
            };
            string[] vKeyPressStates = new string[28] {
                "MAKE",
                "BREAK",
                "MAKE","BREAK","MAKE","BREAK","MAKE","BREAK","MAKE","BREAK","MAKE","BREAK",
                "MAKE","BREAK","MAKE","BREAK","MAKE","BREAK","MAKE","BREAK","MAKE","BREAK",
                "MAKE", "MAKE", "BREAK", "BREAK", "MAKE", "BREAK"
            };
            for (int i = 0; i < 28; ++i, index = (index + 1) % 28)
            {
                KeyEvent e = buffer[index];
                if (e.KeyPressState != vKeyPressStates[i])
                    return "";
                if (i < 2 || i >= 22)
                {
                    if (i == 23 || i == 25)
                    {
                        // Ovaj slucaj obradjujem posebno, zato sto je na srpskoj tastaturi znak '?' u stvari '-',
                        // tj. umesto OEMQUESTION imamo OEMMINUS
                        // TODO4: Mozda da izostavim proveru za ova dva indeksa (23 i 25), zato sto bi za neku
                        // tastaturu koja nije ni srpska ni engleska mogla da se pojavi neka treca vrednost. Takodje,
                        // i za znak ';' bi nesto slicno moglo da se desi.
                        if (e.VKeyName != "OEMQUESTION" && e.VKeyName != "OEMMINUS")
                            return "";
                    }
                    else if (e.VKeyName != vKeyNames[i])
                        return "";
                }
                else
                {
                    if (!isDigit(e.VKeyName))
                        return "";
                    if (e.KeyPressState == "MAKE")
                    {
                        int nextIndex = (index + 1) % 28;
                        KeyEvent nextEv = buffer[nextIndex];
                        if (nextEv.KeyPressState != "BREAK")
                            return "";
                    }
                    else if (e.KeyPressState == "BREAK")
                    {
                        int prevIndex = index - 1;
                        if (prevIndex == -1)
                            prevIndex = 27;
                        KeyEvent prevEv = buffer[prevIndex];
                        if (prevEv.KeyPressState != "MAKE")
                            return "";
                        char digit = getDigit(e.VKeyName);
                        if (digit != getDigit(prevEv.VKeyName))
                            return "";
                        result += digit;
                    }
                    else
                        return "";
                }
            }
            return result;
        }

        private bool isDigit(string vKeyName)
        {
            if (vKeyName.Length != 2 || vKeyName[0] != 'D')
                return false;
            switch (vKeyName[1])
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return true;
                default:
                    return false;
            }
        }

        private char getDigit(string vKeyName)
        {
            return vKeyName[1];
        }

        private Int64 getBigEndianEquivalent(string serijskiBroj)
        {
            string hex = Int64.Parse(serijskiBroj).ToString("X");
            if (hex.Length < 8)
            {
                for (int i = 0; i < 8 - hex.Length; ++i)
                {
                    hex = "0" + hex;
                }
            }

            // Reverse the byte order by processing pairs in reverse and prepending them
            string reverseHex = "";
            for (int i = 0; i < hex.Length; i += 2)
            {
                reverseHex = hex.Substring(i, 2) + reverseHex;
            }

            return Convert.ToInt64(reverseHex, 16);
        }

        public override bool tryReadCard(out int broj, out string serijskiBroj)
        {
            broj = -1;
            if (!concurrentQueue.TryDequeue(out serijskiBroj))
            {
                return false;
            }

            // Decimalni broj koji se ocitava sa R10A citaca je dobijen little-endian converzijom hexadecimalnog
            // stringa za serijski broj koji se nalazi na kartici. Npr ako se na kartici nalazi string DD3C0D2B,
            // (DD je na indeksu 0), R10A ce ocitati 722287837 decimalno. Panonit i OMNIKEY citaci tretiraju
            // string kao big endian (DD je most significant bajt u broju), i konvertovace ga u 3711700267, i tako
            // smestiti u bazu podataka.
            Int64 serijskiBrojKartice = getBigEndianEquivalent(serijskiBroj);

            Clan clan = CitacKarticaDictionary.Instance.findClanBySerijskiBrojKartice(serijskiBrojKartice);
            if (clan != null)
            {
                broj = clan.BrojKartice.Value;
                return true;
            }
            return false;
        }

        public override void readCard(out int broj, out string serijskiBroj)
        {
            throw new NotImplementedException("Citac kartica R10A ne podrzava upisivanje");
        }

        public override bool writeCard(string sID, out long serialCardNo)
        {
            throw new NotImplementedException("Citac kartica R10A ne podrzava upisivanje");
        }
    }
}