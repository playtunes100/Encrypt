using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;


namespace Encrypt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        OpenFileDialog fileSelector = new OpenFileDialog();

        private void encryptButton_Click(object sender, EventArgs e)
        {
            //Checks if any file has been chosen.
            if (!String.IsNullOrEmpty(fileSelector.FileName))
            {
                //Checks if a password has been entered. 
                if (!String.IsNullOrWhiteSpace(keyTextBox.Text))
                {
                    string key = keyTextBox.Text;
                    byte[] rawData = File.ReadAllBytes(fileSelector.FileName);
                    listBox1.Items.Add("Encrypting...");
                    using (AesCryptoServiceProvider aesObj = new AesCryptoServiceProvider())
                    {
                        aesObj.KeySize = 128;
                        aesObj.BlockSize = 128;
                        aesObj.Key = hashinAlg(key);
                        aesObj.IV = hashinAlg(key);
                        aesObj.Mode = CipherMode.CBC;
                        aesObj.Padding = PaddingMode.PKCS7;


                        byte[] encrypted = EncryptStringToBytes_Aes(rawData, aesObj.Key, aesObj.IV);
                        File.WriteAllBytes(fileSelector.FileName, encrypted);
                        listBox1.Items.Add("Encryption Successful!");

                        keyTextBox.Clear();
                    }
                }
                else
                    MessageBox.Show("Enter a Password!");

            }
            else
                MessageBox.Show("Select a valid File!");
        }

        static byte[] hashinAlg(String key)
        {
            byte[] bytekey;
            byte[] hashedkey;
            bytekey = Encoding.UTF8.GetBytes(key);

            hashedkey = new MD5CryptoServiceProvider().ComputeHash(bytekey);
            

            return hashedkey;
        }
        [STAThread]
        private void button1_Click(object sender, EventArgs e)
        {
            
            
            
        }

        static byte[] EncryptStringToBytes_Aes(byte[] byteData, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (byteData == null || byteData.Length <= 0)
                throw new ArgumentNullException("byteData");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;

            // Creates an AesCryptoServiceProvider object with the specified key and IV
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Creates an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Creates the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(byteData, 0, byteData.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Returns the encrypted bytes from the memory stream.
            return encrypted;
        }

        static byte[] DecryptStringFromBytes_Aes(byte[] encryptedData, byte[] Key, byte[] IV)
        {
            // Checks arguments.
            if (encryptedData == null || encryptedData.Length <= 0)
                throw new ArgumentNullException("encryptedData");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] decrypted;

            // Creates an AesCryptoServiceProvider object with the specified key and IV
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;


                // Creates a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Creates the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {

                        csDecrypt.Write(encryptedData, 0, encryptedData.Length);
                        csDecrypt.FlushFinalBlock();
                        decrypted = msDecrypt.ToArray();
                    }
                }
            }

            return decrypted;
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            //checks if any file has been chosen
            if (!String.IsNullOrEmpty(fileSelector.FileName))
            {
                //Checks if a password has been entered. 
                if (!String.IsNullOrWhiteSpace(keyTextBox.Text))
                {
                    string key = keyTextBox.Text;
                    byte[] rawData = File.ReadAllBytes(fileSelector.FileName);
                    listBox1.Items.Add("Decrypting...");
                    using (AesCryptoServiceProvider aesObj = new AesCryptoServiceProvider())
                    {

                        aesObj.KeySize = 128;
                        aesObj.BlockSize = 128;
                        aesObj.Key = hashinAlg(key);
                        aesObj.IV = hashinAlg(key);
                        aesObj.Mode = CipherMode.CBC;
                        aesObj.Padding = PaddingMode.PKCS7;

                        byte[] decrypted = DecryptStringFromBytes_Aes(rawData, aesObj.Key, aesObj.IV);
                        File.WriteAllBytes(fileSelector.FileName, decrypted);
                        listBox1.Items.Add("Decryption Successful!");

                        keyTextBox.Clear();
                    }
                }
                else
                    MessageBox.Show("Enter a Password!");
            }
            else
                MessageBox.Show("Select a valid File!");


        }

        private void button2_Click(object sender, EventArgs e)
        {
           //Opens a dialog to choose a file.
            fileSelector.ShowDialog();
            fileName.Text = "..." + fileSelector.FileName.Substring(fileSelector.FileName.Length - 24);
            listBox1.Items.Add("File: " + fileSelector.FileName+" selected!");

        }
    }
}
