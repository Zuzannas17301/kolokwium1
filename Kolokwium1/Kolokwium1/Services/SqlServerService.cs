using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Kolokwium1.DTOs.Responses;
using Kolokwium1.Models;

namespace Kolokwium1.Services
{
    public class SqlServerService : IDbService
    {
        private static string ConString = "Data Source=db-mssql;Initial Catalog=s17301;Integrated Security=True";

        public GetMedicamentResponse GetMedicament(int id)
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
                        $"Select Name, Description, Type from Medicament where IdMedicament = {id}";

                    var dataReader = command.ExecuteReader();

                    if (!dataReader.Read())
                    {
                        dataReader.Close();
                        transaction.Rollback();
                        return null;
                    }

                    var medicamentDetails = new GetMedicamentResponse()
                    {
                        Name = (dataReader["Name"].ToString()),
                        Description = (dataReader["Description"].ToString()),
                        Type = (dataReader["Type"].ToString()),
                        PrescitionList = new List<Presciption>()
                    };

                    command.CommandText =
                        $"Select IdPrescription FROM Prescription_Medicament where IdMedicament = {id}";
                    dataReader.Close();

                    dataReader = command.ExecuteReader();
                    var prescriptionsIdList = new List<int>();

                    while (dataReader.Read())
                    {
                        prescriptionsIdList.Add(int.Parse(dataReader["IdPrescription"].ToString()));
                    }

                    foreach (var idPres in prescriptionsIdList)
                    {
                        dataReader.Close();
                        command.CommandText =
                            $"SELECT Dose, Details FROM Prescription_Medicament WHERE IdMedicament = {id} AND IdPrescription = {idPres}";
                        dataReader = command.ExecuteReader();

                        var dose = int.Parse(dataReader["Dose"].ToString());
                        var details = dataReader["Details"].ToString();
                        dataReader.Close();

                        command.CommandText =
                            $"SELECT Date, DueDate, IdPatient, IdDoctor FROM Prescription WHERE IdPrescription = {idPres} order by Date desc";
                        dataReader = command.ExecuteReader();
                        medicamentDetails.PrescitionList.Add(new Presciption()
                        {
                            IdPrescription = idPres,
                            Date = DateTime.Parse((string) dataReader["Date".ToString()]),
                            DueDate = DateTime.Parse((string) dataReader["DueDate"].ToString()),
                            IdPatient = int.Parse(dataReader["IdPatient"].ToString()),
                            IdDoctor = int.Parse(dataReader["IdDoctor"].ToString())
                        });
                    }
                    
                    dataReader.Close();
                    transaction.Commit();
                    return medicamentDetails;

                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return null;
                }

            }
        }
    }
}