// ······································································//
// <copyright file="MainPage.xaml.cs" company="Jay Bautista Mendoza">    //
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.          //
//     THIS IS PART OF MY PERSONAL OPEN SOURCE WPF WINDOW TEMPLATE.      //
//     THIS IS NOT PRIVATE PROPERTY. FEEL FREE TO MODIFY OR USE IT.      //
// </copyright>                                                          //
// ······································································//

namespace SqlToXml.Views
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Input;
    using SqlToXml.Services;

    /// <summary>Interaction logic for MainPage</summary>
    public partial class MainPage : Page
    {
        #region FIELDS · PRIVATE · NON-STATIC · NON-READONLY

        /// <summary>An instance of the Batch Rename Service.</summary>
        private SqlToXmlService batchRenameService;

        /// <summary>An instance of the Folder Browser Dialog.</summary>
        private FolderBrowserDialog folderBrowserDialog;

        /// <summary>List of invalid path characters.</summary>
        /// <remarks>Code is used only for optimization.</remarks>
        private char[] invalidPathChars;

        /// <summary>List of invalid file characters.</summary>
        /// <remarks>Code is used only for optimization.</remarks>
        private char[] invalidNameChars;

        #endregion

        #region CONSTRUCTORS · PUBLIC · NON-STATIC

        /// <summary>Initializes a new instance of the <see cref="MainPage" /> class.</summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.batchRenameService = new SqlToXmlService();
            this.folderBrowserDialog = new FolderBrowserDialog();

            this.invalidPathChars = Path.GetInvalidPathChars();
            this.invalidNameChars = Path.GetInvalidFileNameChars();

            this.PathTextBox.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            this.FilesRadioButton.IsChecked = true;
            this.SetFilesOrFoldersOptionsVisibility();
            this.ModeTabControl.SelectedIndex = 0;
            this.SetTextBoxVisibilities();
            this.ShowStatus("NOTE │ No text string to find.");

            this.InsertIndexTextBox.Text = "0";
            this.NewPatternTextBox.Text = "1 0";
            this.SeparatorTextBox.Text = " · ";

            this.InsertIndexTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, this.DisablePaste));
            this.NewPatternTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, this.DisablePaste));
            this.SeparatorTextBox.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, this.DisablePaste));
        }
        #endregion

        #region METHODS · PRIVATE · STATIC
        /// <summary>Determines if a text is allowed and a valid size (font).</summary>
        /// <param name="text">Text to test for validity.</param>
        /// <returns>Returns 'true' if valid, otherwise, returns 'false'.</returns>
        private static bool IsTextValidSize(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        /// <summary>Determines if a text is a valid pattern.</summary>
        /// <param name="text">Text to test for validity.</param>
        /// <returns>Returns 'true' if valid, otherwise, returns 'false'.</returns>
        private static bool IsTextValidPattern(string text)
        {
            Regex regex = new Regex(@"[^0-9\s]+");
            return !regex.IsMatch(text);
        }
        #endregion
        
        #region EVENTS · PRIVATE · NON-STATIC
        
        /// <summary>Disable Paste.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">ExecutedRoutedEventArgs 'e'.</param>
        private void DisablePaste(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>Selection changed event method for the Mode tab control.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">SelectionChangedEventArgs 'e'.</param>
        private void ModeTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetTextBoxVisibilities();
        }

        /// <summary>Checked event method for the Files radio button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void FilesRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.SetFilesOrFoldersOptionsVisibility();            
        }

        /// <summary>Unchecked event method for the Files radio button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void FilesRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.SetFilesOrFoldersOptionsVisibility();            
        }

        /// <summary>Checked event method for the Folders radio button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void FoldersRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            this.SetFilesOrFoldersOptionsVisibility();            
        }

        /// <summary>Unchecked event method for the Folders radio button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void FoldersRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.SetFilesOrFoldersOptionsVisibility();            
        }
             
        /// <summary>Text changed event method for the Find textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        private void FindTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetTextBoxVisibilities();
        }

        /// <summary>Text changed event method for the Replace textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        private void ReplaceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetTextBoxVisibilities();
        }

        /// <summary>Text changed event method for the Insert textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        private void InsertTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetTextBoxVisibilities();
        }

        /// <summary>Text changed event method for the Insert Index textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        private void InsertIndexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetTextBoxVisibilities();

            string text = this.InsertIndexTextBox.Text;

            if (text.StartsWith("0") && text.Length > 1)
            {
                this.InsertIndexTextBox.Text = text.Substring(1);
                this.InsertIndexTextBox.CaretIndex = 1;
            }
        }

        /// <summary>Preview text input event method for the Insert Index textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextCompositionEventArgs 'e'.</param>
        private void InsertIndexTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !MainPage.IsTextValidSize(e.Text);
        }

        /// <summary>Text changed event method for the New Pattern textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        private void NewPatternTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetTextBoxVisibilities();
        }

        /// <summary>Preview text input event method for the New Pattern textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextCompositionEventArgs 'e'.</param>
        private void NewPatternTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !MainPage.IsTextValidPattern(e.Text);
        }

        /// <summary>Text changed event method for the Separator textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        private void SeparatorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetTextBoxVisibilities();
        }
        
        /// <summary>Preview text input event method for the Separator textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextCompositionEventArgs 'e'.</param>
        private void SeparatorTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = MainPage.IsTextValidPattern(e.Text);
        }

        /// <summary>Text changed event method for the Source Directory textbox.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextChangedEventArgs 'e'.</param>
        /// <remarks>This sets visibilities of controls based on value validity of the source directory.</remarks>
        private void PathTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.IsValidPath(this.PathTextBox.Text))
            {
                this.SetControlsVisibility(System.Windows.Visibility.Collapsed);
                this.ShowStatus("ERROR │ The source directory is invalid.");
                return;
            }

            if (!Directory.Exists(this.PathTextBox.Text))
            {
                this.SetControlsVisibility(System.Windows.Visibility.Collapsed);
                this.ShowStatus("ERROR │ The source directory does not exist.");
                return;
            }

            this.SetControlsVisibility(System.Windows.Visibility.Visible);
            this.SetTextBoxVisibilities();
        }

        /// <summary>Click event method for the Browse Source Directory button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void BrowsePathButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult dialogResult = this.folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this.PathTextBox.Text = this.folderBrowserDialog.SelectedPath;
            }
        }

        /// <summary>Drop event method for the Drop label.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">DragEventArgs 'e'.</param>
        private void DropPathLabel_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
            {
                try
                {
                    string path = ((string[])e.Data.GetData(System.Windows.DataFormats.FileDrop, true))[0];

                    if (this.batchRenameService.IsFolder(path))
                    {
                        this.PathTextBox.Text = path;
                    }
                    else
                    {
                        this.PathTextBox.Text = Path.GetDirectoryName(path);
                    }
                }
                catch (Exception exception)
                {
                    this.ShowStatus(string.Concat("ERROR │ ", exception.Message));
                }
            }
        }

        /// <summary>Click event method for the Rename All button.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">RoutedEventArgs 'e'.</param>
        private void RenameAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.ShowStatus("PLEASE WAIT │ Executing Rename...");

                string directory = this.PathTextBox.Text;
                bool filesRename = (bool)this.FilesRadioButton.IsChecked;
                bool extensions = (bool)this.IncludeExtensionsCheckBox.IsChecked;
                bool recursive = filesRename ? (bool)this.RecursiveFilesCheckBox.IsChecked : (bool)this.RecursiveFoldersCheckBox.IsChecked;

                switch (this.ModeTabControl.SelectedIndex)
                {
                    case 0:
                    {
                        string text = this.FindTextBox.Text;
                        string replacement = this.ReplaceTextBox.Text;
                        new Thread(() => this.ExecuteFindReplace(directory, filesRename, extensions, recursive, text, replacement)).Start();
                        break;
                    }

                    case 1:
                    {
                        string text = this.InsertTextBox.Text;
                        int index = int.Parse(this.InsertIndexTextBox.Text);
                        bool fromEnd = (bool)this.FromEndCheckBox.IsChecked;
                        new Thread(() => this.ExecuteIndexInsert(directory, filesRename, extensions, recursive, text, index, fromEnd)).Start();
                        break;
                    }

                    case 2:
                    {
                        string order = this.NewPatternTextBox.Text;
                        string separator = this.SeparatorTextBox.Text;
                        new Thread(() => this.ExecuteChangeOrder(directory, filesRename, extensions, recursive, order, separator)).Start();
                        break;
                    }

                    default: break;
                }
            }
            catch (Exception exception)
            {
                this.ShowStatus(string.Concat("ERROR │ ", exception.Message));
            }
        }

        #endregion
             
        #region METHODS · PRIVATE · NON-STATIC

        /// <summary>Show application execution status.</summary>
        /// <param name="status">Status text to show on the TextBox.</param>
        private void ShowStatus(string status)
        {
            Action action = () => this.StatusTextBox.Text = status;
            Dispatcher.Invoke(action);
        }

        /// <summary>Set the visibility of controls based on validity of radio button values.</summary>
        private void SetFilesOrFoldersOptionsVisibility()
        {
            if ((bool)this.FilesRadioButton.IsChecked)
            {
                this.FilesOptionsGrid.Visibility = System.Windows.Visibility.Visible;
                this.FoldersOptionsGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                this.FilesOptionsGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.FoldersOptionsGrid.Visibility = System.Windows.Visibility.Visible;
            }
        }

        /// <summary>Set the visibility of controls based on validity of path.</summary>
        /// <param name="visibility">Visibility value to use in setting.</param>
        private void SetControlsVisibility(System.Windows.Visibility visibility)
        {
            this.ModeGrid.Visibility = this.RenameAllButton.Visibility = this.OptionsGrid.Visibility = visibility;
        }

        /// <summary>Set the visibility of controls based on validity of textbox values.</summary>
        private void SetTextBoxVisibilities()
        {
            switch (this.ModeTabControl.SelectedIndex)
            {
                case 0: /* FIND AND REPLACE */
                    if (string.IsNullOrEmpty(this.FindTextBox.Text))
                    {
                        this.RenameAllButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.ShowStatus("NOTE │ No text string to find.");
                        return;
                    }

                    if (!this.IsValidName(this.FindTextBox.Text))
                    {
                        this.RenameAllButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.ShowStatus("NOTE │ Invalid find text for filename.");
                        return;
                    }

                    if (!this.IsValidName(this.ReplaceTextBox.Text))
                    {
                        this.RenameAllButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.ShowStatus("NOTE │ Invalid replace text for filename.");
                        return;
                    }
                    
                    break;

                case 1: /* INSERT */
                    if (string.IsNullOrEmpty(this.InsertTextBox.Text))
                    {
                        this.RenameAllButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.ShowStatus("NOTE │ No text string to insert.");
                        return;
                    }

                    if (!this.IsValidName(this.InsertTextBox.Text))
                    {
                        this.RenameAllButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.ShowStatus("NOTE │ Invalid insert text for filename.");
                        return;
                    }

                    break;

                case 2: /* REORDER */
                    if (string.IsNullOrEmpty(this.NewPatternTextBox.Text))
                    {
                        this.RenameAllButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.ShowStatus("NOTE │ Pattern text is not defined.");
                        return;
                    }

                    if (string.IsNullOrEmpty(this.SeparatorTextBox.Text))
                    {
                        this.RenameAllButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.ShowStatus("NOTE │ Separator text is not defined.");
                        return;
                    }

                    if (!this.IsValidName(this.SeparatorTextBox.Text))
                    {
                        this.RenameAllButton.Visibility = System.Windows.Visibility.Collapsed;
                        this.ShowStatus("NOTE │ Invalid separator text for filename.");
                        return;
                    }

                    break;
                default: break;
            }

            this.RenameAllButton.Visibility = System.Windows.Visibility.Visible;
            this.ShowStatus(string.Empty);
        }

        /// <summary>Execute a Find-Replace file or folder rename.</summary>
        /// <param name="directory">Target directory where the files or folders to rename are located.</param>
        /// <param name="filesRename">True if renaming files, otherwise false (for folders).</param>
        /// <param name="extensions">Include the file or folder's extension in the rename process.</param>
        /// <param name="recursive">>Include subdirectories in the rename process.</param>
        /// <param name="text">Text to find in each file or folder.</param>
        /// <param name="replacement">Replacement text.</param>
        private void ExecuteFindReplace(string directory, bool filesRename, bool extensions, bool recursive, string text, string replacement)
        {
            try
            {
                string renameReturn = filesRename
                    ? this.batchRenameService.FindReplaceFiles(text, replacement, extensions, directory, recursive)
                    : this.batchRenameService.FindReplaceFolders(text, replacement, directory, recursive);

                if (!string.IsNullOrEmpty(renameReturn))
                {
                    this.ShowStatus(string.Concat("SERVICE ERROR │ ", renameReturn));
                }
                else
                {
                    this.ShowStatus("Successfully renamed all!".ToUpper());
                }
            }
            catch (Exception exception)
            {
                this.ShowStatus(string.Concat("ERROR │ ", exception.Message));
            }
        }

        /// <summary>Execute a Index-Insert file or folder rename.</summary>
        /// <param name="directory">Target directory where the files or folders to rename are located.</param>
        /// <param name="filesRename">True if renaming files, otherwise false (for folders).</param>
        /// <param name="extensions">Include the file or folder's extension in the rename process.</param>
        /// <param name="recursive">>Include subdirectories in the rename process.</param>
        /// <param name="text">Text to insert in each file or folder.</param>
        /// <param name="index">Index location from the file or folder name.</param>
        /// <param name="fromEnd">Determines if the index should count from the end of the f name.</param>
        private void ExecuteIndexInsert(string directory, bool filesRename, bool extensions, bool recursive, string text, int index, bool fromEnd)
        {
            try
            {
                string renameReturn = filesRename
                    ? this.batchRenameService.IndexInsertFiles(text, index, fromEnd, extensions, directory, recursive)
                    : this.batchRenameService.IndexInsertFolders(text, index, fromEnd, directory, recursive);

                if (!string.IsNullOrEmpty(renameReturn))
                {
                    this.ShowStatus(string.Concat("SERVICE ERROR │ ", renameReturn));
                }
                else
                {
                    this.ShowStatus("Successfully renamed all!".ToUpper());
                }
            }
            catch (Exception exception)
            {
                this.ShowStatus(string.Concat("ERROR │ ", exception.Message));
            }
        }

        /// <summary>Execute a Change-Order file or folder rename.</summary>
        /// <param name="directory">Target directory where the files or folders to rename are located.</param>
        /// <param name="filesRename">True if renaming files, otherwise false (for folders).</param>
        /// <param name="extensions">Include the file or folder's extension in the rename process.</param>
        /// <param name="recursive">>Include subdirectories in the rename process.</param>
        /// <param name="order">Order of the new target pattern for the file or folder name.</param>
        /// <param name="separator">Separator of the file or folder names in determining the order and pattern.</param>
        private void ExecuteChangeOrder(string directory, bool filesRename, bool extensions, bool recursive, string order, string separator)
        {
            try
            {
                string renameReturn = filesRename
                    ? this.batchRenameService.ChangeOrderFiles(order, separator, extensions, directory, recursive)
                    : this.batchRenameService.ChangeOrderFolders(order, separator, directory, recursive);

                if (!string.IsNullOrEmpty(renameReturn))
                {
                    this.ShowStatus(string.Concat("SERVICE ERROR │ ", renameReturn));
                }
                else
                {
                    this.ShowStatus("Successfully renamed all!".ToUpper());
                }
            }
            catch (Exception exception)
            {
                this.ShowStatus(string.Concat("ERROR │ ", exception.Message));
            }
        }

        /// <summary>Checks a file path string for validity.</summary>
        /// <param name="path">File path string to validate.</param>
        /// <returns>True if valid, otherwise False.</returns>
        private bool IsValidPath(string path)
        {
            foreach (char invalid in this.invalidPathChars)
            {
                if (path.Any(m => m == invalid))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Checks a file name string for validity.</summary>
        /// <param name="name">File name string to validate.</param>
        /// <returns>True if valid, otherwise False.</returns>
        private bool IsValidName(string name)
        {
            foreach (char invalid in this.invalidNameChars)
            {
                if (name.Any(m => m == invalid))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
