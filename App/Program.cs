﻿using CoreApp.Entities.DB;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;

namespace CoreApp
{
    class Program
    {
        private static readonly string ConnectionString = @"data source=OSTMYLLY\SQLEXPRESS;initial catalog=Thesis;integrated security=True";

        private static readonly int Rows = 10000;
        private static readonly int SeedId = 2;

        static void Main()
        {
            Console.WriteLine("SeedId is: " + SeedId + " and number of rows is: " + Rows);

            // ADO.Net

            var adosw = new Stopwatch();

            Console.WriteLine("\n------------------------------------\n");
            Console.WriteLine("ADO.Net starts!\n");
            Console.WriteLine("------------------------------------\n");

            adosw.Start();

            ADONetDAL.InsertData(SeedId, Rows);
            ADONetDAL.GetData();
            ADONetDAL.DeleteData();

            adosw.Stop();

            // Dapper

            var dappersw = new Stopwatch();

            Console.WriteLine("\n------------------------------------\n");
            Console.WriteLine("Dapper starts!\n");
            Console.WriteLine("------------------------------------\n");

            dappersw.Start();

            DapperDAL.InsertData(SeedId, Rows);
            DapperDAL.GetData();
            DapperDAL.DeleteData();

            dappersw.Stop();

            // EF
            var efsw = new Stopwatch();

            Console.WriteLine("\n------------------------------------\n");
            Console.WriteLine("Entity Framework starts!\n");
            Console.WriteLine("------------------------------------\n");

            efsw.Start();

            EntityFrameworkDAL.InsertData(SeedId, Rows);
            EntityFrameworkDAL.GetData();
            EntityFrameworkDAL.DeleteData();

            efsw.Stop();

            Console.WriteLine("------------------------------------\n");
            Console.WriteLine("Entity Framework time: " + efsw.Elapsed);
            Console.WriteLine("Dapper time:           " + dappersw.Elapsed);
            Console.WriteLine("ADO.Net time:          " + adosw.Elapsed);

            Console.WriteLine("Press any key to close this window.");
            Console.ReadKey();
        }

        #region EntityFramework

        /// <summary>
        /// Inserts data with EntityFramework
        /// </summary>
        /// <param name="seed">Randomnumber generators seed</param>
        static void InsertDataEF(int seed, int rows)
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Inserting data!");

                sw.Start();

                var gen = new Random(seed);
                var i = 0;
                var result = false;

                using (var db = new ThesisContext())
                {
                    while (i < rows)
                    {
                        var row = new RandomNumber() { IntValue = gen.Next() };

                        db.RandomNumber.Add(row);

                        i++;
                    }

                    db.SaveChanges();

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
        /// Gets data from database with EntityFramework
        /// </summary>
        /// <returns></returns>
        static void GetDataEF()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Fetching data!");

                sw.Start();

                var result = 0;

                using (var db = new ThesisContext())
                {
                    result = (from rn in db.RandomNumber
                              orderby rn.IntValue descending
                              select rn.IntValue).FirstOrDefault();
                }

                sw.Stop();

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
        /// Truncates table
        /// </summary>
        /// <returns></returns>
        static void DeleteDataEF()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Truncating table!");

                sw.Start();

                var result = 0;

                using (var db = new ThesisContext())
                {
                    db.Database.ExecuteSqlInterpolated($"TRUNCATE TABLE RandomNumber");
                }

                sw.Stop();

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

        #endregion

        #region Dapper

        /// <summary>
        /// Insert data to table with Dapper
        /// </summary>
        /// <param name="seed">Randomnumber generators seed</param>
        private static void InsertDataDapper(int seed, int rows)
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Inserting data!");

                sw.Start();

                var gen = new Random(seed);
                var i = 0;
                var result = false;

                using (var conn = new SqlConnection(ConnectionString))
                {
                    while (i < rows)
                    {
                        conn.Execute("INSERT INTO RandomNumber(IntValue) Values(@val)", new { val = gen.Next() });

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
        static void GetDataDapper()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Fetching data!");

                sw.Start();

                var result = 0;

                using (var conn = new SqlConnection(ConnectionString))
                {
                    result = conn.Query<int>("SELECT TOP(1) IntValue FROM RandomNumber ORDER BY IntValue DESC").FirstOrDefault();
                }

                sw.Stop();

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
        static void DeleteDataDapper()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Truncating table!");

                sw.Start();

                var result = 0;

                using (var conn = new SqlConnection(ConnectionString))
                {
                    result = conn.Execute("TRUNCATE TABLE RandomNumber");
                }

                sw.Stop();

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
        #endregion

        #region ADO.Net

        /// <summary>
        /// Insert data to table with ADO.Net
        /// </summary>
        /// <param name="seed">Randomnumber generators seed</param>
        private static void InsertDataADONet(int seed, int rows)
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Inserting data!");

                sw.Start();

                var gen = new Random(seed);
                var i = 0;
                var result = false;

                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    while (i < rows)
                    {
                        using (SqlCommand cmd = new SqlCommand("INSERT INTO RandomNumber(IntValue) Values(" + gen.Next() + ")", conn))
                        {
                            cmd.ExecuteNonQuery();
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
        static void GetDataADONet()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Fetching data!");

                sw.Start();

                var result = 0;

                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SELECT TOP(1) IntValue FROM RandomNumber ORDER BY IntValue DESC", conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            result = (int)reader["IntValue"];
                        }
                    }
                }

                sw.Stop();

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
        /// Insert data to table with ADO.Net
        /// </summary>
        /// <param name="seed">Randomnumber generators seed</param>
        private static void DeleteDataADONet()
        {
            try
            {
                var sw = new Stopwatch();

                Console.WriteLine("--Truncating table!");

                sw.Start();

                var result = false;

                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("TRUNCATE TABLE RandomNumber", conn))
                    {
                        cmd.ExecuteNonQuery();
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
        #endregion
    }
}

