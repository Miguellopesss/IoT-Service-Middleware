using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json.Linq;
using SOMIOD_API.Models;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Web.Services.Description;

namespace SOMIOD_API.Controllers
{
    public class SomiodController : ApiController
    {
        private static MqttClient mClient;
        string connectionString = Properties.Settings.Default.ConnStr;
        static SomiodController()
        {
        }

        ///////////////////////////////-----GET
        // GET: api/somiod
        [HttpGet]
        [Route("api/somiod")]
        public IHttpActionResult Get()
        {
            var requestHeaders = Request.Headers;
            if (requestHeaders.Contains("somiod-locate") && requestHeaders.GetValues("somiod-locate").First() == "application")
            {
                return GetApplicationNames();
            }
            else
            {
                return GetAllApplications();
            }
        }

        private IHttpActionResult GetAllApplications()
        {
            List<Application> applications = new List<Application>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Application";
                    using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Application app = new Application
                                {
                                    Id = (int)reader["Id"],
                                    Name = (string)reader["Name"],
                                    CreationDateTime = reader["CreationDateTime"] != DBNull.Value ? (DateTime)reader["CreationDateTime"] : default(DateTime)
                                };
                                applications.Add(app);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(applications);
        }

        private IHttpActionResult GetApplicationNames()
        {
            List<string> applicationNames = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Name FROM Application";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                applicationNames.Add((string)reader["Name"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(applicationNames);
        }

        // api/somiod/AppilcationName
        [Route("api/somiod/{appName}")]
        [HttpGet]
        public IHttpActionResult GetAplicationContainers(string appName)
        {
            var requestHeaders = Request.Headers;
            if (requestHeaders.Contains("somiod-locate") && requestHeaders.GetValues("somiod-locate").First() == "container")
            {
                return GetApplicationAllContainers(appName);
            }
            else if (requestHeaders.Contains("somiod-locate") && requestHeaders.GetValues("somiod-locate").First() == "notification")
            {
                return GetApplicationAllNotification(appName);
            }
            else if (requestHeaders.Contains("somiod-locate") && requestHeaders.GetValues("somiod-locate").First() == "record")
            {
                return GetApplicationAllRecord(appName);
            }
            else
            {
                return GetApplication(appName);
            }
        }

        private IHttpActionResult GetApplicationAllNotification(string appName)
        {
            IHttpActionResult applicationResult = GetApplication(appName);
            if (applicationResult is NotFoundResult)
            {
                return NotFound();
            }

            Application app = ((OkNegotiatedContentResult<Application>)applicationResult).Content;
            List<Container> listContainers = new List<Container>();
            List<string> listNotificationNames = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string getContainersQuery = "SELECT Id, Name, CreationDateTime, Parent FROM Container WHERE Parent = @appId";
                    using (SqlCommand getContainersCommand = new SqlCommand(getContainersQuery, connection))
                    {
                        getContainersCommand.Parameters.AddWithValue("@appId", app.Id);
                        using (SqlDataReader containerReader = getContainersCommand.ExecuteReader())
                        {
                            while (containerReader.Read())
                            {
                                Container container = new Container
                                {
                                    Id = (int)containerReader["Id"],
                                    Name = (string)containerReader["Name"],
                                    CreationDateTime = (DateTime)containerReader["CreationDateTime"],
                                    Parent = (int)containerReader["Parent"]
                                };
                                listContainers.Add(container);
                            }
                        }
                    }
                    foreach (Container container in listContainers)
                    {
                        string getNotificationsQuery = "SELECT Name FROM Notification WHERE Parent = @containerId";
                        using (SqlCommand getNotificationsCommand = new SqlCommand(getNotificationsQuery, connection))
                        {
                            getNotificationsCommand.Parameters.AddWithValue("@containerId", container.Id);
                            using (SqlDataReader notificationReader = getNotificationsCommand.ExecuteReader())
                            {
                                while (notificationReader.Read())
                                {
                                    string notificationName = (string)notificationReader["Name"];
                                    listNotificationNames.Add(notificationName);
                                }
                            }
                        }
                    }
                }
                return Ok(listNotificationNames);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private IHttpActionResult GetApplicationAllRecord(string appName)
        {
            IHttpActionResult applicationResult = GetApplication(appName);
            if (applicationResult is NotFoundResult)
            {
                return NotFound();
            }

            Application app = ((OkNegotiatedContentResult<Application>)applicationResult).Content;
            List<Container> listContainers = new List<Container>();
            List<string> listRecordNames = new List<string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string getContainersQuery = "SELECT Id, Name, CreationDateTime, Parent FROM Container WHERE Parent = @appId";
                    using (SqlCommand getContainersCommand = new SqlCommand(getContainersQuery, connection))
                    {
                        getContainersCommand.Parameters.AddWithValue("@appId", app.Id);
                        using (SqlDataReader containerReader = getContainersCommand.ExecuteReader())
                        {
                            while (containerReader.Read())
                            {
                                Container container = new Container
                                {
                                    Id = (int)containerReader["Id"],
                                    Name = (string)containerReader["Name"],
                                    CreationDateTime = (DateTime)containerReader["CreationDateTime"],
                                    Parent = (int)containerReader["Parent"]
                                };
                                listContainers.Add(container);
                            }
                        }
                    }

                    foreach (Container container in listContainers)
                    {
                        string getRecordsQuery = "SELECT Name FROM Record WHERE Parent = @containerId";
                        using (SqlCommand getRecordsCommand = new SqlCommand(getRecordsQuery, connection))
                        {
                            getRecordsCommand.Parameters.AddWithValue("@containerId", container.Id);
                            using (SqlDataReader recordReader = getRecordsCommand.ExecuteReader())
                            {
                                while (recordReader.Read())
                                {
                                    string recordName = (string)recordReader["Name"];
                                    listRecordNames.Add(recordName);
                                }
                            }
                        }
                    }
                }
                return Ok(listRecordNames);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }

