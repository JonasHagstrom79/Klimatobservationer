using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Klimatobservationer//.Observation
{
    class ObservationDataBase
    {
        private static readonly string connectionString = "Server = localhost; Port=5432; Database=climate; User ID = demouser; Password=Hemligt";

        #region Add/Remove

        /// <summary>
        /// Adds observer to Database
        /// </summary>
        /// <param name="observer"></param>
        /// <returns>Object</returns>
        public Observer AddObserver(Observer observer) 
        {
            string stmt = "insert into observer(firstname, lastname) values (@firstname, @lastname) returning id";
                       

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);
                command.Parameters.AddWithValue("firstname", observer.FirstName);
                command.Parameters.AddWithValue("lastname", observer.LastName);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    observer.ObserverId = (int)reader["id"];
                }
                
                return observer;
                
            }
            catch (PostgresException ex)
            {                
                string errorcode = ex.SqlState;                
                throw new Exception("Fel i AddObserver", ex);                
            }
            
        }

        /// <summary>
        /// Removes Observer from DB
        /// </summary>
        /// <param name="observer"></param>
        public void RemoveObserver(Observer observer) 
        {
            string stmt = "delete from observer where id = @id";

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);
                command.Parameters.AddWithValue("id", observer.ObserverId);             
                              
                using var reader = command.ExecuteReader();

            }
            catch (PostgresException exception)
            {
                string errorcode = exception.SqlState;                
                throw new Exception($"{observer.FirstName} {observer.LastName} har utfört klimatobservationer och kan därför ej raderas från databasen", exception);
            }            
        
        }

        /// <summary>
        /// Adds a measurement to DB
        /// </summary>
        /// <param name="observation"></param>
        /// <param name="measurement"></param>
        /// <param name="index"></param>
        /// <returns>Object</returns>
        public Measurement AddMeasurementWithIndex(Observation observation, Measurement measurement, int index) 
        {
            string stmt = "insert into measurement (value, observation_id, category_id) values(@value, @observation_id, @category_id) returning id";
            
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("value", measurement.Value);
                command.Parameters.AddWithValue("observation_id", observation.ObserverId);
                command.Parameters.AddWithValue("category_id", index);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    observation.ObserverId = (int)reader["id"]; 
                }
                return measurement; 
            }
            catch (PostgresException ex)
            {
                string errorcode = ex.SqlState;
                throw new Exception("Ange Mätpunkt innan du anger antal", ex); 
            }

        }


        /// <summary>
        /// Adds an Observation in DB
        /// </summary>
        /// <param name="observation"></param>
        /// <returns>Object</returns>
        public Observation AddObservation(Observation observation, Observer observer) 
        {
            
            string stmt = "insert into observation (observer_id, geolocation_id ) values(@observer_id, @geolocation_id) returning id";

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("observer_id", observer.ObserverId);
                command.Parameters.AddWithValue("geolocation_id", 1 );                               

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    observation.ObserverId = (int)reader["id"]; 
                }
                return observation;
            }
            catch (PostgresException ex)
            {
                string errorcode = ex.SqlState;
                throw new Exception("Fel i AddObservation", ex);
            }

        }

        /// <summary>
        /// Updates a measurment made by an observer
        /// </summary>
        /// <param name="measurement"></param>
        /// <param name="value"></param>
        /// <returns>Object</returns>
        public Measurement UpdateMeasurement(Measurement measurement, int value)
        {
            string stmt = "update measurement set value = @value where observation_id = @observation_id"; 
                                 

            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                using var command = new NpgsqlCommand(stmt, conn);

                command.Parameters.AddWithValue("id", measurement.MeasurementId);
                command.Parameters.AddWithValue("value", value);
                command.Parameters.AddWithValue("observation_id", measurement.ObservationId);
                command.Parameters.AddWithValue("category_id", measurement.CategoryId);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    measurement.MeasurementId = (int)reader["id"]; 
                }                
                return measurement; 
            }
            catch (PostgresException ex)
            {
                string errorcode = ex.SqlState;              
                throw new Exception("Fel i UpdateMeasurement", ex);
            }

        }




        #endregion

        #region Read

        /// <summary>
        /// Gets all observers from database, ordered by lastname ascending
        /// </summary>
        /// <returns>List</returns>
        public List<Observer> GetObservers() 
        {            
            string stmt = "select * from observer order by lastname asc";
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
            using var reader = command.ExecuteReader();

            Observer observer = null;           

            var observers = new List<Observer>();

            while (reader.Read())
            {
                observer = new Observer
                {
                    FirstName = (string)reader["firstname"],
                    LastName = (string)reader["lastname"],
                    ObserverId = (int)reader["id"]
                };
                observers.Add(observer);
            }
            return observers;
        }

        /// <summary>
        /// Gets a list of observations from observer
        /// </summary>
        /// <returns>List</returns>
        public List<Observation> GetObservatons(Observer observer) 
        {
            string stmt = "SELECT * from observation WHERE observer_id = @observer_id";
             
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);           


            Observation observation =  null;
            command.Parameters.AddWithValue("observer_id", observer.ObserverId); 
            using var reader = command.ExecuteReader();                                                               

            var observations = new List<Observation>();

            while (reader.Read())
            {
                observation = new Observation
                {
                    ObservationId = (int)reader["id"],
                    Date = (DateTime)reader["date"],
                    ObserverId = (int)reader["observer_id"],
                    GeolocationId = (int)reader["geolocation_id"]
                };
                observations.Add(observation);
            }
            return observations;
        }

        /// <summary>
        /// Get measurements from an observation
        /// </summary>
        /// <returns>List</returns>
        public List<Measurement> GetMeasurements(Observation observation)
        {
            string stmt = "SELECT * from measurement WHERE observation_id = @observer_id"; 

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();
            using var command = new NpgsqlCommand(stmt, conn);
                                   

            command.Parameters.AddWithValue("observer_id", observation.ObservationId);
            using var reader = command.ExecuteReader();                                                                

            Measurement measurement = null;

            var measurements = new List<Measurement>(); 

            while (reader.Read())
            {
                measurement = new Measurement
                {
                    MeasurementId = (int)reader["id"],
                    Value = (double)reader["value"],
                    ObservationId = (int)reader["observation_id"],
                    CategoryId = (int)reader["category_id"]
                };
                measurements.Add(measurement);
            }
            return measurements;
        }

        #endregion

        


    }

}
