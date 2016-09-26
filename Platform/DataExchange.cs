using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

// Класс для работы с именованными областями памяти

namespace Platform
{
    public delegate void AnswerHandler(Dictionary<string, string> answer);
    public delegate void GetTickHandler(Tick tick);
    public delegate void GetTickHandlers(string str);

    public class DataExchange
    {
        // Создаст, или подключится к уже созданной памяти с таким именем
        public MemoryMappedFile Memory;
        // Создает поток для чтения
        private StreamReader SR_Memory;
        // Создает поток для записи
        private StreamWriter SW_Memory;

        public enum DefineNameMaping
        {
            QUIKCommand, QUIKAnswer, Deal, Quote
        }

        private int size = 0;
        private bool run = false;
        private DefineNameMaping name;

        public DataExchange(DefineNameMaping n, int s)
        {
            size = s;
            name = n;
            run = false;
            //выделяет именованную память размером 256 байт для отправки КОМАНД в QUIK, создает потоки чтения/записи
            Memory = MemoryMappedFile.CreateOrOpen(name.ToString(), size, MemoryMappedFileAccess.ReadWrite);
            SR_Memory = new StreamReader(Memory.CreateViewStream(), Encoding.Default);
            SW_Memory = new StreamWriter(Memory.CreateViewStream(), Encoding.Default);

            //Очищает память при первом запуске
            //Встает в начало потока для записи
            SW_Memory.BaseStream.Seek(0, SeekOrigin.Begin);
            // Очищает память, заполняя "нулевыми байтами"
            for (int i = 0; i < size; i++) SW_Memory.Write("\0");
            // Очищает все буферы для SW_Memory и вызывает запись всех данных буфера в основной поток
            SW_Memory.Flush();
            // Встает в начало потока для записи
            SW_Memory.BaseStream.Seek(0, SeekOrigin.Begin);
            // Записывает в первый байт "2", сообщая тем самым, что можно записывать в память
            SW_Memory.Write("0");
            // Очищает все буферы для SW_Memory и вызывает запись всех данных буфера в основной поток
            SW_Memory.Flush();

        }

        public string GetData()
        {
            // Встает в начало потока (позиция 0)
            SR_Memory.BaseStream.Seek(0, SeekOrigin.Begin);
            // Считывает данные из потока памяти, обрезая ненужные байты                
            return SR_Memory.ReadToEnd().Trim('\0', '\r', '\n');
        }

        public void Sleep()
        {
            //Пауза против зависания
            Thread.Sleep(1);
        }

        public void Send(string str)
        {
            string rezult = GetData();
            // Если в потоке первым байтом записан "0", это означает, что DLL завершила чтение и можно записывать следующую команду
            if (rezult.IndexOf("0") == 0)
            {
                //Дополняет строку команды "нулевыми байтами" до нужной длины
                for (int i = str.Length + 1; i < size; i++) str += "\0";
                //Встает в 1-ю позицию
                SW_Memory.BaseStream.Seek(1, SeekOrigin.Begin);
                //Записывает строку
                SW_Memory.Write(str);
                //Сохраняет изменения в памяти
                SW_Memory.Flush();

                SW_Memory.BaseStream.Seek(0, SeekOrigin.Begin);
                //Записывает в первый байт "1", сообщая тем самым, что можно читать команду
                SW_Memory.Write("1");
                //Сохраняет изменения в памяти
                SW_Memory.Flush();
            }
        }

        public void Clear()
        {
            // Встает в начало потока для записи
            SW_Memory.BaseStream.Seek(0, SeekOrigin.Begin);
            // Очищает память, заполняя "нулевыми байтами"
            for (int i = 1; i < size; i++) SW_Memory.Write("\0");
            // Очищает все буферы для SW_Memory и вызывает запись всех данных буфера в основной поток
            SW_Memory.Flush();
            // Встает в начало потока для записи
            SW_Memory.BaseStream.Seek(0, SeekOrigin.Begin);
            // Записывает в первый байт "2", сообщая тем самым, что можно записывать в память
            SW_Memory.Write("0");
            // Очищает все буферы для SW_Memory и вызывает запись всех данных буфера в основной поток
            SW_Memory.Flush();
        }
        
