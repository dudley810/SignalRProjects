using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace FamilyChat
{
    class Program
    {
        static HubConnection hubConnection;
        static IHubProxy myHubProxy;
        static void Main(string[] args)
        {
            string signalrserver = "http://" + args[0] + "/signalr/hubs";
            hubConnection = new HubConnection(signalrserver, "ClientName=FamilyChat");
            //uncomment for debugging -- also see app.xaml.cs
            //hubConnection.TraceLevel = TraceLevels.All;
            //hubConnection.TraceWriter = Console.Out;

            myHubProxy = hubConnection.CreateHubProxy("familyChatHub");
            myHubProxy.On<string>("SendMessage", Receive_SendMessage);
            hubConnection.StateChanged += hubConnection_StateChanged;
            hubConnection.Credentials = CredentialCache.DefaultCredentials;

            hubConnection.Start();

            bool stopchat = false;

            var worker = new Thread(() =>
            {
                while (!stopchat)
                {
                    if (mysignalrstatus == ConnectionState.Disconnected)
                    {
                        stopchat = true;
                    }
                };
                hubConnection.Stop();
            });

            worker.Start();

            string message = null;
            do
            {
                if (mysignalrstatus == ConnectionState.Connected)
                {
                    //Console.Write("Please enter your message (Press enter to quit):");
                    message = Console.ReadLine();
                    SendMessage(message);
                }
                else
                {
                    if (mysignalrstatus == ConnectionState.Disconnected)
                    {
                        Console.WriteLine("Connection Disconnected");
                        message = string.Empty;
                    }
                    else
                    {
                        message = mysignalrstatus.ToString();
                    }
                }
            }
            while (!String.IsNullOrWhiteSpace(message));

            // Stop the worker thread
            stopchat = true;



        }

        static ConnectionState mysignalrstatus;
        static void hubConnection_StateChanged(StateChange obj)
        {
            mysignalrstatus = obj.NewState;
            Console.WriteLine("StateChange: " + obj.NewState.ToString());
        }

        static void SendMessage(string Message)
        {
            myHubProxy.Invoke("SendMessage", new string[] { Environment.UserName, Message });
        }


        static void Receive_SendMessage(string obj)
        {
            Console.WriteLine(obj);
        }
    }
}
