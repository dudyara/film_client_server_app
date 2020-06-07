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

        private ConteinerOfAnimals Conteiner = new ConteinerOfAnimals();
        private char separator = ';';
        private char separator1 = '\n';
        //private string Heads = "Вид животного;Кличка;возраст;среда обитания;наличие потомства";
        private Logger logger = LogManager.GetCurrentClassLogger();

        public Server(int _port)
        {
            udpClient_S = new UdpClient(_port);
            this.port = _port;
            //LoadFromDatabase();
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
            string[] AnimalMessage = Encoding.Unicode.GetString(res).Split(separator1);
            string[] message = AnimalMessage[0].Split(separator);

            switch (ushort.Parse(message[0]))
            {
                case 1:
                    {
                        LoadFromDatabase();
                        Console.WriteLine("Загрузка данных из БД");
                        string otvet = "";
                        for (int i = 0; i < Conteiner.AnimalsCount(); i++)
                        {
                            otvet = otvet + Conteiner[i].Name + ";" + Conteiner[i].Director + ";" + Conteiner[i].Country +
                                ";" + Conteiner[i].Year.ToString() + ";" + Conteiner[i].Cost.ToString() + ";"
                                + Conteiner[i].Gain.ToString() + ";" + Conteiner[i].Oscared.ToString() + "\n" ;
                        }
                        byte[] byteOtvet = Encoding.Unicode.GetBytes(otvet);
                        udpClient_S.SendAsync(byteOtvet, byteOtvet.Length, ep);

                        Conteiner.ClearAnimals();
                        logger.Info("Выполнена операция вывода всех данных на экран\n");
                        Console.WriteLine("Данные загружены");
                        break;
                    }
               
                case 2:
                    {
                        AnimalMessage[0] = AnimalMessage[0].Remove(0, 2);
                        Conteiner.ClearAnimals();
                        Console.WriteLine("Сохранение изменений В БД");
                        
                        using (AnimalContext db = new AnimalContext())
                        {
                            db.Database.ExecuteSqlCommand("Delete from Films");
                            for (int i = 0; i < AnimalMessage.Length - 1; i++)
                            {
                                Films animal = new Films(AnimalMessage[i].Split(separator)[0], 
                                    AnimalMessage[i].Split(separator)[1], AnimalMessage[i].Split(separator)[2],
                                    int.Parse(AnimalMessage[i].Split(separator)[3]), 
                                    int.Parse(AnimalMessage[i].Split(separator)[4]),
                                    int.Parse(AnimalMessage[i].Split(separator)[5]), bool.Parse(AnimalMessage[i].Split(separator)[6]));
                                db.Animals.Add(animal);
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
            using (AnimalContext db = new AnimalContext())
            {

                var animals = db.Animals;
                foreach (Films u in animals)
                {
                    Conteiner.AddAnimal(u);
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

