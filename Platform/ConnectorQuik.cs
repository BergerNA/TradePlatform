using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


// Коннектор к Quik

namespace Platform
{
    // Команды для получения данных от сервера
    public enum SendCommand
    {
        Connect, GetSecurity, GetClassCode
    }

    public class ConnectorQuik
    {
        public bool connected;
        private DataExchange deal;
        private DataExchange command;
        private DataExchange quote;
        private DataExchange answer;

        // Конструктор, инициализирующий соединение с Quik. 
        // Создание или подключение именованных областей памяти.
        // Запуск в отдельных потоках получение: сделок, стакана, 
        // запросов и инф. сообщений, статуса соединения.
        // Подписка на события.
        public ConnectorQuik()
        {
            connected = false;
            deal = new DataExchange(DataExchange.DefineNameMaping.Deal, 4000);
            command = new DataExchange(DataExchange.DefineNameMaping.QUIKCommand, 500);
            quote = new DataExchange(DataExchange.DefineNameMaping.Quote, 1400);
            answer = new DataExchange(DataExchange.DefineNameMaping.QUIKAnswer, 3000);
            deal.Start();
            command.Start();
            quote.Start();
            answer.Start();
            answer.Event_TerminalAnswer += new AnswerHandler(GetAnswer);
            deal.Event_GetTick += new GetTickHandler(EGetTick);
            deal.Event_GetTicks += Deal_Event_GetTicks;
          /*  new Thread(() =>          // Передача различных команд
            {
                command.ThreadRun();
            }).Start();*/
            new Thread(() =>            // Передача сделок
            {
                deal.ThreadRun();
            }).Start();
         /*   new Thread(() =>          // Передача стакана
            {
                quote.ThreadRun();
            }).Start();*/
            new Thread(() =>            // Получение ответа
            {
                answer.ThreadRun();
            }).Start();
        }

        // Событие получения сделки (в виде строки)
        private void Deal_Event_GetTicks(string str)
        {
            if (Event_GetTicks != null) Event_GetTicks.Invoke(str);
        }

        // Событие получения сделки (в виде тика)
        private void EGetTick(Tick tick)
        {
           Event_GetTick.Invoke(tick);
        }

        // Соединение с Quik
        public void Connect()
        {
            command.Send(SendCommand.Connect.ToString());
        }

        // Проверка соединения
        public bool Connected()
        {
            return connected;
        }

        // Отправка команды, запроса
        public void SendCom(SendCommand cmd, string str)
        {
            if (str != "")
                command.Send(cmd.ToString() + str);
            else
                command.Send(cmd.ToString());
        }

        // Получение ответа
        public void GetAnswer(Dictionary<string, string> answer)
        {
            switch (answer["ID"])
            {
                // Статус соединения
                case "Connect":
                {
                    if (answer["Value"].IndexOf("1") == 0) connected = true;
                    else connected = false;
                    if (Event_GetConnect != null) Event_GetConnect.Invoke(connected);
                }
                    break;

                    // Разрыв соединения
                case "Disconnect":
                    {
                        if (answer["Value"] == "1") connected = true;
                        else connected = false;
                    }
                    break;

                    // Получение списка инструментов
                case "SecurityList":
                    {
                        Event_GetSecurity.Invoke(answer["Value"].Split(','));
                    }
                    break;

                    // Получение списка классов инструментов
                case "ClassNameList":
                    {
                        Event_GetClassCode.Invoke(answer["Value"].Split(','));
                    }
                    break;
                case "Deal":
                {
                    
                }
                break;
            }
        }
        //Событие OnCount c типом делегата MethodContainer.
        public event AnswerHandlers Event_GetClassCode;
        public delegate void AnswerHandlers(string[] str);
        public event AnswerHandlers Event_GetSecurity;

        public event AnswerConnect Event_GetConnect;
        public delegate void AnswerConnect(bool connect);

        public delegate void GetTickHandlers(Tick tick);
        public event GetTickHandlers Event_GetTick;

        public delegate void GetTickHandlerss(string str);
        public event GetTickHandlerss Event_GetTicks;

        public void Dispose()
        {
            deal.Stop();
          //  deal.Clear();
            command.Stop();
         //   command.Clear();
            quote.Stop();
         //   quote.Clear();
            answer.Stop();
          //  answer.Clear();
        }
    }
}
