using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // Dichiarazione Socket Client
            Socket client = null;
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // Dichiarazione EndPoint del server
            IPAddress ipAddr = null;
            string strIPAddress = "";
            string strPort = "";
            int nPort = 0;


            //Dichiarazione Variabili per comunicare con il server
            string sendString = "";
            string receivedString = "";
            byte[] buff = new byte[128];
            int nReceivedBytes = 0;
            try
            {
                // Settagio da Console dell'EndPoint
                Console.WriteLine("Benvenuto nel Client Socket");
                Console.Write("IP del Server: ");
                strIPAddress = Console.ReadLine();
                Console.Write("Porta del Server: ");
                strPort = Console.ReadLine();

                if (!IPAddress.TryParse(strIPAddress.Trim(), out ipAddr))
                {
                    Console.Write("IP non valido.");
                    return;
                }
                if (!int.TryParse(strPort, out nPort))
                {
                    Console.Write("Porta non valida.");
                    return;
                }
                if (nPort <= 0 || nPort >= 65535)
                {
                    Console.Write("Porta non valida.");
                    return;
                }
                Console.WriteLine("Endpoint: " + ipAddr.ToString() + " " + nPort);

                // Connessione al server
                client.Connect(ipAddr, nPort);

                //Inizio chat con il server
                Console.WriteLine("Chatta con il server. ");
                while (true)
                {
                    // Prendo il messaggio & condizione di uscita
                    sendString = Console.ReadLine();
                    if (sendString.ToUpper().Trim() == "QUIT")
                    {
                        //dico al server di interrompersi
                        buff = Encoding.ASCII.GetBytes(sendString);
                        client.Send(buff);
                        break;
                    }
                    //Pulisco il buffer e ricevo il messaggio
                    Array.Clear(buff, 0, buff.Length);
                    nReceivedBytes = client.Receive(buff);
                    receivedString = Encoding.ASCII.GetString(buff);
                    Console.WriteLine("S: " + receivedString);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                /* In ogni occasione chiudo la connessione per sicurezza */
                if (client != null)
                {
                    if (client.Connected)
                    {
                        client.Shutdown(SocketShutdown.Both);//disabilita la send e receive
                    }
                    client.Close();
                    client.Dispose();
                }
            }
            Console.WriteLine("Premi Enter per chiudere...");
            Console.ReadLine();
        }
    }
}
