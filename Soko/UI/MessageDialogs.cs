using System;
using System.Windows.Forms;

namespace Soko.UI
{
    public class MessageDialogs
    {
        public static void showMessage(string message, string caption)
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void showError(string errorMessage, string caption)
        {
            MessageBox.Show(errorMessage, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static bool queryConfirmation(string message, string caption)
        {
            DialogResult value = MessageBox.Show(message, caption, MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            return (value == DialogResult.Yes);
        }

    }
}
