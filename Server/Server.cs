using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using NLog;
using System.Threading;

namespace Server
{
    class Server
    {

        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private UdpClient udpClient_S;
        private int port;

        private ContainerOfFilms Container = new ContainerOfFilms();
        private char separator = ';';
        private char separator1 = '\n';
        private Logger logger = LogManager.GetCurrentClassLogger();

        public Server(int _port)
        {
            udpClient_S = new UdpClient(_port);
            this.port = _port;
            Console.WriteLine("Сервер работает");
        }

        public void StartListenAsync()
        {
            while (true)
            {
                allDone.Reset();
                udpClient_S.BeginReceive(RequestCallback, udpClient_S);
                allDone.WaitOne();
            }
        }


        private void RequestCallback(IAsyncResult ar)
        {
            allDone.Set();
            var listener = (UdpClient)ar.AsyncState;
            var ep = (IPEndPoint)udpClient_S.Client.LocalEndPoint;
            var res = listener.EndReceive(ar, ref ep);
            string[] FilmMessage = Encoding.Unicode.GetString(res).Split(separator1);
            string[] message = FilmMessage[0].Split(separator);

            switch (ushort.Parse(message[0]))
            {
                case 1:
                    {
                        LoadFromDatabase();
                        Console.WriteLine("Загрузка данных из БД");
                        string otvet = "";
                        for (int i = 0; i < Container.FilmsCount(); i++)
                        {
                            otvet = otvet + Container[i].Name + ";" + Container[i].Director + ";" + Container[i].Country +
                                ";" + Container[i].Year.ToString() + ";" + Container[i].Cost.ToString() + ";"
                                + Container[i].Gain.ToString() + ";" + Container[i].Oscared.ToString() + "\n" ;
                        }
                        byte[] byteOtvet = Encoding.Unicode.GetBytes(otvet);
                        udpClient_S.SendAsync(byteOtvet, byteOtvet.Length, ep);

                        Container.ClearFilms();
                        logger.Info("Выполнена операция вывода всех данных на экран\n");
                        Console.WriteLine("Данные загружены");
                        break;
                    }
               
                case 2:
                    {
                        FilmMessage[0] = FilmMessage[0].Remove(0, 2);
                        Container.ClearFilms();
                        Console.WriteLine("Сохранение изменений В БД");
                        
                        using (FilmContext db = new FilmContext())
                        {
                            db.Database.ExecuteSqlCommand("Delete from Films");
                            for (int i = 0; i < FilmMessage.Length - 1; i++)
                            {
                                Films film = new Films(FilmMessage[i].Split(separator)[0],
                                    FilmMessage[i].Split(separator)[1], FilmMessage[i].Split(separator)[2],
                                    int.Parse(FilmMessage[i].Split(separator)[3]), 
                                    int.Parse(FilmMessage[i].Split(separator)[4]),
                                    int.Parse(FilmMessage[i].Split(separator)[5]), bool.Parse(FilmMessage[i].Split(separator)[6]));
                                db.Films.Add(film);
                                db.SaveChanges();
                            }
                            
                            //db.SaveChanges();
                        }
                        logger.Info("выполнена операция записи в базу данных завершения программы");
                        Console.WriteLine("Изменения сохранены");

                        break;
                    }

                case 3:
                    {
                        Environment.Exit(0);
                        break;
                    }
                default:
                    {
                        string otvet = "Произошла ошибка, сервер не знаком с подобной командой, попробуйте снова";
                        byte[] byteOtvet = Encoding.Unicode.GetBytes(otvet);
                        udpClient_S.SendAsync(byteOtvet, byteOtvet.Length, ep);
                    }
                    break;

            }

        }

        //считывание из БД
        private void LoadFromDatabase()
        {
            using (FilmContext db = new FilmContext())
            {

                var films = db.Films;
                foreach (Films u in films)
                {
                    Container.AddFilm(u);
                }
            }
        }

    }

    class program
    {

        static void Main(string[] args)
        {
            Server server = new Server(8001);
            server.StartListenAsync();

        }
    }
}

