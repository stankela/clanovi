using Mifare1kTest;
using PCSC;
using PCSC.Iso7816;
using System;
using System.Collections.Generic;

public class pcsc_examples
{
    public static void Mifare1kTest()
    {
        using (var context = ContextFactory.Instance.Establish(SCardScope.System))
        {
            var readerNames = context.GetReaders();
            if (IsEmpty(readerNames))
            {
                Console.Error.WriteLine("You need at least one reader in order to run this example.");
                Console.ReadKey();
                return;
            }

            var readerName = ChooseReader2(readerNames);
            if (readerName == null)
            {
                return;
            }

            using (var isoReader = new IsoReader(
                context: context,
                readerName: readerName,
                mode: SCardShareMode.Shared,
                protocol: SCardProtocol.Any,
                releaseContextOnDispose: false))
            {
                var card = new MifareCard(isoReader);

                var loadKeySuccessful = card.LoadKey(
                    KeyStructure.NonVolatileMemory,
                    0x00, // first key slot
                    new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF } // key
                    //new byte[] { 0x13, 0x07, 0x20, 0x04, 0xab, 0xcd } // key

                );

                if (!loadKeySuccessful)
                {
                    throw new Exception("LOAD KEY failed.");
                }

                var authSuccessful = card.Authenticate(MSB, LSB, KeyType.KeyA, 0x00);
                if (!authSuccessful)
                {
                    throw new Exception("AUTHENTICATE failed.");
                }

                var result = card.ReadBinary(MSB, LSB, 16);
                Console.WriteLine("Result (before BINARY UPDATE): {0}",
                    (result != null)
                        ? BitConverter.ToString(result)
                        : null);

                var updateSuccessful = card.UpdateBinary(MSB, LSB, DATA_TO_WRITE);

                if (!updateSuccessful)
                {
                    throw new Exception("UPDATE BINARY failed.");
                }

                result = card.ReadBinary(MSB, LSB, 16);
                Console.WriteLine("Result (after BINARY UPDATE): {0}",
                    (result != null)
                        ? BitConverter.ToString(result)
                        : null);
            }
        }

        Console.ReadKey();
    }

    private static bool IsEmpty(ICollection<string> readerNames) => readerNames == null || readerNames.Count < 1;

    private static readonly byte[] DATA_TO_WRITE = {
            0x0F, 0x0E, 0x0D, 0x0C, 0x0B, 0x0A, 0x09, 0x08, 0x07, 0x06, 0x05, 0x04, 0x03, 0x02, 0x01, 0x00
        };

    private const byte MSB = 0x00;
    private const byte LSB = 0x08/*0x0c*/;

    /// <summary>
    /// Asks the user to select a smart-card reader containing the Mifare chip
    /// </summary>
    /// <param name="readerNames">Collection of available smart-card readers</param>
    /// <returns>The selected reader name or <c>null</c> if none</returns>
    private static string ChooseReader2(IList<string> readerNames)
    {
        Console.WriteLine(new string('=', 79));
        Console.WriteLine("WARNING!! This will overwrite data in MSB {0:X2} LSB {1:X2} using the default key.", MSB,
            LSB);
        Console.WriteLine(new string('=', 79));

        // Show available readers.
        Console.WriteLine("Available readers: ");
        for (var i = 0; i < readerNames.Count; i++)
        {
            Console.WriteLine($"[{i}] {readerNames[i]}");
        }

        // Ask the user which one to choose.
        Console.Write("Which reader has an inserted Mifare 1k/4k card? ");

        var line = Console.ReadLine();

        int choice;
        if (int.TryParse(line, out choice) && (choice >= 0) && (choice <= readerNames.Count))
        {
            return readerNames[choice];
        }

        Console.WriteLine("An invalid number has been entered.");
        Console.ReadKey();

        return null;
    }

    public static void Transmit()
    {
        using (var context = ContextFactory.Instance.Establish(SCardScope.System))
        {
            var readerNames = context.GetReaders();
            if (NoReaderFound(readerNames))
            {
                Console.WriteLine("You need at least one reader in order to run this example.");
                Console.ReadKey();
                return;
            }

            var readerName = ChooseRfidReader(readerNames);
            if (readerName == null)
            {
                return;
            }

            using (var rfidReader = context.ConnectReader(readerName, SCardShareMode.Shared, SCardProtocol.Any))
            {
                var apdu = new CommandApdu(IsoCase.Case2Short, rfidReader.Protocol)
                {
                    CLA = 0xFF,
                    Instruction = InstructionCode.GetData,
                    P1 = 0x00,
                    P2 = 0x00,
                    Le = 0 // We don't know the ID tag size
                };

                using (rfidReader.Transaction(SCardReaderDisposition.Leave))
                {
                    Console.WriteLine("Retrieving the UID .... ");

                    var sendPci = SCardPCI.GetPci(rfidReader.Protocol);
                    var receivePci = new SCardPCI(); // IO returned protocol control information.

                    var receiveBuffer = new byte[256];
                    var command = apdu.ToArray();

                    var bytesReceived = rfidReader.Transmit(
                        sendPci, // Protocol Control Information (T0, T1 or Raw)
                        command, // command APDU
                        command.Length,
                        receivePci, // returning Protocol Control Information
                        receiveBuffer,
                        receiveBuffer.Length); // data buffer

                    var responseApdu =
                        new ResponseApdu(receiveBuffer, bytesReceived, IsoCase.Case2Short, rfidReader.Protocol);
                    Console.WriteLine("SW1: {0:X2}, SW2: {1:X2}\nUid: {2}",
                        responseApdu.SW1,
                        responseApdu.SW2,
                        responseApdu.HasData ? BitConverter.ToString(responseApdu.GetData()) : "No uid received");
                }
            }
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    private static string ChooseRfidReader(IList<string> readerNames)
    {
        // Show available readers.
        Console.WriteLine("Available readers: ");
        for (var i = 0; i < readerNames.Count; i++)
        {
            Console.WriteLine($"[{i}] {readerNames[i]}");
        }

        // Ask the user which one to choose.
        Console.Write("Which reader is an RFID reader? ");
        var line = Console.ReadLine();

        int choice;
        if (int.TryParse(line, out choice) && choice >= 0 && (choice <= readerNames.Count))
        {
            return readerNames[choice];
        }

        Console.WriteLine("An invalid number has been entered.");
        Console.ReadKey();
        return null;
    }

    private static bool NoReaderFound(ICollection<string> readerNames) =>
        readerNames == null || readerNames.Count < 1;
}
