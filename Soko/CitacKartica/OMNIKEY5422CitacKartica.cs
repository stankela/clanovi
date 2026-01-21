using System;
using System.Threading;
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
            findContactlessReader();
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

        private void readSerialNumber(out string serijskiBroj)
        {
            serijskiBroj = "-1";
            lock (readAndWriteLock)
            {
                if (String.IsNullOrEmpty(readerName))
                {
                    findContactlessReader();
                    if (String.IsNullOrEmpty(readerName))
                    {
                        return;
                    }
                }
                var reader = new SmartCardReader(readerName);
                try
                {
                    //Console.WriteLine($"Connecting to {reader.PcscReaderName}");
                    ReaderHelper.ConnectToReaderWithCard(reader);
                    if (!reader.IsConnected)
                    {
                        return;
                    }

                    //Console.WriteLine($"Connected\nConnection Mode: {reader.ConnectionMode}");

                    serijskiBroj = readSerialNumberCommand(reader);
                }
                catch (Exception e)
                {
                    readerName = null;
                    Console.WriteLine(e.Message);
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

        private void LoadKeyCommand(ISmartCardReader smartCardReader, string description, byte keySlot, LoadKeyCommand.KeyType keyType, LoadKeyCommand.Persistence persistence, LoadKeyCommand.Transmission transmission, LoadKeyCommand.KeyLength keyLength, string key)
        {
            var loadKeyCommand = new HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand();

            string input = loadKeyCommand.GetApdu(keySlot, keyType, persistence, transmission, keyLength, key);
            string output = ReaderHelper.SendCommand(smartCardReader, input);
            ConsoleWriter.Instance.PrintCommand(description + key, input, output);
        }

        private void LoadKeyWithoutSecureSession(string readerName, string key, byte keySlot)
        {
            var reader = new SmartCardReader(readerName);

            try
            {
                ConsoleWriter.Instance.PrintSplitter();
                ConsoleWriter.Instance.PrintTask($"Connecting to { reader.PcscReaderName}");

                ReaderHelper.ConnectToReader(reader);

                ConsoleWriter.Instance.PrintMessage($"Connected\nConnection Mode: {reader.ConnectionMode}");
                ConsoleWriter.Instance.PrintSplitter();

                LoadKeyCommand(reader, "Load Mifare Key: ", keySlot,
                    HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand.KeyType.CardKey,
                    HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand.Persistence.Persistent,
                    HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand.Transmission.Plain,
                    HidGlobal.OK.Readers.AViatoR.Components.LoadKeyCommand.KeyLength._6Bytes, key);

                ConsoleWriter.Instance.PrintSplitter();
            }
            catch (Exception e)
            {
                ConsoleWriter.Instance.PrintError(e.Message);
            }
            finally
            {
                if (reader.IsConnected)
                {
                    reader.Disconnect(CardDisposition.Unpower);
                    ConsoleWriter.Instance.PrintMessage("Reader connection closed");
                }
                ConsoleWriter.Instance.PrintSplitter();
            }
        }

        private string readBlock(ISmartCardReader reader, byte blockNumber, byte keySlot)
        {
            ReaderHelper.GeneralAuthenticateMifare(reader, "Authenticate with key from slot nr ", blockNumber,
                GeneralAuthenticateCommand.MifareKeyType.MifareKeyA, keySlot);
            return ReaderHelper.ReadBinaryMifareCommand(reader, "Read Binary block nr ", blockNumber, 0x00);
        }

        protected override ulong ReadDataCard(ref string sID, ref string sName, ref string serialNumber)
        {
            readSerialNumber(out serialNumber);
            if (serialNumber == "-1")
                return 0;

            byte keySlot = 0x02;
            LoadKeyWithoutSecureSession(readerName, KEY, keySlot);

            var reader = new SmartCardReader(readerName);

            try
            {
                ReaderHelper.ConnectToReaderWithCard(reader);
                if (!reader.IsConnected)
                {
                    return 0;
                }

                TipKartice tipKartice;
                string blockData = readBlock(reader, 0x0c, keySlot);
                // Ukloni 4 trailing bajta koji oznacavaju success code (9000 - ok, 6982 - fail). Kada je rezultat
                // fail, blockData ce sadrzavati samo ta cetiri bajta, tj blockData ce nakon uklanjanja biti "";
                blockData = blockData.Substring(0, blockData.Length - 4);
                if (String.IsNullOrEmpty(blockData) || blockData == "00000000000000000000000000000000")
                    tipKartice = TipKartice.Prazna;
                else
                {
                    //LogSector(sectorNo, data);
                    if (blockData == "53445632303233000000000000000000")
                        tipKartice = TipKartice.NoviFormat;
                    else
                        tipKartice = TipKartice.Panonit;
                }

                System.Windows.Forms.MessageBox.Show(blockData);

                if (tipKartice == TipKartice.Prazna || tipKartice == TipKartice.Panonit)
                    return 2;

                // TipKartice je NoviFormat

                blockData = readBlock(reader, 0x14, keySlot);
                blockData = blockData.Substring(0, blockData.Length - 4);
                if (String.IsNullOrEmpty(blockData))
                    return 0;
                //LogSector(sectorNo, data);
                sName = DecodeName(blockData);
                Log.Info("sName", sName);  // TODO4: Iskljuci Log od readera kada sve podesis

                blockData = readBlock(reader, 0x06, keySlot);
                blockData = blockData.Substring(0, blockData.Length - 4);
                if (String.IsNullOrEmpty(blockData))
                    return 0;
                //LogSector(sectorNo, data);
                sID = DecodeID(blockData, serialNumber);
                Log.Info("sID", sID);

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

        private void writeBlock(ISmartCardReader reader, byte blockNumber, byte keySlot, string data)
        {
            ReaderHelper.GeneralAuthenticateMifare(reader, "Authenticate with key from slot nr ", blockNumber,
                GeneralAuthenticateCommand.MifareKeyType.MifareKeyA, keySlot);
            ReaderHelper.UpdateBinaryCommand(reader, "Update Binary block nr ", UpdateBinaryCommand.Type.Plain,
                blockNumber, data);

        }

        private ulong SetKeys(ISmartCardReader reader, byte keySlot)
        {
            // Write key to blocks 7, 15 and 23. The structure of the key is as follows:
            // KEY is the A key for the sector. FF078069 are the access bits for that sector (they define what data
            // is modifiable with which key). DEFAULT_KEY is the B key.
            string keyBlock = KEY + "FF078069" + DEFAULT_KEY;
            writeBlock(reader, 7, keySlot, keyBlock);
            Log.Info("Write key to block 7:", keyBlock);
            writeBlock(reader, 15, keySlot, keyBlock);
            Log.Info("Write key to block 15:", keyBlock);
            writeBlock(reader, 23, keySlot, keyBlock);
            Log.Info("Write key to block 23:", keyBlock);
            return 1;
        }

        private ulong SetData(ISmartCardReader reader, string sID, string sName, string CurrentCardNo, byte keySlot)
        {
            // sID

            // write encrypted sID to block 6
            string value = EncodeID(sID, CurrentCardNo);
            Log.Info("Write data to block 6:", value);
            writeBlock(reader, 6, keySlot, value);

            // sName

            // write "SDV" to block 20
            if (sName != "SDV")
                throw new Exception("Greska u programu.");
            value = "53445600000000000000000000000000";
            Log.Info("Write data to block 20:", value);
            writeBlock(reader, 20, keySlot, value);

            // Napravi da kartica bude u formatu TipKartice.NoviFormat - write "SDV2023" to block 12
            value = "53445632303233000000000000000000";
            Log.Info("Write data to block 12:", value);
            writeBlock(reader, 12, keySlot, value);

            return 1;
        }

        // Konvertuje iz TipKartice.NoviFormat u TipKartice.Prazna
        private ulong WritePraznaDataCard(ISmartCardReader reader, byte keySlot, byte defaultKeySlot)
        {
            try
            {
                // write default key to blocks 7, 15 and 23
                string keyBlock = DEFAULT_KEY + "FF078069" + DEFAULT_KEY;
                writeBlock(reader, 7, keySlot, keyBlock);
                Log.Info("Write key to block 7:", keyBlock);
                writeBlock(reader, 15, keySlot, keyBlock);
                Log.Info("Write key to block 15:", keyBlock);
                writeBlock(reader, 23, keySlot, keyBlock);
                Log.Info("Write key to block 23:", keyBlock);

                // write zero data to blocks 6, 12 and 20
                string value = "00000000000000000000000000000000";
                Log.Info("Write data to block 6:", value);
                writeBlock(reader, 6, defaultKeySlot, value);
                Log.Info("Write data to block 12:", value);
                writeBlock(reader, 12, defaultKeySlot, value);
                Log.Info("Write data to block 20:", value);
                writeBlock(reader, 20, defaultKeySlot, value);

                //mReader.EnableCheck = true;  // TODO4: RAII ?
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

        protected override ulong WriteDataCard(string sID, string sName, out Int64 serialCardNo)
        {
            serialCardNo = -1;

            string serialNumber;
            readSerialNumber(out serialNumber);
            if (serialNumber == "-1")
                return 0;

            //mReader.EnableCheck = false;
            //Thread.Sleep(200);

            byte keySlot = 0x02;
            LoadKeyWithoutSecureSession(readerName, KEY, keySlot);
            byte defaultKeySlot = 0x01;
            LoadKeyWithoutSecureSession(readerName, DEFAULT_KEY, defaultKeySlot);

            var reader = new SmartCardReader(readerName);

            try
            {
                ReaderHelper.ConnectToReaderWithCard(reader);
                if (!reader.IsConnected)
                {
                    return 0;
                }

                if (Options.Instance.WritePraznaDataCard)
                    return WritePraznaDataCard(reader, keySlot, defaultKeySlot);

                serialCardNo = Convert.ToInt64(serialNumber, 16);

                if (SetKeys(reader, defaultKeySlot) == 1)  // Prazna kartica
                    return SetData(reader, sID, sName, serialNumber, keySlot);

                // NoviFormat ili Panonit kartica.

                Thread.Sleep(200);
                //if (!mReader.ConnectDevice(comPort) || !mReader.BindCard(false))
                //  return 0;
                return SetData(reader, sID, sName, serialNumber, keySlot);

                //mReader.EnableCheck = true;  // TODO4: RAII ?
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
    }
}