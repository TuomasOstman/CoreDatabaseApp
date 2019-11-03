using CoreApp.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics;
using System.Linq;

namespace CoreApp
{
    public class DapperDAL
    {
        #region Dapper

        private static readonly string ConnectionString = @"data source=OSTMYLLY\SQLEXPRESS;initial catalog=Thesis;integrated security=True";

        /// <summary>
        /// Insert data to table with Dapper
        /// </summary>
        /// <param name="seed">Randomnumber generators seed</param>
        public static void InsertData(int seedID, int rows)
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Inserting data!");

                sw.Start();

                var i = 0;
                var result = false;
                var seed = 0;

                using (var conn = new SqlConnection(ConnectionString))
                {
                    seed = conn.Query<int>("SELECT SeedValue FROM Seed WHERE SeedID = @ID", new { ID = seedID }).FirstOrDefault();

                    var gen = new Random(seed);

                    while (i < rows)
                    {
                        conn.Execute(@"INSERT INTO RandomObject([RandomObjectID], [RandomString], [RandomDateTimeOffset], [RandomInt], [RandomSeedId]) 
                                                             Values(@RandomObjectID, @RandomString, @RandomDateTimeOffset, @RandomInt, @RandomSeedId)",
                                                             new
                                                             {
                                                                 RandomObjectID = i,
                                                                 RandomString = RandomStringGenerator.RandomString(gen, 15),
                                                                 RandomDateTimeOffset = RandomDateTimeOffsetGenerator.RandomDateTimeOffset(gen),
                                                                 RandomInt = gen.Next(),
                                                                 RandomSeedId = seedID
                                                             });

                        i++;
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
        /// Gets data from database with Dapper
        /// </summary>
        /// <returns>Data string</returns>
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
                    result = conn.Query<RandomObjectModel>(@"SELECT TOP(1) RandomObjectID, RandomString, RandomDateTimeOffset, RandomInt, RandomSeedId
                                                             FROM RandomObject
                                                             ORDER BY RandomDateTimeOffset DESC").FirstOrDefault();
                }

                sw.Stop();

                Console.WriteLine("--Result was: " + result.RandomObjectID + ", " + result.RandomString + ", " + result.RandomDateTimeOffset + ", " + result.RandomInt + ", " + result.RandomSeedId);
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
        /// Gets data from database with Dapper
        /// </summary>
        /// <returns>Data string</returns>
        public static void DeleteData()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Truncating table!");

                sw.Start();

                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Execute("TRUNCATE TABLE RandomObject");
                }

                sw.Stop();

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
