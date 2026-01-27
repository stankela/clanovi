using System;
using HidGlobal.OK.Readers;
using HidGlobal.OK.Readers.AViatoR.Components;
using HidGlobal.OK.Readers.Components;
using HidGlobal.OK.SampleCodes.Utilities;
using RFID.Utils;

namespace Soko
{
    public class OMNIKEY5422CitacKartica : CitacKartica
    {
        private string readerName;

        public OMNIKEY5422CitacKartica()
        {
        }

        private void findContactlessReader()
        {
            foreach (var reader in ContextHandler.Instance.ListReaders())
            {
                if (reader.Contains("CL"))
                {
                    readerName = reader;
                }
            }
        }

        private string readSerialNumberCommand(ISmartCardReader reader)
        {
            //ReaderHelper.GetDataCommand(reader, "Get Data Command", GetDataCommand.Type.Default);
            var getData = new GetDataCommand();
            string input = getData.GetApdu(GetDataCommand.Type.Default);  // Sending command: FFCA000000
            string output = reader.Transmit(input);  // Example output: DD3C0D2B9000
            return output.Substring(0, output.Length - 4);  // DD3C0D2B
        }

        private void LoadKeyCommand(ISmartCardReader smartCardReader, string description, byte keySlot, LoadKeyCommand.KeyType keyType, LoadKeyCommand.Persistence persistence, LoadKeyCommand.Transmission transmission, LoadKeyCommand.KeyLength keyLength, string key)
        {
            var loadKeyCommand = new HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand();

            string input = loadKeyCommand.GetApdu(keySlot, keyType, persistence, transmission, keyLength, key);
            string output = ReaderHelper.SendCommand(smartCardReader, input);
            ConsoleWriter.Instance.PrintCommand(description + key, input, output);
        }

        private void LoadKeyWithoutSecureSession(ISmartCardReader reader, string key, byte keySlot)
        {
            LoadKeyCommand(reader, "Load Mifare Key: ", keySlot,
                HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand.KeyType.CardKey,
                HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand.Persistence.Persistent,
                HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand.Transmission.Plain,
                HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand.KeyLength._6Bytes, key);
        }

