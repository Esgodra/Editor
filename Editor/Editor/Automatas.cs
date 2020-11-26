using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
    class Automatas
    {

        //Automata que analiza las asignaciones de variable
        public static Boolean Asignacion(string Cadena_Linea) {
            //Declaracion de variables
            int Estado = 1;
            string cadena = Cadena_Linea;
            int contador = 0;
            int total = cadena.Length; ;
            char caracter;
            //Inicio de ciclo
            while (contador < total)
            {

                //Cortamos la cadena en la posicion que corresponde y la asignamos a caracter
                caracter = Convert.ToChar(cadena.Substring(contador, 1));
                switch (Estado)
                {
                    case 1:
                        //Verificamos que la cadena conmienze con una letra
                        if (Char.IsLetter(caracter))
                        {
                            Estado = 2;
                        }
                        else if (Char.IsNumber(caracter))
                        {
                            Estado = 5;
                        }
                        else if (caracter == ':')
                        {
                            Estado = 3;
                        }
                        //De lo contrario salimos a la rutina de error
                        else
                        {
                            return false;
                        }
                        break;
                    case 2:
                        if (Char.IsLetter(caracter) || Char.IsNumber(caracter))
                        {
                            Estado = 2;
                        }
                        else
                        {
                            //Salimos del ciclo
                            return false;
                        }
                        break;
                    case 3:
                        if (caracter == '=')
                        {
                            Estado = 4;
                        }
                        else
                        { //Salimos del ciclo
                            return false;
                        }
                        break;
                    case 4:
                        if (caracter == ' ') {
                            return true;
                        }
                        else {
                            return false;
                        }
                    case 5:
                        if (Char.IsNumber(caracter))
                        {
                            Estado = 5;
                        }
                        else
                        {
                            //Salimos del ciclo
                            return false;
                        }
                        break;
                } //Fin de estructura case
                contador = contador + 1;
            }/*Fin del ciclo while
            if (Estado != 2 && Estado != 5 && Estado != 4)
            {
                return false;
            }
            else
            {
                return true;
            }*/
            return true;
        }
        //Automata que analiza los numeros reales
        public static Boolean Numeros(string Cadena_Linea) {
            //Declaracion de variables
            int Estado = 1;
            string cadena = Cadena_Linea;
            int contador = 0;
            int total = cadena.Length; ;
            char caracter;
            //Inicio de ciclo
            while (contador < total)
            {

                //Cortamos la cadena en la posicion que corresponde y la asignamos a caracter
                caracter = Convert.ToChar(cadena.Substring(contador, 1));
                switch (Estado)
                {
                    case 1:
                        //Verificamos que la cadena conmienze con un digito
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 2;
                        }
                        //De lo contrario salimos a la rutina de error
                        else
                        {
                            return false;
                        }
                        break;
                    case 2:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 2;
                        }
                        else if (caracter == '.')
                        {
                            Estado = 3;
                        }
                        else if (caracter == 'E')
                        {
                            Estado = 5;
                        }
                        else {
                            return false;
                        }
                        break;
                    case 3:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 4;
                        }
                        else
                        { //Salimos del ciclo
                            return false;
                        }
                        break;
                    case 4:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 4;
                        }
                        else if (caracter == 'E')
                        {
                            Estado = 5;
                        }
                        else {
                            return false;
                        }
                        break;
                    case 5:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 7;
                        }
                        else if (caracter == '-' || caracter == '+') {
                            Estado = 6;
                        }
                        else
                        {
                            //Salimos del ciclo
                            return false;
                        }
                        break;
                    case 6:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 7;
                        }
                        else {
                            return false;
                        }
                        break;
                    case 7:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 7;
                        }
                        else {
                            return false;
                        }
                        break;
                } //Fin de estructura case
                contador = contador + 1;
            }//Fin del ciclo while
            //Si no salio ninguna rutina de error se ejecuta este true
            if (Estado == 4 || Estado == 7)
            {
                return true;
            }
            else {
                return false;
            }
        }

        //Automata para nombres de variable
        internal static bool NombreVariable(string Cadena_Linea)
        {
            //Declaracion de variables
            int Estado = 1;
            string cadena = Cadena_Linea;
            int contador = 0;
            int total = cadena.Length; ;
            char caracter;
            //Inicio de ciclo
            while (contador < total)
            {

                //Cortamos la cadena en la posicion que corresponde y la asignamos a caracter
                caracter = Convert.ToChar(cadena.Substring(contador, 1));
                switch (Estado)
                {
                    case 1:
                        //Verificamos que la cadena conmienze con una letra
                        if (Char.IsLetter(caracter))
                        {
                            Estado = 3;
                        }
                        //De lo contrario vamos al estado 2
                        else
                        {
                            Estado = 2;
                        }
                        break;
                    case 2:
                        //Si llegamos a este estado, salimos a rutina de error
                        return false;
                    case 3:
                        //Verificamos que el siguiente caracter sigue siendo una letra o un digito
                        if (Char.IsLetterOrDigit(caracter))
                        {
                            Estado = 3;
                        }
                        else // De lo contrario
                        { //Salimos del ciclo
                            return false;
                        }
                        break;
                } //Fin de estructura case
                contador = contador + 1;
            }//Fin del ciclo while
            //Si no salio ninguna rutina de error se ejecuta este true
            if (Estado == 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        //Automata para numeros Racionales
        internal static bool Racional(string Cadena_Linea)
        {
            //Declaracion de variables
            int Estado = 1;
            string cadena = Cadena_Linea;
            int contador = 0;
            int total = cadena.Length; ;
            char caracter;
            //Inicio de ciclo
            while (contador < total)
            {

                //Cortamos la cadena en la posicion que corresponde y la asignamos a caracter
                caracter = Convert.ToChar(cadena.Substring(contador, 1));
                switch (Estado)
                {
                    case 1:
                        //Verificamos que la cadena conmienze con un numero
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 3;
                        }
                        //O que comience con un signo
                        else if (caracter == '-' || caracter == '+')
                        {
                            Estado = 2;
                        }
                        else 
                        {
                            return false;
                        }
                        break;
                    case 2:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 3;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 3:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 3;
                        }
                        else if(caracter == '.') 
                        {
                            Estado = 4;
                        }
                        else // De lo contrario
                        { //Salimos del ciclo
                            return false;
                        }
                        break;
                    case 4:
                        if (Char.IsDigit(caracter))
                        {
                            Estado = 4;
                        }
                        else 
                        {
                            return false;
                        }
                        break;
                } //Fin de estructura case
                contador = contador + 1;
            }//Fin del ciclo while
            //Si no salio ninguna rutina de error se ejecuta este true
            if (Estado == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Automata para 
        internal static bool Binario(string Cadena_Linea)
        {
            //Declaracion de variables
            int Estado = 1;
            string cadena = Cadena_Linea;
            int contador = 0;
            int total = cadena.Length; ;
            char caracter;
            //Inicio de ciclo
            while (contador < total)
            {

                //Cortamos la cadena en la posicion que corresponde y la asignamos a caracter
                caracter = Convert.ToChar(cadena.Substring(contador, 1));
                switch (Estado)
                {
                    case 1:
                        //Verificamos que la cadena comienze con un 0
                        if (caracter=='0')
                        {
                            Estado = 2;
                        }
                        //De lo contrario verificamos si comienza con un 1
                        else if (caracter == '1')
                        {
                            Estado = 4;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 2:
                        if (caracter == '0')
                        {
                            Estado = 3;
                        }
                        else if (caracter == '1')
                        {
                            Estado = 4;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 3:
                        return false;
                    case 4:
                        if (caracter == '1' || caracter == '0')
                        {
                            Estado = 4;
                        }
                        else 
                        {
                            return false;
                        }
                        break;
                } //Fin de estructura case
                contador = contador + 1;
            }//Fin del ciclo while
            //Si no salio ninguna rutina de error se ejecuta este true
            if (Estado == 4)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}