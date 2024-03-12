using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Utility
{
    // DEBUG TODO: We could use Lazinator methods instead of calling MD5HashGenerator. 
    public class MD5HashGenerator
    {
        private static readonly Object locker = new Object();

        public static String GenerateKey(Object sourceObject)
        {
            if (sourceObject == null)
            {
                throw new ArgumentNullException(nameof(sourceObject), "Null as parameter is not allowed");
            }

            try
            {
                string hashString = ComputeHash(ObjectToByteArray(sourceObject));
                return hashString;
            }
            catch
            {
                throw new ApplicationException($"Object was not serializable");
            }
        }

        private static byte[] ObjectToByteArray(Object objectToSerialize)
        {
            if (objectToSerialize == null) throw new ArgumentNullException(nameof(objectToSerialize));

            string jsonString;
            lock (locker) // Ensure thread-safety
            {
                // Customize serialization options if necessary
                var options = new JsonSerializerOptions
                {
                    // Example of possible options, adjust or remove as needed:
                    WriteIndented = false, // Indentation not needed for hashing
                    // Add any other options you might need for serialization
                };
                jsonString = JsonSerializer.Serialize(objectToSerialize, options);
            }
            return Encoding.UTF8.GetBytes(jsonString);
        }

        public static string ComputeHash(byte[] objectAsBytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                try
                {
                    byte[] result = md5.ComputeHash(objectAsBytes);
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in result)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    return sb.ToString();
                }
                catch
                {
                    Console.WriteLine("Hash has not been generated.");
                    return null;
                }
            }
        }

        public static Guid GetDeterministicGuid(byte[] byteArray)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] result = md5.ComputeHash(byteArray);
                return new Guid(result);
            }
        }

        public static Guid GetDeterministicGuid(Object objectToSerialize)
        {
            return GetDeterministicGuid(ObjectToByteArray(objectToSerialize));
        }
    }
}
