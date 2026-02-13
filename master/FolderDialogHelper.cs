using System.IO;
using System.Windows.Forms;

namespace TTG_Tools
{
    internal static class FolderDialogHelper
    {
        public static string SelectFolder(string initialPath = null, string description = "")
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = description ?? string.Empty;
                folderDialog.UseDescriptionForTitle = true;

                if (!string.IsNullOrWhiteSpace(initialPath) && Directory.Exists(initialPath))
                {
                    folderDialog.SelectedPath = initialPath;
                }

                return folderDialog.ShowDialog() == DialogResult.OK ? folderDialog.SelectedPath : null;
            }
        }
    }
}
