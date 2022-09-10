namespace VDJ_Controller
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.connect = new Guna.UI.WinForms.GunaButton();
            this.bridgeActive = new Guna.UI.WinForms.GunaSwitch();
            this.gunaLabel1 = new Guna.UI.WinForms.GunaLabel();
            this.control = new Guna.UI.WinForms.GunaLabel();
            this.portSelect = new Guna.UI.WinForms.GunaComboBox();
            this.port_text = new Guna.UI.WinForms.GunaLabel();
            this.statusView = new Guna.UI.WinForms.GunaLabel();
            this.SuspendLayout();
            // 
            // connect
            // 
            this.connect.AnimationHoverSpeed = 0.07F;
            this.connect.AnimationSpeed = 0.03F;
            this.connect.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.connect.BorderColor = System.Drawing.Color.Black;
            this.connect.DialogResult = System.Windows.Forms.DialogResult.None;
            this.connect.FocusedColor = System.Drawing.Color.Empty;
            this.connect.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.connect.ForeColor = System.Drawing.Color.White;
            this.connect.Image = null;
            this.connect.ImageSize = new System.Drawing.Size(20, 20);
            this.connect.Location = new System.Drawing.Point(55, 140);
            this.connect.Name = "connect";
            this.connect.OnHoverBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.connect.OnHoverBorderColor = System.Drawing.Color.Black;
            this.connect.OnHoverForeColor = System.Drawing.Color.White;
            this.connect.OnHoverImage = null;
            this.connect.OnPressedColor = System.Drawing.Color.Black;
            this.connect.Size = new System.Drawing.Size(109, 46);
            this.connect.TabIndex = 0;
            this.connect.Text = "CONNECT";
            this.connect.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.connect.Click += new System.EventHandler(this.connect_Click);
            // 
            // bridgeActive
            // 
            this.bridgeActive.BaseColor = System.Drawing.SystemColors.Control;
            this.bridgeActive.CheckedOffColor = System.Drawing.Color.DarkGray;
            this.bridgeActive.CheckedOnColor = System.Drawing.Color.OrangeRed;
            this.bridgeActive.FillColor = System.Drawing.Color.White;
            this.bridgeActive.Location = new System.Drawing.Point(66, 58);
            this.bridgeActive.Name = "bridgeActive";
            this.bridgeActive.Size = new System.Drawing.Size(98, 20);
            this.bridgeActive.TabIndex = 2;
            this.bridgeActive.CheckedChanged += new System.EventHandler(this.bridgeActive_CheckedChanged);
            // 
            // gunaLabel1
            // 
            this.gunaLabel1.AutoSize = true;
            this.gunaLabel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.gunaLabel1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.gunaLabel1.Location = new System.Drawing.Point(104, 61);
            this.gunaLabel1.Name = "gunaLabel1";
            this.gunaLabel1.Size = new System.Drawing.Size(60, 15);
            this.gunaLabel1.TabIndex = 3;
            this.gunaLabel1.Text = "Bridge On";
            // 
            // control
            // 
            this.control.AutoSize = true;
            this.control.BackColor = System.Drawing.Color.Black;
            this.control.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.control.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.control.Location = new System.Drawing.Point(400, 61);
            this.control.Name = "control";
            this.control.Size = new System.Drawing.Size(82, 21);
            this.control.TabIndex = 4;
            this.control.Text = "CONTROL";
            // 
            // portSelect
            // 
            this.portSelect.BackColor = System.Drawing.Color.Transparent;
            this.portSelect.BaseColor = System.Drawing.Color.White;
            this.portSelect.BorderColor = System.Drawing.Color.Silver;
            this.portSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.portSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.portSelect.FocusedColor = System.Drawing.Color.Empty;
            this.portSelect.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.portSelect.ForeColor = System.Drawing.Color.Black;
            this.portSelect.FormattingEnabled = true;
            this.portSelect.Location = new System.Drawing.Point(222, 160);
            this.portSelect.Name = "portSelect";
            this.portSelect.OnHoverItemBaseColor = System.Drawing.Color.OrangeRed;
            this.portSelect.OnHoverItemForeColor = System.Drawing.Color.White;
            this.portSelect.Size = new System.Drawing.Size(121, 26);
            this.portSelect.TabIndex = 5;
            this.portSelect.SelectedIndexChanged += new System.EventHandler(this.portSelect_SelectedIndexChanged);
            this.portSelect.Click += new System.EventHandler(this.portSelect_Click);
            // 
            // port_text
            // 
            this.port_text.AutoSize = true;
            this.port_text.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.port_text.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.port_text.Location = new System.Drawing.Point(219, 137);
            this.port_text.Name = "port_text";
            this.port_text.Size = new System.Drawing.Size(29, 15);
            this.port_text.TabIndex = 6;
            this.port_text.Text = "Port";
            // 
            // statusView
            // 
            this.statusView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusView.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.statusView.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusView.Location = new System.Drawing.Point(12, 9);
            this.statusView.Name = "statusView";
            this.statusView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusView.Size = new System.Drawing.Size(520, 32);
            this.statusView.TabIndex = 7;
            this.statusView.Text = "Initializing...";
            this.statusView.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(544, 239);
            this.Controls.Add(this.statusView);
            this.Controls.Add(this.port_text);
            this.Controls.Add(this.portSelect);
            this.Controls.Add(this.control);
            this.Controls.Add(this.gunaLabel1);
            this.Controls.Add(this.bridgeActive);
            this.Controls.Add(this.connect);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "VDJ Controller Bridge";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI.WinForms.GunaButton connect;
        private Guna.UI.WinForms.GunaSwitch bridgeActive;
        private Guna.UI.WinForms.GunaLabel gunaLabel1;
        private Guna.UI.WinForms.GunaLabel control;
        private Guna.UI.WinForms.GunaComboBox portSelect;
        private Guna.UI.WinForms.GunaLabel port_text;
        private Guna.UI.WinForms.GunaLabel statusView;
    }
}