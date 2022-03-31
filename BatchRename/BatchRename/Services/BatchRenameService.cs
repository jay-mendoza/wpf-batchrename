//-----------------------------------------------------------------------
// <copyright file="SqlToXmlService.cs" company="Jay Bautista Mendoza">
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace SqlToXml.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>Initializes a new instance of the <see cref="Renamer" /> class.</summary>
    public class SqlToXmlService
    {
        #region FIELDS │ PRIVATE │ NON-STATIC
        
        /// <summary>Name of the class.</summary>
        private const string ClassName = "SqlToXmlService";

        #endregion
        
        #region METHODS │ PUBLIC │ NON-STATIC

        /// <summary>Determine if a given path is a folder or not.</summary>
        /// <param name="path">Path to determine.</param>
        /// <returns>True if the path is a folder, otherwise, False.</returns>
        public bool IsFolder(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory;
        }

        /// <summary>Find and replace text in each file in a specified directory.</summary>
        /// <param name="text">Text to find in each file.</param>
        /// <param name="replacement">Replacement text.</param>
        /// <param name="extensions">Include the file's extension in the rename process.</param>
        /// <param name="directory">Target directory where the files to rename are located.</param>
        /// <param name="recursive">Include subdirectories in the rename process.</param>
        /// <returns>Null if process is successful, otherwise, it returns the exception.</returns>
        public string FindReplaceFiles(string text, string replacement, bool extensions, string directory, bool recursive = false)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    return string.Format("[{0}] The directory does not exist.", ClassName);
                }

                if (extensions)
                {
                    foreach (string file in Directory.GetFiles(directory).Select(n => Path.GetFileName(n)).Where(f => f.Contains(text)))
                    {
                        File.Move(Path.Combine(directory, file), Path.Combine(directory, file.Replace(text, replacement)));
                    }
                }
                else
                {
                    foreach (string file in Directory.GetFiles(directory).Where(f => f.Contains(text)))
                    {
                        File.Move(file, Path.Combine(directory, Path.GetFileNameWithoutExtension(file).Replace(text, replacement) + Path.GetExtension(file)));
                    }
                }

                if (recursive)
                {
                    foreach (string subDirectory in Directory.GetDirectories(directory))
                    {
                        string subException = this.FindReplaceFiles(text, replacement, extensions, subDirectory, true);

                        if (subException != null)
                        {
                            return string.Concat("[SqlToXmlService] ", subException);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return string.Concat("[SqlToXmlService] ", exception.Message);
            }

            return null;
        }

        /// <summary>Find and replace text in each folder in a specified directory.</summary>
        /// <param name="text">Text to find in each folder.</param>
        /// <param name="replacement">Replacement text.</param>
        /// <param name="directory">Target directory where the folders to rename are located.</param>
        /// <param name="recursive">Include subdirectories in the rename process.</param>
        /// <returns>Null if process is successful, otherwise, it returns the exception.</returns>
        public string FindReplaceFolders(string text, string replacement, string directory, bool recursive = false)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    return string.Format("[{0}] The directory does not exist.", ClassName);
                }

                string[] folderDirectories = Directory.GetDirectories(directory);

                foreach (string folder in folderDirectories.Select(n => Path.GetFileName(n)).Where(f => f.Contains(text)))
                {
                    Directory.Move(Path.Combine(directory, folder), Path.Combine(directory, folder.Replace(text, replacement)));
                }

                if (recursive)
                {
                    foreach (string subDirectory in folderDirectories)
                    {
                        string subException = this.FindReplaceFolders(text, replacement, subDirectory, true);

                        if (subException != null)
                        {
                            return string.Concat("[SqlToXmlService] ", subException);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return string.Concat("[SqlToXmlService] ", exception.Message);
            }

            return null;
        }

        /// <summary>Insert text at a given index in each file in a specified directory.</summary>
        /// <param name="text">Text to insert in each file.</param>
        /// <param name="index">Index location from the file name.</param>
        /// <param name="fromEnd">Determines if the index should count from the end of the file name.</param>
        /// <param name="extensions">Include the file's extension in the rename process.</param>
        /// <param name="directory">Target directory where the files to rename are located.</param>
        /// <param name="recursive">Include subdirectories in the rename process.</param>
        /// <returns>Null if process is successful, otherwise, it returns the exception.</returns>
        public string IndexInsertFiles(string text, int index, bool fromEnd, bool extensions, string directory, bool recursive = false)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    return string.Format("[{0}] The directory does not exist.", ClassName);
                }

                if (extensions)
                {
                    if (!fromEnd)
                    {
                        foreach (string file in Directory.GetFiles(directory).Select(n => Path.GetFileName(n)))
                        {
                            if (index < file.Length)
                            {
                                File.Move(Path.Combine(directory, file), Path.Combine(directory, file.Insert(index, text)));
                            }
                        }
                    }
                    else
                    {
                        foreach (string file in Directory.GetFiles(directory).Select(n => Path.GetFileName(n)))
                        {
                            if (index < file.Length)
                            {
                                File.Move(Path.Combine(directory, file), Path.Combine(directory, file.Insert(file.Length - index, text)));
                            }
                        }
                    }
                }
                else
                {
                    if (!fromEnd)
                    {
                        foreach (string file in Directory.GetFiles(directory))
                        {
                            string name = Path.GetFileNameWithoutExtension(file);

                            if (index < name.Length)
                            {
                                File.Move(file, Path.Combine(directory, name.Insert(index, text) + Path.GetExtension(file)));
                            }
                        }
                    }
                    else
                    {
                        foreach (string file in Directory.GetFiles(directory))
                        {
                            string name = Path.GetFileNameWithoutExtension(file);

                            if (index < name.Length)
                            {
                                File.Move(file, Path.Combine(directory, name.Insert(name.Length - index, text) + Path.GetExtension(file)));
                            }
                        }
                    }
                }

                if (recursive)
                {
                    foreach (string subDirectory in Directory.GetDirectories(directory))
                    {
                        string subException = this.IndexInsertFiles(text, index, fromEnd, extensions, subDirectory, true);

                        if (subException != null)
                        {
                            return string.Concat("[SqlToXmlService] ", subException);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return string.Concat("[SqlToXmlService] ", exception.Message);
            }

            return null;
        }

        /// <summary>Insert text at a given index in each folder in a specified directory.</summary>
        /// <param name="text">Text to insert in each folder.</param>
        /// <param name="index">Index location from the folder name.</param>
        /// <param name="fromEnd">Determines if the index should count from the end of the folder name.</param>
        /// <param name="directory">Target directory where the folders to rename are located.</param>
        /// <param name="recursive">Include subdirectories in the rename process.</param>
        /// <returns>Null if process is successful, otherwise, it returns the exception.</returns>
        public string IndexInsertFolders(string text, int index, bool fromEnd, string directory, bool recursive = false)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    return string.Format("[{0}] The directory does not exist.", ClassName);
                }

                string[] folderDirectories = Directory.GetDirectories(directory);

                if (!fromEnd)
                {
                    foreach (string folder in folderDirectories.Select(n => Path.GetFileName(n)))
                    {
                        if (index < folder.Length)
                        {
                            Directory.Move(Path.Combine(directory, folder), Path.Combine(directory, folder.Insert(index, text)));
                        }
                    }
                }
                else
                {
                    foreach (string folder in folderDirectories.Select(n => Path.GetFileName(n)))
                    {
                        if (index < folder.Length)
                        {
                            Directory.Move(Path.Combine(directory, folder), Path.Combine(directory, folder.Insert(folder.Length - index, text)));
                        }
                    }
                }

                if (recursive)
                {
                    foreach (string subDirectory in folderDirectories)
                    {
                        string subException = this.IndexInsertFolders(text, index, fromEnd, subDirectory, true);

                        if (subException != null)
                        {
                            return string.Concat("[SqlToXmlService] ", subException);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return string.Concat("[SqlToXmlService] ", exception.Message);
            }

            return null;
        }

        /// <summary>Change the order of text in each file in a specified directory.</summary>
        /// <param name="order">Order of the new target pattern for the file name.</param>
        /// <param name="separator">Separator of the file names in determining the order and pattern.</param>
        /// <param name="extensions">Include the file's extension in the rename process.</param>
        /// <param name="directory">Target directory where the files to rename are located.</param>
        /// <param name="recursive">Include subdirectories in the rename process.</param>
        /// <returns>Null if process is successful, otherwise, it returns the exception.</returns>
        public string ChangeOrderFiles(string order, string separator, bool extensions, string directory, bool recursive = false)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    return string.Format("[{0}] The directory does not exist.", ClassName);
                }

                List<int> indices = this.GetIndicesFromPattern(order);

                if (extensions)
                {
                    foreach (string file in Directory.GetFiles(directory).Select(n => Path.GetFileName(n)))
                    {
                        List<string> substrings = this.SplitString(file, separator);

                        if (!this.VerifyIndices(indices, substrings.Count))
                        {
                            continue;
                        }

                        List<string> orderedString = new List<string>();

                        foreach (int index in indices)
                        {
                            orderedString.Add(substrings[index]);
                        }

                        File.Move(Path.Combine(directory, file), Path.Combine(directory, string.Join(separator, orderedString)));
                    }
                }
                else
                {
                    foreach (string file in Directory.GetFiles(directory))
                    {
                        List<string> substrings = this.SplitString(Path.GetFileNameWithoutExtension(file), separator);

                        if (!this.VerifyIndices(indices, substrings.Count))
                        {
                            continue;
                        }

                        List<string> orderedString = new List<string>();

                        foreach (int index in indices)
                        {
                            orderedString.Add(substrings[index]);
                        }

                        File.Move(file, Path.Combine(directory, string.Join(separator, orderedString) + Path.GetExtension(file)));
                    }
                }

                if (recursive)
                {
                    foreach (string subDirectory in Directory.GetDirectories(directory))
                    {
                        string subException = this.ChangeOrderFiles(order, separator, extensions, subDirectory, true);

                        if (subException != null)
                        {
                            return string.Concat("[SqlToXmlService] ", subException);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return string.Concat("[SqlToXmlService] ", exception.Message);
            }

            return null;
        }

        /// <summary>Change the order of text in each folder in a specified directory.</summary>
        /// <param name="order">Order of the new target pattern for the folder name.</param>
        /// <param name="separator">Separator of the folder names in determining the order and pattern.</param>
        /// <param name="directory">Target directory where the folders to rename are located.</param>
        /// <param name="recursive">Include subdirectories in the rename process.</param>
        /// <returns>Null if process is successful, otherwise, it returns the exception.</returns>
        public string ChangeOrderFolders(string order, string separator, string directory, bool recursive = false)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    return string.Format("[{0}] The directory does not exist.", ClassName);
                }

                string[] folderDirectories = Directory.GetDirectories(directory);
                List<int> indices = this.GetIndicesFromPattern(order);

                foreach (string folder in folderDirectories.Select(n => Path.GetFileName(n)))
                {
                    List<string> substrings = this.SplitString(folder, separator);

                    if (!this.VerifyIndices(indices, substrings.Count))
                    {
                        continue;
                    }

                    List<string> orderedString = new List<string>();

                    foreach (int index in indices)
                    {
                        orderedString.Add(substrings[index]);
                    }

                    Directory.Move(Path.Combine(directory, folder), Path.Combine(directory, string.Join(separator, orderedString)));
                }

                if (recursive)
                {
                    foreach (string subDirectory in folderDirectories)
                    {
                        string subException = this.ChangeOrderFolders(order, separator, subDirectory, true);

                        if (subException != null)
                        {
                            return string.Concat("[SqlToXmlService] ", subException);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                return string.Concat("[SqlToXmlService] ", exception.Message);
            }

            return null;
        }

        #endregion

        #region METHODS │ PRIVATE │ NON-STATIC

        /// <summary>Split a string using a separator.</summary>
        /// <param name="stringToSplit">String to split.</param>
        /// <param name="separator">Separator to use in splitting the string.</param>
        /// <returns>List of sub-strings. Returns null if there is an exception.</returns>
        private List<string> SplitString(string stringToSplit, string separator)
        {
            try
            {
                if (!stringToSplit.Contains(separator))
                {
                    return new List<string>() { stringToSplit };
                }

                return new List<string>(stringToSplit.Split(new string[] { separator }, StringSplitOptions.None));
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Split an ordered pattern string using a space as separator.</summary>
        /// <param name="pattern">Pattern string to parse and get indices from.</param>
        /// <returns>List of ordered integer of the pattern.</returns>
        private List<int> GetIndicesFromPattern(string pattern)
        {
            try
            {
                List<int> patternList = new List<int>();

                int patternNumber = 0;

                foreach (string s in this.SplitString(pattern, " "))
                {
                    if (!int.TryParse(s, out patternNumber))
                    {
                        return null;
                    }

                    patternList.Add(patternNumber);
                }

                return patternList;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>Verifies a list of pattern index integers. It must not contain an index number greater than the given count.</summary>
        /// <param name="indices">Index integers.</param>
        /// <param name="count">The count to determine if the indices are correct.</param>
        /// <returns>True if it is valid, otherwise false.</returns>
        private bool VerifyIndices(List<int> indices, int count)
        {
            foreach (int index in indices)
            {
                if (index >= count)
                {
                    return false;
                }
            }

            return true;
        }
        
        #endregion
    }
}