        private string readBlock(ISmartCardReader reader, byte blockNumber, byte keySlot)
        {
            try
            {
                ReaderHelper.GeneralAuthenticateMifare(reader, "Authenticate with key from slot nr ", blockNumber,
                    GeneralAuthenticateCommand.MifareKeyType.MifareKeyA, keySlot);
                string result = ReaderHelper.ReadBinaryMifareCommand(reader, "Read Binary block nr ", blockNumber, 0x00);
                // Ukloni 4 trailing bajta koji oznacavaju success code (9000 - ok, 6982 - fail). Kada je rezultat
                // fail, blockData ce sadrzavati samo ta cetiri bajta, tj blockData ce nakon uklanjanja biti "";
                result = result.Substring(0, result.Length - 4);
                return result;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private ulong ReadDataCardImpl(ISmartCardReader reader, string serialNumber, out string sID,
            out string sName)
        {
            sID = "";
            sName = "";

            byte keySlot = 0x02;
            LoadKeyWithoutSecureSession(reader, KEY, keySlot);

            string blockData = readBlock(reader, 0x0c, keySlot);
            TipKartice tipKartice = getTipKartice(blockData);

            //System.Windows.Forms.MessageBox.Show(blockData);

            if (tipKartice == TipKartice.Prazna || tipKartice == TipKartice.Panonit)
                return 2;

            // TipKartice je NoviFormat

            blockData = readBlock(reader, 0x14, keySlot);
            if (String.IsNullOrEmpty(blockData))
                return 0;
            //LogSector(sectorNo, data);
            sName = DecodeName(blockData);
            Log.Info("sName", sName);  // TODO4: Iskljuci Log od readera kada sve podesis

            blockData = readBlock(reader, 0x06, keySlot);
            if (String.IsNullOrEmpty(blockData))
                return 0;
            //LogSector(sectorNo, data);
            sID = DecodeID(blockData, serialNumber);
            Log.Info("sID", sID);

            return 1;
        }

        protected override ulong ReadDataCard(out string sID, out string sName, out string serialNumber)
        {
            sID = "";
            sName = "";
            serialNumber = "";

            if (String.IsNullOrEmpty(readerName))
            {
                findContactlessReader();
                if (String.IsNullOrEmpty(readerName))
                {
                    // Citac kartica nije prikljucen, pa je readerName ostao null
                    return 0;
                }
            }
            var reader = new SmartCardReader(readerName);
            try
            {
                //Console.WriteLine($"Connecting to {reader.PcscReaderName}");
                ReaderHelper.ConnectToReaderWithCard(reader);
                if (!reader.IsConnected)
                {
                    return 0;
                }
                //Console.WriteLine($"Connected\nConnection Mode: {reader.ConnectionMode}");

                serialNumber = readSerialNumberCommand(reader);
                return ReadDataCardImpl(reader, serialNumber, out sID, out sName);
            }
            catch (Exception e)
            {
                readerName = null;
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                if (reader.IsConnected)
                {
                    reader.Disconnect(CardDisposition.Unpower);
                }
            }
        }

        private bool writeBlock(ISmartCardReader reader, byte blockNumber, byte keySlot, string data)
        {
            try
            {
                ReaderHelper.GeneralAuthenticateMifare(reader, "Authenticate with key from slot nr ", blockNumber,
                    GeneralAuthenticateCommand.MifareKeyType.MifareKeyA, keySlot);
                ReaderHelper.UpdateBinaryCommand(reader, "Update Binary block nr ", UpdateBinaryCommand.Type.Plain,
                    blockNumber, data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private ulong SetKeys(ISmartCardReader reader, byte keySlot, string keyBlock)
        {
            // TODO4: Kod svih OMNIKEY komandi (writeBlock, readBlock, itd) bi trebalo proveravati rezultat proverom
            // kodova: 9000 - ok, 6982 - fail

            // Write key to blocks 7, 15 and 23
            // keySlot - slot koji sadrzi trenutni key, keyBlock - blok sa novim key
            if (!writeBlock(reader, 7, keySlot, keyBlock))
                return 0;
            Log.Info("Write key to block 7:", keyBlock);
            if (!writeBlock(reader, 15, keySlot, keyBlock))
                return 0;
            Log.Info("Write key to block 15:", keyBlock);
            if (!writeBlock(reader, 23, keySlot, keyBlock))
                return 0;
            Log.Info("Write key to block 23:", keyBlock);
            return 1;
        }

        private ulong SetData(ISmartCardReader reader, string sID, string sName, string CurrentCardNo, byte keySlot)
        {
            // sID

            // write encrypted sID to block 6
            string value = EncodeID(sID, CurrentCardNo);
            if (!writeBlock(reader, 6, keySlot, value))
                return 0;
            Log.Info("Write data to block 6:", value);

            // sName

            // write "SDV" to block 20
            if (sName != "SDV")
                throw new Exception("Greska u programu.");
            if (!writeBlock(reader, 20, keySlot, SDV_BLOCK))
                return 0;
            Log.Info("Write data to block 20:", SDV_BLOCK);

            // Napravi da kartica bude u formatu TipKartice.NoviFormat - write "SDV2023" to block 12
            if (!writeBlock(reader, 12, keySlot, SDV2023_BLOCK))
                return 0;
            Log.Info("Write data to block 12:", SDV2023_BLOCK);

            return 1;
        }

        // Konvertuje iz TipKartice.NoviFormat u TipKartice.Prazna
        private ulong WritePraznaDataCard(ISmartCardReader reader, byte keySlot, byte defaultKeySlot)
        {
            try
            {
                // write default key to blocks 7, 15 and 23
                if (SetKeys(reader, keySlot, DEFAULT_KEY_BLOCK) != 1)
                    return 0;

                // write zero data to blocks 6, 12 and 20
                if (!writeBlock(reader, 6, defaultKeySlot, ZERO_BLOCK))
                    return 0;
                Log.Info("Write data to block 6:", ZERO_BLOCK);
                if (!writeBlock(reader, 12, defaultKeySlot, ZERO_BLOCK))
                    return 0;
                Log.Info("Write data to block 12:", ZERO_BLOCK);
                if (!writeBlock(reader, 20, defaultKeySlot, ZERO_BLOCK))
                    return 0;
                Log.Info("Write data to block 20:", ZERO_BLOCK);
                return 1;
            }
            catch (Exception e)
            {
                ConsoleWriter.Instance.PrintError(e.Message);
                return 0;
            }
            finally
            {
                if (reader.IsConnected)
                {
                    reader.Disconnect(CardDisposition.Unpower);
                }
            }
        }

        private ulong WriteDataCardImpl(ISmartCardReader reader, string serialNumber, string sID, string sName)
        {
            byte keySlot = 0x02;
            LoadKeyWithoutSecureSession(reader, KEY, keySlot);
            byte defaultKeySlot = 0x01;
            LoadKeyWithoutSecureSession(reader, DEFAULT_KEY, defaultKeySlot);

            if (Options.Instance.WritePraznaDataCard)
            {
                string naslov = "WritePraznaDataCard";
                string pitanje = "Da li zelite da napravite praznu karticu?";
                UI.PotvrdaDialog dlg = new UI.PotvrdaDialog(naslov, pitanje);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    return WritePraznaDataCard(reader, keySlot, defaultKeySlot);
            }

            if (SetKeys(reader, defaultKeySlot, KEY_BLOCK) == 1)  // Prazna kartica
                return SetData(reader, sID, sName, serialNumber, keySlot);

            // NoviFormat ili Panonit kartica.
            System.Windows.Forms.MessageBox.Show("NoviFormat ili Panonit");

            //Thread.Sleep(200);
            return SetData(reader, sID, sName, serialNumber, keySlot);
        }

        protected override ulong WriteDataCard(string sID, string sName, out Int64 serialCardNo)
        {
            serialCardNo = -1;
            if (String.IsNullOrEmpty(readerName))
            {
                findContactlessReader();
            }
            var reader = new SmartCardReader(readerName);
            try
            {
                //Console.WriteLine($"Connecting to {reader.PcscReaderName}");
                ReaderHelper.ConnectToReaderWithCard(reader);
                if (!reader.IsConnected)
                {
                    return 0;
                }
                //Console.WriteLine($"Connected\nConnection Mode: {reader.ConnectionMode}");

                string serialNumber = readSerialNumberCommand(reader);
                serialCardNo = Convert.ToInt64(serialNumber, 16);
                return WriteDataCardImpl(reader, serialNumber, sID, sName);
            }
            catch (Exception e)
            {
                readerName = null;
                Console.WriteLine(e.Message);
                return 0;
            }
            finally
            {
                if (reader.IsConnected)
                {
                    reader.Disconnect(CardDisposition.Unpower);
                }
            }
        }
    }
}