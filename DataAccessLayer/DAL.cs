using System;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace DataAccessLayer
{
    public class DBFacade
    {
        private SqlConnection connection;
        private SqlCommand command;
        private string connectionString = string.Empty;

        public DBFacade(string ConnectionString)
        {
            this.connectionString = ConnectionString;
            connection = new SqlConnection(connectionString);
        }
        private void Connect()
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException exception)
                    {
                        throw exception;
                    }
                }
            }
        }
        private void Disconnect()
        {
            if (connection != null)
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }
        public DataTable GetTable(string query, bool isProcedure, params SqlParameter[] sqlParameters)
        {
            Connect();
            command = connection.CreateCommand();
            command.CommandText = query;
            if (isProcedure)
            {
                command.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                command.CommandType = CommandType.Text;
            }
            foreach (var parameters in sqlParameters)
            {
                command.Parameters.Add(parameters);
            }
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            Disconnect();
            return table;
        }
        public bool Execute(string query, bool isProcedure, params SqlParameter[] sqlParameters)
        {
            Connect();
            command = connection.CreateCommand();
            command.CommandText = query;

            if (isProcedure)
            {
                command.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                command.CommandType = CommandType.Text;
            }
            foreach (var parameters in sqlParameters)
            {
                command.Parameters.Add(parameters);
            }
            int tmp = command.ExecuteNonQuery();
            Disconnect();
            return (tmp != 0);
        }
        public long GetScalar(string query, bool isProcedure, params SqlParameter[] sqlParameters)
        {
            Connect();
            command = connection.CreateCommand();
            command.CommandText = query;

            if (isProcedure)
            {
                command.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                command.CommandType = CommandType.Text;
            }
            
            foreach (var parameters in sqlParameters)
            {
                command.Parameters.Add(parameters);
            }
            long tmp = (long)command.ExecuteScalar();
            Disconnect();
            return tmp;
        }
    }
}