        public void ThreadRun()
        {
            Dictionary<string, string> DictionaryParam = new Dictionary<string, string>();
            while (run)
            {
                string rezult = GetData();          
                // Если в потоке первым байтом записана "1", это означает, что DLL завершила запись и можно читать
                if (rezult.IndexOf("1") == 0)
                {
                    //Стирает запись, сообщая о получении информации и будет обработан
                    Clear();
                    switch (name)
                    {
                            case DefineNameMaping.Deal:
                            {
                                //Удаляет "1" из начала
                                rezult = rezult.Substring(1);
                                // Получает массив пар типа "name=value"
                                string[] Arrg = rezult.Split('@');
                                for (int j = 0; j < Arrg.Length-1; j++)
                                {
                                    if(Arrg[j].IndexOf("N") != 0) continue;
                                    AddTick(Arrg[j]);
                                }
                            }
                            break;

                            case DefineNameMaping.QUIKAnswer:
                        {
                        //        MessageBox.Show(rezult);
                                //Удаляет "1" из начала
                                rezult = rezult.Substring(1);
                                // Получает массив пар типа "name=value"
                                string[] Arr = rezult.Split(';');
                                // Очищает словарь от старых значений
                                DictionaryParam.Clear();
                                // Заполняет в цикле словарь значениями
                                for (int i = 0; i < Arr.Length; i++)
                                {
                                    // Разделяет имя и значение пары в массиве
                                    string[] KeyValue = Arr[i].Split('=');
                                    // Добавляет пару в словарь
                                    DictionaryParam.Add(KeyValue[0], KeyValue[1]);
                                }
                                Event_TerminalAnswer.Invoke(DictionaryParam);
                            }
                            break;
                            case DefineNameMaping.QUIKCommand:

                            break;
                            case DefineNameMaping.Quote:

                            break;
                    }
                    
                }
                // SW.Stop(); //Останавливаем
                // MessageBox.Show(Convert.ToString(SW.ElapsedMilliseconds)); // Время выполнения в миллисекундах
                // Чтоб процесс не "забивал" одно из ядер процессора на 100% нужна пауза в 1 миллисекунду
                Thread.Sleep(1);
            }
        }

        private Int64 PrevNum = 0;
        private Tick tic = new Tick();
        internal void AddTick(Dictionary<string, string> Params)
        {
            // Проверяет существуют ли все необходимые ключи в переданном словаре
            Int64 num = Convert.ToInt64(Params["NUM"]);
            if (PrevNum != num)//(Params["SECCODE"] == "RIH6" && PrevNum != num)
            {
                try
                {
                    tic.priceTick = Convert.ToDouble(Params["PRICE"]);
                    tic.dateTimeTick = Convert.ToDateTime(Params["DATA"]);
                    tic.volumeTick = Convert.ToInt32(Params["QTY"]);
                    tic.paperCode = Params["SECCODE"];
                    //  if (Convert.ToInt32(Params["FLAGS"]) == 1)
                    // tickDataBaseTableAdapter.Insert(num, Params["CLASSCODE"], Params["SECCODE"], Convert.ToDateTime(Params["DATA"]), Convert.ToInt32(Params["PRICE"]), Convert.ToInt32(Params["QTY"]), true);
                    //  else tickDataBaseTableAdapter.Insert(num, Params["CLASSCODE"], Params["SECCODE"], Convert.ToDateTime(Params["DATA"]), Convert.ToInt32(Params["PRICE"]), Convert.ToInt32(Params["QTY"]), false);
                    PrevNum = num;
                    Event_GetTick.Invoke(tic);
                    
                }
                catch (KeyNotFoundException) { }
            }
        }

        internal void AddTick( string Params)
        {
            Event_GetTicks.Invoke(Params);
        }


        public void Start()
        {
            run = true;
        }

        public void Stop()
        {
            run = false;
            SR_Memory.Close();
            SW_Memory.Close();
            Memory.Dispose();
        }

        //Событие OnCount c типом делегата MethodContainer.
        public event AnswerHandler Event_TerminalAnswer;
        public event GetTickHandler Event_GetTick;
        public event GetTickHandlers Event_GetTicks;
    }

}
