using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Helpers
{
    public static class PersonHelper
    {
        private const string _CONNECTION_STRING = @"Server=DESKTOP-MLES57C;Database=PhoneBook;Integrated Security=true";

        public static List<Person> GetAllConnected()
        {
            SqlConnection conn = new SqlConnection(_CONNECTION_STRING);

            SqlCommand cmd = new SqlCommand("select * from People", conn);

            if (conn.State == ConnectionState.Closed) conn.Open();

            //Read Proccess
            SqlDataReader dr = cmd.ExecuteReader();

            List<Person> people = new List<Person>();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Person person = new Person();
                    person.Id = Convert.ToInt32(dr["Id"]);
                    person.FirstName = dr["FirstName"].ToString();
                    person.LastName = dr["LastName"].ToString();
                    person.Phone = dr["Phone"].ToString();
                    person.Email = dr["Email"].ToString();

                    people.Add(person);
                }

                return people;
            }
            dr.Close();
            conn.Close();
            return null;
        }

        public static List<Person> GetAllPeople()
        {
            SqlDataAdapter da = new SqlDataAdapter("select * from People", _CONNECTION_STRING);

            DataTable dt = new DataTable();
            da.Fill(dt);

            var list = (from DataRow dr in dt.Rows
                        select new Person
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            Phone = dr["Phone"].ToString(),
                            Email = dr["Email"].ToString(),
                        }
               ).ToList();

            return list;
        }

        public static bool InsertPerson(Person person)
        {
            SqlConnection conn = new SqlConnection(_CONNECTION_STRING);

            SqlCommand cmd = new SqlCommand("insert into People values ( @FirstName, @LastName, @Phone, @Email)", conn);
            cmd.Parameters.Add("@Firstname", SqlDbType.NVarChar).Value = person.FirstName;
            cmd.Parameters.Add("@Lastname", SqlDbType.NVarChar).Value = person.LastName;
            cmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = person.Phone;
            cmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = person.Email;

            if (conn.State == ConnectionState.Closed) conn.Open();

            bool result = cmd.ExecuteNonQuery() > 0;

            conn.Close();

            return result;
        }

        public static bool DeletePerson(int id)
        {
            SqlConnection conn = new SqlConnection(_CONNECTION_STRING);

            SqlCommand cmd = new SqlCommand("delete from People where Id = @id", conn);

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            if (conn.State == ConnectionState.Closed) conn.Open();

            bool result = cmd.ExecuteNonQuery() > 0;

            conn.Close();

            return result;
        }

        public static List<Person> FilterPeople(string data)
        {
            List<Person> result = new List<Person>();

            SqlConnection conn = new SqlConnection(_CONNECTION_STRING);

            SqlCommand cmd = new SqlCommand();

            int res;

            if (int.TryParse(data, out res))
            {
                cmd = new SqlCommand($"select * from People where Id = {res} or FirstName = '{data}' or LastName = '{data}' or  Phone = '{data}' or Email = '{data}'", conn);
            }
            else
            {
                cmd = new SqlCommand($"select * from People where FirstName = '{data}' or LastName = '{data}' or  Phone = '{data}' or Email = '{data}'", conn);
            }


            if (conn.State == ConnectionState.Closed) conn.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Person newPerson = new()
                {
                    Id = Convert.ToInt32(dr["Id"]),
                    FirstName = dr["FirstName"].ToString(),
                    LastName = dr["LastName"].ToString(),
                    Phone = dr["Phone"].ToString(),
                    Email = dr["Email"].ToString()
                };

                result.Add(newPerson);
            }

            dr.Close();
            conn.Close();

            return result;
        }
    }
}
