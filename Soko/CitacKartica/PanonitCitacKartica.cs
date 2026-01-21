using System;
using RFID.NativeInterface;
using RFID.Utils;

namespace Soko
{
    public class PanonitCitacKartica : CitacKartica
    {
        private RFIDReader mReader;

        private int comPort;

        public override void SetComPort(int comPort)
        {
            this.comPort = comPort;
        }

        public PanonitCitacKartica(int comPort)
        {
            this.comPort = comPort;
            mReader = RFIDReader.OpenRFIDReader();
        }

        // NOTE: Thread safe creation
        /*private static Object creationLock = new Object();
        public static CitacKartica Instance
        {
            get
            {
                lock (creationLock)
                {
                    if (instance == null)
                        instance = new CitacKartica();
                    return instance;
                }
            }
        }*/

        private void LogSector(int sectorNo, RFIDReader.RFIDSectorData data)
        {
            Log.Info("", sectorNo.ToString() + "----------------------------");
            Log.Info("", data.Block0);
            Log.Info("", data.Block1);
            Log.Info("", data.Block2);
            Log.Info("", data.PasswordA + " " + data.Control + " " + data.PasswordB);
        }

        private ulong ReadDataCardImpl(ref string sID, ref string sName)
        {
            if (!mReader.ConnectDevice(comPort) || !mReader.BindCard(false))
                return 0;

            TipKartice tipKartice;
            int sectorNo = 3;
            RFIDReader.RFIDSectorData data = mReader.ReadData(sectorNo, false, KEY);
            if (data == null)
                tipKartice = TipKartice.Prazna;
            else
            {
                LogSector(sectorNo, data);
                if (data.Block0 == "53445632303233000000000000000000")
                    tipKartice = TipKartice.NoviFormat;
                else if (data.Block0 == "00000000000000000000000000000000")
                    tipKartice = TipKartice.Prazna;
                else
                    tipKartice = TipKartice.Panonit;
            }

            if (tipKartice == TipKartice.Prazna || tipKartice == TipKartice.Panonit)
                return 2;

            // TipKartice je NoviFormat

            sectorNo = 5;
            data = mReader.ReadData(sectorNo, false, KEY);
            if (data == null)
                return 0;
            LogSector(sectorNo, data);
            sName = DecodeName(data.Block0);
            Log.Info("sName", sName);  // TODO4: Iskljuci Log od readera kada sve podesis

            sectorNo = 1;
            data = mReader.ReadData(sectorNo, false, KEY);
            if (data == null)
                return 0;
            LogSector(sectorNo, data);
            sID = DecodeID(data.Block2, mReader.CurrentCardNo);
            Log.Info("sID", sID);

            return 1;
        }

        protected override ulong ReadDataCard(ref string sID, ref string sName, ref string serialNumber)
        {
            ulong retval = ReadDataCardImpl(ref sID, ref sName);
            if (retval == 1)
                mReader.Beep();
            return retval;
        }

        private ulong SetKeys()
        {
            // Write key to blocks 7, 15 and 23. The structure of the key is as follows:
            // KEY is the A key for the sector. FF078069 are the access bits for that sector (they define what data
            // is modifiable with which key). DEFAULT_KEY is the B key.
            string keyBlock = KEY + "FF078069" + DEFAULT_KEY;
            if (!mReader.WriteDataToBlock(1, 3, false, DEFAULT_KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 7:", keyBlock);
            if (!mReader.WriteDataToBlock(3, 3, false, DEFAULT_KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 15:", keyBlock);
            if (!mReader.WriteDataToBlock(5, 3, false, DEFAULT_KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 23:", keyBlock);
            return 1;
        }

        private ulong SetData(string sID, string sName, string CurrentCardNo)
        {
            // sID

            // write encrypted sID to block 6
            string value = EncodeID(sID, CurrentCardNo);
            Log.Info("Write data to block 6:", value);
            if (!mReader.WriteDataToBlock(1, 2, false, KEY, value))
                return 0;

            // sName

            // write "SDV" to block 20
            if (sName != "SDV")
                throw new Exception("Greska u programu.");
            value = "53445600000000000000000000000000";
            Log.Info("Write data to block 20:", value);
            if (!mReader.WriteDataToBlock(5, 0, false, KEY, value))
                return 0;

            // Napravi da kartica bude u formatu TipKartice.NoviFormat - write "SDV2023" to block 12
            value = "53445632303233000000000000000000";
            Log.Info("Write data to block 12:", value);
            if (!mReader.WriteDataToBlock(3, 0, false, KEY, value))
                return 0;

            return 1;
        }

        private ulong WriteDataCardImpl(string sID, string sName, out Int64 serialCardNo)
        {
            //mReader.EnableCheck = false;
            //RFIDReader.Sleep(200);

            serialCardNo = 0;

            if (!mReader.ConnectDevice(comPort) || !mReader.BindCard(false))
                return 0;

            if (Options.Instance.WritePraznaDataCard)
                return WritePraznaDataCard();

            serialCardNo = Convert.ToInt64(mReader.CurrentCardNo, 16);

            if (SetKeys() == 1)  // Prazna kartica
                return SetData(sID, sName, mReader.CurrentCardNo);

            // NoviFormat ili Panonit kartica.

            RFIDReader.Sleep(200);
            if (!mReader.ConnectDevice(comPort) || !mReader.BindCard(false))
                return 0;
            return SetData(sID, sName, mReader.CurrentCardNo);

            //mReader.EnableCheck = true;  // TODO4: RAII ?
        }

        // Konvertuje iz TipKartice.NoviFormat u TipKartice.Prazna
        private ulong WritePraznaDataCard()
        {
            // write default key to blocks 7, 15 and 23
            string keyBlock = DEFAULT_KEY + "FF078069" + DEFAULT_KEY;
            if (!mReader.WriteDataToBlock(1, 3, false, KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 7:", keyBlock);
            if (!mReader.WriteDataToBlock(3, 3, false, KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 15:", keyBlock);
            if (!mReader.WriteDataToBlock(5, 3, false, KEY, keyBlock))
                return 0;
            Log.Info("Write key to block 23:", keyBlock);

            // write zero data to blocks 6, 12 and 20
            string value = "00000000000000000000000000000000";
            Log.Info("Write data to block 6:", value);
            if (!mReader.WriteDataToBlock(1, 2, false, DEFAULT_KEY, value))
                return 0;
            Log.Info("Write data to block 12:", value);
            if (!mReader.WriteDataToBlock(3, 0, false, DEFAULT_KEY, value))
                return 0;
            Log.Info("Write data to block 20:", value);
            if (!mReader.WriteDataToBlock(5, 0, false, DEFAULT_KEY, value))
                return 0;

            //mReader.EnableCheck = true;  // TODO4: RAII ?
            return 1;
        }

        protected override ulong WriteDataCard(string sID, string sName, out Int64 serialCardNo)
        {
            serialCardNo = 0;
            ulong retval = WriteDataCardImpl(sID, sName, out serialCardNo);
            if (retval == 1)
                mReader.Beep();
            return retval;
        }
    }
}