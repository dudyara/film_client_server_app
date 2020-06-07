using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Lab4
{
    class Model
    {

        public static UdpClient udpClient = new UdpClient(8002);

        public static ListOfFilms LoadMethod()
        {
            char separator1 = '\n';
            char separator = ';';
            string send = "1";
            SendMessage(send);
            string[] AnimalMessage = ReceiveMessage().Split(separator1);
            ListOfFilms listOfAnimals = new ListOfFilms();

            for (int i = 0; i < AnimalMessage.Length - 1; i++)
            {
                Films animal = new Films(AnimalMessage[i].Split(separator)[0],
                    AnimalMessage[i].Split(separator)[1],AnimalMessage[i].Split(separator)[2],
                    int.Parse(AnimalMessage[i].Split(separator)[3]), int.Parse(AnimalMessage[i].Split(separator)[4]),
                    int.Parse(AnimalMessage[i].Split(separator)[5]), bool.Parse(AnimalMessage[i].Split(separator)[6]));
                listOfAnimals.AddFilm(animal);
            }
            return listOfAnimals;
        }

        //метод для сохранения данных
        public static void SaveMethod(ListOfFilms listOfAnimals)
        {
            string message = "";
            for (int i = 0; i < listOfAnimals.FilmsCount(); i++)
            {
                message = message + listOfAnimals[i].Name + ";" + listOfAnimals[i].Director + ";" +
                    listOfAnimals[i].Country + ";" + listOfAnimals[i].Year + ";" +
                    listOfAnimals[i].Cost.ToString() + ";" + listOfAnimals[i].Gain.ToString() + 
                    ";" + listOfAnimals[i].Oscared.ToString() + "\n";
            }
            SendMessage("2;" + message);
        }
        //метод для отправки сообщения о закрытии
        public static void CloseMethod()
        {
            SendMessage("3");
        }

        // метод отправки
        private static void SendMessage(string sis)
        {   
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(sis);
                //адрес и порт сервера к которому будем подглючаться
                udpClient.Send(data, data.Length, "127.0.0.1", 8001); //отправка
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static string ReceiveMessage()
        {
            
            IPEndPoint remoteIp = (IPEndPoint)udpClient.Client.LocalEndPoint; //адрес входящего подключения
            try
            {
                byte[] data = udpClient.Receive(ref remoteIp); //получаем данные
                string message = Encoding.Unicode.GetString(data);
                return message;

            }
            catch (Exception ex)
            {
                string s = "произошла ошибка";
                Console.WriteLine(ex.Message);
                return s;
            }
        }
    }
}
