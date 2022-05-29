# Rss feed aggregator
## Description

### Задача: 

Нужно написать REST API агрегатор новостей.Пользователь подает на вход адрес новостного сайта
или его RSS-ленты

База данных агрегатора начинает автоматически пополняться новостями с этого сайта.У
пользователя есть возможность просматривать список новостей из базы и искать их по подстроке
в заголовке новости.В качестве примера требуется подключить 2 любых новостных сайта на
выбор.Результат - исходный код агрегатора, а также рабочие адреса.Язык C# .Net. Хранилище -
любая реляционная база


### Реализация:

Фоновый таск, собирающий новости, реализован через Quartz

Presentation layer - папки Controllers, Responses, Requests

BLL - папки Services, BackgroundJobs, Validation(nuget Fluent validation)

DAL - папка DAL, использованы mysql+ef, креденшиалы в secrets.json, репозитории, unit-of-work


## ToDo
xUnit

## RSS
https://www.inform.kz/rss/rus.xml
https://www.finam.ru/analysis/forecasts/rsspoint


![image](https://user-images.githubusercontent.com/17678757/168676429-e8a832ba-c763-487d-b8dc-aa937ecd4059.png)
