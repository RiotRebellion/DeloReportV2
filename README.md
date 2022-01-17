# Отчёты по работе пользователей с поручениями в СЭД "ДЕЛО"

Данный модуль позволяет создавать отчёты по работе пользователей с поручениями в СЭД "ДЕЛО"

!!!ВАЖНО!!!
Поддерживает только MS SQL

## Оглавление 

1. [Установка](установка)
2. [Инструкция](инструкция)
3. [Виды отчётов](виды_отчётов)  

<a name="установка"><h2>Установка</h2></a>
Для установки на рабочее место необходимо выполнить следующие действия
1. Настроить в файле AppSettings.json строку подключения для вашей БД
2. Собрать проект DeloReportV2
3. В папка_проекта/DeloReportV2/Bin появится сборка в папке выбранной вами конфигурации

<a name="инструкция"><h2>Инструкция</h2></a>
1) Запустите .exe файл
__Если в панели снизу написано "Нет подключения", то вероятнее всего неправильно введена строка подключения к базе данных приложения__
2) Нажмите кнопку "Формирование списка сотрудников"
3) В появившемся окне выберите сотрудников, по которым желаете сформировать отчёт и закройте окно
4) Выберите вид отчёта
5) Выберите диапазон дат
6) Нажмите "Сформировать отчёт"

<a name="виды_отчётов"><h2>Виды отчётов</h2></a> 
В модуле реализовано 3 видов отчётов. Отчёты делятся на подробные и неподробные.
В подробных отчётах формируется перечень с номером документа, датой создания документа, текстом поручения, датой ввода отчёта по поручению и сроки поручения (Если оно контрольное)
В неподробных отчётах формируются количественные показатели работы сотрудников с отчётами в СЭД "ДЕЛО"

!!!ВАЖНО!!!
Подробные отчёты необходимо формировать для одного пользователя

Виды отчётов:
+ Подробный отчёт по поручениям (Все) - Подробные отчёт по контролируемым и неконтрольным поручениям
+ Подробный отчёт по поручениям (Контрольные) - Подробные отчёт по контролируемым поручениям
+ Подробный отчёт по поручениям (Неконтролируемые) - Подробные отчёт по неконтролируемым поручениям
+ Отчёт по поручениям (Все) - Отчёт по контролируемым и неконтролируемым поручениям
+ Отчёт по поручениям (Контрольные) - Отчёт по контролируемым поручениям
+ Отчёт по поручениям (Неконтролируемые) - Отчёт по неконтролируемым поручениям
