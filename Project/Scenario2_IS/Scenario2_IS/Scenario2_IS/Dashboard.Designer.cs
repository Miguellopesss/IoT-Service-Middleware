namespace Scenario2_IS
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonPut = new System.Windows.Forms.Button();
            this.buttonPost = new System.Windows.Forms.Button();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.buttonGet = new System.Windows.Forms.Button();
            this.textBoxRecordName = new System.Windows.Forms.TextBox();
            this.textBoxNotifName = new System.Windows.Forms.TextBox();
            this.labelContent = new System.Windows.Forms.Label();
            this.textBoxContent = new System.Windows.Forms.TextBox();
            this.labelEndpoint = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxContName = new System.Windows.Forms.TextBox();
            this.textBoxAppName = new System.Windows.Forms.TextBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxSomiodLocate = new System.Windows.Forms.ComboBox();
            this.buttonGetSomiodLocate = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxEndpoint = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxEvent = new System.Windows.Forms.ComboBox();
            this.comboBoxEnabled = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(568, 219);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(90, 23);
            this.buttonDelete.TabIndex = 31;
            this.buttonDelete.Text = "DEL (Delete)";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonPut
            // 
            this.buttonPut.Location = new System.Drawing.Point(568, 190);
            this.buttonPut.Name = "buttonPut";
            this.buttonPut.Size = new System.Drawing.Size(90, 23);
            this.buttonPut.TabIndex = 30;
            this.buttonPut.Text = "PUT (Update)";
            this.buttonPut.UseVisualStyleBackColor = true;
            this.buttonPut.Click += new System.EventHandler(this.buttonPut_Click);
            // 
            // buttonPost
            // 
            this.buttonPost.Location = new System.Drawing.Point(568, 161);
            this.buttonPost.Name = "buttonPost";
            this.buttonPost.Size = new System.Drawing.Size(90, 23);
            this.buttonPost.TabIndex = 29;
            this.buttonPost.Text = "POST (Create)";
            this.buttonPost.UseVisualStyleBackColor = true;
            this.buttonPost.Click += new System.EventHandler(this.buttonPost_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(487, 9);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 13);
            this.labelName.TabIndex = 26;
            this.labelName.Text = "Name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(548, 6);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(162, 20);
            this.textBoxName.TabIndex = 22;
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Location = new System.Drawing.Point(15, 91);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.Size = new System.Drawing.Size(454, 164);
            this.richTextBoxOutput.TabIndex = 17;
            this.richTextBoxOutput.Text = "";
            // 
            // buttonGet
            // 
            this.buttonGet.Location = new System.Drawing.Point(242, 58);
            this.buttonGet.Name = "buttonGet";
            this.buttonGet.Size = new System.Drawing.Size(90, 23);
            this.buttonGet.TabIndex = 35;
            this.buttonGet.Text = "GET";
            this.buttonGet.UseVisualStyleBackColor = true;
            this.buttonGet.Click += new System.EventHandler(this.buttonGet_Click);
            // 
            // textBoxRecordName
            // 
            this.textBoxRecordName.Location = new System.Drawing.Point(349, 6);
            this.textBoxRecordName.Name = "textBoxRecordName";
            this.textBoxRecordName.Size = new System.Drawing.Size(120, 20);
            this.textBoxRecordName.TabIndex = 50;
            // 
            // textBoxNotifName
            // 
            this.textBoxNotifName.Location = new System.Drawing.Point(371, 32);
            this.textBoxNotifName.Name = "textBoxNotifName";
            this.textBoxNotifName.Size = new System.Drawing.Size(98, 20);
            this.textBoxNotifName.TabIndex = 62;
            // 
            // labelContent
            // 
            this.labelContent.AutoSize = true;
            this.labelContent.Location = new System.Drawing.Point(487, 35);
            this.labelContent.Name = "labelContent";
            this.labelContent.Size = new System.Drawing.Size(47, 13);
            this.labelContent.TabIndex = 65;
            this.labelContent.Text = "Content:";
            // 
            // textBoxContent
            // 
            this.textBoxContent.Location = new System.Drawing.Point(548, 32);
            this.textBoxContent.Name = "textBoxContent";
            this.textBoxContent.Size = new System.Drawing.Size(162, 20);
            this.textBoxContent.TabIndex = 64;
            // 
            // labelEndpoint
            // 
            this.labelEndpoint.AutoSize = true;
            this.labelEndpoint.Location = new System.Drawing.Point(487, 87);
            this.labelEndpoint.Name = "labelEndpoint";
            this.labelEndpoint.Size = new System.Drawing.Size(49, 13);
            this.labelEndpoint.TabIndex = 67;
            this.labelEndpoint.Text = "Enabled:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 79;
            this.label4.Text = "Container name:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 78;
            this.label5.Text = "Application name:";
            // 
            // textBoxContName
            // 
            this.textBoxContName.Location = new System.Drawing.Point(102, 32);
            this.textBoxContName.Name = "textBoxContName";
            this.textBoxContName.Size = new System.Drawing.Size(162, 20);
            this.textBoxContName.TabIndex = 77;
            // 
            // textBoxAppName
            // 
            this.textBoxAppName.Location = new System.Drawing.Point(109, 6);
            this.textBoxAppName.Name = "textBoxAppName";
            this.textBoxAppName.Size = new System.Drawing.Size(155, 20);
            this.textBoxAppName.TabIndex = 76;
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Location = new System.Drawing.Point(548, 111);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(121, 21);
            this.comboBoxType.TabIndex = 80;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(273, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 81;
            this.label1.Text = "Record name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(273, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 82;
            this.label2.Text = "Notification name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 13);
            this.label3.TabIndex = 83;
            this.label3.Text = "Somiod-locate:";
            // 
            // comboBoxSomiodLocate
            // 
            this.comboBoxSomiodLocate.FormattingEnabled = true;
            this.comboBoxSomiodLocate.Location = new System.Drawing.Point(95, 58);
            this.comboBoxSomiodLocate.Name = "comboBoxSomiodLocate";
            this.comboBoxSomiodLocate.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSomiodLocate.TabIndex = 84;
            // 
            // buttonGetSomiodLocate
            // 
            this.buttonGetSomiodLocate.Location = new System.Drawing.Point(349, 58);
            this.buttonGetSomiodLocate.Name = "buttonGetSomiodLocate";
            this.buttonGetSomiodLocate.Size = new System.Drawing.Size(120, 23);
            this.buttonGetSomiodLocate.TabIndex = 85;
            this.buttonGetSomiodLocate.Text = "GET {Somiod-locate}";
            this.buttonGetSomiodLocate.UseVisualStyleBackColor = true;
            this.buttonGetSomiodLocate.Click += new System.EventHandler(this.buttonGetSomiodLocate_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(487, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 86;
            this.label6.Text = "Type:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(487, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 88;
            this.label7.Text = "Endpoint:";
            // 
            // textBoxEndpoint
            // 
            this.textBoxEndpoint.Location = new System.Drawing.Point(548, 58);
            this.textBoxEndpoint.Name = "textBoxEndpoint";
            this.textBoxEndpoint.Size = new System.Drawing.Size(162, 20);
            this.textBoxEndpoint.TabIndex = 87;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(611, 87);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 90;
            this.label8.Text = "Event:";
            // 
            // comboBoxEvent
            // 
            this.comboBoxEvent.FormattingEnabled = true;
            this.comboBoxEvent.Location = new System.Drawing.Point(655, 84);
            this.comboBoxEvent.Name = "comboBoxEvent";
            this.comboBoxEvent.Size = new System.Drawing.Size(55, 21);
            this.comboBoxEvent.TabIndex = 91;
            // 
            // comboBoxEnabled
            // 
            this.comboBoxEnabled.FormattingEnabled = true;
            this.comboBoxEnabled.Location = new System.Drawing.Point(548, 84);
            this.comboBoxEnabled.Name = "comboBoxEnabled";
            this.comboBoxEnabled.Size = new System.Drawing.Size(55, 21);
            this.comboBoxEnabled.TabIndex = 92;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 273);
            this.Controls.Add(this.comboBoxEnabled);
            this.Controls.Add(this.comboBoxEvent);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBoxEndpoint);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonGetSomiodLocate);
            this.Controls.Add(this.comboBoxSomiodLocate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxContName);
            this.Controls.Add(this.textBoxAppName);
            this.Controls.Add(this.labelEndpoint);
            this.Controls.Add(this.labelContent);
            this.Controls.Add(this.textBoxContent);
            this.Controls.Add(this.textBoxNotifName);
            this.Controls.Add(this.textBoxRecordName);
            this.Controls.Add(this.buttonGet);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonPut);
            this.Controls.Add(this.buttonPost);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.richTextBoxOutput);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonPut;
        private System.Windows.Forms.Button buttonPost;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.Button buttonGet;
        private System.Windows.Forms.TextBox textBoxRecordName;
        private System.Windows.Forms.TextBox textBoxNotifName;
        private System.Windows.Forms.Label labelContent;
        private System.Windows.Forms.TextBox textBoxContent;
        private System.Windows.Forms.Label labelEndpoint;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxContName;
        private System.Windows.Forms.TextBox textBoxAppName;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxSomiodLocate;
        private System.Windows.Forms.Button buttonGetSomiodLocate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxEndpoint;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxEvent;
        private System.Windows.Forms.ComboBox comboBoxEnabled;
    }
}

