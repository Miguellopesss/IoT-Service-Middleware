using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AppB_Switch {
    public partial class AppB_Form : Form {
        private static readonly HttpClient client = new HttpClient();
        private string configFilePath = "config.xml";
        private string apiEndpoint = "http://localhost:61626/api/somiod/";
        private string appName = "Switch";

        private async void AppB_Form_Load(object sender, EventArgs e) {
            try {
                await CheckApplication();

                MessageBox.Show("Inicialização completa.");
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao criar aplicação: {ex.Message}");
                MessageBox.Show("Erro ao inicializar a aplicação.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public AppB_Form() {
            InitializeComponent();
        }

        private async Task CreateApplication() {
            var xmlData = new XElement("Application",
                new XElement("Name", appName)
            ).ToString();

            var content = new StringContent(xmlData, Encoding.UTF8, "application/xml");
            var response = await client.PostAsync(apiEndpoint, content);

            response.EnsureSuccessStatusCode();
        }

        private async Task SendRecord(string content) {
            var url = $"{apiEndpoint}/Lighting/light_bulb/record"; 

            var recordName = Guid.NewGuid().ToString(); 
            var xmlData = new XElement("Record",
                new XElement("Name", recordName),
                new XElement("Content", content)
            ).ToString();

            try {
                var contentString = new StringContent(xmlData, Encoding.UTF8, "application/xml");
                var response = await client.PostAsync(url, contentString);

                if (!response.IsSuccessStatusCode) {
                    if (response.StatusCode == HttpStatusCode.BadRequest) {
                        await SendRecord(content);
                    } else {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        MessageBox.Show($"Erro ao enviar record: {responseBody}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                } else {
                    Console.WriteLine($"Record '{content}' enviado com sucesso.");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Erro ao enviar record: {ex.Message}");
            }
        }

        private async Task CheckApplication() {
            var url = $"{apiEndpoint}/{appName}";
            var response = await client.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound) {
                MessageBox.Show("Aplicação não encontrada. A criar aplicação...", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await CreateApplication();
            }
        }


        private async void button1_Click(object sender, EventArgs e) {
            await SendRecord("Light ON");
        }

        private async void button2_Click(object sender, EventArgs e) {
            await SendRecord("Light OFF");
        }

    }
}
