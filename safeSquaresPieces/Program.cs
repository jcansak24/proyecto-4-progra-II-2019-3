using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace safeSquaresPieces
{
    class Program
    {
        static void Main(string[] args)
        {
            int numLineas = 0, nFila=1, mCol=1, cont=1, numReinas, numCaballos, numPeones;
            string linea;
            TextReader leer = new StreamReader("queens.txt");
            while ((linea = leer.ReadLine()) != "0 0") numLineas++;
            leer.Close();
            string[] line = new string[2001];
            string[] queens = new string[numLineas / 4];
            string[] knights = new string[numLineas / 4];
            string[] paws = new string[numLineas / 4];
            string[][] matrizTablero;
            
            int[] filasP;
            int[] columP;
            int[] filasK;
            int[] columK;
            int[] filasQ;
            int[] columQ;

            TextReader leerArch = new StreamReader("queens.txt");
            while ((linea = leerArch.ReadLine()) != "0 0")
            {
                for (int i=1; i<=4;i++)
                {                   
                    switch (i)
                    {
                        case 1:
                            line = linea.Split();
                            nFila = int.Parse(line[0]);
                            mCol = int.Parse(line[1]);
                            linea = leerArch.ReadLine();
                            break;

                        case 2:
                            queens = linea.Split();
                            linea = leerArch.ReadLine();
                            break;

                        case 3:
                            knights = linea.Split();
                            linea = leerArch.ReadLine();
                            break;

                        case 4:
                            paws = linea.Split();                            
                            break;
                    }                    
                }
                numReinas = int.Parse(queens[0]);
                numCaballos = int.Parse(knights[0]);
                numPeones = int.Parse(paws[0]);
                matrizTablero = new string[nFila+1][];
                int numSeguras = 0;

                for (int k = 1; k <= nFila; k++) matrizTablero[k] = new string[mCol+1];

                filasQ = new int[numReinas];
                columQ = new int[numReinas];

                for (int j = 1, a = 0, b = 0 ; j <= numReinas*2; j++)
                {
                    if (j % 2 != 0) filasQ[a++] = int.Parse(queens[j]);
                    else columQ[b++] = int.Parse(queens[j]);
                }
                
                filasK = new int[numCaballos];
                columK = new int[numCaballos];

                for (int j = 1, a = 0, b = 0 ; j <= numCaballos*2; j++)
                {
                    if (j % 2 != 0) filasK[a++] = int.Parse(knights[j]);
                    else columK[b++] = int.Parse(knights[j]);
                }

                filasP = new int[numPeones];
                columP = new int[numPeones];

                for (int j = 1, a = 0, b = 0; j <= numPeones*2; j++)
                {
                    if (j % 2 != 0) filasP[a++] = int.Parse(paws[j]);
                    else columP[b++] = int.Parse(paws[j]);
                }

                int r = 0, c = 0, p = 0;
                for (int x = 1; x <= nFila; x++)
                {
                    for (int y = 1; y <= mCol; y++)
                    {
                        if (r < numReinas && (x == filasQ[r] && y == columQ[r])) { matrizTablero[x][y] = "Q"; r++; }
                        else
                        {
                            if (c < numCaballos && (x == filasK[c] && y == columK[c])) { matrizTablero[x][y] = "K"; c++; }
                            else {
                                if (p < numPeones && (x == filasP[p] && y == columP[p])) { matrizTablero[x][y] = "P"; p++; }
                                else { matrizTablero[x][y] = "S"; }
                            }
                        }                                           
                    }                    
                }
                for (int x = 1; x <= nFila; x++)
                {
                    for (int y = 1; y <= mCol; y++)
                    {
                        if (matrizTablero[x][y] == "Q")
                        {
                            ObjetoDelegadoIzAr ElDelegadoIzq = new ObjetoDelegadoIzAr(Queens.Izquierda); 
                            ElDelegadoIzq(matrizTablero, x, y); 
                            ObjetoDelegadoDerAb ElDelegadoDer = new ObjetoDelegadoDerAb(Queens.Derecha); 
                            ElDelegadoDer(matrizTablero, x, y, mCol);
                            ObjetoDelegadoIzAr ElDelegadoArr = new ObjetoDelegadoIzAr(Queens.Arriba); 
                            ElDelegadoArr(matrizTablero, x, y);
                            ObjetoDelegadoDerAb ElDelegadoAba = new ObjetoDelegadoDerAb(Queens.Abajo); 
                            ElDelegadoAba(matrizTablero, x, y, nFila);
                            ObjetoDelegadoDiag ElDelegadoDiagDer = new ObjetoDelegadoDiag(Queens.DiagonalDerecha); 
                            ElDelegadoDiagDer(matrizTablero, x, y, mCol, nFila);
                            ObjetoDelegadoDiag ElDelegadoDiagIzq = new ObjetoDelegadoDiag(Queens.DiagonalIzquierda); 
                            ElDelegadoDiagIzq(matrizTablero, x, y, mCol, nFila);
                        }
                        else
                        {
                            if (matrizTablero[x][y] == "K")
                            {
                                delegaCabUp deleKnight = new delegaCabUp(Knights.Movimiento);
                                deleKnight(matrizTablero, x, y, mCol, nFila);
                                for (int i = 0; i < numReinas; i++) matrizTablero[filasQ[i]][columQ[i]] = "Q";                                                            
                                for (int i = 0; i < numPeones; i++) matrizTablero[filasP[i]][columP[i]] = "P";
                            }
                        }
                    }                    
                }
                for (int x = 1; x<= nFila; x++)
                {
                    for (int y = 1; y <= mCol; y++)
                    {
                        if (matrizTablero[x][y]=="S") numSeguras++;                        
                    }
                }
                Console.WriteLine($"Board {cont++} has {numSeguras} safe squares");
            }
            Console.ReadLine();
        }
        
        delegate void ObjetoDelegadoIzAr(string[][] matrizTab, int x, int y); 
        delegate void ObjetoDelegadoDerAb(string[][] matrizTab, int x, int y, int dim); 
        delegate void ObjetoDelegadoDiag(string[][] matrizTab, int x, int y, int col, int fila); 
        delegate void delegaCabUp(string[][] matrizTab, int x, int y, int col, int fila); 
        class Queens
        {
            public static void Izquierda(string[][] matrizTab, int x, int y) {
                bool band = false;
                int z = y - 1;
                do
                {
                    if (z == 0) band = true;                     
                    else
                    {
                        if (matrizTab[x][z] != "S" && matrizTab[x][z] != "-") band = true;                        
                        else matrizTab[x][z] = "-";                         
                    }                    
                    z--;
                } while (!band);
            }
            public static void Derecha(string[][] matrizTab, int x, int y, int col)
            {
                bool band = false;
                int z = y + 1;
                do
                {
                    if ( z == col + 1) band = true;
                    else {
                        if (matrizTab[x][z] != "S" && matrizTab[x][z] != "-") band = true;
                        else matrizTab[x][z] = "-";
                    }
                    z++;
                } while (!band);}
            public static void Arriba(string[][] matrizTab, int x, int y)
            {
                bool band = false;
                int z = x - 1;
                do
                {
                    if ( z == 0) band = true;
                    else {
                        if (matrizTab[z][y] != "S" && matrizTab[z][y] != "-") band = true;
                        else matrizTab[z][y] = "-";
                    }
                    z--;
                } while (!band);
            }
            public static void Abajo(string[][] matrizTab, int x, int y, int fila)
            {
                bool band = false;
                int z = x + 1;
                do
                {
                    if (z == fila + 1) band = true;
                    else
                    {
                        if (matrizTab[z][y] != "S" && matrizTab[z][y] != "-") band = true;                        
                        else matrizTab[z][y] = "-";                        
                    }
                    z++;
                } while (!band);
            }
            public static void DiagonalIzquierda(string[][] matrizTab, int x, int y, int col, int fila)
            {
                bool band = false;
                int z = y - 1, auxX =x;
                x += 1;
                do
                {
                    if (z == 0 || x == fila + 1) band = true;                    
                    else
                    {
                        if (matrizTab[x][z] != "S" && matrizTab[x][z] != "-") band = true;                        
                        else matrizTab[x][z] = "-";                        
                    }
                    z--; x++;
                } while (!band);
                x = auxX-1;
                z = y + 1;
                do
                {
                    if (z == col + 1 || x == 0) band = true;                    
                    else
                    {
                        if (matrizTab[x][z] != "S" && matrizTab[x][z] != "-") band = true;                        
                        else matrizTab[x][z] = "-";                        
                    }
                    z++; x--;
                } while (!band);
            }
            public static void DiagonalDerecha(string[][] matrizTab, int x, int y, int col, int fila)
            {
                bool band = false;
                int z = y + 1, auxX = x;
                x -= 1;
                do
                {
                    if (z == col + 1 || x == 0) band = true;                    
                    else
                    {
                        if (matrizTab[x][z] != "S" && matrizTab[x][z] != "-") band = true;                        
                        else matrizTab[x][z] = "-";                       
                    }
                    z++; x--;
                } while (!band);
                x = auxX + 1;
                z = y + 1;
                do
                {
                    if (z == col + 1 || x == fila + 1) band = true;                                        
                    else
                    {
                        if (matrizTab[x][z] != "S" && matrizTab[x][z] != "-") band = true;
                        else matrizTab[x][z] = "-";                        
                    }
                    z++; x++;
                } while (!band);
            }            
        }

        class Knights
        {
            public static void Movimiento(string[][] matrizTab, int x, int y, int col, int fila)
            {
                
                if ((x == 1 && y == 1 && y + 2 <= col && x + 2 <= fila) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S"))
                {
                    matrizTab[x + 1][y + 2] = "-"; matrizTab[x + 2][y + 1] = "-";
                }
                else
                {
                    if ((x == 1 && y == col && y - 2 >= 1 && x + 2 <= fila) && (matrizTab[x + 1][y - 2] == "S" || matrizTab[x + 2][y - 1] == "S"))
                    {
                        matrizTab[x + 1][y - 2] = "-"; matrizTab[x + 2][y - 1] = "-";
                    }
                    else
                    {
                        if ((x == 1 && y == col - 1 && x + 2 <= fila && y - 2 >= 1) && (matrizTab[x + 1][y - 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S"))
                        {
                            matrizTab[x + 1][y - 2] = "-"; matrizTab[x + 2][y + 1] = "-"; matrizTab[x + 2][y - 1] = "-";
                        }
                        else
                        {
                            if ((x == 1 && x + 2 <= fila && y - 2 >= 1 && y <= col - 2) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S" || matrizTab[x + 1][y - 2] == "S"))
                            {
                                matrizTab[x + 1][y + 2] = "-";  matrizTab[x + 2][y + 1] = "-"; matrizTab[x + 2][y - 1] = "-"; matrizTab[x + 1][y - 2] = "-";
                            }
                            else
                            {
                                if ((x == 1 && x + 2 <= fila && y - 1 >= 1 && y + 2 <= col) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S"))
                                {
                                    matrizTab[x + 1][y + 2] = "-"; matrizTab[x + 2][y + 1] = "-"; matrizTab[x + 2][y - 1] = "-";
                                }
                                else
                                {
                                    if ((y == 1 && x>1 && x + 2 <= fila && x - 1 >= 1 && y+2 <= col) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x - 1][y + 2] == "S"))
                                    {
                                        matrizTab[x + 1][y + 2] = "-";  matrizTab[x + 2][y + 1] = "-";  matrizTab[x - 1][y + 2] = "-";
                                    }
                                    else
                                    {
                                        if ((x > 1 && y == col && y - 2 >= 1 && x + 2 <= fila && x - 1 >= 1) && (matrizTab[x + 1][y - 2] == "S" || matrizTab[x + 2][y - 1] == "S"))
                                        {
                                            matrizTab[x + 1][y - 2] = "-";  matrizTab[x + 2][y - 1] = "-";
                                        }
                                        else
                                        {
                                            if ((y == col - 1 && x > 1 && x + 2 <= fila && x - 1 >= 1 && y - 2 >= 1) && (matrizTab[x + 1][y - 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S" || matrizTab[x - 1][y - 2] == "S"))
                                            {
                                                matrizTab[x + 1][y - 2] = "-";  matrizTab[x + 2][y + 1] = "-";  matrizTab[x + 2][y - 1] = "-";  matrizTab[x - 1][y - 2] = "-";
                                            }
                                            else
                                            {
                                                if ((y > 1 && x > 1 && x + 2 <= fila && y - 2 >= 1 && y <= col - 2 && x - 1 >= 1) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S" || matrizTab[x + 1][y - 2] == "S" || matrizTab[x - 1][y - 2] == "S" || matrizTab[x - 1][y + 2] == "S"))
                                                {
                                                    matrizTab[x + 1][y + 2] = "-";  matrizTab[x + 2][y + 1] = "-";  matrizTab[x + 2][y - 1] = "-";  matrizTab[x + 1][y - 2] = "-";  matrizTab[x - 1][y - 2] = "-";  matrizTab[x - 1][y + 2] = "-";
                                                }
                                                else
                                                {
                                                    if ((y > 1 && x > 1 && x + 2 <= fila && x - 1 >= 1 && y + 2 <= col && y - 1 >= 1) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S" || matrizTab[x - 1][y + 2] == "S"))
                                                    {
                                                        matrizTab[x + 1][y + 2] = "-";  matrizTab[x + 2][y + 1] = "-";  matrizTab[x + 2][y - 1] = "-"; matrizTab[x - 1][y + 2] = "-";
                                                    }
                                                    else
                                                    {
                                                        if ((y == 1 && x > 1 && x + 2 <= fila && x - 2 >= 1 && y + 2 <= col) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S"))
                                                        {
                                                            matrizTab[x + 1][y + 2] = "-";  matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 1][y + 2] = "-";  matrizTab[x + 2][y + 1] = "-";
                                                        }
                                                        else
                                                        {
                                                            if ((x > 1 && y == col && y - 2 >= 1 && x + 2 <= fila && x - 2 >= 1) && (matrizTab[x + 1][y - 2] == "S" || matrizTab[x - 1][y - 2] == "S" || matrizTab[x - 2][y - 1] == "S" || matrizTab[x + 2][y - 1] == "S"))
                                                            {
                                                                matrizTab[x + 1][y - 2] = "-"; matrizTab[x - 1][y - 2] = "-"; matrizTab[x - 2][y - 1] = "-"; matrizTab[x + 2][y - 1] = "-";
                                                            }
                                                            else
                                                            {
                                                                if ((y == col - 1 && x > 1 && x + 2 <= fila && x - 2 >= 1 && y - 2 >= 1) && (matrizTab[x + 1][y - 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 2][y - 1] == "S" || matrizTab[x - 1][y - 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S"))
                                                                {
                                                                    matrizTab[x + 1][y - 2] = "-"; matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 2][y - 1] = "-";  matrizTab[x - 1][y - 2] = "-";  matrizTab[x + 2][y + 1] = "-"; matrizTab[x + 2][y - 1] = "-";
                                                                }
                                                                else
                                                                {
                                                                    if ((y > 1 && x > 1 && x + 2 <= fila && y - 2 >= 1 && y <= col - 2 && x - 2 >= 1) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 2][y - 1] == "S" || matrizTab[x + 1][y - 2] == "S" || matrizTab[x - 1][y - 2] == "S" || matrizTab[x - 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S"))
                                                                    {
                                                                        matrizTab[x + 1][y + 2] = "-";  matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 2][y - 1] = "-"; matrizTab[x + 1][y - 2] = "-";  matrizTab[x - 1][y - 2] = "-";  matrizTab[x - 1][y + 2] = "-";  matrizTab[x + 2][y + 1] = "-";  matrizTab[x + 2][y - 1] = "-";
                                                                    }
                                                                    else
                                                                    {
                                                                        if ((y > 1 && x > 1 && x + 2 <= fila && x - 2 >= 1 && y + 2 <= col && y - 1 >= 1) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 2][y - 1] == "S" || matrizTab[x - 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S"))
                                                                        {
                                                                            matrizTab[x + 1][y + 2] = "-";  matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 2][y - 1] = "-";  matrizTab[x - 1][y + 2] = "-";  matrizTab[x + 2][y + 1] = "-";  matrizTab[x + 2][y - 1] = "-";
                                                                        }
                                                                        else
                                                                        {
                                                                            if ((y == 1 && x > 1 && x + 1 <= fila && x - 2 >= 1 && y + 2 <= col) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 1][y + 2] == "S"))
                                                                            {
                                                                                matrizTab[x + 1][y + 2] = "-";  matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 1][y + 2] = "-";
                                                                            }
                                                                            else
                                                                            {
                                                                                if ((x > 1 && y == col && y - 2 >= 1 && x + 1 <= fila && x - 2 >= 1) && (matrizTab[x + 1][y - 2] == "S" || matrizTab[x - 1][y - 2] == "S" || matrizTab[x - 2][y - 1] == "S"))
                                                                                {
                                                                                    matrizTab[x + 1][y - 2] = "-"; matrizTab[x - 1][y - 2] = "-";  matrizTab[x - 2][y - 1] = "-";
                                                                                }
                                                                                else
                                                                                {
                                                                                    if ((y == col - 1 && x > 1 && x + 1 <= fila && x - 2 >= 1 && y - 2 >= 1) && (matrizTab[x + 1][y - 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 2][y - 1] == "S" || matrizTab[x - 1][y - 2] == "S"))
                                                                                    {
                                                                                        matrizTab[x + 1][y - 2] = "-"; matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 2][y - 1] = "-";  matrizTab[x - 1][y - 2] = "-";
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if ((y > 1 && x > 1 && x + 1 <= fila && y - 2 >= 1 && y <= col - 2 && x - 2 >= 1) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 2][y - 1] == "S" || matrizTab[x + 1][y - 2] == "S" || matrizTab[x - 1][y - 2] == "S" || matrizTab[x - 1][y + 2] == "S"))
                                                                                        {
                                                                                            matrizTab[x + 1][y + 2] = "-"; matrizTab[x - 2][y + 1] = "-"; matrizTab[x - 2][y - 1] = "-"; matrizTab[x + 1][y - 2] = "-"; matrizTab[x - 1][y - 2] = "-";  matrizTab[x - 1][y + 2] = "-";
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if ((y > 1 && x > 1 && x + 1 <= fila && x - 2 >= 1 && y + 2 <= col && y - 1 >= 1) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 2][y - 1] == "S" || matrizTab[x - 1][y + 2] == "S"))
                                                                                            {
                                                                                                matrizTab[x + 1][y + 2] = "-"; matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 2][y - 1] = "-"; matrizTab[x - 1][y + 2] = "-";
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                if ((y == 1 && x == fila && x - 2 >= 1 && y + 2 <= col) && (matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 1][y + 2] == "S"))
                                                                                                {
                                                                                                    matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 1][y + 2] = "-";
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    if ((y == col && x == fila && y - 2 >= 1 && x - 2 >=1) && (matrizTab[x - 1][y - 2] == "S" || matrizTab[x - 2][y - 1] == "S"))
                                                                                                    {
                                                                                                        matrizTab[x - 1][y - 2] = "-";  matrizTab[x - 2][y - 1] = "-";
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        if ((x == fila && y == col - 1 && x - 2 >= 1 && y - 2 >= 1) && (matrizTab[x - 1][y - 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 2][y - 1] == "S"))
                                                                                                        {
                                                                                                            matrizTab[x - 1][y - 2] = "-";  matrizTab[x - 2][y + 1] = "-"; matrizTab[x - 2][y - 1] = "-";
                                                                                                        }
                                                                                                        else
                                                                                                        {
                                                                                                            if ((x == fila && x - 2 >= 1 && y - 2 >= 1 && y <= col - 2) && (matrizTab[x - 1][y + 2] == "S" || matrizTab[x - 2][y + 1] == "S" || matrizTab[x - 2][y - 1] == "S" || matrizTab[x - 1][y - 2] == "S"))
                                                                                                            {
                                                                                                                matrizTab[x - 1][y + 2] = "-";  matrizTab[x - 2][y + 1] = "-";  matrizTab[x - 2][y - 1] = "-"; matrizTab[x - 1][y - 2] = "-";
                                                                                                            }
                                                                                                            else
                                                                                                            {
                                                                                                                if ((x == fila && x - 2 >= 1 && y - 1 >= 1 && y + 2 <= col) && (matrizTab[x + 1][y + 2] == "S" || matrizTab[x + 2][y + 1] == "S" || matrizTab[x + 2][y - 1] == "S"))
                                                                                                                {
                                                                                                                    matrizTab[x + 1][y + 2] = "-"; matrizTab[x + 2][y + 1] = "-";  matrizTab[x + 2][y - 1] = "-";
                                                                                                                }
                                                                                                            }
                                                                                                        }                                                                                                        
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }                                                                    
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }                
            }
        }
    }
}