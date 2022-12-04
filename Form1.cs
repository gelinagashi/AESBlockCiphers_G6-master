using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
 private void label1_Click(object sender, EventArgs e)
        {

        }
       
     private void btnEncrypt_Click(object sender, EventArgs e)
        {
           clearTextResults();
        if (txtKey.Text.Trim() == "" || txtMessage.Text.Trim() == "")
            {
                MessageBox.Show("Do not leave any field blank");
                return;
            }
       List<byte[,]> keys = generateKeys();
       List<char[]> roundMessage = splitMessage(txtMessage.Text.Trim());
       String encrypted = "";
       
            foreach (char[] chunk in roundMessage)
            {
                encrypted += encrypt(keys, chunk);
            }
            
        txtCiphertext.Text = encrypted;
            
        }
          private string encrypt(List<byte[,]> keys, char[] roundMessage)
        {
