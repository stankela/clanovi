using System;
using RFID.NativeInterface;
using RFID.Utils;

namespace Soko
{
    public class PanonitCitacKartica : CitacKartica
    {
        private RFIDReader mReader;

        private int comPort;

        public void SetComPort(int comPort)
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

        private ulong ReadDataCardImpl(out string sID, out string sName)
        {
            sID = "";
            sName = "";

            if (!mReader.ConnectDevice(comPort) || !mReader.BindCard(false))
                return 0;

            int sectorNo = 3;
            RFIDReader.RFIDSectorData data = mReader.ReadData(sectorNo, false, KEY);
            if (data != null)
            {
                LogSector(sectorNo, data);
            }
            TipKartice tipKartice = getTipKartice(data != null ? data.Block0 : null);

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

        protected override ulong ReadDataCard(out string sID, out string sName, out string serialNumber)
        {
            serialNumber = "";

            ulong retval = ReadDataCardImpl(out sID, out sName);
            if (retval == 1)
                mReader.Beep();
            return retval;
        }

        private ulong SetKeys(string password)
        {
            // Write key to blocks 7, 15 and 23
            if (!mReader.WriteDataToBlock(1, 3, false, password, KEY_BLOCK))
                return 0;
            Log.Info("Write key to block 7:", KEY_BLOCK);
            if (!mReader.WriteDataToBlock(3, 3, false, password, KEY_BLOCK))
                return 0;
            Log.Info("Write key to block 15:", KEY_BLOCK);
            if (!mReader.WriteDataToBlock(5, 3, false, password, KEY_BLOCK))
                return 0;
            Log.Info("Write key to block 23:", KEY_BLOCK);
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
            Log.Info("Write data to block 20:", SDV_BLOCK);
            if (!mReader.WriteDataToBlock(5, 0, false, KEY, SDV_BLOCK))
                return 0;

            // Napravi da kartica bude u formatu TipKartice.NoviFormat - write "SDV2023" to block 12
            Log.Info("Write data to block 12:", SDV2023_BLOCK);
            if (!mReader.WriteDataToBlock(3, 0, false, KEY, SDV2023_BLOCK))
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

            if (SetKeys(DEFAULT_KEY) == 1)  // Prazna kartica
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
            // TODO4: Mislim da ovde treba upisivati default key (tako je kod OMNIKEY 5422)
            // Write key to blocks 7, 15 and 23
            if (SetKeys(KEY) == 0)
                return 0;

            // write zero data to blocks 6, 12 and 20
            Log.Info("Write data to block 6:", ZERO_BLOCK);
            if (!mReader.WriteDataToBlock(1, 2, false, DEFAULT_KEY, ZERO_BLOCK))
                return 0;
            Log.Info("Write data to block 12:", ZERO_BLOCK);
            if (!mReader.WriteDataToBlock(3, 0, false, DEFAULT_KEY, ZERO_BLOCK))
                return 0;
            Log.Info("Write data to block 20:", ZERO_BLOCK);
            if (!mReader.WriteDataToBlock(5, 0, false, DEFAULT_KEY, ZERO_BLOCK))
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