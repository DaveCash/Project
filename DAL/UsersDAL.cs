﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Models;
using System.Security.Cryptography;

namespace DAL
{
    public class UsersDAL
    {
        public static User Login(string username, string password)
        {
            DBConnection dbConnection = new DBConnection();
            dbConnection.ExecuteCmd("SELECT * FROM UserData WHERE Name='" + username + "'");

            if (dbConnection.Reader.Read())
            {
                var pass = dbConnection.Reader["Password"].ToString();
                var name = dbConnection.Reader["Name"].ToString();
                var level = dbConnection.Reader["UserLevelID"].ToString();

                dbConnection.Connection.Close();

                if (!String.IsNullOrEmpty(pass) && pass == password)
                {
                    User user = new User();
                    user.UserName = name;
                    user.UserLevel = (UserLevel)Enum.Parse(typeof(UserLevel), level);

                    return user;
                }
            }

            return null;
        }

        public static User RegisterUser(string username, string password)
        {
            User user = new User();

            string hash = CreateHash(password);

            DBConnection dbConnection = new DBConnection();
            dbConnection.ExecuteNonQuery(""+
            "INSERT INTO "+ 
            "UserData (UserLevelID, [Name], [Password], PasswordHash) " + 
            "VALUES (1,'"+ username +"','"+ password +"', '"+ hash +"');");

            return user;
        }

        public static User ValidateUser(string username, string password)
        {
            User user = new User();

            DBConnection dbConnection = new DBConnection();
            dbConnection.ExecuteCmd("SELECT * FROM UserData WHERE Name='" + username + "'");

            dbConnection.Reader.Read();
            string correctHash = dbConnection.Reader["PasswordHash"].ToString();

            bool isValid = ValidatePassword(password, correctHash);

            return user;
        }

        public static string CreateHash(string password)
        {
            // Generate a random salt
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[24];
            csprng.GetBytes(salt);

            // Hash the password and encode the parameters
            byte[] hash = PBKDF2(password, salt, 1000, 24);
            return 1000 + ":" +
                Convert.ToBase64String(salt) + ":" +
                Convert.ToBase64String(hash);
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            // Extract the parameters from the hash
            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            int iterations = Int32.Parse(split[0]);
            byte[] salt = Convert.FromBase64String(split[1]);
            byte[] hash = Convert.FromBase64String(split[2]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}