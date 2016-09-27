namespace HotelSimulatie.GUI
{
    partial class SettingsScreen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbCleaningEmergency = new System.Windows.Forms.TextBox();
            this.tbCleaning = new System.Windows.Forms.TextBox();
            this.tbDead = new System.Windows.Forms.TextBox();
            this.tbCinema = new System.Windows.Forms.TextBox();
            this.tbRestaurant = new System.Windows.Forms.TextBox();
            this.tbStairs = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buConfirm = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // tbCleaningEmergency
            // 
            this.tbCleaningEmergency.Location = new System.Drawing.Point(365, 331);
            this.tbCleaningEmergency.Name = "tbCleaningEmergency";
            this.tbCleaningEmergency.Size = new System.Drawing.Size(100, 20);
            this.tbCleaningEmergency.TabIndex = 26;
            // 
            // tbCleaning
            // 
            this.tbCleaning.Location = new System.Drawing.Point(365, 296);
            this.tbCleaning.Name = "tbCleaning";
            this.tbCleaning.Size = new System.Drawing.Size(100, 20);
            this.tbCleaning.TabIndex = 25;
            // 
            // tbDead
            // 
            this.tbDead.Location = new System.Drawing.Point(365, 254);
            this.tbDead.Name = "tbDead";
            this.tbDead.Size = new System.Drawing.Size(100, 20);
            this.tbDead.TabIndex = 24;
            // 
            // tbCinema
            // 
            this.tbCinema.Location = new System.Drawing.Point(365, 210);
            this.tbCinema.Name = "tbCinema";
            this.tbCinema.Size = new System.Drawing.Size(100, 20);
            this.tbCinema.TabIndex = 23;
            // 
            // tbRestaurant
            // 
            this.tbRestaurant.Location = new System.Drawing.Point(365, 169);
            this.tbRestaurant.Name = "tbRestaurant";
            this.tbRestaurant.Size = new System.Drawing.Size(100, 20);
            this.tbRestaurant.TabIndex = 22;
            // 
            // tbStairs
            // 
            this.tbStairs.Location = new System.Drawing.Point(365, 125);
            this.tbStairs.Name = "tbStairs";
            this.tbStairs.Size = new System.Drawing.Size(100, 20);
            this.tbStairs.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(142, 338);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Cleaning emergency";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(142, 303);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Schoonmaakbeurt";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(142, 261);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Dood";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 217);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Bioscoop duur";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 176);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Restaurant Etentje";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Trappen";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Hotel Tijdseenheid";
            // 
            // buConfirm
            // 
            this.buConfirm.Location = new System.Drawing.Point(365, 382);
            this.buConfirm.Name = "buConfirm";
            this.buConfirm.Size = new System.Drawing.Size(100, 37);
            this.buConfirm.TabIndex = 27;
            this.buConfirm.Text = "Confirm";
            this.buConfirm.UseVisualStyleBackColor = true;
            this.buConfirm.Click += new System.EventHandler(this.buConfirm_Click_1);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "2",
            "1",
            "0.5"});
            this.comboBox1.Location = new System.Drawing.Point(365, 53);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 28;
            // 
            // SettingsScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.buConfirm);
            this.Controls.Add(this.tbCleaningEmergency);
            this.Controls.Add(this.tbCleaning);
            this.Controls.Add(this.tbDead);
            this.Controls.Add(this.tbCinema);
            this.Controls.Add(this.tbRestaurant);
            this.Controls.Add(this.tbStairs);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "SettingsScreen";
            this.Size = new System.Drawing.Size(1598, 885);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCleaningEmergency;
        private System.Windows.Forms.TextBox tbCleaning;
        private System.Windows.Forms.TextBox tbDead;
        private System.Windows.Forms.TextBox tbCinema;
        private System.Windows.Forms.TextBox tbRestaurant;
        private System.Windows.Forms.TextBox tbStairs;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buConfirm;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
