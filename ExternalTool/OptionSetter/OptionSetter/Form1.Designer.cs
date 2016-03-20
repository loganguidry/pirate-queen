namespace OptionSetter
{
    partial class Form1
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblWalkingSpeed = new System.Windows.Forms.Label();
            this.sliderWalkingSpeed = new System.Windows.Forms.TrackBar();
            this.lblWalkingSpeedData = new System.Windows.Forms.Label();
            this.lblRunningSpeedData = new System.Windows.Forms.Label();
            this.sliderRunningSpeed = new System.Windows.Forms.TrackBar();
            this.lblRunningSpeed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.sliderWalkingSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderRunningSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Monotype Corsiva", 19.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(95, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Pirate Queen";
            // 
            // lblWalkingSpeed
            // 
            this.lblWalkingSpeed.AutoSize = true;
            this.lblWalkingSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWalkingSpeed.Location = new System.Drawing.Point(21, 57);
            this.lblWalkingSpeed.Name = "lblWalkingSpeed";
            this.lblWalkingSpeed.Size = new System.Drawing.Size(138, 24);
            this.lblWalkingSpeed.TabIndex = 1;
            this.lblWalkingSpeed.Text = "Walking Speed";
            // 
            // sliderWalkingSpeed
            // 
            this.sliderWalkingSpeed.Location = new System.Drawing.Point(165, 57);
            this.sliderWalkingSpeed.Maximum = 14;
            this.sliderWalkingSpeed.Name = "sliderWalkingSpeed";
            this.sliderWalkingSpeed.Size = new System.Drawing.Size(195, 56);
            this.sliderWalkingSpeed.TabIndex = 2;
            this.sliderWalkingSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.sliderWalkingSpeed.Value = 2;
            this.sliderWalkingSpeed.ValueChanged += new System.EventHandler(this.sliderSpeed_ValueChanged);
            // 
            // lblWalkingSpeedData
            // 
            this.lblWalkingSpeedData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWalkingSpeedData.AutoSize = true;
            this.lblWalkingSpeedData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWalkingSpeedData.Location = new System.Drawing.Point(125, 81);
            this.lblWalkingSpeedData.Name = "lblWalkingSpeedData";
            this.lblWalkingSpeedData.Size = new System.Drawing.Size(18, 20);
            this.lblWalkingSpeedData.TabIndex = 3;
            this.lblWalkingSpeedData.Text = "3";
            this.lblWalkingSpeedData.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblRunningSpeedData
            // 
            this.lblRunningSpeedData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRunningSpeedData.AutoSize = true;
            this.lblRunningSpeedData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunningSpeedData.Location = new System.Drawing.Point(125, 143);
            this.lblRunningSpeedData.Name = "lblRunningSpeedData";
            this.lblRunningSpeedData.Size = new System.Drawing.Size(18, 20);
            this.lblRunningSpeedData.TabIndex = 6;
            this.lblRunningSpeedData.Text = "5";
            this.lblRunningSpeedData.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // sliderRunningSpeed
            // 
            this.sliderRunningSpeed.Location = new System.Drawing.Point(165, 119);
            this.sliderRunningSpeed.Maximum = 14;
            this.sliderRunningSpeed.Name = "sliderRunningSpeed";
            this.sliderRunningSpeed.Size = new System.Drawing.Size(195, 56);
            this.sliderRunningSpeed.TabIndex = 5;
            this.sliderRunningSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.sliderRunningSpeed.Value = 4;
            this.sliderRunningSpeed.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // lblRunningSpeed
            // 
            this.lblRunningSpeed.AutoSize = true;
            this.lblRunningSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRunningSpeed.Location = new System.Drawing.Point(16, 119);
            this.lblRunningSpeed.Name = "lblRunningSpeed";
            this.lblRunningSpeed.Size = new System.Drawing.Size(143, 24);
            this.lblRunningSpeed.TabIndex = 4;
            this.lblRunningSpeed.Text = "Running Speed";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 215);
            this.Controls.Add(this.lblRunningSpeedData);
            this.Controls.Add(this.sliderRunningSpeed);
            this.Controls.Add(this.lblRunningSpeed);
            this.Controls.Add(this.lblWalkingSpeedData);
            this.Controls.Add(this.sliderWalkingSpeed);
            this.Controls.Add(this.lblWalkingSpeed);
            this.Controls.Add(this.lblTitle);
            this.Name = "Form1";
            this.Text = "Pirate Queen Settings";
            ((System.ComponentModel.ISupportInitialize)(this.sliderWalkingSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sliderRunningSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWalkingSpeed;
        private System.Windows.Forms.TrackBar sliderWalkingSpeed;
        private System.Windows.Forms.Label lblWalkingSpeedData;
        private System.Windows.Forms.Label lblRunningSpeedData;
        private System.Windows.Forms.TrackBar sliderRunningSpeed;
        private System.Windows.Forms.Label lblRunningSpeed;
    }
}

