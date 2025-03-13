using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Drawing;
using System.Linq;
using System.Net;

namespace AppA_LightBulb {
    public partial class MainForm : Form {
        private static readonly HttpClient client = new HttpClient();
        private string configFilePath = "config.xml";
        private string outputFilePath;
        private DateTime startTime;
        private DateTime endTime;

        private string apiEndpoint = "http://localhost:61626/api/somiod/";

        private MqttClient mqttClient;
        private string mqttBroker = "127.0.0.1";
        private string topic = "light_bulb";

        private string appName = "Lighting"; 
        private string containerName = "light_bulb"; 

        public MainForm() {
            InitializeComponent();
            LoadConfiguration();
            InitializeMqtt();
        }

        private async void MainForm_Load(object sender, EventArgs e) {
            try {
                LoadConfiguration();
                Console.WriteLine("Configurações carregadas com sucesso.");


                await CheckApplication();

                await CheckContainer();

                await CheckNotification();

                LoadLastNotificationState();

                MessageBox.Show("Configurações carregadas com sucesso.");
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao criar recursos: {ex.Message}");
                MessageBox.Show("Erro ao inicializar os recursos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadConfiguration() {
            var config = XElement.Load(configFilePath);
            outputFilePath = config.Element("OutputFile").Value;

            startTime = DateTime.Parse(config.Element("TimeRange").Element("Start").Value);
            endTime = DateTime.Parse(config.Element("TimeRange").Element("End").Value);
        }

        private async Task CreateApplication() {
            var xmlData = new XElement("Application",
                new XElement("Name", appName)
            ).ToString();

            var content = new StringContent(xmlData, Encoding.UTF8, "application/xml");
            var response = await client.PostAsync(apiEndpoint, content);

            response.EnsureSuccessStatusCode();
        }


        private async Task CreateContainer() {
            var xmlData = new XElement("Container",
                new XElement("Name", containerName)
            ).ToString();

            var url = apiEndpoint + appName;
            var content = new StringContent(xmlData, Encoding.UTF8, "application/xml");
            var response = await client.PostAsync(url, content);

            response.EnsureSuccessStatusCode();
        }

        private async Task CreateNotification() {
            var doc = new XDocument(
                    new XElement("Notification",
                        new XElement("Name", "Sub1"),
                        new XElement("Event", 1),
                        new XElement("Endpoint", $"mqtt://127.0.0.1/{topic}"),
                        new XElement("Enabled", true)
                    )
                ).ToString();

            var url = apiEndpoint + appName + "/" + containerName + "/notif";
            var content = new StringContent(doc, Encoding.UTF8, "application/xml");
            var response = await client.PostAsync(url, content);

            response.EnsureSuccessStatusCode();
        }

        private void LoadLastNotificationState() {
            try {
                if (File.Exists(outputFilePath)) {
                    var doc = XDocument.Load(outputFilePath);

                    var lastNotification = doc.Root.Elements("Notification").LastOrDefault();

                    if (lastNotification != null) {
                        string lastMessage = lastNotification.Element("Message").Value.Trim();

                        DisplayImageBasedOnPayload(lastMessage);
                    } else {
                        Console.WriteLine("Nenhuma notificação encontrada.");
                    }
                } else {
                    Console.WriteLine($"Ficheiro {outputFilePath} não encontrado.");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao carregar o estado da última notificação: {ex.Message}");
            }
        }


        private void InitializeMqtt() {
            mqttClient = new MqttClient(mqttBroker);
            mqttClient.MqttMsgPublishReceived += OnMqttMessageReceived;

            string clientId = Guid.NewGuid().ToString();
            mqttClient.Connect(clientId);

            mqttClient.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            Console.WriteLine($"Subscribed to topic: {topic}");
        }

        private void OnMqttMessageReceived(object sender, MqttMsgPublishEventArgs e) {
            var payload = Encoding.UTF8.GetString(e.Message);
            Console.WriteLine("Recieved Message: " + payload);
            HandleNotification(payload);
        }

        private void HandleNotification(string payload) {
            DateTime now = DateTime.Now;

            if (now >= startTime && now <= endTime) {
                var doc = new XDocument(
                    new XElement("Notification",
                        new XElement("Timestamp", now),
                        new XElement("Message", payload)
                    )
                );

                AppendToXmlFile(doc);
                Console.WriteLine("Notificação guardada.");

                DisplayImageBasedOnPayload(payload);
            } else {
                Console.WriteLine("A notificação não está no intervalo de tempo.");
            }
        }

        private void DisplayImageBasedOnPayload(string payload) {
            try {
                if (payload.Trim().ToLower() == "light on") {
                    pictureBox.Image = Image.FromFile("light_on.jpg");
                    Console.WriteLine("Displayed: light_on.jpg");
                } else if (payload.Trim().ToLower() == "light off") {
                    pictureBox.Image = Image.FromFile("light_off.jpg");
                    Console.WriteLine("Displayed: light_off.jpg");
                } else {
                    Console.WriteLine($"Mensagem desconhecida recebida: {payload}");
                }
            }
            catch (FileNotFoundException ex) {
                Console.WriteLine($"Erro: Imagem não encontrada. {ex.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao exibir imagem: {ex.Message}");
            }
        }

        private void AppendToXmlFile(XDocument doc) {
            if (File.Exists(outputFilePath)) {
                var existingDoc = XDocument.Load(outputFilePath);
                existingDoc.Root.Add(doc.Root);
                existingDoc.Save(outputFilePath);
            } else {
                var newDoc = new XDocument(new XElement("Notifications", doc.Root));
                newDoc.Save(outputFilePath);
            }
        }

        private void pictureBox_Click(object sender, EventArgs e) {

        }
        private async Task CheckApplication() {
            var url = $"{apiEndpoint}/{appName}";
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound) {
                MessageBox.Show("Aplicação não encontrada. A criar aplicação...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CreateApplication();
            }
        }

        private async Task CheckContainer() {
            var url = $"{apiEndpoint}/{appName}/{containerName}";
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound) {
                MessageBox.Show("Contentor não encontrada. A criar contentor...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CreateContainer();
            }
        }

        private async Task CheckNotification() {
            var url = $"{apiEndpoint}/{appName}/{containerName}/notif/Sub1";
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound) {
                MessageBox.Show("Notificação não encontrada. A criar notificação...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CreateNotification();
            }
        }
    }
}
    