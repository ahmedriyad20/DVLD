using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Classes
{
    internal class clsUtil
    {
        public static string GenerateGUID()
        {
            Guid newGuid = Guid.NewGuid();
            string guid = Guid.NewGuid().ToString();
            return guid;
        }

        public static bool CreateFolderIfDoesNotExist(string FolderPath)
        {
            if(!Directory.Exists(FolderPath))
            {
                try
                {
                    Directory.CreateDirectory(FolderPath);
                    return true;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Error creating folder " + ex.Message);
                    return false;
                }
                
            }

            return true;
        }

        public static string ReplaceFileNameWithGUID(string sourceFile)
        {
            FileInfo fileInfo = new FileInfo(sourceFile);
            string extension = fileInfo.Extension;
            return GenerateGUID() + extension;
        }

        public static bool CopyImageToProjectImagesFolder(ref string sourceFile)
        {
            // this funciton will copy the image to the
            // project images foldr after renaming it
            // with GUID with the same extention, then it will update the sourceFileName with the new name.

            string DestinationFolder = @"C:\DVLD3-People-Images\";
            if(!CreateFolderIfDoesNotExist(DestinationFolder))
            {
                return false;
            }

            
            string destinationFile;

            destinationFile = DestinationFolder + ReplaceFileNameWithGUID(sourceFile);

            try
            {
                File.Copy(sourceFile, destinationFile, true);
                
            }
            catch (IOException iox)
            {
                MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            sourceFile = destinationFile;
            return true;
        }

        public static bool DeletePersonImageFile(string sourcFile)
        {
            if(File.Exists(sourcFile))
            {
                try
                {
                    File.Delete(sourcFile);
                    return true;
                }
                catch (IOException iox)
                {
                    MessageBox.Show(iox.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        public static string ComputeHash(string Data)
        {
            //SHA is Secutred Hash Algorithm.
            // Create an instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Data));

                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
