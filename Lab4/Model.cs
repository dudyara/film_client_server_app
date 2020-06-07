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
            string[] FilmMessage = ReceiveMessage().Split(separator1);
            ListOfFilms ListOfFilms = new ListOfFilms();

            for (int i = 0; i < FilmMessage.Length - 1; i++)
            {
                Films film = new Films(FilmMessage[i].Split(separator)[0],
                    FilmMessage[i].Split(separator)[1], FilmMessage[i].Split(separator)[2],
                    int.Parse(FilmMessage[i].Split(separator)[3]), int.Parse(FilmMessage[i].Split(separator)[4]),
                    int.Parse(FilmMessage[i].Split(separator)[5]), bool.Parse(FilmMessage[i].Split(separator)[6]));
                ListOfFilms.AddFilm(film);
            }
            return ListOfFilms;
        }

        //метод для сохранения данных
        public static void SaveMethod(ListOfFilms ListOfFilms)
        {
            string message = "";
            for (int i = 0; i < ListOfFilms.FilmsCount(); i++)
            {
                message = message + ListOfFilms[i].Name + ";" + ListOfFilms[i].Director + ";" +
                    ListOfFilms[i].Country + ";" + ListOfFilms[i].Year + ";" +
                    ListOfFilms[i].Cost.ToString() + ";" + ListOfFilms[i].Gain.ToString() + 
                    ";" + ListOfFilms[i].Oscared.ToString() + "\n";
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
