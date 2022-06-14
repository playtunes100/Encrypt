# Encrypt

## How to use it

When installed the program has a simple layout as seen in the figure below.
 ![App main window](https://github.com/playtunes100/Encrypt/blob/39bc61feb6e7f24140896d704138cd52c3da41b1/images/main.jpg)
1.	To use it, first choose a file to encrypt by Clicking on the Select button. (If you want to keep a copy of the original un-encrypted file, make a copy because the program overrides the file).
 
2.	Then enter a password in the textbox.
 
3.	The next and last step is to Click on the Encrypt button.

A message saying Encryption Successful is an indication of a successful encryption.
![App main window](https://github.com/playtunes100/Encrypt/blob/39bc61feb6e7f24140896d704138cd52c3da41b1/images/successful.jpg)


## How it works

Programming language: C#
Framework: .NET 4.7.2

The first thing the program does when the Encrypt button is Clicked is open a binary file, read the contents of the file into a byte array, and then close the file.
 
Then it creates a AesCryptoServiceProvider object where the key is entered and also used as the IV. The key size and the block size of 128 is chosen to compliment the MD5 hash which hashes the password entered into a 128-bit hash.
 
EncryptStringToBytes_Aes is then called to encrypt the array of bytes. The program then Writes the bytes back to a file(which is encrypted and inaccessible).
 
In the EncryptStringToBytes_Aes method an encryptor is created to transform the stream of bytes in the memory stream, then a memory and crypto stream are created to capture the bytes. The method then returns the encrypted bytes.
 

## Resources

https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aescryptoserviceprovider?view=net-6.0
https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes?view=net-6.0
