namespace AES
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
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.lblKey = new System.Windows.Forms.Label();
            this.lblPlain = new System.Windows.Forms.Label();
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCiphertext = new System.Windows.Forms.TextBox();
            this.txtKeyResult = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStartRoundResult = new System.Windows.Forms.TextBox();
            this.txtSubBytesResult = new System.Windows.Forms.TextBox();
            this.txtShiftRowsResult = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtMixColumnsResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(22, 106);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(149, 58);
            this.txtMessage.TabIndex = 1;
            this.txtMessage.Text = "GelinaGashi";
            // 
            // txtKey
            // 
            this.txtKey.Location = new System.Drawing.Point(22, 47);
            this.txtKey.Name = "txtKey";
            this.txtKey.Size = new System.Drawing.Size(149, 20);
            this.txtKey.TabIndex = 0;
            this.txtKey.Text = "Gelina";
            // 
            // lblKey
            // 
            this.lblKey.AutoSize = true;
            this.lblKey.Location = new System.Drawing.Point(19, 20);
            this.lblKey.Name = "lblKey";
            this.lblKey.Size = new System.Drawing.Size(28, 13);
            this.lblKey.TabIndex = 2;
            this.lblKey.Text = "Key:";
            this.lblKey.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblPlain
            // 
