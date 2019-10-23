using System;
using System.Collections.Generic;
using Modelo;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Programa
{
    class Program
    {
        static void Main(string[] args)
        {
            //Ejercicio01_________________________________________________________________________________________________________
            List<Equipo> equipos = LigaDAO.Instance.Equipos;
            List<Jugador> jugadores = LigaDAO.Instance.Jugadores;
            List<Partido> partidos = LigaDAO.Instance.Partidos;

            Console.WriteLine("Equipos :");
            equipos.ForEach(Console.WriteLine);
            Console.WriteLine("\nJugadores :");
            jugadores.ForEach(Console.WriteLine);
            Console.WriteLine("\nPartidos :");
            partidos.ForEach(Console.WriteLine);



            //Ejercicio02___________________________________________________________________________________________________________

            //Jugadores Real Barcelona por fecha de alta
            Console.WriteLine("\nJugadores Real barcelona por fecha de alta");
            equipos.Where((e) => e.Nombre == "Regal Barcelona").First().Jugadores.OrderBy
                ((e) => e.FechaAlta).ToList().ForEach(Console.WriteLine);

            //Jugadores de gran canaria por apellidos
            Console.WriteLine("\nJugadores de Gran canarias ordenados por apellidos");
            equipos.Where((equipo) => equipo.Nombre == "Gran Canaria").First().Jugadores.OrderBy
                ((jugador) => jugador.Apellido).ToList().ForEach(Console.WriteLine);

            //El jugador mas alto y el euipo al que pertenece
            Console.WriteLine("\nJugador mas alto y el equipo al que pertenece");
            Jugador jugadorMasAlto = jugadores.OrderByDescending((e) => e.Altura).First();
            Console.WriteLine("Jugador: " + jugadorMasAlto.Nombre + jugadorMasAlto.Apellido + " Equipo: " + jugadorMasAlto.Equipo.Nombre);

            //Jugadores que juegan de pivot
            Console.WriteLine("\nJugadores que juegan como pivot");
            jugadores.Where((e) => e.Posicion == "Pivot").ToList().ForEach(Console.WriteLine);

            //Ejercicio03_____________________________________________________________________________________________

            //Equipo que tiene el jugador que mas cobre
            Console.WriteLine("\nEquipo con el jugador de mayor sueldo:");
            Jugador jugadorMasCaro = jugadores.OrderByDescending((jug) => jug.Salario).First();
            Console.WriteLine(" Equipo: " + jugadorMasCaro.Equipo.Nombre);

            //Jugadores que miden mas de 2 metros
            Console.WriteLine("\nJugadores que midan mas de 2 metros");
            jugadores.Where((jug) => jug.Altura > 2).ToList().ForEach(Console.WriteLine);

            //Quienes son los capitanes de los equipos
            Console.WriteLine("\nCapitanes de los equipos");
            jugadores.Where((jug) => jug.Capitan != null && jug.Capitan.Id == jug.Id).
                ToList().ForEach(Console.WriteLine);

            //Ejercicio04___________________________________________________________________________________________

            //Lista de strings de los jugadores 
            Console.WriteLine("\nLista de string de jugadores (NOMBRE APELLIDO EQUIPO)");
            List<string> listaJugString = new List<string>();
            jugadores.ForEach(jug => listaJugString.Add(jug.Nombre + jug.Apellido + jug.Equipo.Nombre));
            listaJugString.ForEach(Console.WriteLine);

            //Equipo con mas victorias
            Console.WriteLine("\nEl equipo que mas victorias ha obternido es:");
            string[] resultadoSplit = new string[2]; //creamos un array de string para guardar el valor del resultado del partido spliteado
            Dictionary<Equipo, int> partidosGanados = new Dictionary<Equipo, int>(); //Creamos un hashmap Clave Valor

            partidos.ForEach(part =>
            {

                resultadoSplit = part.Resultado.Split("-");
                if (Int32.Parse(resultadoSplit[0]) > Int32.Parse(resultadoSplit[1]))
                {
                    try
                    {
                        partidosGanados.Add(part.Local, 1);
                    }
                    catch (ArgumentException)
                    {
                        partidosGanados.ToDictionary(x => x.Key, y => y.Value + 1);
                    }

                } else if (Int32.Parse(resultadoSplit[0]) < Int32.Parse(resultadoSplit[1])) {

                    try
                    {
                        partidosGanados.Add(part.Visitante, 1);

                    }
                    catch (ArgumentException)
                    {
                        partidosGanados.ToDictionary(x => x.Key, y => y.Value + 1);

                    }
                }

            });
            var maxValue = partidosGanados.Values.Max();
            Console.WriteLine(partidosGanados.Where(e => e.Value == maxValue).Select(e => e.Key).First().ToString());


            //Al puertas me lo como con patatas
            XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XComment("Creating an XML Tree using LINQ to XML"),

                new XElement("Equipo",
                    from equipo in equipos
                    select new XElement("Equipo", new XAttribute("Id", equipo.ID),
                                new XElement("Nombre", equipo.Nombre),
                                new XElement("Ciudad", equipo.Ciudad),
                                new XElement("Web", equipo.Web),
                                new XElement("Puntos", equipo.Puntos)),

                            from jug in jugadores
                            select new XElement("Jugadores", new XAttribute("Id", jug.Id),
                                            new XElement("Nombre", jug.Nombre),
                                            new XElement("Apellido", jug.Apellido),
                                            new XElement("Posicion", jug.Posicion),
                                            new XElement("Capitan", jug.Capitan),
                                            new XElement("FechaAlta", jug.FechaAlta),
                                            new XElement("Salario", jug.Salario),
                                            new XElement("Equipo", jug.Equipo.Nombre),
                                            new XElement("Altura", jug.Altura))
                            ));
            xmlDocument.Save("SerializacionLinq.xml");


            Console.WriteLine("Serializacion acabada");                    

   

            Console.ReadLine();
        }
    }
}
