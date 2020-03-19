namespace Services_Ej1_Cliente
{
    partial class IPChange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IPChange));
            this.txbIP = new System.Windows.Forms.TextBox();
            this.btnConnection = new System.Windows.Forms.Button();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.lblIPChange = new System.Windows.Forms.Label();
            this.lblPortChange = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txbIP
            // 
            this.txbIP.Location = new System.Drawing.Point(12, 12);
            this.txbIP.Name = "txbIP";
            this.txbIP.Size = new System.Drawing.Size(100, 22);
            this.txbIP.TabIndex = 0;
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(125, 85);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(126, 23);
            this.btnConnection.TabIndex = 2;
            this.btnConnection.Text = "Set Connection";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnIP_Click);
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(12, 48);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(100, 22);
            this.txbPort.TabIndex = 3;
            // 
            // lblIPChange
            // 
            this.lblIPChange.AutoSize = true;
            this.lblIPChange.ForeColor = System.Drawing.Color.Red;
            this.lblIPChange.Location = new System.Drawing.Point(125, 13);
            this.lblIPChange.Name = "lblIPChange";
            this.lblIPChange.Size = new System.Drawing.Size(48, 17);
            this.lblIPChange.TabIndex = 4;
            this.lblIPChange.Text = "Invalid";
            this.lblIPChange.Visible = false;
            // 
            // lblPortChange
            // 
            this.lblPortChange.AutoSize = true;
            this.lblPortChange.ForeColor = System.Drawing.Color.Red;
            this.lblPortChange.Location = new System.Drawing.Point(125, 51);
            this.lblPortChange.Name = "lblPortChange";
            this.lblPortChange.Size = new System.Drawing.Size(48, 17);
            this.lblPortChange.TabIndex = 5;
            this.lblPortChange.Text = "Invalid";
            this.lblPortChange.Visible = false;
            // 
            // IPChange
            // 
            this.AcceptButton = this.btnConnection;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 120);
            this.Controls.Add(this.lblPortChange);
            this.Controls.Add(this.lblIPChange);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.btnConnection);
            this.Controls.Add(this.txbIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IPChange";
            this.Text = "IPChange";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txbIP;
        private System.Windows.Forms.Button btnConnection;
        private System.Windows.Forms.TextBox txbPort;
        private System.Windows.Forms.Label lblIPChange;
        private System.Windows.Forms.Label lblPortChange;
    }
}