using CoreApp.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace CoreApp
{
    public class ADONetDAL
    {
        #region ADO.Net

        private static readonly string ConnectionString = @"data source=OSTMYLLY\SQLEXPRESS;initial catalog=Thesis;integrated security=True";

        /// <summary>
        /// Insert data to table with ADO.Net
        /// </summary>
        /// <param name="seed">Randomnumber generators seed</param>
        public static void InsertData(int seedID, int rows)
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Inserting data!");

                sw.Start();

                var seed = 0;
                var result = false;
                var randomObjectList = new List<RandomObjectModel>();

                using (var conn = new SqlConnection(ConnectionString))
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT SeedValue FROM Seed WHERE SeedID = @ID", conn))
                        {
                            cmd.Parameters.Add("@ID", SqlDbType.Int).Value = seedID;

                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();

                            while (reader.Read())
                            {
                                seed = (int)reader["SeedValue"];
                            }
                            conn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error when fetching Seed!");
                        throw ex;
                    }

                    Console.WriteLine("--Seed is : " + seed);

                    var gen = new Random(seed);
                    var i = 0;

                    while (i < rows)
                    {
                        using (SqlCommand cmd = new SqlCommand(@"INSERT INTO RandomObject([RandomObjectID], [RandomString], [RandomDateTimeOffset], [RandomInt], [SeedId]) 
                                                             Values(@RandomObjectID, @RandomString, @RandomDateTimeOffset, @RandomInt, @SeedId)", conn))
                        {
                            cmd.Parameters.Add("@RandomObjectID", SqlDbType.Int).Value = i;
                            cmd.Parameters.Add("@RandomString", SqlDbType.NVarChar).Value = RandomStringGenerator.RandomString(gen, 15);
                            cmd.Parameters.Add("@RandomDateTimeOffset", SqlDbType.DateTimeOffset).Value = RandomDateTimeOffsetGenerator.RandomDateTimeOffset(gen);
                            cmd.Parameters.Add("@RandomInt", SqlDbType.Int).Value = gen.Next();
                            cmd.Parameters.Add("@SeedId", SqlDbType.Int).Value = seedID;

                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            i++;
                        }
                    }

                    result = true;

                    sw.Stop();
                }

                Console.WriteLine("--Result was: " + result);
                Console.WriteLine("--Time Elapsed: " + sw.Elapsed + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: " + ex);
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Gets data from database with ADO.Net
        /// </summary>
        public static void GetData()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Fetching data!");

                sw.Start();

                RandomObjectModel result = null;

                using (var conn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(@"SELECT TOP(1) RandomObjectID, RandomString, RandomDateTimeOffset, RandomInt, SeedId
                                                             FROM RandomObject 
                                                             ORDER BY RandomDateTimeOffset DESC", conn))
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = new RandomObjectModel
                            {
                                RandomObjectID = (int)reader["RandomObjectID"],
                                RandomString = (string)reader["RandomString"],
                                RandomDateTimeOffset = (DateTimeOffset)reader["RandomDateTimeOffset"],
                                RandomInt = (int)reader["RandomInt"],
                                RandoSeedId = (int)reader["SeedId"]
                            };
                        }
                        conn.Close();
                    }
                }

                sw.Stop();

                Console.WriteLine("--Result was: " + result.RandomObjectID + ", " + result.RandomString + ", " + result.RandomDateTimeOffset + ", " + result.RandomInt + ", " + result.RandoSeedId);
                Console.WriteLine("--Time Elapsed: " + sw.Elapsed + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: " + ex);
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Insert data to table with ADO.Net
        /// </summary>
        /// <param name="seed">Randomnumber generators seed</param>
        public static void DeleteData()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Truncating table!");

                sw.Start();

                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("TRUNCATE TABLE RandomObject", conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    sw.Stop();
                }

                Console.WriteLine("--Time Elapsed: " + sw.Elapsed + "\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: " + ex);
                Console.WriteLine("Press any key to continue.");
                Console.ReadKey();
            }
        }
        #endregion
    }
}
