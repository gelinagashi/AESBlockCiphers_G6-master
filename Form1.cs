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
            
            return finalMessage;
        }
        
        
        private List<byte[,]> generateKeys()
        {
            string key = txtKey.Text.Trim();
            
             //Add Padding
            // we take 'o' character as default padding
            
         for (int i = key.Length; i < 16; i++)
            {
                key += "o";
            }
            
       //Split string to characters
            char[] keyChars = key.ToCharArray();

            //prepare array to get byte equivalent of chars
            byte[] keyBytes = new byte[16];
            byte[] keyHex = new byte[16];
             string msg = "";
            //store characters as bytes
            
            for (int i = 0; i < 16; i++)
            {
                keyBytes[i] = (byte)keyChars[i];

                msg += keyBytes[i] + "\n";

            }
/*
            keyBytes[0] = 0x2b;
            keyBytes[1] = 0x28;
            keyBytes[2] = 0xab;
            keyBytes[3] = 0x09;
            keyBytes[4] = 0x7e;
            keyBytes[5] = 0xae;
            keyBytes[6] = 0xf7;
            keyBytes[7] = 0xcf;
            keyBytes[8] = 0x15;
            keyBytes[9] = 0xd2;
            keyBytes[10] = 0x15;
            keyBytes[11] = 0x4f;
            keyBytes[12] = 0x16;
            keyBytes[13] = 0xa6;
            keyBytes[14] = 0x88;
            keyBytes[15] = 0x3c;
            */

            msg = "";
            int index = 0;

            //Prepare to save all keys here
            List<byte[,]> roundKeys = new List<byte[,]>();

            //split 128 bits to 4 sections with 4 bytes
            byte[,] splitKey = new byte[4,4];

            int xPos = 0, yPos = -1;
            
            for (int i = 0; i < keyBytes.Length; i++)
            {
                if (i % 4 == 0)
                {
                    yPos++;
                    xPos = 0;

                }

                splitKey[xPos, yPos] = keyBytes[i];

                xPos++;
            }

            roundKeys.Add(splitKey);

            printMatrix("Key 0:", splitKey, 4);

            byte[,] generatedKey = new byte[4,4];
            //Start generating 10 other keys
            
             for (int i = 1; i < 11; i++)
            {
                generatedKey = getKey(roundKeys[i - 1], i - 1);
                roundKeys.Add(generatedKey);

                printMatrix("Key " + i + ": ", generatedKey, 4);
            }

            return roundKeys;
        }

        private byte[,] getKey(byte[,] prevKey, int index)
        {
            //Rcon Table
            byte[] rconTable = { 1, 2, 4, 8, 16, 32, 64, 128, 27, 54};

            byte[,] retKey = new byte[4,4];

            byte[] newKey = new byte[4];

            byte[] w0 = new byte[4];
            byte[] w3 = new byte[4];


            for (int i = 0; i < 4; i++)
            {
                w0[i] = prevKey[i, 0];
                w3[i] = prevKey[i, 3];
            }
                
            
            w3 = shiftByte(w3);

            //translate w3 to values from sbox
            w3 = exchangeW3(w3);

            byte[] rcon = { rconTable[index], 0, 0, 0 };

            for (int k = 0; k < 4; k++)
            {
                retKey[k,0] = (byte)(w0[k] ^ w3[k] ^ rcon[k]);
            }
            

            // Generating W1, W2, W3
            for (int i = 1; i < 4; i++)
            {
                newKey = new byte[4];

                byte[] current = new byte[4];
                byte[] prev = new byte[4];

                for (int h = 0; h < 4; h++)
                {
                    prev[h] = prevKey[h, i];
                    current[h] = retKey[h, i-1];
                }
                
                //byte[] current = prevKey[i];
                //byte[] prev = retKey[i - 1];

                for (int j = 0; j < 4; j++)
                {
                    byte b = (byte)(current[j] ^ prev[j]);
                    retKey[j, i] = b;
                }
                int a = 5;
            }

            return retKey;
        }


        private int[,] getSubBytes(int[,] inMatrix)
        {

            string[] hexMs = new string[16];
            //return message to hex
            int index = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    hexMs[index] = inMatrix[i, j].ToString("x2");
                    index++;
                }
            }

            int msLength = hexMs.Length;

            int[,] subBytedMatrix = new int[4, 4];

            int xPos = 0, yPos = -1;
              for (int i = 0; i < msLength; i++)
            {
                char x = hexMs[i][0];
                char y = hexMs[i][1];

                int val = getValueFromSbox(x, y);

                if(i%4 == 0)
                {
                    yPos++;
                    xPos = 0;
                   
                }

                subBytedMatrix[yPos, xPos] = val;

                xPos++;
            }

            //MessageBox.Show(subBytedMatrix.GetLength(0) + "");

            return subBytedMatrix;
        }
          private int[,] getShiftRows(int[,] matrix)
        {
     
            for(int i = 1; i < 4; i++)
            {

                int shifts = i;
                while (shifts > 0)
                {
                    int tmp = matrix[i,0];

                    for (int j = 0; j < 3; j++)
                    {
                        matrix[i,j] = matrix[i,j+1];
                    }

                    matrix[i,3] = tmp;


                    shifts--;
                }

            }

            return matrix;
        }
        
        private int[,] getMixedColumns(int [,] matrix)
        {
            /*
            matrix = new int[4, 4] {
                {0xd4, 0xe0, 0xb8, 0x1e },
                {0xbf, 0xb4, 0x41, 0x27 },
                {0x5d, 0x52, 0x11, 0x98 },
                {0x30, 0xae, 0xf1, 0xe5 }
            };
            */

            int[,] newMatrix = new int[4, 4];

            string msg = "";
            for(int a = 0; a < 4; a++)
            {
                for(int b = 0; b < 4; b++)
                {
                    msg += matrix[a, b] + " ";
                }
                msg += "\n";
            }

       