        private IHttpActionResult GetApplication(string appName)
        {
            try
            {
                string query = "SELECT * FROM Application WHERE Name = @appName";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@appName", appName);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Application app = null;
                            if (reader.Read())
                            {
                                app = new Application
                                {
                                    Id = (int)reader["Id"],
                                    Name = (string)reader["Name"],
                                    CreationDateTime = reader.GetDateTime(reader.GetOrdinal("CreationDateTime"))
                                };
                            }
                            if (app == null)
                            {
                                return NotFound(); 
                            }
                            return Ok(app); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }

        private IHttpActionResult GetApplicationAllContainers(string appName)
        {
            IHttpActionResult applicationResult = GetApplication(appName);
            if (applicationResult is NotFoundResult)
            {
                return NotFound();
            }
            Application app = ((OkNegotiatedContentResult<Application>)applicationResult).Content;
            List<string> listContainerNames = new List<string>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand getContainersCommand = new SqlCommand("SELECT Name FROM Container WHERE Parent = @appId", connection))
                    {
                        getContainersCommand.Parameters.AddWithValue("@appId", app.Id);
                        using (SqlDataReader reader = getContainersCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string containerName = (string)reader["Name"];
                                listContainerNames.Add(containerName);
                            }
                        }
                    }
                }
                return Ok(listContainerNames);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }

        // api/somiod/AppilcationName/ContainerName
        [Route("api/somiod/{appName}/{contName}")]
        [HttpGet]
        public IHttpActionResult GetContainer(string appName, string contName)
        {
            try
            {
                string query = @"
            SELECT c.* 
            FROM Container c
            INNER JOIN Application a ON c.Parent = a.Id
            WHERE c.Name = @contName AND a.Name = @appName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@contName", contName);
                        command.Parameters.AddWithValue("@appName", appName);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Container container = null;
                            if (reader.Read())
                            {
                                container = new Container
                                {
                                    Id = (int)reader["Id"],
                                    Name = (string)reader["Name"],
                                    CreationDateTime = reader.GetDateTime(reader.GetOrdinal("CreationDateTime")),
                                    Parent = (int)reader["Parent"]
                                };
                            }
                            if (container == null)
                            {
                                return NotFound();
                            }
                            return Ok(container); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }

        // api/somiod/appName/contName/notif/NotifName
        [Route("api/somiod/{appName}/{contName}/notif/{NotifName}")]
        [HttpGet]
        public IHttpActionResult GetNotification(string appName, string contName, string NotifName)
        {
            try
            {
                string query = @"
            SELECT n.* 
            FROM Notification n
            INNER JOIN Container c ON n.Parent = c.Id
            INNER JOIN Application a ON c.Parent = a.Id
            WHERE n.Name = @notifName AND c.Name = @contName AND a.Name = @appName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@notifName", NotifName);
                    command.Parameters.AddWithValue("@contName", contName);
                    command.Parameters.AddWithValue("@appName", appName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Notification notification = null;
                        if (reader.Read())
                        {
                            notification = new Notification
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Endpoint = (string)reader["Endpoint"],
                                CreationDateTime = reader.GetDateTime(reader.GetOrdinal("CreationDatetime")),
                                Parent = (int)reader["Parent"],
                                Event = (int)reader["Event"],
                                Enabled = (bool)reader["enabled"]
                            };
                        }

                        if (notification == null)
                        {
                            return NotFound();
                        }
                        return Ok(notification); 
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }

        // api/somiod/appName/contName/record/recordName
        [Route("api/somiod/{appName}/{contName}/record/{recordName}")]
        [HttpGet]
        public IHttpActionResult GetRecord(string appName, string contName, string recordName)
        {
            try
            {
                string query = @"
            SELECT r.* 
            FROM Record r
            INNER JOIN Container c ON r.Parent = c.Id
            INNER JOIN Application a ON c.Parent = a.Id
            WHERE r.Name = @recordName AND c.Name = @contName AND a.Name = @appName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@recordName", recordName);
                    command.Parameters.AddWithValue("@contName", contName);
                    command.Parameters.AddWithValue("@appName", appName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Record record = null;
                        if (reader.Read())
                        {
                            record = new Record
                            {
                                Id = (int)reader["Id"],
                                Name = (string)reader["Name"],
                                Content = (string)reader["Content"],
                                CreationDateTime = reader.GetDateTime(reader.GetOrdinal("CreationDatetime")),
                                Parent = (int)reader["Parent"]
                            };
                        }

                        if (record == null)
                        {
                            return NotFound(); // Retorna 404 se o registro não foi encontrado
                        }
                        return Ok(record); // Retorna 200 com o registro
                    }
                }
            }
            catch (Exception ex)
            {
                // Log do erro (se necessário)
                return InternalServerError(ex); // Retorna 500 para erros inesperados
            }
        }

        ///////////////////////////////-----POST
        // Criar aplicacao
        [Route("api/somiod/")]
        [HttpPost]
        public IHttpActionResult PostApplication([FromBody] Application value)
        {
            if (value == null)
            {
                return BadRequest("Application object is null.");
            }

            try
            {
                string query = "INSERT INTO Application(Name,CreationDatetime) VALUES (@name, @date);";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@date", DateTime.Now);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("O Nome já existe na tabela NomeTable."))
                {
                    return BadRequest("O Nome já existe na tabela NomeTable.");
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Criar container
        [Route("api/somiod/{appName}/")]
        [HttpPost]
        public IHttpActionResult PostContainer(string appName, [FromBody] Container value)
        {
            if (value == null)
            {
                return BadRequest("Container object is null.");
            }
            try
            {
                int? parentId = null;
                string getAppIdQuery = "SELECT Id FROM Application WHERE Name = @appName";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand getAppCommand = new SqlCommand(getAppIdQuery, connection))
                    {
                        getAppCommand.Parameters.AddWithValue("@appName", appName);
                        object result = getAppCommand.ExecuteScalar();
                        if (result == null)
                        {
                            return BadRequest($"A aplicação '{appName}' não foi encontrada.");
                        }
                        parentId = (int)result;
                    }
                }

                    string query = "INSERT INTO Container(Name,CreationDatetime,Parent) VALUES (@name, @date, @parent);";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.Parameters.AddWithValue("@parent", parentId);


                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("O Nome já existe na tabela NomeTable."))
                {
                    return BadRequest("O Nome já existe na tabela NomeTable.");
                }
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {

                return InternalServerError();
            }
        }

        // Criar notificacao
        [Route("api/somiod/{appName}/{contName}/notif/")]
        [HttpPost]
        public IHttpActionResult PostNotification(string appName, string contName, [FromBody] Notification value)
        {
            if (value == null)
            {
                return BadRequest("Notification object is null.");
            }
            try
            {
                int? parentId = null;
                string query = @"
            SELECT c.Id 
            FROM Container c
            INNER JOIN Application a ON c.Parent = a.Id
            WHERE c.Name = @contName AND a.Name = @appName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@contName", contName);
                        command.Parameters.AddWithValue("@appName", appName);
                        object result = command.ExecuteScalar();
                        if (result == null)
                        {
                            return BadRequest($"O container '{contName}' não pertence à aplicação '{appName}' ou não foi encontrado.");
                        }
                        parentId = (int)result;
                    }
                }
                string insertQuery = "INSERT INTO Notification(Name, CreationDatetime, Parent, Event, Endpoint, Enabled) VALUES (@name, @date, @parent, @event, @endpoint, @enabled);";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@parent", parentId);
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.Parameters.AddWithValue("@event", value.Event);
                    command.Parameters.AddWithValue("@endpoint", value.Endpoint);
                    command.Parameters.AddWithValue("@enabled", value.Enabled);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        return Ok();
                    }
                    return BadRequest();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("O Nome já existe na tabela NomeTable."))
                {
                    return BadRequest("O Nome já existe na tabela NomeTable.");
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // Criar record
        [Route("api/somiod/{appName}/{contName}/record/")]
        [HttpPost]
        public IHttpActionResult PostRecord(string appName, string contName, [FromBody] Record value)
        {
            if (value == null)
            {
                return BadRequest("Record object is null.");
            }
            try
            {
                int? parentId = null;
                string getContainerIdQuery = @"
            SELECT c.Id 
            FROM Container c
            JOIN Application a ON c.Parent = a.Id
            WHERE c.Name = @contName AND a.Name = @appName";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand getContainerCommand = new SqlCommand(getContainerIdQuery, connection))
                    {
                        getContainerCommand.Parameters.AddWithValue("@contName", contName);
                        getContainerCommand.Parameters.AddWithValue("@appName", appName);
                        object result = getContainerCommand.ExecuteScalar();
                        if (result == null)
                        {
                            return BadRequest($"O container '{contName}' não foi encontrado na aplicação '{appName}'.");
                        }
                        parentId = (int)result;
                    }
                }

                string query = "INSERT INTO Record(Name, CreationDatetime, Content, Parent) VALUES (@name, @date, @content, @parent);";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@name", value.Name);
                    command.Parameters.AddWithValue("@date", DateTime.Now);
                    command.Parameters.AddWithValue("@parent", parentId);
                    command.Parameters.AddWithValue("@content", value.Content);

                    int rows = command.ExecuteNonQuery();
                    if (rows > 0)
                    {
                        string message = value.Content;
                        SendNotification(contName, message);

                        return Ok();
                    }
                    return BadRequest();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("O Nome já existe na tabela NomeTable."))
                {
                    return BadRequest("O Nome já existe na tabela NomeTable.");
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private void SendNotification(string contName, string message)
        {
            string getNotificationsQuery = @"
        SELECT n.Endpoint, a.Name 
        FROM Notification n
        JOIN Container c ON n.Parent = c.Id
        JOIN Application a ON c.Parent = a.Id
        WHERE c.Name = @contName AND n.Event = 1 AND n.Enabled = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(getNotificationsQuery, connection))
                {
                    command.Parameters.AddWithValue("@contName", contName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string endpoint = reader.GetString(0);
                            string appName = reader.GetString(1);

                            if (endpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    using (HttpClient client = new HttpClient())
                                    {
                                        var content = new StringContent(message, Encoding.UTF8, "application/json");
                                        client.PostAsync(endpoint, content).Wait();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Error sending HTTP notification to {endpoint}: {ex.Message}");
                                }
                            }
                            else if (endpoint.StartsWith("mqtt", StringComparison.OrdinalIgnoreCase))
                            {
                                string brokerAddress = endpoint.Substring("mqtt://".Length).Split('/')[0];
                                string topic = endpoint.Substring($"mqtt://{brokerAddress}/".Length);

                                try
                                {
                                    mClient = null;
                                    if (mClient == null || !mClient.IsConnected)
                                    {
                                        mClient = new MqttClient(brokerAddress);
                                        mClient.Connect(Guid.NewGuid().ToString());

                                        if (!mClient.IsConnected)
                                        {
                                            Debug.WriteLine("Erro ao conectar ao broker MQTT.");
                                            return;
                                        }
                                    }
                                    Debug.WriteLine($"Publishing to topic {topic} {message}");
                                    mClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Erro ao conectar ao broker MQTT: {ex.Message}");
                                }
                            }


                        }
                    }
                }
            }
        }

        ///////////////////////////////-----PUT / UPDATE
        // PUT: api/somiod/ApplicationName/ [Atualizar application]
        [Route("api/somiod/{appName}")]
        [HttpPut]
        public IHttpActionResult PutApplication(string appName, [FromBody] Application value)
        {
            if (value == null)
            {
                return BadRequest("Dados inválidos. Certifique-se de que todos os campos obrigatórios foram preenchidos.");
            }
            try
            {
                string query = @"UPDATE Application SET Name = @newName WHERE Name = @currentName";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@newName", value.Name);
                        command.Parameters.AddWithValue("@currentName", appName);

                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            return Ok($"Application '{appName}' atualizado com sucesso.");
                        }
                        return NotFound(); 
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("O Nome já existe na tabela NomeTable."))
                {
                    return BadRequest("O Nome já existe na tabela NomeTable.");
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }

        // PUT: api/somiod/ApplicationName/ContainerName [Atualizar container]
        [Route("api/somiod/{appName}/{contName}")]
        [HttpPut]
        public IHttpActionResult PutContainer(string appName, string contName, [FromBody] Container value)
        {
            if (value == null || string.IsNullOrEmpty(value.Name))
            {
                return BadRequest("Dados inválidos. Certifique-se de que todos os campos obrigatórios foram preenchidos.");
            }
            try
            {
                string query = @" UPDATE Container SET Name = @newName WHERE Name = @currentName AND Parent = (SELECT Id FROM Application WHERE Name = @appName)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@newName", value.Name);     
                        command.Parameters.AddWithValue("@currentName", contName);     
                        command.Parameters.AddWithValue("@appName", appName);          

                        int rows = command.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            return Ok($"Container '{contName}' na aplicação '{appName}' atualizado com sucesso.");
                        }
                        else
                        {
                            return NotFound(); 
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("O Nome já existe na tabela NomeTable."))
                {
                    return BadRequest("O Nome já existe na tabela NomeTable.");
                }
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex); 
            }
        }

        ///////////////////////////////-----DELETE 
        [HttpDelete]
        [Route("api/somiod/{appName}")]
        public IHttpActionResult DeleteApplication(string appName)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand checkContainersCommand = new SqlCommand("SELECT COUNT(*) FROM Container WHERE Parent = (SELECT Id FROM Application WHERE Name = @name)", connection);
                checkContainersCommand.Parameters.AddWithValue("@name", appName);
                int containerCount = (int)checkContainersCommand.ExecuteScalar();

                if (containerCount > 0)
                {
                    return BadRequest("Cannot delete application because there are containers associated with it.");
                }

                SqlCommand deleteCommand = new SqlCommand("DELETE FROM Application WHERE Name = @name", connection);
                deleteCommand.Parameters.AddWithValue("@name", appName);
                int rowsAffected = deleteCommand.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                Debug.WriteLine("Exception: " + ex.Message);
                throw; 
            }
            return Ok("Application deleted successfully.");
        }

        [HttpDelete]
        [Route("api/somiod/{appName}/{containerName}")]
        public IHttpActionResult DeleteContainer(string appName, string containerName)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                SqlCommand getContainerIdCommand = new SqlCommand("SELECT Id FROM Container WHERE Name = @containerName AND Parent = (SELECT Id FROM Application WHERE Name = @appName)", connection);
                getContainerIdCommand.Parameters.AddWithValue("@containerName", containerName);
                getContainerIdCommand.Parameters.AddWithValue("@appName", appName);
                object containerIdObj = getContainerIdCommand.ExecuteScalar();

                if (containerIdObj == null)
                {
                    return NotFound();
                }
                int containerId = (int)containerIdObj;
                SqlCommand checkNotificationsCommand = new SqlCommand("SELECT COUNT(*) FROM Notification WHERE Parent = @containerId", connection);
                checkNotificationsCommand.Parameters.AddWithValue("@containerId", containerId);
                int notificationCount = (int)checkNotificationsCommand.ExecuteScalar();

                if (notificationCount > 0)
                {
                    return BadRequest("Cannot delete container because there are notifications associated with it.");
                }

                SqlCommand checkRecordsCommand = new SqlCommand("SELECT COUNT(*) FROM Record WHERE Parent = @containerId", connection);
                checkRecordsCommand.Parameters.AddWithValue("@containerId", containerId);
                int recordCount = (int)checkRecordsCommand.ExecuteScalar();

                if (recordCount > 0)
                {
                    return BadRequest("Cannot delete container because there are records associated with it.");
                }

                SqlCommand deleteCommand = new SqlCommand("DELETE FROM Container WHERE Id = @containerId", connection);
                deleteCommand.Parameters.AddWithValue("@containerId", containerId);
                int rowsAffected = deleteCommand.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                Debug.WriteLine("Exception: " + ex.Message);
                throw; 
            }
            return Ok("Container deleted successfully.");
        }

        [HttpDelete]
        [Route("api/somiod/{appName}/{containerName}/record/{recordName}")]
        public IHttpActionResult DeleteRecord(string appName, string containerName, string recordName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
            DELETE FROM Record 
            WHERE Name = @recordName 
            AND Parent = (
                SELECT Id FROM Container 
                WHERE Name = @containerName 
                AND Parent = (
                    SELECT Id FROM Application 
                    WHERE Name = @appName
                )
            )";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@recordName", recordName);
                        command.Parameters.AddWithValue("@containerName", containerName);
                        command.Parameters.AddWithValue("@appName", appName);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            string message = $"Record '{recordName}' deleted from container '{containerName}'.";

                            SendDeleteNotification(containerName, message);

                            return Ok();
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private void SendDeleteNotification(string contName, string message)
        {
            string getNotificationsQuery = @"
    SELECT n.Endpoint, a.Name 
    FROM Notification n
    JOIN Container c ON n.Parent = c.Id
    JOIN Application a ON c.Parent = a.Id
    WHERE c.Name = @contName AND n.Event = 2 AND n.Enabled = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(getNotificationsQuery, connection))
                {
                    command.Parameters.AddWithValue("@contName", contName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string endpoint = reader.GetString(0);
                            string appName = reader.GetString(1);

                            if (endpoint.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    using (HttpClient client = new HttpClient())
                                    {
                                        var content = new StringContent(message, Encoding.UTF8, "application/json");
                                        client.PostAsync(endpoint, content).Wait();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Error sending HTTP notification to {endpoint}: {ex.Message}");
                                }
                            }
                            else if (endpoint.StartsWith("mqtt", StringComparison.OrdinalIgnoreCase))
                            {
                                string brokerAddress = endpoint.Substring("mqtt://".Length).Split('/')[0];
                                string topic = endpoint.Substring($"mqtt://{brokerAddress}/".Length);
                                try
                                {
                                    mClient = null;
                                    if (mClient == null || !mClient.IsConnected)
                                    {
                                        mClient = new MqttClient(brokerAddress);
                                        mClient.Connect(Guid.NewGuid().ToString());

                                        if (!mClient.IsConnected)
                                        {
                                            Debug.WriteLine("Erro ao conectar ao broker MQTT.");
                                            return;
                                        }
                                    }
                                    Debug.WriteLine($"Publishing to topic {topic} {message}");
                                    mClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Erro ao conectar ao broker MQTT: {ex.Message}");
                                }
                            }

                        }
                    }
                }
            }
        }

        [HttpDelete]
        [Route("api/somiod/{appName}/{containerName}/notif/{NotifName}")]
        public IHttpActionResult DeleteNotification(string appName, string containerName, string NotifName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
            DELETE FROM Notification 
            WHERE Name = @notificationName 
            AND Parent = (
                SELECT Id FROM Container 
                WHERE Name = @containerName 
                AND Parent = (
                    SELECT Id FROM Application 
                    WHERE Name = @appName
                )
            )";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@notificationName", NotifName);
                        command.Parameters.AddWithValue("@containerName", containerName);
                        command.Parameters.AddWithValue("@appName", appName);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return Ok();
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


    }

}
