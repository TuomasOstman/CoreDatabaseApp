using System;
using System.Diagnostics;
using Dapper;
using System.Linq;
using Microsoft.Data.SqlClient;
using App.Entities.DB;

namespace CoreDatabaseApp
{
    class Program
    {
        private static readonly string _connectionString = @"data source=OSTMYLLY\SQLEXPRESS;initial catalog=Thesis;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        static void Main()
        {
            var eFStopwatch = new Stopwatch();

            // EntityFramework
            eFStopwatch.Start();

            Console.WriteLine("Fetchin data With EntityFramework!\n");

            var EFResult = GetDataEF();

            Console.WriteLine("Result was: \n");

            Console.WriteLine(EFResult + "\n");

            eFStopwatch.Stop();

            Console.WriteLine("Time Elapsed: " + eFStopwatch.Elapsed + "\n");

            Console.WriteLine("\n------------------------------------\n");

            //Dapper

            var dapperStopwatch = new Stopwatch();
            dapperStopwatch.Start();

            Console.WriteLine("Fetchin data With Dapper!\n");

            var DapperResult = GetDataDapper();
            Console.WriteLine("Result was: \n");

            Console.WriteLine(DapperResult + "\n");

            dapperStopwatch.Stop();

            Console.WriteLine("Time Elapsed: " + dapperStopwatch.Elapsed + "\n");

            Console.WriteLine("Press any key to close this window.");
            Console.ReadKey();
        }

        static string GetDataEF()
        {
            try
            {

                using (var db = new ThesisContext())
                {
                    return (from tt in db.TestTable
                            where tt.Id == 1
                            select tt.Data).FirstOrDefault().ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: " + ex);
                return ex.Message;
            }
        }
        static string GetDataDapper()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    return conn.Query<string>("SELECT Data FROM TestTable WHERE ID = 1").FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: " + ex);
                return ex.Message;
            }
        }
    }
}
