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
        
            //convert message to matrix of bytes
            int[,] initialMatrix = new int[4, 4];

            int xPos = 0, yPos = -1; 
            for (int i = 0; i < 16; i++)
            {
            if (i % 4 == 0)
                {
                    yPos++;
                    xPos = 0;

                }
                initialMatrix[xPos, yPos] = (byte) roundMessage[i];

                xPos++;
            }
             /*
            initialMatrix = new int[4, 4] { { 0x32, 0x88, 0x31, 0xe0},
                                            { 0x43, 0x5a, 0x31, 0x37},
                                            { 0xf6, 0x30, 0x98, 0x07},
                                            { 0xa8, 0x8d, 0xa2, 0x34}};
                                                                          */
            //Print first round of matrix
            printMatrix("Input:", initialMatrix, 0, false);
            
            //fill other section matrix with blank lines
            printMatrix("Input:", initialMatrix, 1, true);
            printMatrix("Input:", initialMatrix, 2, true);
            printMatrix("Input:", initialMatrix, 3, true);
            
            //first round key
            int[,] finalMatrix = addRoundKey(keys[0], initialMatrix);
            
             int[,] subBytedMatrix, shiftedMatrix, mixedColumns;
             int[,] roundedMatrix = new int[4, 4];
       
            for (int i = 0; i < 9; i++)
            {
                printMatrix("Round: " + (i + 1) + " ", finalMatrix, 0, false);
                subBytedMatrix = getSubBytes(finalMatrix);

                printMatrix("Round: " + (i + 1) + " ", subBytedMatrix, 1, false);
                shiftedMatrix = getShiftRows(subBytedMatrix);

                printMatrix("Round: " + (i + 1) + " ", shiftedMatrix, 2, false);
                mixedColumns = getMixedColumns(shiftedMatrix);
                
                printMatrix("Round: " + (i + 1) + " ", mixedColumns, 3, false);
                roundedMatrix = addRoundKey(keys[i+1], mixedColumns);

                finalMatrix = roundedMatrix;
                
                int a = 0;
            }
            
            printMatrix("Round: 10 ", finalMatrix, 0, false);
            subBytedMatrix = getSubBytes(finalMatrix);


            printMatrix("Round: 10 ", subBytedMatrix, 1, false);
            shiftedMatrix = getShiftRows(subBytedMatrix);
           
            printMatrix("Round: 10 ", shiftedMatrix, 2, false);
            roundedMatrix = addRoundKey(keys[10], shiftedMatrix);


            printMatrix("Round: 10 ", finalMatrix, 3, true);

            printMatrix("Output: ", roundedMatrix, 0, false);
           
            string finalMessage = "";
            
             //rotate matrix
            //we dont know why
            //but it is a must ??
            
             finalMatrix = new int[4, 4];
             
              for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                    finalMatrix[i,j] = roundedMatrix[j,i];
            }
            
             for (int i=0;i<4;i++)
            {
                for (int j = 0; j < 4; j++)
                    finalMessage += (char) roundedMatrix[j,i];
            }   
