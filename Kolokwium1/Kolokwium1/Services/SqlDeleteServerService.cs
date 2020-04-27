using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1.Services
{
    public class SqlDeleteServerService : IDeleteService
    {
        private static string ConString = "Data Source=db-mssql;Initial Catalog=s17301;Integrated Security=True";

        public string DeletePatient(int id)
        {
            using (var connection = new SqlConnection())
            using (var command = new SqlCommand())
            {
                connection.ConnectionString = ConString;
                command.Connection = connection;

                connection.Open();
                var transaction = connection.BeginTransaction();

                try
                {
                    command.Transaction = transaction;

                    command.CommandText =
                        $"Select * from Prescription where IdPatient = {id}";

                    var dataReader = command.ExecuteReader();


                    if (!dataReader.Read())
                    {
                        dataReader.Close();

                        command.CommandText =
                            $"Delete from Patient where IdPatient = {id}";
                        dataReader.Close();
                        //  dataReader = command.ExecuteNonQuery();

                        transaction.Commit();
                        return "Usunieto pacjenta";
                        
                    }
                    else
                    {
                        command.CommandText =
                            $"Delete from Prescription_Medicament where Prescription_Medicament.IdPrescription = Prescription.IdPrescription and and Prescription.IdPatient = {id}";
                        dataReader.Close();

                        command.CommandText =
                            $"Delete from Prescription where IdPatient = {id}";

                        
                        dataReader.Close();
                        transaction.Commit();
                        return "Usunięto pacjenta wraz z danymi o jego receptach";
                    }

                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return null;
                }
            }

            //return "";
        }
    }
}
