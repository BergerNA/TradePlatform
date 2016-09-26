# TradePlatform
This is trading platform. It's implements the connection to the trading platform Quik through named pipes. Represents financial data via OpenGl in cluster chart.

TradePlatform реализует кластерный график цены. 
Построение графика осуществляется на основе поступающих в реальном времени, тиковых данных из 
торговой платформы Quik. 

Передача данных происходит посредством именованной области памяти File mapping. Прием и отправка данных 
реализованны в потокобезопасном виде как со стороны C#, так и со стороны Quik (связка QLua-API Quik + с++ DLL-реализующая File mapping). 

Отображение кластеного графика реализовнно на графической библиотеке OpenGl.
