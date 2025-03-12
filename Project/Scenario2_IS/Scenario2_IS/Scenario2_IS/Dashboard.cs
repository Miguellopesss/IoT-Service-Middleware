using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

//using System.Web.Script.Serialization;
using System.Windows.Forms;
using RestSharp;

namespace Scenario2_IS
{
    public partial class Dashboard : Form
    {
        string baseURI = @"http://localhost:61626/"; //TODO: needs to be updated!
        RestClient Client = null;
        public Dashboard()
        {
            InitializeComponent();
            Client = new RestClient(baseURI);
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            comboBoxType.DataSource = Enum.GetValues(typeof(Res_Type));
            comboBoxSomiodLocate.DataSource = Enum.GetValues(typeof(Res_Type));
            comboBoxEvent.DataSource = Enum.GetValues(typeof(Event_Type));
            comboBoxEnabled.DataSource = Enum.GetValues(typeof(Enabled_Type));
        }

        private void buttonGet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxAppName.Text) && string.IsNullOrWhiteSpace(textBoxContName.Text) &&
                string.IsNullOrWhiteSpace(textBoxRecordName.Text) && string.IsNullOrWhiteSpace(textBoxNotifName.Text))
            {
                RestRequest request = new RestRequest("api/somiod", Method.Get);
                request.AddHeader("Accept", "application/xml");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Applications retrieved!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && string.IsNullOrWhiteSpace(textBoxContName.Text) &&
                string.IsNullOrWhiteSpace(textBoxRecordName.Text) && string.IsNullOrWhiteSpace(textBoxNotifName.Text))
            {
                RestRequest request = new RestRequest("api/somiod/{appName}", Method.Get);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddHeader("Accept", "application/xml");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if (response.StatusCode != HttpStatusCode.OK)
                {   
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Application retrieved!");
                } 
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) &&
                string.IsNullOrWhiteSpace(textBoxRecordName.Text) && string.IsNullOrWhiteSpace(textBoxNotifName.Text))
            {
                RestRequest request = new RestRequest("api/somiod/{appName}/{contName}", Method.Get);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("contName", textBoxContName.Text);
                request.AddHeader("Accept", "application/xml");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Container retrieved!");
                }
                
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) &&
                !string.IsNullOrWhiteSpace(textBoxRecordName.Text) && string.IsNullOrWhiteSpace(textBoxNotifName.Text))
            {
                RestRequest request = new RestRequest("api/somiod/{appName}/{containerName}/record/{recordName}", Method.Get);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("containerName", textBoxContName.Text);
                request.AddUrlSegment("recordName", textBoxRecordName.Text);
                request.AddHeader("Accept", "application/xml");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Record retrieved!");
                }
                
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) &&
                string.IsNullOrWhiteSpace(textBoxRecordName.Text) && !string.IsNullOrWhiteSpace(textBoxNotifName.Text))
            {
                RestRequest request = new RestRequest("api/somiod/{appName}/{containerName}/notif/{notificationName}", Method.Get);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("containerName", textBoxContName.Text);
                request.AddUrlSegment("notificationName", textBoxNotifName.Text);
                request.AddHeader("Accept", "application/xml");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if(response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Notification retrieved!");
                }
                
            }
            else
            {
                MessageBox.Show("\t        To send Get Requests \n\n All Applications: *NONE* \n Application: AppName \n Container: AppName/ContName \n Record: AppName/ContName/RecordName \n Notification: AppName/ContName/NotifName");
            }
        }

        private void buttonGetSomiodLocate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxAppName.Text) && comboBoxSomiodLocate.Text == "Application")
            {
                RestRequest request = new RestRequest("api/somiod", Method.Get);
                request.AddHeader("Accept", "application/xml");
                request.AddHeader("somiod-locate", "application");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Applications retrieved!");
                }

            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && comboBoxSomiodLocate.Text == "Container")
            {
                RestRequest request = new RestRequest("api/somiod/{appName}", Method.Get);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddHeader("Accept", "application/xml");
                request.AddHeader("somiod-locate", "container");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Containers retrieved!");
                }
                
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && comboBoxSomiodLocate.Text == "Record")
            {
                RestRequest request = new RestRequest("api/somiod/{appName}", Method.Get);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddHeader("Accept", "application/xml");
                request.AddHeader("somiod-locate", "record");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Records retrieved!");
                }
                
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && comboBoxSomiodLocate.Text == "Notification")
            {
                RestRequest request = new RestRequest("api/somiod/{appName}", Method.Get);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddHeader("Accept", "application/xml");
                request.AddHeader("somiod-locate", "notification");

                var response = Client.Execute(request);
                richTextBoxOutput.Clear();
                richTextBoxOutput.Text = response.Content;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Notifications retrieved!");
                }
                
            }
            else
            {
                MessageBox.Show("\tTo send {somiod-locate} Get Requests \n\n Applications: *NONE*/somiod-locate: Application \n Containers: AppName/somiod-locate: Container \n Records: AppName/somiod-locate: Record \n Notifications: AppName/somiod-locate: Notification");
            }
        }

        private void buttonPost_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxName.Text) && comboBoxType.Text == "Application")
            {
                string name = textBoxName.Text;
                string xml = $"<Application><Name>{name}</Name></Application>";
                RestRequest request = new RestRequest("api/somiod/", Method.Post);
                request.AddHeader("Content-Type", "application/xml");
                request.AddBody(xml, "application/xml");

                var response = Client.Execute(request);
                
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Application created!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxName.Text) 
                && comboBoxType.Text == "Container")
            {
                string name = textBoxName.Text;
                string xml = $"<Container><Name>{name}</Name></Container>";
                RestRequest request = new RestRequest("api/somiod/{appName}", Method.Post);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddHeader("Content-Type", "application/xml");
                request.AddBody(xml, "application/xml");
                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Container created!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) 
                && !string.IsNullOrWhiteSpace(textBoxName.Text) && !string.IsNullOrWhiteSpace(textBoxContent.Text) 
                && comboBoxType.Text == "Record")
            {
                string name = textBoxName.Text;
                string content = textBoxContent.Text;
                string xml = $"<Record><Name>{name}</Name><Content>{content}</Content></Record>";
                RestRequest request = new RestRequest("api/somiod/{appName}/{contName}/record", Method.Post);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("contName", textBoxContName.Text);
                request.AddHeader("Content-Type", "application/xml");
                request.AddBody(xml, "application/xml");
                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Record created!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) 
                && !string.IsNullOrWhiteSpace(textBoxName.Text) && !string.IsNullOrWhiteSpace(textBoxEndpoint.Text) && comboBoxType.Text == "Notification")
            {
                string name = textBoxName.Text;
                string endpoint = textBoxEndpoint.Text;
             
                string eventValue = "1";
                if (comboBoxEvent.Text == "One")
                {
                    eventValue = "1";
                }
                else
                {
                    eventValue = "2";
                }
                string enabledValue = "true";
                if (comboBoxEnabled.Text == "False")
                {
                    enabledValue = "false";
                }
                else
                {
                    enabledValue = "true";
                }

                string xml = $"<Notification><Name>{name}</Name><Event>{eventValue}</Event><Endpoint>{endpoint}</Endpoint><Enabled>{enabledValue}</Enabled></Notification>";
                RestRequest request = new RestRequest("api/somiod/{appName}/{contName}/notif", Method.Post);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("contName", textBoxContName.Text);
                request.AddHeader("Content-Type", "application/xml");
                request.AddBody(xml, "application/xml");
                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Notification created!");
                }
            }
            else
            {
                MessageBox.Show("\t\t\tTo send Post Requests \n\n Applications: Name/Type: Application \n Containers: AppName/Name/Type: Container \n Records: AppName/ContName/Name/Content/Type: Record \n Notifications: \nAppName/ContName/Name/Event/Endpoint/Enabled/Type: Notification \n");
            }
            
        }

        private void buttonPut_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxName.Text) && comboBoxType.Text == "Application")
            {
                string name = textBoxName.Text;
                string xml = $"<Application><Name>{name}</Name></Application>";
                RestRequest request = new RestRequest("api/somiod/{appName}", Method.Put);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddHeader("Content-Type", "application/xml");
                request.AddBody(xml, "application/xml");

                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Application updated!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) &&
                !string.IsNullOrWhiteSpace(textBoxName.Text) && (comboBoxType.Text == "Container"))
            {
                string name = textBoxName.Text;
                string xml = $"<Container><Name>{name}</Name></Container>";
                RestRequest request = new RestRequest("api/somiod/{appName}/{contName}", Method.Put);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("contName", textBoxContName.Text);
                request.AddHeader("Content-Type", "application/xml");
                request.AddBody(xml, "application/xml");

                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Container updated!");
                }
            }
            else if (comboBoxType.Text == "Record" || comboBoxType.Text == "Notification")
            {
                MessageBox.Show("This action is not allowed!");
            }
            else
            {
                MessageBox.Show("\t\tTo send Put Requests \n\n Applications: AppName/Name/Type: Application \n Containers: AppName/ContName/Name/Type: Container \n");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && comboBoxType.Text == "Application")
            {
                RestRequest request = new RestRequest("api/somiod/{appName}", Method.Delete);
                request.AddUrlSegment("appName", textBoxAppName.Text);

                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Application Deleted!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) && (comboBoxType.Text == "Container"))
            {
                RestRequest request = new RestRequest("api/somiod/{appName}/{contName}", Method.Delete);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("contName", textBoxContName.Text);

                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Container Deleted!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) && comboBoxType.Text == "Record")
            {
                RestRequest request = new RestRequest("api/somiod/{appName}/{contName}/record/{recordName}", Method.Delete);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("contName", textBoxContName.Text);
                request.AddUrlSegment("recordName", textBoxRecordName.Text);

                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Record Deleted!");
                }
            }
            else if (!string.IsNullOrWhiteSpace(textBoxAppName.Text) && !string.IsNullOrWhiteSpace(textBoxContName.Text) && comboBoxType.Text == "Notification")
            {
                RestRequest request = new RestRequest("api/somiod/{appName}/{contName}/notification/{notifName}", Method.Delete);
                request.AddUrlSegment("appName", textBoxAppName.Text);
                request.AddUrlSegment("contName", textBoxContName.Text);
                request.AddUrlSegment("notifName", textBoxNotifName.Text);

                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    MessageBox.Show("ERRO\n" + response.ErrorException);
                }
                else
                {
                    MessageBox.Show("Notification Deleted!");
                }
            }
            else
            {
                MessageBox.Show("\t\tTo send Delete Requests \n\n Applications: AppName/Type: Application \n Containers: AppName/ContName/Type: Container \n Records: AppName/ContName/RecordName/Type: Record \n Notifications: AppName/ContName/NotifName/Type: Notification \n");
            }
        }
    }
}
