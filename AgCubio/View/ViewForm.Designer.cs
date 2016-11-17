namespace AgCubio
{
    partial class ViewForm
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
            this.serverLabel = new System.Windows.Forms.Label();
            this.nameLabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.fcountfront = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.isDeadPanel = new System.Windows.Forms.Panel();
            this.foodEatenLabel = new System.Windows.Forms.Label();
            this.foodeatenfront = new System.Windows.Forms.Label();
            this.reconnectButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.serverBox = new System.Windows.Forms.TextBox();
            this.errorLabel = new System.Windows.Forms.Label();
            this.fCountLabel = new System.Windows.Forms.Label();
            this.massLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.isDeadPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverLabel.Location = new System.Drawing.Point(345, 374);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(70, 24);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Server:";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(292, 308);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(123, 24);
            this.nameLabel.TabIndex = 1;
            this.nameLabel.Text = "Player Name:";
            // 
            // connectButton
            // 
            this.connectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connectButton.Location = new System.Drawing.Point(447, 420);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(106, 34);
            this.connectButton.TabIndex = 4;
            this.connectButton.TabStop = false;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // fcountfront
            // 
            this.fcountfront.AutoSize = true;
            this.fcountfront.Location = new System.Drawing.Point(835, 36);
            this.fcountfront.Name = "fcountfront";
            this.fcountfront.Size = new System.Drawing.Size(65, 13);
            this.fcountfront.TabIndex = 7;
            this.fcountfront.Text = "Food Count:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(868, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Mass";
            // 
            // isDeadPanel
            // 
            this.isDeadPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.isDeadPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.isDeadPanel.Controls.Add(this.foodEatenLabel);
            this.isDeadPanel.Controls.Add(this.foodeatenfront);
            this.isDeadPanel.Controls.Add(this.reconnectButton);
            this.isDeadPanel.Controls.Add(this.closeButton);
            this.isDeadPanel.Location = new System.Drawing.Point(0, 0);
            this.isDeadPanel.Name = "isDeadPanel";
            this.isDeadPanel.Size = new System.Drawing.Size(65, 74);
            this.isDeadPanel.TabIndex = 12;
            this.isDeadPanel.Visible = false;
            // 
            // foodEatenLabel
            // 
            this.foodEatenLabel.AutoSize = true;
            this.foodEatenLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.foodEatenLabel.Location = new System.Drawing.Point(735, 772);
            this.foodEatenLabel.Name = "foodEatenLabel";
            this.foodEatenLabel.Size = new System.Drawing.Size(0, 24);
            this.foodEatenLabel.TabIndex = 4;
            // 
            // foodeatenfront
            // 
            this.foodeatenfront.AutoSize = true;
            this.foodeatenfront.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.foodeatenfront.Location = new System.Drawing.Point(609, 772);
            this.foodeatenfront.Name = "foodeatenfront";
            this.foodeatenfront.Size = new System.Drawing.Size(112, 24);
            this.foodeatenfront.TabIndex = 3;
            this.foodeatenfront.Text = "Food eaten:";
            // 
            // reconnectButton
            // 
            this.reconnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reconnectButton.Location = new System.Drawing.Point(417, 772);
            this.reconnectButton.Name = "reconnectButton";
            this.reconnectButton.Size = new System.Drawing.Size(153, 53);
            this.reconnectButton.TabIndex = 2;
            this.reconnectButton.Text = "Reconnect";
            this.reconnectButton.UseVisualStyleBackColor = true;
            this.reconnectButton.Click += new System.EventHandler(this.reconnectButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeButton.Location = new System.Drawing.Point(417, 831);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(153, 48);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // nameBox
            // 
            this.nameBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameBox.Location = new System.Drawing.Point(447, 308);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(106, 22);
            this.nameBox.TabIndex = 13;
            this.nameBox.Text = "Player 1";
            // 
            // serverBox
            // 
            this.serverBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serverBox.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.serverBox.Location = new System.Drawing.Point(447, 374);
            this.serverBox.Name = "serverBox";
            this.serverBox.Size = new System.Drawing.Size(106, 22);
            this.serverBox.TabIndex = 14;
            this.serverBox.Text = "localhost";
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Location = new System.Drawing.Point(444, 490);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(0, 13);
            this.errorLabel.TabIndex = 15;
            // 
            // fCountLabel
            // 
            this.fCountLabel.AutoSize = true;
            this.fCountLabel.Location = new System.Drawing.Point(911, 36);
            this.fCountLabel.Name = "fCountLabel";
            this.fCountLabel.Size = new System.Drawing.Size(0, 13);
            this.fCountLabel.TabIndex = 16;
            // 
            // massLabel
            // 
            this.massLabel.AutoSize = true;
            this.massLabel.Location = new System.Drawing.Point(906, 61);
            this.massLabel.Name = "massLabel";
            this.massLabel.Size = new System.Drawing.Size(0, 13);
            this.massLabel.TabIndex = 17;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.titleLabel.Location = new System.Drawing.Point(410, 201);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(162, 42);
            this.titleLabel.TabIndex = 18;
            this.titleLabel.Text = "AgCubio";
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 962);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.massLabel);
            this.Controls.Add(this.fCountLabel);
            this.Controls.Add(this.errorLabel);
            this.Controls.Add(this.serverBox);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.isDeadPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.fcountfront);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.serverLabel);
            this.Name = "ViewForm";
            this.Text = "AgCubio";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ViewForm_Paint);
            this.isDeadPanel.ResumeLayout(false);
            this.isDeadPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label fcountfront;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel isDeadPanel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.TextBox serverBox;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.Label fCountLabel;
        private System.Windows.Forms.Button reconnectButton;
        private System.Windows.Forms.Label massLabel;
        private System.Windows.Forms.Label foodeatenfront;
        private System.Windows.Forms.Label foodEatenLabel;
        private System.Windows.Forms.Label titleLabel;
    }
}

