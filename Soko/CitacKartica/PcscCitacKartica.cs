using Mifare1kTest;
using PCSC;
using PCSC.Iso7816;
using RFID.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soko
{
    // Citac kartica koji koristi pcsc-sharp library
    public class PcscCitacKartica : CitacKartica
    {
        private string readerName;

        public PcscCitacKartica()
        {
        }

        private void findContactlessReader()
        {
            using (var context = ContextFactory.Instance.Establish(SCardScope.System))
            {
                foreach (var reader in context.GetReaders())
                {
                    if (reader.Contains("CL"))
                    {
                        readerName = reader;
                    }
                }
            }
        }

        private string readSerialNumberCommand(IIsoReader reader)
        {
            var card = new MifareCard(reader);
            var result = card.ReadUID();
            return (result != null) ? RFID.NativeInterface.RFIDReader.ToHexString(result) : null;
        }

        private void LoadKeyWithoutSecureSession(MifareCard card, string key, byte keySlot)
        {
            var loadKeySuccessful = card.LoadKey(
                KeyStructure.NonVolatileMemory,
                keySlot, RFID.NativeInterface.RFIDReader.ToDigitsBytes(key)
            );

            if (!loadKeySuccessful)
            {
                throw new Exception("LOAD KEY failed.");
            }
        }

        private string readBlock(MifareCard card, byte blockNumber, byte keySlot)
        {
            try
            {
                var authSuccessful = card.Authenticate(0x00, blockNumber, KeyType.KeyA, keySlot);
                if (!authSuccessful)
                {
                    throw new Exception("AUTHENTICATE failed.");
                }

                var result = card.ReadBinary(0x00, blockNumber, 16);
                return (result != null) ? RFID.NativeInterface.RFIDReader.ToHexString(result) : null;
            }
            catch (Exception)
            {
                return "";
            }
        }

        private ulong ReadDataCardImpl(IIsoReader reader, string serialNumber, out string sID,
            out string sName)
        {
            sID = "";
            sName = "";

            var card = new MifareCard(reader);

            byte keySlot = 0x02;
            LoadKeyWithoutSecureSession(card, KEY, keySlot);

            string blockData = readBlock(card, 0x0c, keySlot);
            TipKartice tipKartice = getTipKartice(blockData);

            //System.Windows.Forms.MessageBox.Show(blockData);

            if (tipKartice == TipKartice.Prazna || tipKartice == TipKartice.Panonit)
                return 2;

            // TipKartice je NoviFormat

            blockData = readBlock(card, 0x14, keySlot);
            if (String.IsNullOrEmpty(blockData))
                return 0;
            //LogSector(sectorNo, data);
            sName = DecodeName(blockData);
            Log.Info("sName", sName);  // TODO4: Iskljuci Log od readera kada sve podesis

            blockData = readBlock(card, 0x06, keySlot);
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
            try
            {
                using (var context = ContextFactory.Instance.Establish(SCardScope.System))
                {
                    using (var isoReader = new IsoReader(
                        context: context,
                        readerName: readerName,
                        mode: SCardShareMode.Shared,
                        protocol: SCardProtocol.Any,
                        releaseContextOnDispose: false))
                    {
                        serialNumber = readSerialNumberCommand(isoReader);
                        return ReadDataCardImpl(isoReader, serialNumber, out sID, out sName);
                    }
                }
            }
            catch (Exception e)
            {
                readerName = null;
                Console.WriteLine(e.Message);
                return 0;
            }
        }

        private bool writeBlock(MifareCard card, byte blockNumber, byte keySlot, string data)
        {
            try
            {
                var authSuccessful = card.Authenticate(0x00, blockNumber, KeyType.KeyA, keySlot);
                if (!authSuccessful)
                {
                    throw new Exception("AUTHENTICATE failed.");
                }

                var updateSuccessful = card.UpdateBinary(0x00, blockNumber,
                    RFID.NativeInterface.RFIDReader.ToDigitsBytes(data));
                if (!updateSuccessful)
                {
                    throw new Exception("UPDATE BINARY failed.");
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private ulong SetKeys(MifareCard card, byte keySlot, string keyBlock)
        {
            // TODO4: Kod svih OMNIKEY komandi (writeBlock, readBlock, itd) bi trebalo proveravati rezultat proverom
            // kodova: 9000 - ok, 6982 - fail

            // Write key to blocks 7, 15 and 23
            // keySlot - slot koji sadrzi trenutni key, keyBlock - blok sa novim key
            if (!writeBlock(card, 7, keySlot, keyBlock))
                return 0;
            Log.Info("Write key to block 7:", keyBlock);
            if (!writeBlock(card, 15, keySlot, keyBlock))
                return 0;
            Log.Info("Write key to block 15:", keyBlock);
            if (!writeBlock(card, 23, keySlot, keyBlock))
                return 0;
            Log.Info("Write key to block 23:", keyBlock);
            return 1;
        }

        private ulong SetData(MifareCard card, string sID, string sName, string CurrentCardNo, byte keySlot)
        {
            // sID

            // write encrypted sID to block 6
            string value = EncodeID(sID, CurrentCardNo);
            if (!writeBlock(card, 6, keySlot, value))
                return 0;
            Log.Info("Write data to block 6:", value);

            // sName

            // write "SDV" to block 20
            if (sName != "SDV")
                throw new Exception("Greska u programu.");
            if (!writeBlock(card, 20, keySlot, SDV_BLOCK))
                return 0;
            Log.Info("Write data to block 20:", SDV_BLOCK);

            // Napravi da kartica bude u formatu TipKartice.NoviFormat - write "SDV2023" to block 12
            if (!writeBlock(card, 12, keySlot, SDV2023_BLOCK))
                return 0;
            Log.Info("Write data to block 12:", SDV2023_BLOCK);

            return 1;
        }

        // Konvertuje iz TipKartice.NoviFormat u TipKartice.Prazna
        private ulong WritePraznaDataCard(MifareCard card, byte keySlot, byte defaultKeySlot)
        {
            try
            {
                // write default key to blocks 7, 15 and 23
                if (SetKeys(card, keySlot, DEFAULT_KEY_BLOCK) != 1)
                    return 0;

                // write zero data to blocks 6, 12 and 20
                if (!writeBlock(card, 6, defaultKeySlot, ZERO_BLOCK))
                    return 0;
                Log.Info("Write data to block 6:", ZERO_BLOCK);
                if (!writeBlock(card, 12, defaultKeySlot, ZERO_BLOCK))
                    return 0;
                Log.Info("Write data to block 12:", ZERO_BLOCK);
                if (!writeBlock(card, 20, defaultKeySlot, ZERO_BLOCK))
                    return 0;
                Log.Info("Write data to block 20:", ZERO_BLOCK);
                return 1;
            }
            catch (Exception e)
            {
                HidGlobal.OK.SampleCodes.Utilities.ConsoleWriter.Instance.PrintError(e.Message);
                return 0;
            }
        }

        private ulong WriteDataCardImpl(IIsoReader reader, string serialNumber, string sID, string sName)
        {
            var card = new MifareCard(reader);

            byte keySlot = 0x02;
            LoadKeyWithoutSecureSession(card, KEY, keySlot);
            byte defaultKeySlot = 0x01;
            LoadKeyWithoutSecureSession(card, DEFAULT_KEY, defaultKeySlot);

            if (Options.Instance.WritePraznaDataCard)
            {
                string naslov = "WritePraznaDataCard";
                string pitanje = "Da li zelite da napravite praznu karticu?";
                UI.PotvrdaDialog dlg = new UI.PotvrdaDialog(naslov, pitanje);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    return WritePraznaDataCard(card, keySlot, defaultKeySlot);
            }

            if (SetKeys(card, defaultKeySlot, KEY_BLOCK) == 1)  // Prazna kartica
                return SetData(card, sID, sName, serialNumber, keySlot);

            // NoviFormat ili Panonit kartica.
            //System.Windows.Forms.MessageBox.Show("NoviFormat ili Panonit");

            //Thread.Sleep(200);
            return SetData(card, sID, sName, serialNumber, keySlot);
        }

        protected override ulong WriteDataCard(string sID, string sName, out Int64 serialCardNo)
        {
            serialCardNo = -1;
            if (String.IsNullOrEmpty(readerName))
            {
                findContactlessReader();
                // TODO4: Ovaj deo fali u WriteDataCard kod OMNIKEY citaca. Proveri da li treba ili ne (i ovde i kod
                // OMNIKEY)
                if (String.IsNullOrEmpty(readerName))
                {
                    // Citac kartica nije prikljucen, pa je readerName ostao null
                    return 0;
                }
            }
            try
            {
                using (var context = ContextFactory.Instance.Establish(SCardScope.System))
                {
                    using (var isoReader = new IsoReader(
                        context: context,
                        readerName: readerName,
                        mode: SCardShareMode.Shared,
                        protocol: SCardProtocol.Any,
                        releaseContextOnDispose: false))
                    {
                        string serialNumber = readSerialNumberCommand(isoReader);
                        serialCardNo = Convert.ToInt64(serialNumber, 16);
                        return WriteDataCardImpl(isoReader, serialNumber, sID, sName);
                    }
                }
            }
            catch (Exception e)
            {
                readerName = null;
                Console.WriteLine(e.Message);
                return 0;
            }
        }
    }
}
